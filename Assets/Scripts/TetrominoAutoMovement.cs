using UnityEngine;

public class TetrominoAutoMovement : MonoBehaviour
{
    [Range(0.1f, 1f)]
    private float _moveStep = .5f;
    private float _automaticMoveDelay = 1f;
    private float _lastAutomaticMoveTime = 0;

    // Update is called once per frame
    void Update()
    {
        if (_lastAutomaticMoveTime + _automaticMoveDelay < Time.time)
        {
            Vector3 movement = new Vector3(_moveStep, 0, 0);

            transform.position = transform.position + movement;

            _lastAutomaticMoveTime = Time.time;
        }

    }

}
