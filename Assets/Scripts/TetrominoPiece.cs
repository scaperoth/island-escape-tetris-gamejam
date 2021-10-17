using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoPiece : MonoBehaviour
{
    private float _raycastOffset = 1f;
    private Transform _transform;
    public Vector3 Position
    {
        get
        {
            return _transform.position;
        }
    }

    private Vector3[] _checkDirections = new Vector3[]
    {
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };

    public Vector3[] CheckDirections
    {
        get
        {
            return _checkDirections;
        }
    }

    private void Awake()
    {
        _transform = transform;
    }

#if UNITY_EDITOR
    private void FixedUpdate()
    {
        Debug.DrawRay(Position, Vector3.right * _raycastOffset, Color.red);
        Debug.DrawRay(Position, Vector3.forward * _raycastOffset, Color.green);
        Debug.DrawRay(Position, Vector3.back * _raycastOffset, Color.blue);
    }
#endif

}
