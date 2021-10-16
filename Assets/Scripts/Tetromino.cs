using UnityEngine;

public class Tetromino : MonoBehaviour
{

    [SerializeField]
    private TetrominoPiece[] _tetrominoPieces = new TetrominoPiece[] { };

    private TetrominoMovementControl _tetrominoMovementControl;
    private TetrominoAutoMovement _tetrominoAutoMovement;


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
        _tetrominoAutoMovement = GetComponent<TetrominoAutoMovement>();
    }

    private void OnEnable()
    {
        if (_tetrominoMovementControl != null)
        {
            _tetrominoMovementControl.OnTetrominoStuck.AddListener(DisableControl);
        }
    }

    private void OnDisable()
    {
        if (_tetrominoMovementControl != null)
        {
            _tetrominoMovementControl.OnTetrominoStuck.RemoveListener(DisableControl);
        }
    }

    public void EnableControl()
    {
        if (_tetrominoMovementControl != null)
        {
            _tetrominoMovementControl.enabled = true;
        }
        if (_tetrominoAutoMovement != null)
        {
            _tetrominoAutoMovement.enabled = true;
        }
    }

    public void DisableControl()
    {
        if (_tetrominoMovementControl != null)
        {
            _tetrominoMovementControl.enabled = false;
        }
        if (_tetrominoAutoMovement != null)
        {
            _tetrominoAutoMovement.enabled = false;
        }
    }
}
