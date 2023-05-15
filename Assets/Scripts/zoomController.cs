using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleZoom;

public class zoomController : MonoBehaviour
{
    public CrossGameManager crossGameManager;
    public UIItemViewController uIItemViewController;
    Item item;
    public int itemId;
    public GameObject contentIMG;
    public Sprite convertedSprite;
    int imgWidth;
    int imgHeight;
    int newImgWidth;
    int newImgHeight;
    int newScreenWidth = 800;
    public Sprite loadingWheel;
    //public SimpleZoom zoom;


    public zoomController() {
    }

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        LoadImage_static();
    }

    public void LoadImage_static() {
        StartCoroutine(crossGameManager.strapiService.getItems(LoadImage));
    }

    async void LoadImage(StrapiItemResponse res) {

        this.itemId = crossGameManager.fundObjektIDtoImageZoom;
        this.item = res.data[this.itemId];

        foreach (Item theOne in res.data) {
            if (theOne.id == crossGameManager.fundObjektIDtoImageZoom) {
                this.item = theOne;
            }
        }

        convertedSprite = await this.item.attributes.sliderItems[uIItemViewController.currentImageonSlider].media.ToSprite();
        contentIMG.GetComponent<Image>().sprite = convertedSprite;


        imgWidth = this.item.attributes.sliderItems[uIItemViewController.currentImageonSlider].media.data.attributes.width;
        imgHeight = this.item.attributes.sliderItems[uIItemViewController.currentImageonSlider].media.data.attributes.height;
        newImgHeight = (imgHeight * newScreenWidth) / imgWidth;

        contentIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(newScreenWidth, newImgHeight);

    }

    public void Reset() {
        
        contentIMG.GetComponent<Image>().sprite = loadingWheel;
 
    }
}
