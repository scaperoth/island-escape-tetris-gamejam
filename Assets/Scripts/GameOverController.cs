using UnityEngine;
using UnityEngine.Events;

public class GameOverController : MonoBehaviour
{
    public UnityEvent OnRestart;
    public UnityEvent OnQuit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            OnQuit.Invoke();
        }

        if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.R))
        {
            OnRestart.Invoke();
        }
    }
}
