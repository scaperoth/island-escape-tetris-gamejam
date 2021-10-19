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
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    public Tetromino SpawnTetromino(PlayField _playField)
    {
        if(_tetrominosToSpawn.Length == 0 || _playField == null)
        {
            return null;
        }

        int randomIndex = Random.Range(0, _tetrominosToSpawn.Length - 1);
        PooledObject pooledObject = _tetrominosToSpawn[randomIndex];

        // Spawn object with random 2D rotation.
        PooledObject instance = _objectPool.Spawn(pooledObject, transform.position, Quaternion.Euler(0f, -90f, 0f), false);

        // We can avoid GetComponent<>() for a frequently accessed component, which is nice.
        Tetromino tetromino = instance.As<Tetromino>();

        tetromino.SetPlayfield(_playField);
        //tetromino.transform.localPosition = _offsets.position;
        tetromino.transform.localRotation = Quaternion.Euler(0, -90, 0);
        
        return tetromino;
    }
}
