using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Warp : MonoBehaviour
{
    [SerializeField] Camera camera;
    private float cameraZPos;
    private Transform objTrans;
    private Vector3 objPos;
    private float objZPos;
    // Start is called before the first frame update
    void Start()
    {
        cameraZPos = camera.transform.position.z;
        objTrans = objTrans.GetComponent<Transform>();
        objZPos = objTrans.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
