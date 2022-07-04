using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementRenderer : MonoBehaviour
{

    [SerializeField] private Material newMaterial;

    private void FixedUpdate()
    {
        GameObject[] findMaterials = GameObject.FindGameObjectsWithTag("TileGround"); //find all of the objects with this tag within the scene and store it in this array


        foreach (GameObject findMaterial in findMaterials) //then loop through the array and execute this
        {
            if (findMaterial.GetComponent<Renderer>().material != newMaterial) // if the material with the given tag doesn't match this then execute code below
            {
                findMaterial.GetComponent<Renderer>().material = newMaterial; // change the material to this
            }
        }
    }

}
