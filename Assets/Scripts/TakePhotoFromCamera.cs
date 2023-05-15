using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePhotoFromCamera : MonoBehaviour {
    public Camera _camera;
    private static TakePhotoFromCamera instance;
    private bool takeScreenShotOnNextFrame;

    private void Start() {
        instance = this;
        if (!this._camera) {
        this._camera = GetComponent<Camera>();
        }
    }
    private void OnPostRender() {
        if (this.takeScreenShotOnNextFrame) {
            this.takeScreenShotOnNextFrame = false;
            RenderTexture renderTexture = this._camera.targetTexture;
            RenderTexture renderResult = new RenderTexture(renderTexture.width, renderTexture.height, 24, RenderTextureFormat.ARGB32);
            this._camera.Render();
            RenderTexture.active = renderTexture;
            Debug.Log("is on post render");

            Texture2D virtualPhoto = new Texture2D(renderTexture.width, renderTexture.width, TextureFormat.RGB24, false);
            virtualPhoto.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.width), 0, 0);



            Debug.Log(virtualPhoto);
            RenderTexture.active = null; // "just in case" 

        
            this._camera.targetTexture = null;

            byte[] bytes;
            bytes = virtualPhoto.EncodeToPNG();

            System.IO.File.WriteAllBytes("/", bytes);


        }
    }
    public void TakeScreenShot(int width, int height) {
        this._camera.targetTexture = RenderTexture.GetTemporary(width, height, 24);
        this.takeScreenShotOnNextFrame = true;
        Debug.Log("sets bool to true");

    }

    public void CaptureCamera() {

        Debug.Log("presses button");

        TakeScreenShot(100,100);
    }




}

