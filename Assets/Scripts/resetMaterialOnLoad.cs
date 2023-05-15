using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetMaterialOnLoad : MonoBehaviour
{
    public float startValue = -1;
    public string varName = "Effect_Start";
    void Start()
    {
        Material mat = GetComponent<Renderer>().sharedMaterial;

        mat.SetFloat(varName, startValue);
    }

 
}
