using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve_Effect_Manager : MonoBehaviour
{
    [SerializeField] private float curveStart;
    [SerializeField] private float curveShallowness;
    [SerializeField] private float curveMaxHeight;
    [SerializeField] private float curveLeftAmount;
    [SerializeField] private bool useWarp;
    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalFloat("_curveStart", curveStart);
        Shader.SetGlobalFloat("_curveShallowness", curveShallowness);
        Shader.SetGlobalFloat("_curveMaxHeight", curveMaxHeight);
        Shader.SetGlobalFloat("_curveMaxWidth", curveLeftAmount);

        float useWarpVal = useWarp ? 1.0f : 0.0f;
        Shader.SetGlobalFloat("_useWarp", useWarpVal);
    }

    // Update is called once per frame
    void Update()
    {
        
        }
}
