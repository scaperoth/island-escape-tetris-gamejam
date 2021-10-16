using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Tetromino))]
public class TetrominoMovementControl : MonoBehaviour
{
    private Tetromino _tetromino;

    [SerializeField]
    private TetrominoRotationCollider _rotationCollider;
    private int _rotationColliderLayer = 3;
    private int _spawnAreaLayer = 6;
    private int _collisionCheckLayerMask;

    // player movment
    [Range(0.1f, 1f)]
    private float _moveStep = 1f;
    private float _lastMoveTime = 0f;
    private bool _checkVerticalAllowanceAfterMove = false;
    private bool _checkPositionAfterMove = false;

    // automatic movement
    private float _moveDelay = 1f;

    // rotation
    private bool _rotationEnabled = true;

    private bool[] _blockedMovement = new bool[]
    {
        false, // right
        false, // up
        false // down 
    };

    public UnityEvent OnTetrominoStuck;
    public UnityEvent OnGameOver;

    private void Start()
    {
        _tetromino = GetComponent<Tetromino>();
        _collisionCheckLayerMask = ~((1 << _rotationColliderLayer) | (1 << _spawnAreaLayer));
    }

    private void OnEnable()
    {
        _rotationCollider.OnRotationCollisionEnter.AddListener(DisableRotation);
        _rotationCollider.OnRotationCollisionExit.AddListener(EnableRotation);
        _rotationCollider.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _rotationCollider.OnRotationCollisionEnter.RemoveListener(DisableRotation);
        _rotationCollider.OnRotationCollisionExit.RemoveListener(EnableRotation);
        _rotationCollider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // don't allow movement until after checks are done
        if (_checkPositionAfterMove || _checkVerticalAllowanceAfterMove)
        {
            return;
        }

        if (_rotationEnabled)
        {
            HandleRotation();
        }

        // get input and normalize. horiztontal can only be 0 or 1
        // and vertical can be -1, 0, 1
        float horizontal = Input.GetAxis("Horizontal");
        bool verticalPressed = Input.GetButtonDown("Vertical");
        float vertical = verticalPressed ? Input.GetAxis("Vertical") : 0;
        int intVert = vertical > 0 ? Mathf.CeilToInt(vertical) : Mathf.FloorToInt(vertical);

        // stop movement if blocked
        if (intVert > 0 && _blockedMovement[1])
        {
            intVert = 0;
        }
        else if (intVert < 0 && _blockedMovement[2])
        {
            intVert = 0;
        }

        float adjustedMoveDelay = horizontal > 0 ? _moveDelay / 5f : _moveDelay;

        Move(0, intVert);
        if (_lastMoveTime + adjustedMoveDelay < Time.time)
        {
            Move(1, 0);
            _lastMoveTime = Time.time;
        }
    }

    private void Move(int horiz, int vert)
    {
        Vector3 movement = new Vector3(horiz * _moveStep, 0, vert * _moveStep);

        Vector3 newPosition = transform.position + movement;

        transform.position = newPosition;

        _checkPositionAfterMove = horiz > 0;
        _checkVerticalAllowanceAfterMove = vert != 0;
    }

    private void FixedUpdate()
    {
        if (_checkVerticalAllowanceAfterMove)
        {
            CheckAllowedMovement();
            _checkVerticalAllowanceAfterMove = false;
        }

        if (_checkPositionAfterMove)
        {
            bool isStopped = CheckIfStopped();
            if (isStopped)
            {
                bool gameOver = CheckIfGameOver();

                if (gameOver)
                {
                    OnGameOver.Invoke();
                }
                else
                {
                    OnTetrominoStuck.Invoke();
                }
            }
            _checkPositionAfterMove = false;
        }
    }

    public void DisableRotation()
    {
        _rotationEnabled = false;
    }
    public void EnableRotation()
    {
        _rotationEnabled = true;
    }

    void HandleRotation()
    {
        if (Input.GetButtonDown("Rotate"))
        {
            Vector3 newRotation = new Vector3(0, 90 % 360, 0) + transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(newRotation);
        }
    }

    bool CheckIfStopped()
    {
        RaycastHit hit;
        _blockedMovement[0] = false;

        foreach (TetrominoPiece tetrominoPiece in _tetromino.Pieces)
        {
            Vector3 position = tetrominoPiece.Position;
            bool forwardBlocked = Physics.Raycast(position, Vector3.right, out hit, _moveStep, _collisionCheckLayerMask);

            if (forwardBlocked && !ReferenceEquals(hit.collider.gameObject, gameObject))
            {
                _blockedMovement[0] = true;
                return true;
            }
        }

        return false;
    }

    bool CheckIfGameOver()
    {
        RaycastHit uphit;
        RaycastHit downhit;

        foreach (TetrominoPiece tetrominoPiece in _tetromino.Pieces)
        {
            Vector3 position = tetrominoPiece.Position;

            bool upInSpawnArea = Physics.Raycast(position, Vector3.forward, out uphit, 10, (1 << _spawnAreaLayer));
            bool downInSpawnArea = Physics.Raycast(position, Vector3.back, out downhit, 10, (1 << _spawnAreaLayer));

            if (upInSpawnArea || downInSpawnArea)
            {
                return true;
            }
        }

        return false;
    }

    bool CheckAllowedMovement()
    {
        RaycastHit uphit;
        RaycastHit downhit;

        _blockedMovement[1] = false;
        _blockedMovement[2] = false;

        foreach (TetrominoPiece tetrominoPiece in _tetromino.Pieces)
        {
            Vector3 position = tetrominoPiece.Position;
            bool upBlocked = Physics.Raycast(position, Vector3.forward, out uphit, _moveStep, _collisionCheckLayerMask);
            bool downBlocked = Physics.Raycast(position, Vector3.back, out downhit, _moveStep, _collisionCheckLayerMask);

            if (upBlocked && !ReferenceEquals(uphit.collider.gameObject, gameObject))
            {
                _blockedMovement[1] = upBlocked;
            }

            if (downBlocked && !ReferenceEquals(downhit.collider.gameObject, gameObject))
            {
                _blockedMovement[2] = downBlocked;
            }
        }

        return false;
    }
}
