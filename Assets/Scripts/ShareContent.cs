using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Services;
using UnityEditor;
using UnityEngine.UIElements;

public class ShareContent : MonoBehaviour
{
    byte[] bytes;
    RenderTexture rt;
    GameObject imgTexture;
    Texture2D screenShot;
    UIItemViewController uIItemViewController;
    CrossGameManager crossGameManager;
    Item item;
    public int itemId;
    public int contentWidth;
    public int contentHeight;
    Texture2D convertedTexture;

    public ShareContent() {
    }

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    void Start()
    {
        uIItemViewController = this.gameObject.GetComponent<UIItemViewController>();
    }

    async void LoadContent(StrapiItemResponse res) {
        this.itemId = crossGameManager.fundObjektIDtoImageZoom;
        this.item = res.data[this.itemId];

        foreach (Item theOne in res.data) {
            if (theOne.id == crossGameManager.fundObjektIDtoImageZoom) {
                this.item = theOne;
            }
        }
        convertedTexture = await this.item.attributes.sliderItems[uIItemViewController.currentImageonSlider].media.GetTexture2D();

        contentWidth = this.item.attributes.sliderItems[uIItemViewController.currentImageonSlider].media.data.attributes.width;
        contentHeight = this.item.attributes.sliderItems[uIItemViewController.currentImageonSlider].media.data.attributes.height;
        print("ITEM: " + this.itemId + " current: " + uIItemViewController.currentImageonSlider);
    }

    public void ShareContent_static() {
        StartCoroutine(crossGameManager.strapiService.getItems(LoadContent));
        GetContent();
        Share();
    }

    public void GetContent() {

        
        //rt = new RenderTexture(contentWidth, contentHeight, 24);
        /*
        convertedTexture.GetPixels();

        screenShot = new Texture2D(contentWidth, contentHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, contentWidth, contentHeight);
        convertedTexture.ReadPixels(rec,0,0);
        convertedTexture.Apply();*/
        bytes = convertedTexture.EncodeToPNG();
        print("share get content");

    }

    private void Share() {
        string filePath = Path.Combine(Application.temporaryCachePath, "shared_img.png");

        File.WriteAllBytes(filePath, bytes);

        // To avoid memory leaks
        //Destroy(screenShot);
        new NativeShare().AddFile(filePath)
        .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
        .Share();

        //RenderTexture.ReleaseTemporary(rt);

        //screenShot = null;
        convertedTexture = null;
        print("share succeded");
    }
}
