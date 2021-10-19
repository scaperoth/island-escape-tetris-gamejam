using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private TetrominoSpawner _tetrominoSpawner;
    [SerializeField]
    private PlayField _playField;

    //Ray[] _gridRays;
    Tetromino _lastSpawnedTetromino;

    private void Start()
    {
        _lastSpawnedTetromino = _tetrominoSpawner.SpawnTetromino(_playField);
        OnSpawn(_lastSpawnedTetromino);
    }

    private void OnSpawn(Tetromino tetromino)
    {
        Offset _offsets = tetromino.GetSpawnOffset();

        tetromino.OnTetrominoStopped.AddListener(HandleTetrominoStopped);
        tetromino.OnGameOver.AddListener(HandleGameOver);
        tetromino.gameObject.SetActive(true);
    }

    private void HandleTetrominoStopped(Tetromino tetromino)
    {
        tetromino.OnTetrominoStopped.RemoveListener(HandleTetrominoStopped);
        tetromino.OnTetrominoStopped.RemoveListener(HandleGameOver);

        _lastSpawnedTetromino = _tetrominoSpawner.SpawnTetromino(_playField);
        OnSpawn(_lastSpawnedTetromino);

        Debug.Log("DONE, Spawning...");
    }

    private void HandleGameOver(Tetromino tetromino)
    {
        Debug.Log("GAME OVER!");
        tetromino.enabled = false;
    }
}
