using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _targetGameOverPosition;
    [SerializeField]
    private float _speed;

    private bool _moveTowardsGameOver;

    public void MoveToGameOverPosition()
    {
        _moveTowardsGameOver = true;
    }

    private void Update()
    {
        if (_moveTowardsGameOver && transform.position != _targetGameOverPosition.position && transform.rotation != _targetGameOverPosition.rotation)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _targetGameOverPosition.localPosition, _speed * Time.deltaTime);
            transform.localRotation = _targetGameOverPosition.localRotation;
        }
        else if (transform.position != _targetGameOverPosition.position && transform.rotation != _targetGameOverPosition.rotation)
        {
            _moveTowardsGameOver = false;
        }

    }
}
