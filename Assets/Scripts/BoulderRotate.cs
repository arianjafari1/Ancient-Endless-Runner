using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderRotate : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// 27/06/22 - created boulder
    ///          - boulder rotates
    ///          - flipped camera to show boulder (in editor)
    /// 
    /// </summary>
    [SerializeField] private float rotationAmount;
    [SerializeField] private GameObject boulder;
    private void FixedUpdate()
    {
        boulder.transform.Rotate(Vector3.forward, rotationAmount);
    }
}
