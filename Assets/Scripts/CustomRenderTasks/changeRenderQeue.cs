using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeRenderQeue : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] hiddenMeshes;

    void Start()
    {
        foreach (GameObject hiddenMesh in this.hiddenMeshes) {
            hiddenMesh.GetComponent<Renderer>().material.renderQueue = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
