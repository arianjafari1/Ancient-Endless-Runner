using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderRotate : MonoBehaviour
{
    [SerializeField] private float rotationAmount;
    [SerializeField] private GameObject boulder;
    private void FixedUpdate()
    {
        boulder.transform.Rotate(Vector3.forward, rotationAmount);
    }
}
