using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderRotate : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 27/06/22 by Malachi
    /// 
    /// 27/06/22 - created boulder
    ///          - boulder rotates
    ///          - flipped camera to show boulder (in editor)
    /// 28/06/22 - boulder rotates right instead of forwards with a default rotation on the other axis at 90 degrees
    ///          - was causing problems with moving the boulder when the other way
    /// 22/07/22 - Oakley came in and messed with your script!
    /// 14/08/22 - Added getter/setter for rotation amount. 
    /// </summary>


    [SerializeField] private float forwardRotationAmount;
    [SerializeField] private GameObject boulder;

    public float RotationAmount
    {
        get
        {
            return forwardRotationAmount;
        }
        set
        {
            forwardRotationAmount = value;
        }
    }
    /// <summary>
    /// rotates the boulder in the direction of the player to give the illusion of movement 
    /// </summary>
    private void FixedUpdate()
    {
        boulder.transform.Rotate(Vector3.right, forwardRotationAmount);
    }
}
