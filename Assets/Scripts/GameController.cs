
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [Header("Game Area")]
    [SerializeField]
    private PlayField _playField;
    [SerializeField]
    private TetrominoSpawner _tetrominoSpawner;

    [Header("Scoring")]
    [SerializeField]
    private int _baseScore = 100;
    [SerializeField]
    private IntVariable _gameScore;

    [Header("Effects")]
    [SerializeField]
    ParticleSystem _particles;

    // events
    public UnityEvent OnRespawn;
    public UnityEvent OnGameOver;
    public UnityEvent OnPause;

    // state store
    bool _gameOver = false;
    Tetromino _lastSpawnedTetromino;

    private void Start()
    {
        // kick off the spawner
        _lastSpawnedTetromino = _tetrominoSpawner.SpawnTetromino(_playField);
        OnSpawn(_lastSpawnedTetromino);

        // reset the score
        _gameScore.SetValue(0);
    }

    private void OnEnable()
    {
        _playField.OnLineCleared.AddListener(UpdateScore);
    }

    private void OnDisable()
    {
        _playField.OnLineCleared.RemoveListener(UpdateScore);
    }

    private void OnSpawn(Tetromino tetromino)
    {
        // after spawn, subscribe to events and turn on object
        tetromino.OnTetrominoStopped.AddListener(HandleTetrominoStopped);
        tetromino.OnGameOver.AddListener(HandleGameOver);
        tetromino.gameObject.SetActive(true);
    }

    private void HandleTetrominoStopped(Tetromino tetromino)
    {
        // when the tetromino is stopped, clear the events and get another one
        tetromino.OnTetrominoStopped.RemoveListener(HandleTetrominoStopped);
        tetromino.OnTetrominoStopped.RemoveListener(HandleGameOver);
        _lastSpawnedTetromino = _tetrominoSpawner.SpawnTetromino(_playField);
        OnSpawn(_lastSpawnedTetromino);

        // fire event for anything looking
        OnRespawn.Invoke();

    }

    private void Update()
    {
        // reload control for game over
        if (_gameOver && Input.GetButtonDown("Submit"))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        // pause screen
        if (!_gameOver && Input.GetButtonDown("Cancel"))
        {
            _lastSpawnedTetromino.enabled = false;
            OnPause.Invoke();
        }
    }

    private void HandleGameOver(Tetromino tetromino)
    {
        tetromino.enabled = false;
        Transform poolTransform = _tetrominoSpawner.ObjectPool.transform;
        for(int i = 0; i < poolTransform.childCount; i++)
        {
            Tetromino pooledTetromino = poolTransform.GetChild(i).GetComponent<Tetromino>(); 
            foreach (Rigidbody child in pooledTetromino.BlockRigidBodies)
            {
                child.useGravity = true;
                child.isKinematic = false;
            }
        }

        Vector3 explosionPos = _playField.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 50);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(500, explosionPos, 10f, 100);
                
        }
        _particles.Play();
        OnGameOver.Invoke();
        _gameOver = true;
    }

    private void UpdateScore(int rowsCleared)
    {
        int newScore = (rowsCleared + (rowsCleared - 1)) * _baseScore;
        _gameScore.AddToValue(newScore);
    }

    public void UnPause()
    {
        _lastSpawnedTetromino.enabled = true;
    }
}
