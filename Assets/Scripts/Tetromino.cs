using UnityEngine;
using UnityEngine.Events;

public struct Offset
{
    public Vector3 position;
    public Vector3 rotation;
}

public class Tetromino : MonoBehaviour
{
    [SerializeField]
    private TetrominoPiece[] _tetrominoPieces = new TetrominoPiece[] { };

    private TetrominoMovementControl _tetrominoMovementControl;

    public UnityEvent<Tetromino> OnTetrominoStopped;
    public UnityEvent<Tetromino> OnGameOver;

    public TetrominoPiece[] Pieces
    {
        get
        {
            return _tetrominoPieces;
        }
    }

    private void Awake()
    {
        _tetrominoMovementControl = GetComponent<TetrominoMovementControl>();
    }

    private void OnEnable()
    {
        if (_tetrominoMovementControl != null)
        {
            _tetrominoMovementControl.OnTetrominoStuck.AddListener(TetrominoStuck);
            _tetrominoMovementControl.OnGameOver.AddListener(GameOver);
        }
    }

    private void OnDisable()
    {
        if (_tetrominoMovementControl != null)
        {
            _tetrominoMovementControl.OnTetrominoStuck.RemoveListener(TetrominoStuck);
            _tetrominoMovementControl.OnGameOver.RemoveListener(GameOver);
        }
    }

    public void EnableControl()
    {
        if (_tetrominoMovementControl != null)
        {
            _tetrominoMovementControl.enabled = true;
        }
    }

    public void DisableControl()
    {
        if (_tetrominoMovementControl != null)
        {
            _tetrominoMovementControl.enabled = false;
        }
    }

    private void TetrominoStuck()
    {
        _tetrominoMovementControl.OnTetrominoStuck.RemoveListener(TetrominoStuck);
        _tetrominoMovementControl.OnGameOver.RemoveListener(GameOver);
        OnTetrominoStopped.Invoke(this);
    }

    private void GameOver()
    {
        OnGameOver.Invoke(this);
    }

    // TODO: Don't do this...
    public Offset GetSpawnOffset()
    {
        Offset offset;
        switch (gameObject.name)
        {
            case "LongShape":
                offset.position = new Vector3(.5f, 0f, .25f);
                offset.rotation = new Vector3(0f, -90, 0f);
                break;
            case "SquareShape":
                offset.position = new Vector3(0f, 0f, .25f);
                offset.rotation = new Vector3(0f, 0, 0f);
                break;
            default:
                offset.position = new Vector3(.25f, 0f, 0f);
                offset.rotation = new Vector3(0f, -90, 0f);
                break;
        }

        return offset;
    }
}
