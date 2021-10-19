using System;
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
    private PlayField _playField;
    [SerializeField]
    Vector3 _pivotOffset = Vector3.zero;

    public UnityEvent<Tetromino> OnTetrominoStopped;
    public UnityEvent<Tetromino> OnGameOver;
    private Transform[] _childTransforms;
    private Rigidbody[] _childRigidBody;
    public Rigidbody[] BlockRigidBodies {
        get
        {
            return _childRigidBody;
        }
    }

    private float _moveTimeAdjustment = 1f / 5f;
    private float _fallTime = .5f;
    private float _lastFall = 0;

    private void Awake()
    {
        if (_childTransforms == null || _childTransforms.Length == 0)
        {
            SetChildTransforms();
        }
    }

    private void OnEnable()
    {
        if (_childTransforms == null || _childTransforms.Length == 0)
        {
            SetChildTransforms();
        }

        if (_playField != null && !IsValidGridPos(Vector3.right))
        {
            enabled = false;
            OnGameOver.Invoke(this);
        }
    }

    void Update()
    {
        if (!_playField)
        {
            Debug.LogWarning("WARNING: Tetromino needs reference to playfield to validate grid positions. Use SetPlayField after instantiation");
            return;
        }

        bool vertPress = Input.GetButtonDown("Vertical");
        float horizValue = Input.GetAxis("Horizontal");
        float vertValue = Input.GetAxis("Vertical");

        // Move down
        if (vertPress && vertValue < 0)
        {
            // Modify position
            Vector3 movement = Vector3.back;

            // See if valid
            if (IsValidGridPos(movement))
            {
                // It's valid. Update grid.
                transform.position += _playField.RoundVec3(movement);
                UpdateGrid();
            }
        }

        // Move up
        if (vertPress && vertValue > 0)
        {
            // Modify position
            Vector3 movement = Vector3.forward;

            // See if valid
            if (IsValidGridPos(movement))
            {
                transform.position += _playField.RoundVec3(movement);
                // It's valid. Update grid.
                UpdateGrid();
            }
        }

        // Rotate
        else if (Input.GetButtonDown("Rotate"))
        {
            Vector3 rotation = new Vector3(0, -90, 0);

            // See if valid
            if (IsValidGridRot(rotation))
            {
                foreach (Transform child in _childTransforms)
                {
                    Vector3 v = child.position - (transform.position + _pivotOffset);
                    v = Quaternion.Euler(rotation) * v;
                    child.transform.position = v + (transform.position + _pivotOffset);
                }

                // It's valid. Update grid.
                UpdateGrid();
            }
        }

        // Move Downwards and Fall
        else if ((horizValue > 0 && Time.time - _lastFall >= (_fallTime * _moveTimeAdjustment)) ||
            Time.time - _lastFall >= _fallTime)
        {
            // Modify position
            Vector3 movement = Vector3.right;

            // See if valid
            if (IsValidGridPos(movement))
            {
                transform.position += _playField.RoundVec3(movement);
                // It's valid. Update grid.
                UpdateGrid();
            }
            else
            {
                // Clear filled horizontal lines
                _playField.DeleteFullRows();

                OnTetrominoStopped.Invoke(this);

                // Disable script
                enabled = false;
            }

            _lastFall = Time.time;
        }
    }

    private void SetChildTransforms()
    {
        Transform shapeManager = transform.GetChild(0);
        _childTransforms = new Transform[shapeManager.childCount];
        _childRigidBody = new Rigidbody[shapeManager.childCount];
        for (int i = 0; i < _childTransforms.Length; i++)
        {
            _childTransforms[i] = shapeManager.GetChild(i);
            _childRigidBody[i] = _childTransforms[i].GetComponent<Rigidbody>();
        }
    }

    public void SetPlayfield(PlayField playField)
    {
        if (!_playField)
        {
            _playField = playField;
        }
    }

    bool IsValidGridPos(Vector3 movment)
    {
        if (!_playField)
        {
            Debug.LogError("Tetromino needs reference to playfield to validate grid positions. Use SetPlayField after instantiation");
            return false;
        }

        foreach (Transform child in _childTransforms)
        {
            Vector3 v = _playField.RoundVec3(child.position + movment);
            int x = (int)v.x;
            int z = (int)v.z;

            // Not inside Border?
            if (!_playField.InsideBorder(v))
                return false;

            try
            {
                // Block in grid cell (and not part of same group)?
                if (_playField.grid[x, z] != null &&
                    _playField.grid[x, z].parent.parent != transform)
                {
                    return false;
                }
            }catch(Exception e)
            {
                return false;
            }
        }
        return true;
    }


    bool IsValidGridRot(Vector3 rotation)
    {
        if (!_playField)
        {
            Debug.LogError("Tetromino needs reference to playfield to validate grid positions. Use SetPlayField after instantiation");
            return false;
        }

        foreach (Transform child in _childTransforms)
        {
            Vector3 v = child.position - (transform.position + _pivotOffset);
            v = Quaternion.Euler(rotation) * v;
            v = v + (transform.position + _pivotOffset);
            int x = (int)v.x;
            int z = (int)v.z;

            // Not inside Border?
            if (!_playField.InsideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (_playField.grid[x, z] != null &&
                _playField.grid[x, z].parent.parent != transform)
            {
                return false;
            }
        }
        return true;
    }

    void UpdateGrid()
    {
        // Remove old children from grid
        for (int z = 0; z < _playField.height; ++z)
        {
            for (int x = 0; x < _playField.width; ++x)
            {
                Transform loc = _playField.grid[x, z];
                if (loc != null && ReferenceEquals(loc.parent.parent.gameObject, gameObject))
                    _playField.grid[x, z] = null;
            }
        }

        // Add new children to grid
        foreach (Transform child in _childTransforms)
        {
            Vector3 v = _playField.RoundVec3(child.position);
            _playField.grid[(int)v.x, (int)v.z] = child;
        }
    }
}
