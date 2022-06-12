using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    /// Code done by Arian- start
    [SerializeField] private float movementSpeed = 0.000000001f; //declare variable float for movement speed of the tile to be edited in Unity editor
    [SerializeField] private Transform tilePosition; //getting the position of tile
    [SerializeField] private Transform targetZ; // the target z position for where the tile should go to (behind the camera)
    private float t = 0.1f;
    /// Code done by Arian- end

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        
    }


    private void FixedUpdate()
    {

        /// Code done by Arian- start
        //combining MoveTowards and Lerp to move the tiles towards the camera, MoveTowards for constant speed, and lerp for smooth movement
        Vector3 a = tilePosition.transform.position; //assigning Vector3 the position of tilePosition
        Vector3 b = targetZ.position; //assigning Vector3 b the postion of targetZ
        tilePosition.transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), movementSpeed);
        /// Code done by Arian- end


        /// Code done by Arian
        //tilePosition.transform.position = new Vector3(0, 0, transform.position.z - movementSpeed);


        //Vector3 a = tilePosition.transform.position;
        //Vector3 b = targetZ.position;
        //tilePosition.transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), movementSpeed);
        /// Code done by Arian

    }




}
