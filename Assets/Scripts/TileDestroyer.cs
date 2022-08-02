using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDestroyer : MonoBehaviour
{
    /// Code done by Arian- start
    private void OnTriggerEnter(Collider collisionInfo) //check for collision info
    {
        /// <summary>
        /// 15/06/2022 -[Arian] created TileDestroyer script and set it a schild of the tileDeath object
        ///            -[Arian] added collision checker to check if the Tiles collide with the object, then destroy them
        /// 01/08/2022 -[Arian] added collision checker to check if the Tiles collide with the object, then it sets them to inactive for the object pooling
        /// 02/08/2022 -[Arian] reversed changes, as it is explained in the documentation of TileMovement and TileManager scripts, but I will leave them as comments
        /// </summary>

        if (collisionInfo.gameObject.CompareTag("Tile")) //if collision info comapre with the tile death point tag, then execute the code undeneath
        {
            Destroy(collisionInfo.gameObject); //destroy the tile
            //Debug.Log("Tile at the back has been destroyed.");

        }

        /// Commented out Object Pooling Starts
        //if (collisionInfo.gameObject.CompareTag("TileMiddle")) //if collision info comapre with the tile death point tag, then execute the code undeneath
        //{
        //    /*Destroy(collisionInfo.gameObject);*/ //destroy the tile
        //    //Debug.Log("Tile at the back has been destroyed.");

        //    collisionInfo.gameObject.SetActive(false);
        //}
        /// Commented out Object Pooling Ends

    }

    /// Code done by Arian- end
}
