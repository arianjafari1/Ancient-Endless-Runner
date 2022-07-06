using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameOver GameOver;


    // Start is called before the first frame update
    void Start()
    {
        GameOver.ShowGameOverScreen(23);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
