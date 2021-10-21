using UnityEngine;
using UnityEngine.Events;

public class StateDrivenCameraController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    public UnityEvent OnGameOverCameraSwitch;
    public UnityEvent OnPlayCameraSwitch;
    public UnityEvent OnPauseCameraSwitch;

    public void SwitchToGameOverCamera()
    {
        _animator.Play("GameOverCamera");
        OnGameOverCameraSwitch.Invoke();
    }

    public void SwitchToPlayCamera()
    {
        _animator.Play("PlayCamera");
        OnPlayCameraSwitch.Invoke();
    }

    public void SwitchToPauseCamera()
    {
        _animator.Play("PauseCamera");
        OnPauseCameraSwitch.Invoke();
    }
}
