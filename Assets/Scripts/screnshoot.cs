using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
//using UnityEngine.UIElements;


public class screnshoot : MonoBehaviour
{
    private static screnshoot instance;

    private Camera myCamera;
    private bool takeScreenshotOnNextFrame;
    public GameObject userInterface;
    public GameObject popUp;
    byte[] bytes;
    string screenshotImg;
    RenderTexture rt;
    GameObject imgTexture;
    Texture2D screenShot;

    private void Awake() {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    IEnumerator ScreenshotTaker() {

        yield return new WaitForEndOfFrame();


        if (takeScreenshotOnNextFrame) {

            takeScreenshotOnNextFrame = false;

            rt = new RenderTexture(Screen.width, Screen.height, 24);
            myCamera.targetTexture = rt;
            screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
            myCamera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenShot.Apply();

            bytes = screenShot.EncodeToPNG();

            ShowScreenshot(screenShot);
        }
    }


    private void ShowScreenshot(Texture2D imageTexture) {
        imgTexture.SetActive(true);
        imgTexture.GetComponent<Image>().sprite = Sprite.Create(imageTexture, new Rect(0, 0, imageTexture.width, imageTexture.height), new Vector2(0.5f, 0.5f));

        imgTexture.gameObject.transform.parent.GetComponent<ARFilterSceneContentLoader>().PostScreenshot();
    }

    private void SaveScreenshot() {
        NativeGallery.SaveImageToGallery(bytes, "GalleryTest", "Image.png", (success, path) => Debug.Log("Media save result: " + success + " " + path));

        RenderTexture.ReleaseTemporary(rt);

        screenShot = null;
        myCamera.targetTexture = null;
    }

    private void ScreenshotShare() {
    
        string filePath = Path.Combine(Application.temporaryCachePath, "shared_img.png");

        File.WriteAllBytes(filePath, bytes);

        // To avoid memory leaks
        Destroy(screenShot);
        new NativeShare().AddFile(filePath)
        .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
        .Share();

        RenderTexture.ReleaseTemporary(rt);

        screenShot = null;
        myCamera.targetTexture = null;
        print("share succeded");
    }

    public void TrashScreenshot() {
        RenderTexture.ReleaseTemporary(rt);
        screenShot = null;
        myCamera.targetTexture = null;
    }

    private void ShareScreenshot(){
        ScreenshotShare();
    }

    private void TakeScreenshot(int width, int height) {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
        StartCoroutine(ScreenshotTaker());
    }

    public static void TakeScreenshot_static(int width, int height, GameObject imgTxt) {
        instance.imgTexture = imgTxt;
        instance.TakeScreenshot(width, height);
    }

    public static void ShareScreenshot_static() {
        instance.ShareScreenshot();
    }

    public static void SaveScreenshot_static() {
        instance.SaveScreenshot();
    }

    public static void TrashScreenshot_static() {
        instance.TrashScreenshot();
    }

}
