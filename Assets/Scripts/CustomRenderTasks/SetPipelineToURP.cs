using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SetPipelineToURP : MonoBehaviour
{
    // Start is called before the first frame update
    private RenderPipelineAsset defaultRenderPipelineAsset;
    public RenderPipelineAsset URPPipelineAsset;
    void Awake()
    {
        this.defaultRenderPipelineAsset = GraphicsSettings.defaultRenderPipeline;
        GraphicsSettings.defaultRenderPipeline = this.URPPipelineAsset;
    }

    void OnDestroy() {
        Debug.Log("OnDestroy");
        GraphicsSettings.defaultRenderPipeline = this.defaultRenderPipelineAsset;
    }
}
