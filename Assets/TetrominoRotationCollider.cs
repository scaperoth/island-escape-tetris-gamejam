using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TetrominoRotationCollider : MonoBehaviour
{

    Transform _parentTransform;
    public UnityEvent OnRotationCollisionEnter;
    public UnityEvent OnRotationCollisionExit;

    private void Start()
    {
        _parentTransform = GetComponentInParent<Transform>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            return;
        }
        if (!ReferenceEquals(other.gameObject, transform.parent.gameObject) && !ReferenceEquals(other.gameObject, gameObject))
        {
            OnRotationCollisionEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            return;
        }
        if (!ReferenceEquals(other.gameObject, transform.parent.gameObject) && !ReferenceEquals(other.gameObject, gameObject))
        {
            OnRotationCollisionExit.Invoke();
        }
    }
}
