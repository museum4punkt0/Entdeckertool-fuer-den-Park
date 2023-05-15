using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenCapture : MonoBehaviour
{
    private CrossGameManager crossGameManager;
    public Camera _camera;
    public RenderTexture renderTexture;
    public Material ScreenShotMaterial;

    public UnityEngine.UI.Image snapshotImageContainer;
    //private void Start() {

    //    if (!this._camera.targetTexture) {
    //        this._camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
    //    }
    //    this.renderTexture.width = Screen.width;
    //    this.renderTexture.height = Screen.height;
    //}

    //public void TakeScreenshot() {
    //    // Force a render to the target texture.
    //    this._camera.targetTexture = this.renderTexture;
    //    this._camera.Render();

    //    // Texture.ReadPixels reads from whatever texture is active. Ours needs to
    //    // be active. But let's remember the old one so we can restore it later.
    //    RenderTexture oldRenderTexture = RenderTexture.active;
    //    RenderTexture.active = this.renderTexture;

    //    // Grab ALL of the pixels.
    //    Texture2D raster = new Texture2D(this.renderTexture.width, this.renderTexture.height);
    //    raster.ReadPixels(new Rect(0, 0, this.renderTexture.width, this.renderTexture.height), 0, 0);
    //    raster.Apply();


    //    byte[] bytes = raster.EncodeToPNG();
    //    // Write them to disk. Change the path and type as you see fit.
    //    File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "output.png"), bytes);

    //    Debug.Log(Path.Combine(Application.persistentDataPath));

    //    // Restore previous settings.
    //    this._camera.targetTexture = null;
    //    RenderTexture.active = oldRenderTexture;

    //    Debug.Log("Screenshot saved.");
    //}


    //public Texture2D screenshot() {

    //    this._camera.targetTexture = this.renderTexture;
    //    this._camera.Render();

    //    // Texture.ReadPixels reads from whatever texture is active. Ours needs to
    //    // be active. But let's remember the old one so we can restore it later.
    //    RenderTexture oldRenderTexture = RenderTexture.active;
    //    RenderTexture.active = this.renderTexture;

    //    // Grab ALL of the pixels.
    //    Texture2D raster = new Texture2D(this.renderTexture.width, this.renderTexture.height);
    //    raster.ReadPixels(new Rect(0, 0, this.renderTexture.width, this.renderTexture.height), 0, 0);
    //    raster.Apply();


    //    //byte[] bytes = raster.EncodeToPNG();
    //    //// Write them to disk. Change the path and type as you see fit.
    //    //File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "output.png"), bytes);

    //    //Debug.Log(Path.Combine(Application.persistentDataPath));

    //    //// Restore previous settings.
    //    //this._camera.targetTexture = null;
    //    //RenderTexture.active = oldRenderTexture;

    //    return raster;
    //}

    private bool grabScreenshot;
    [SerializeField]
    [Tooltip("Assign the camera that is taking the screenshot")]
    CameraRenderEvent cam;
    // Start is called before the first frame update

    void Start() {

        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        if (_camera == null) {
            _camera = Camera.current;
        }


        if (cam == null) {
            // Not the most ideal search, Cameras should be tagged for search, or referenced.
            cam = GameObject.FindObjectOfType<CameraRenderEvent>();
        }
        if (cam != null) {
            //Subscribe to the Render event from the camera
            cam.OnPostRenderEvent += OnPostRender;
        }

  
    }

    void OnEnable() {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }
    void OnDisable() {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }
    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera) {
        OnPostRender();
    }
    public void TakeScreenshot() {
        crossGameManager.ErrorLog("take screenshot");

        grabScreenshot = true;
    }

    private void OnPostRender() {
        if (grabScreenshot) {
            grabScreenshot = false;

            //Set the screen/image width and height parameters
            int screenShotWidth = Screen.width;
            int screenShotHeight = Screen.height;

            RenderTexture rt = new RenderTexture(screenShotWidth, screenShotHeight, 24);
            _camera.targetTexture = rt;
            // store in image
            Texture2D screenShot = new Texture2D(screenShotWidth, screenShotHeight, TextureFormat.RGB24, false);

            _camera.Render();
            RenderTexture.active = rt;

            screenShot.ReadPixels(new Rect(0, 0, screenShotWidth, screenShotHeight), 0, 0, true);
            screenShot.Apply();



            crossGameManager.ErrorLog("grabs shot" + screenShot + screenShotWidth + screenShotHeight);


            snapshotImageContainer.material.SetTexture("_MainTex", screenShot);

            var spawnedObject = GameObject.CreatePrimitive(PrimitiveType.Quad);

            // Set a position Forward for the camera view
            Vector3 pos = cam.transform.position + cam.transform.forward;
            spawnedObject.transform.position = pos;
            // Apply the grabbed screenshot texture to the Quad's material

            if (!spawnedObject.GetComponent<MeshRenderer>()) {
                spawnedObject.AddComponent<MeshRenderer>();
            }


            MeshRenderer renderer = spawnedObject.GetComponent<MeshRenderer>();
            renderer.material = ScreenShotMaterial;
            renderer.material.SetTexture("_MainTex", screenShot);
            renderer.material.mainTexture = screenShot;


            _camera.targetTexture = null;
            RenderTexture.active = null;
            
            
            
            
            
            Destroy(rt);

         
            }
    }
}
