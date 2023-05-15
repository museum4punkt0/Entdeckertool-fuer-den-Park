using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using UnityEngine.UIElements;


public class introSceneContentLoader : MonoBehaviour
{
    public CrossGameManager crossGameManager;
    public string locationPermission_text;
    public string locationPermission_allowBtn;
    public string locationPermission_denyBtn;

    public string cameraPermission_text;
    public string cameraPermission_allowBtn;
    public string cameraPermission_denyBtn;

    public string locationError;
    public string cameraError;
    StrapiService strapiService;


    private void Awake() {

        //crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    public void LoadContent_static() {
        this.strapiService = new StrapiService(Application.persistentDataPath + "/cachedRequests");
        //this.strapiService = new StrapiService(Application.persistentDataPath + "");

        //crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        //StartCoroutine(crossGameManager.strapiService.getIntroScenePageContent(LoadContent));
        StartCoroutine(this.strapiService.getIntroScenePageContent(LoadContent));
    }

    async void LoadContent(StrapiSingleResponse<IntroScenePageData> res) {
        IntroScenePageData _data = res.data;

        locationPermission_text = _data.attributes.locassionPermission.bodyText;
        locationPermission_allowBtn = _data.attributes.locassionPermission.allowButton;
        locationPermission_denyBtn = _data.attributes.locassionPermission.denyButton;

        cameraPermission_text = _data.attributes.cameraPermission.bodyText;
        cameraPermission_allowBtn = _data.attributes.cameraPermission.allowButton;
        cameraPermission_denyBtn = _data.attributes.cameraPermission.denyButton;

        locationError = _data.attributes.locationError;
        cameraError = _data.attributes.cameraError;

        this.gameObject.GetComponent<introSceneButtons>().Populate();
    }
}
