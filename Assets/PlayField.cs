using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    [SerializeField]
    private Vector3 _fieldOfPlay = Vector3.zero;

    [SerializeField]
    private Color _gizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawCube(transform.position, _fieldOfPlay);
    }

}
