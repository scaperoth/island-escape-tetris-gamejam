using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public UnityEvent OnUnpause;
    public UnityEvent OnRestart;
    public UnityEvent OnQuit;

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
            OnQuit.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnRestart.Invoke();
        }
    }
}
