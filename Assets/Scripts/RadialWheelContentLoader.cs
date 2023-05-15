using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class RadialWheelContentLoader : MonoBehaviour
{
    public CrossGameManager crossGameManager;
    public Transform elements;
    public GameObject element1;
    public GameObject element2;
    public GameObject element3;
    public string currentItem;

    public List<Game4ToolContent> defaulttools = new List<Game4ToolContent>();

    public RadialWheel radialWheel;

    public Game4Data _data;

    public RadialWheelContentLoader() {
    }

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    void Start()
    {
        print("start RadialWheelContentLoader");

        _data = this.GetComponent<game4contentLoader>()._data;
        //StartCoroutine(crossGameManager.strapiService.getSpiel4Content(LoadContent));
    }



    //async void LoadContent(StrapiSingleResponse<Game4Data> res) {

         
    //    _data = res.data;
    //    print("loads content");


    //    foreach (Transform child in elements) {
    //        foreach (Game4DataContent item in _data.attributes.content) {
                
                
    //            if (item.headline == currentItem) {
    //                if (child.gameObject.active) {
    //                    foreach (Game4ToolContent tool in item.tool) {
    //                        print("name: " + elements.GetChild(1).GetChild(0).GetChild(0).name);
    //                        //elements.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Sprite.Create((Texture2D)(await item.tool[0].image.GetTexture2D()), new Rect(0, 0, 145, 145), new Vector2(0.5f, 0.5f));
    //                        ////elements.GetChild(1).GetChild(0).GetChild(0).name = item.tool[0].headline;
    //                        //element1.name = item.tool[0].headline;

    //                        //elements.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Sprite.Create((Texture2D)(await item.tool[1].image.GetTexture2D()), new Rect(0, 0, 145, 145), new Vector2(0.5f, 0.5f));
    //                        ////elements.GetChild(2).GetChild(0).GetChild(0).name = item.tool[1].headline;
    //                        //element2.name = item.tool[1].headline;

    //                        //elements.GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Sprite.Create((Texture2D)(await item.tool[2].image.GetTexture2D()), new Rect(0, 0, 145, 145), new Vector2(0.5f, 0.5f));
    //                        ////elements.GetChild(3).GetChild(0).GetChild(0).name = item.tool[2].headline;
    //                        //element3.name = item.tool[2].headline;

    //                    }

    //                }
    //            }
    //        }
    //    }

    //}

    public void Navigate() {
        Game4DataContent content = _data.attributes.content.Find(item => item.headline == currentItem);


        print("has spec content" + content.headline + "with tools: " + content.tool.Count);
        List<Game4ToolContent> tools = content.tool;

        if (tools.Count > 0) {
            radialWheel.ChangeCustomMenu(tools);
        } else {
            radialWheel.ChangeCustomMenu(this.defaulttools);
        }
        

        //StartCoroutine(crossGameManager.strapiService.getSpiel4Content(LoadContent));
    }

}
