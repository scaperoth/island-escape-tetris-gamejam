using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    [SerializeField]
    private Pool _objectPool;
    [SerializeField]
    private PooledObject[] _tetrominosToSpawn;

    [SerializeField]
    private Vector3 _spawnArea = Vector3.zero;
    [SerializeField]
    private Color _gizmosColor = new Color(1, 0, 0, 0.2f);

    void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawCube(transform.position, _spawnArea);
    }

    private void Start()
    {
        SpawnTetromino();
    }

    void SpawnTetromino()
    {
        if(_tetrominosToSpawn.Length == 0)
        {
            return;
        }

        Debug.Log("SPAWNING!");

        int randomIndex = Random.Range(0, _tetrominosToSpawn.Length - 1);
        PooledObject pooledObject = _tetrominosToSpawn[randomIndex];

        // Spawn object with random 2D rotation.
        PooledObject instance = _objectPool.Spawn(pooledObject, transform.position, Quaternion.Euler(0f, -90f, 0f));
        // We can avoid GetComponent<>() for a frequently accessed component, which is nice.
        Tetromino tetromino = instance.As<Tetromino>();
        Offset _offsets = tetromino.GetSpawnOffset();
        tetromino.transform.localPosition += _offsets.position;
        tetromino.transform.localRotation = Quaternion.Euler(_offsets.rotation); 

        tetromino.OnTetrominoStopped.AddListener(HandleTetrominoStopped);
        tetromino.OnTetrominoStopped.AddListener(HandleGameOver);

        tetromino.EnableControl();
    }

    private void HandleTetrominoStopped(Tetromino tetromino)
    {
        Debug.Log("TETROMINO STUCK");
        tetromino.DisableControl();
        tetromino.OnTetrominoStopped.RemoveListener(HandleTetrominoStopped);
        tetromino.OnTetrominoStopped.RemoveListener(HandleGameOver);
        SpawnTetromino();
    }

    private void HandleGameOver(Tetromino tetromino)
    {
        tetromino.DisableControl();
        Debug.Log("GAME OVER!");
    }
}
