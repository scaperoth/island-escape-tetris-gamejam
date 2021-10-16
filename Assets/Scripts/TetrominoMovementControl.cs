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
    private int _raycastLayerMask;

    [Range(0.1f, 1f)]
    private float _moveStep = .5f;
    private float _moveDelay = .2f;
    private float _rotationDelay = .2f;
    private float _lastMoveTime = 0f;
    private float _lastRotationTime = 0f;
    private bool _rotationEnabled = true;

    private bool[] _blockedMovement = new bool[]
    {
        false, // right
        false, // up
        false // down 
    };

    public UnityEvent OnTetrominoStuck;

    private void Start()
    {
        _tetromino = GetComponent<Tetromino>();
        _raycastLayerMask = ~(1 << _rotationColliderLayer); 
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

        if (_rotationEnabled)
        {
            HandleRotation();
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        int intHoriz = horizontal > 0 ? Mathf.CeilToInt(horizontal) : 0;
        int intVert = vertical > 0 ? Mathf.CeilToInt(vertical) : Mathf.FloorToInt(vertical);

        if (intVert > 0 && _blockedMovement[1])
        {
            intVert = 0;
        }else if(intVert < 0 && _blockedMovement[2])
        {
            intVert = 0;
        }

        if (_lastMoveTime + _moveDelay < Time.time)
        {
            Vector3 movement = new Vector3(intHoriz * _moveStep, 0, intVert * _moveStep);

            Vector3 newPosition = transform.position + movement;


            transform.position = newPosition;

            _lastMoveTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        CheckAllowedMovement();
        CheckIfDone();
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
        if (Input.GetButton("Rotate"))
        {
            if (_lastRotationTime + _rotationDelay < Time.time)
            {
                Vector3 newRotation = new Vector3(0, 90 % 360, 0) + transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(newRotation);
                _lastRotationTime = Time.time;
            }
        }
    }

    bool CheckIfDone()
    {
        RaycastHit hit;
        _blockedMovement[0] = false;

        foreach (TetrominoPiece tetrominoPiece in _tetromino.Pieces)
        {
            Vector3 position = tetrominoPiece.Position;
            bool forwardBlocked = Physics.Raycast(position, Vector3.right, out hit, _moveStep, _raycastLayerMask);

            if (forwardBlocked && !ReferenceEquals(hit.collider.gameObject, gameObject))
            {
                _blockedMovement[0] = true;
                OnTetrominoStuck.Invoke();
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
            bool upBlocked = Physics.Raycast(position, Vector3.forward, out uphit, _moveStep, _raycastLayerMask);
            bool downBlocked = Physics.Raycast(position, Vector3.back, out downhit, _moveStep, _raycastLayerMask);

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
