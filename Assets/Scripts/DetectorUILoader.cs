using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using UnityEngine.UIElements;
using UnityEngine;
using TMPro;

public class DetectorUILoader : MonoBehaviour
{
    public CrossGameManager crossGameManager;
    public GameObject UIdoc;
    public TextMeshProUGUI headline;
    VisualElement popUp;
    Label headlinePopUp;
    Label subheadline;
    Button start;
    Button close;

    public DetectorUILoader() {
    }

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    void Start()
    {
        popUp = UIdoc.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("intro-popup");
        headlinePopUp = UIdoc.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("intro-popup").Q<Label>("headline");
        subheadline = UIdoc.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("intro-popup").Q<Label>("subheadline");
        close = UIdoc.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("intro-popup").Q<Button>("intro-close-btn");
        start = UIdoc.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("intro-popup").Q<Button>("intro-start-btn");

        close.clicked += delegate {
            popUp.style.display = DisplayStyle.None;
            UIdoc.SetActive(false);
        };

        start.clicked += delegate {
            popUp.style.display = DisplayStyle.None;
            UIdoc.SetActive(false);
        };

        StartCoroutine(crossGameManager.strapiService.getFunddetektorPageContent(LoadContent));
    }

    async void LoadContent(StrapiSingleResponse<FunddetektorPageData> res) {
        FunddetektorPageData _data = res.data;

        headlinePopUp.text = _data.attributes.firstPopUp.headline;
        subheadline.text = _data.attributes.firstPopUp.subHeadline;
        start.text = _data.attributes.firstPopUp.buttonText;

        headline.text = _data.attributes.headline;
    }

}
