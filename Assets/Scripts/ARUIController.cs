using System;
using System.Collections;
using System.Collections.Generic;
using ARLocation;
using Services;
using UIBuilder;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ARUIController : MonoBehaviour
{
    CrossGameManager crossGameManager;
    public PanoramaSceneManager panoramaSceneManager;
    public VisualElement m_Root;
    public Button closePopUpBTN;
    public Button closeSceneBTN;
    public Button ctaButton;
    public Label headLine;
    public Label subHeadline;
    string stopARheadline;
    string stopARsubheadline;
    string stopARbutton;


    [SerializeField] 
    public ARLocationProvider _arLocationProvider;

    public ARUIController() {
    }

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.m_Root = this.GetComponent<UIDocument>().rootVisualElement;
        closePopUpBTN = this.m_Root.Q<Button>("closePopUp");
        closeSceneBTN = this.m_Root.Q<Button>("closeSceneBtn");
        ctaButton = this.m_Root.Q<Button>("ctaButton");
        headLine = this.m_Root.Q<Label>("headline");
        subHeadline = this.m_Root.Q<Label>("subheadline");

        StartCoroutine(crossGameManager.strapiService.getARPageContent(LoadContent));

        if (this._arLocationProvider.CurrentHeading.accuracy < crossGameManager.AllowedAccuracyMargin) {
            StopARMode();
        } else {
            beginARMode();
   
        }
    }

    async void LoadContent(StrapiSingleResponse<ARPageData> res) {
        ARPageData _data = res.data;

        headLine.text = _data.attributes.firstPopUp.headline;
        subHeadline.text = _data.attributes.firstPopUp.subHeadline;
        ctaButton.text = _data.attributes.firstPopUp.buttonText;

        stopARheadline = _data.attributes.stopARPopUp.headline;
        stopARsubheadline = _data.attributes.stopARPopUp.subHeadline;
        stopARbutton = _data.attributes.stopARPopUp.buttonText;
    }

    void beginARMode() {
        ctaButton.clicked += delegate {
            this.m_Root.Q<VisualElement>("PopUp").style.display = DisplayStyle.None;
            Debug.Log("pushes button");
        };

        closePopUpBTN.clicked += delegate {
            this.m_Root.Q<VisualElement>("PopUp").style.display = DisplayStyle.None;
            Debug.Log("pushes other button");

        };
    }
    void StopARMode()
    {

        headLine.text = "Entschuldigung, Ihr Ger?t ist mit dieser Funktion nicht kompatibel";
        //headLine.text = stopARheadline;
        subHeadline.text = "";
        //subHeadline.text = stopARsubheadline;
        ctaButton.text = "zur?ck gehen";
        //ctaButton.text = stopARbutton;

        ctaButton.clicked += delegate {
            panoramaSceneManager.StopAR();
        };

        closePopUpBTN.clicked += delegate {
            panoramaSceneManager.StopAR();


        };
    }
}
