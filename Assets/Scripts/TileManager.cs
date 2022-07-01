using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    /// Code done by Arian- start
    private TileMovement tileMovement;

    private int randomTile; //random Tile number
    private Vector3 targetZ;
   [SerializeField] bool tileRight;
    
    //[SerializeField] private float movementSpeed = 0f; //declare variable float for movement speed of the tile to be edited in Unity editor
    //private float maxMovementSpeed = 0.5f; //need more test to see when the game breaks
    //private float speedIncreaseEverySecond = 0.0001f; //speed increase every second
    [SerializeField] private Transform tilePosition; //getting the position of tile
    private float t = 0.1f; //target point used for lerp in the fixed update to move the tile backwards

    [SerializeField] private Transform tileSpawnStart;
    [SerializeField] private Transform groundLength; //ground child object of tile
    //private float tileLength = 0f; //assign tileLength to 0, then changing it later in the start function



    //[SerializeField] private GameObject[] tilePrefabs; //array of tile prefabs


    /// Code done by Arian- end

    // Start is called before the first frame update
    private void Awake()
    {
        /// Code done by Arian- start
        //tileLength = groundLength.transform.localScale.z; //assiging the tileLenght the Length of Ground child object of the tile on the z axis, it is more scalable than hardcoding it
        tileMovement = FindObjectOfType<TileMovement>(); //reference the object with code
        randomTile = Random.Range(0, tileMovement.TilePrefabs.Length);

        if (tileRight == false)
        {
            targetZ = tileMovement.TargetZ.position;
        } else if (tileRight == true)
        {
            targetZ = tileMovement.TargetZright.position;
        }

        /// Code done by Arian- end

    }


    private void FixedUpdate()
    {
        /// Code done by Arian- start

        if (tilePosition != null) //preventing an error by first checking if transform tilePosition is not null
        {
            //combining MoveTowards and Lerp to move the tiles towards the camera, MoveTowards for constant speed, and lerp for smooth movement:
            Vector3 a = tilePosition.transform.position; //assigning Vector3 the position of tilePosition
            Vector3 b = targetZ; //assigning Vector3 b the postion of targetZ
            tilePosition.transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), tileMovement.movementSpeedGetterSetter * Time.fixedDeltaTime); //using movetowards and lerp to move tile backwards at constant speed
        }

        if (tileMovement.movementSpeedGetterSetter > tileMovement.maxMovementSpeedGetter + tileMovement.maxMovementSpeedGetter * 20 / 100) //check if movement speed is higher than maximum speed + 20 %
        {
            Debug.LogError("Movement speed should be lower than " + tileMovement.maxMovementSpeedGetter); //custom error message in case movementSpeed goes over the maximum speed
            return; //returning nothing
        }


        if (tileMovement.movementSpeedGetterSetter < tileMovement.maxMovementSpeedGetter) //if the movement speed is lower than 0.5 then
        {
            tileMovement.movementSpeedGetterSetter += tileMovement.speedIncreaseEverySecondGetterSetter * Time.fixedDeltaTime; //increase speed by this amount
        }
        /// Code done by Arian- end

    }


    /// Code done by Arian- start
    private void OnTriggerEnter(Collider collisionInfo) //check for collision infor
    {


        if (collisionInfo.gameObject.CompareTag("Player")) //if collision info comapre with the tile death point tag, then execute the code undeneath
        {
            Instantiate(tileMovement.TilePrefabs[randomTile], tileSpawnStart.position, tileSpawnStart.rotation);
            Debug.Log("New Tile spawned");

        }



    }


    //public float movementSpeedGetterSetter //getters and setters for movement speed
    //{
    //    get 
    //    {
    //        return movementSpeed;
    //    }
    //    set
    //    {
    //        if (value < 0)
    //            movementSpeed = 0;
    //        else
    //            movementSpeed = value;
    //    }
    //}


    /// Code done by Arian- end



}
