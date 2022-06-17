using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    /// Code done by Arian- start
    [SerializeField] private float movementSpeed = 0f; //declare variable float for movement speed of the tile to be edited in Unity editor
    private float maxMovementSpeed = 0.5f; //need more test to see when the game breaks
    private float speedIncreaseEverySecond = 0.0001f; //speed increase every second
    [SerializeField] private Transform tilePosition; //getting the position of tile
    [SerializeField] private Transform targetZ; // the target z position for where the tile should go to (behind the camera)
    private float t = 0.1f; //target point used for lerp in the fixed update to move the tile backwards

    [SerializeField] private Transform tileSpawnStart;
    [SerializeField] private Transform groundLength; //ground child object of tile
    //private float tileLength = 0f; //assign tileLength to 0, then changing it later in the start function


    [SerializeField] private GameObject tilePrefab;
    //[SerializeField] private GameObject[] tilePrefabs; //array of tile prefabs


    /// Code done by Arian- end

    // Start is called before the first frame update
    private void Start()
    {
        /// Code done by Arian- start
        //tileLength = groundLength.transform.localScale.z; //assiging the tileLenght the Length of Ground child object of the tile on the z axis, it is more scalable than hardcoding it
        

        /// Code done by Arian- end

    }


    private void FixedUpdate()
    {

        /// Code done by Arian- start

        if (tilePosition != null) //preventing an error by first checking if transform tilePosition is not null
        {
            //combining MoveTowards and Lerp to move the tiles towards the camera, MoveTowards for constant speed, and lerp for smooth movement:
            Vector3 a = tilePosition.transform.position; //assigning Vector3 the position of tilePosition
            Vector3 b = targetZ.position; //assigning Vector3 b the postion of targetZ
            tilePosition.transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), movementSpeed); //using movetowards and lerp to move tile backwards at constant speed
        }

        if (movementSpeed > maxMovementSpeed) //check if movement speed is higher than maximum speed
        {
            Debug.LogError("Movement speed should be lower than " + maxMovementSpeed); //custom error message in case movementSpeed goes over the maximum speed
            return; //returning nothing
        }


        if (movementSpeed < maxMovementSpeed) //if the movement speed is lower than 0.5 then
        {
            movementSpeed += speedIncreaseEverySecond; //increase speed by this amount
        }
        /// Code done by Arian- end

    }


    /// Code done by Arian- start
    private void OnTriggerEnter(Collider collisionInfo) //check for collision infor
    {


        if (collisionInfo.gameObject.CompareTag("Player")) //if collision info comapre with the tile death point tag, then execute the code undeneath
        {
            Instantiate(tilePrefab, tileSpawnStart.position, tileSpawnStart.rotation);
            Debug.Log("New Tile spawned");

        }



    }

    /// Code done by Arian- end



}
