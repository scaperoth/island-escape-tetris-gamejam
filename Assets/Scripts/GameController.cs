
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private TetrominoSpawner _tetrominoSpawner;
    [SerializeField]
    private PlayField _playField;
    [SerializeField]
    ParticleSystem _particles;
    public UnityEvent OnRespawn;
    public UnityEvent OnGameOver;

    Tetromino _lastSpawnedTetromino;


    private void Start()
    {
        _lastSpawnedTetromino = _tetrominoSpawner.SpawnTetromino(_playField);
        OnSpawn(_lastSpawnedTetromino);
    }

    private void OnSpawn(Tetromino tetromino)
    {
        tetromino.OnTetrominoStopped.AddListener(HandleTetrominoStopped);
        tetromino.OnGameOver.AddListener(HandleGameOver);
        tetromino.gameObject.SetActive(true);
    }

    private void HandleTetrominoStopped(Tetromino tetromino)
    {
        tetromino.OnTetrominoStopped.RemoveListener(HandleTetrominoStopped);
        tetromino.OnTetrominoStopped.RemoveListener(HandleGameOver);
        _lastSpawnedTetromino = _tetrominoSpawner.SpawnTetromino(_playField);

        OnRespawn.Invoke();

        OnSpawn(_lastSpawnedTetromino);

        //HandleGameOver(_lastSpawnedTetromino);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
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
    }
}
