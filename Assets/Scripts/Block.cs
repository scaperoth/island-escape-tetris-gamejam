using UnityEngine;
using UnityEngine.SceneManagement;

public class Block : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_rb.isKinematic) return;

        if (other.gameObject.layer == 4)
        {
            if (_rb.useGravity)
            {
                _rb.useGravity = false;
                _rb.velocity = new Vector3(0, -.5f, 0);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            _rb.isKinematic = true;
        }
    }
}
