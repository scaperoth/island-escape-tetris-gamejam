using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private TetrominoSpawner _tetrominoSpawner;
    //Ray[] _gridRays;
    Tetromino _lastSpawnedTetromino;


    private void Start()
    {
        _lastSpawnedTetromino = _tetrominoSpawner.SpawnTetromino();
        OnSpawn(_lastSpawnedTetromino);
    }

    private void OnSpawn(Tetromino tetromino)
    {
        Offset _offsets = tetromino.GetSpawnOffset();

        tetromino.OnTetrominoStopped.AddListener(HandleTetrominoStopped);
        tetromino.OnGameOver.AddListener(HandleGameOver);

        tetromino.EnableControl();
    }

    private void HandleTetrominoStopped(Tetromino tetromino)
    {
        Debug.Log("TETROMINO STUCK");
        tetromino.DisableControl();
        tetromino.OnTetrominoStopped.RemoveListener(HandleTetrominoStopped);
        tetromino.OnTetrominoStopped.RemoveListener(HandleGameOver);

        _lastSpawnedTetromino = _tetrominoSpawner.SpawnTetromino();
        OnSpawn(_lastSpawnedTetromino);
    }

    private void HandleGameOver(Tetromino tetromino)
    {
        tetromino.DisableControl();
        Debug.Log("GAME OVER!");
    }
}
