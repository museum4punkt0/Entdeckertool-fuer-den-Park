using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour {
    // Grab the camera's view when this variable is true.
    private bool grabScreenshot;
    // Cache variable for our unlit shader
    private Shader unlitTexture;

    public UnityEngine.UI.Image snapshotImageContainer;
    public RawImage rawImage;

    [SerializeField]
    [Tooltip("Assign the camera that is taking the screenshot")]
    private CameraRenderEvent cam;
    

    // Start is called before the first frame update
    void Start() {
        if (cam == null) {
            // Not the most ideal search, Cameras should be tagged for search, or referenced.
            cam = GameObject.FindObjectOfType<CameraRenderEvent>();
        }
        if (cam != null) {
            //Subscribe to the Render event from the camera
            cam.OnPostRenderEvent += OnPostRender;
        }
        // cache a reference to the Unlit shader
        unlitTexture = Shader.Find("Unlit/Texture");
        rawImage.enabled = false;


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
        grabScreenshot = true;
    }
    private void OnPostRender() {

        if (grabScreenshot) {

            grabScreenshot = false;

            //Set the screen/image width and height parameters
            int screenShotWidth = Screen.width;
            int screenShotHeight = Screen.height;
            // store in image
            var screenShot = new Texture2D(screenShotWidth, screenShotHeight, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, screenShotWidth, screenShotHeight), 0, 0);
            screenShot.Apply();
            
            
           



            snapshotImageContainer.enabled = false;

            rawImage.enabled = true;
            rawImage.texture = screenShot;


            //Stop grabbing a screenshot
        }
    }
}