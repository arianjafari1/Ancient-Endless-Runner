using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDestroyer : MonoBehaviour
{
    /// Code done by Arian- start
    private void OnTriggerEnter(Collider collisionInfo) //check for collision info
    {


        if (collisionInfo.gameObject.CompareTag("Tile")) //if collision info comapre with the tile death point tag, then execute the code undeneath
        {
            Destroy(collisionInfo.gameObject); //destroy the tile
            Debug.Log("Collision has happened");

        }

    }

    /// Code done by Arian- end
}
