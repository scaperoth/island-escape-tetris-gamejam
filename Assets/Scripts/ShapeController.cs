using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour
{

    [Range(0.1f, 1f)]
    private float _moveStep = .5f;
    private float _moveDelay = .5f;
    private float _rotationDelay = .2f;
    private float _lastMoveTime = 0f;
    private float _lastRotationTime = 0f;
    private bool _freeze = false;

    private Vector3 _debugPosition = Vector3.zero;
    private Vector3 _debugBox = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
    }
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0, 0, .5f); ;
        Gizmos.DrawCube(_debugPosition, _debugBox);
    }

    private void OnEnable()
    {
        _freeze = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();

        if (_freeze)
        {
            enabled = false;
        }

        if (_lastMoveTime + _moveDelay > Time.time)
        {
            return;
        }

        // float horizontal = Input.GetAxis("Horizontal");
        float horizontal = 1;
        float vertical = Input.GetAxis("Vertical");

        int intHoriz = horizontal > 0 ? Mathf.CeilToInt(horizontal) : 0;
        int intVert = vertical > 0 ? Mathf.CeilToInt(vertical) : Mathf.FloorToInt(vertical);

        bool safePosition = CheckSafePosition(transform.position);

        Vector3 movement = new Vector3(intHoriz * _moveStep, 0, intVert * _moveStep);

        Vector3 newPosition = transform.position + movement;


       /* if (!safePosition && movement.sqrMagnitude > 0)
        {
            return;
        }*/

        transform.position = newPosition;

        _lastMoveTime = Time.time;
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

    bool CheckSafePosition(Vector3 positionToCheckForSafe)
    {
        _debugPosition = positionToCheckForSafe + new Vector3(_moveStep, 0, 0);
        _debugBox = Vector3.one * .1f;
        Collider[] hitColliders = Physics.OverlapBox(_debugPosition, _debugBox);

        foreach(Collider hit in hitColliders)
        {
            return false;
        }

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger TurnOver");
        _freeze = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("TurnOver");
        _freeze = true;
    }
}
