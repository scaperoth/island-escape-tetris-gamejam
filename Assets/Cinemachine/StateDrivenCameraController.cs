using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDrivenCameraController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    public void SwitchToGameOverCamera()
    {
        _animator.Play("GameOverCamera");
    }

    public void SwitchToPlayCamera()
    {
        _animator.Play("PlayCamera");
    }

    public void SwitchToPauseCamera()
    {
        _animator.Play("PauseCamera");
    }
}
