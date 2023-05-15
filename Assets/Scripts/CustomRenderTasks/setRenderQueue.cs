using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setRenderQueue : MonoBehaviour
{
    public List<Renderer> renderers;
    public int queeNumber;
    void Start()
    {
        foreach (Renderer renderer in renderers) {
            renderer.material.renderQueue = queeNumber;
            renderer.materials[0].renderQueue = queeNumber;
        }
    }

    private void Update() {
        foreach (Renderer renderer in renderers) {
            renderer.material.renderQueue = queeNumber;
            renderer.materials[0].renderQueue = queeNumber;
        }
    }

}
