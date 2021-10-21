using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public UnityEvent OnUnpause;

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            OnUnpause.Invoke();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            OnUnpause.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("Main Menu");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
