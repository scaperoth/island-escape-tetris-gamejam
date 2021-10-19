using UnityEngine;
using UnityEngine.SceneManagement;

public class Block : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collider other)
    {
        if(other.gameObject.layer == 4)
        {
            Debug.Log("BAM!");
            if (_rb.useGravity)
            {
                Debug.Log("BOOM!");
                _rb.useGravity = false;
                _rb.AddForce(0, _rb.velocity.y * -45, 0);
            }
        } else if(other.gameObject.layer == 8)
        {
            _rb.isKinematic = true;
            //Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
    }
}
