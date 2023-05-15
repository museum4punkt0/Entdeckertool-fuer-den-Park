using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Services;
using UIBuilder;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class detektorUIController : MonoBehaviour
{
    VisualElement m_Root;
    Button introStartBtn;
    Button introCloseBtn;
    Button infoPanelCloseBtn;
    Button aboutFund;
    Button continueSearchFund;
    Button infoPopUpCloseBtn;
    Button infoPopUpAboutFundBtn;
    Button infoPopUpContinueBtn;
    VisualElement introPopup;
    VisualElement infoPanel;
    VisualElement infoPopUp;
    public UIItemViewController uIItemViewControllerScript;
    public FindToolController findToolControllerScript;
    //public UIDocument mainMenu;
    public CrossGameManager crossGameManagerScript;
    public TourPopUp tourPopUpScript;
    //GameObject crossGameManagerScript;

    void Start()
    {
        crossGameManagerScript = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        uIItemViewControllerScript = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<UIItemViewController>();
        findToolControllerScript = GameObject.FindGameObjectWithTag("FindTool").GetComponent<FindToolController>();

        this.m_Root = GetComponent<UIDocument>().rootVisualElement;

        introStartBtn = m_Root.Q<Button>("intro-start-btn");
        introCloseBtn = m_Root.Q<Button>("intro-close-btn");
        introPopup = m_Root.Q<VisualElement>("intro-popup");
        infoPanel = m_Root.Q<VisualElement>("info-panel");
        infoPanelCloseBtn = m_Root.Q<Button>("infoPanel-close-btn");
        aboutFund = m_Root.Q<Button>("infoPanel-aboutFund-btn");
        continueSearchFund = m_Root.Q<Button>("infoPanel-suchen-btn");
        infoPopUp = m_Root.Q<VisualElement>("info-popup");
        infoPopUpCloseBtn = m_Root.Q<Button>("infoPopUp-close-btn");
        infoPopUpAboutFundBtn = m_Root.Q<Button>("infoPopUp-about-btn");
        infoPopUpContinueBtn = m_Root.Q<Button>("infoPopUp-continue-btn");

        ButtonClick();

        uIItemViewControllerScript.isOpen = true;

    }

    void ButtonClick() {
        introStartBtn.clicked += delegate {
            
            CloseVisualElement(introPopup);
        };

        introCloseBtn.clicked += delegate {
            CloseVisualElement(introPopup);
        };

        infoPanelCloseBtn.clicked += delegate {
            CloseVisualElement(infoPanel);
            SceneManager.LoadScene("FindTool");
        };

        aboutFund.clicked += delegate {
            CloseVisualElement(infoPanel);

            uIItemViewControllerScript.navigate("item", findToolControllerScript.itemOnMapCurrentlyClosestToPlayer.ID.ToString());

            this.gameObject.GetComponent<UIDocument>().sortingOrder = -1;

            if (SceneManager.GetActiveScene().name != "MainScene") {
                crossGameManagerScript.AddToScore(crossGameManagerScript.colorFromHex("#1CB3FF"), 12);
            }
        };

        continueSearchFund.clicked += delegate {
            CloseVisualElement(infoPanel);
           // SceneManager.LoadScene("FindTool");
            //Application.LoadLevel("FindTool");
        };

        infoPopUpCloseBtn.clicked += delegate {
            CloseVisualElement(infoPopUp);
            this.gameObject.GetComponent<UIDocument>().sortingOrder = -1;
        };

        infoPopUpAboutFundBtn.clicked += delegate {
            CloseVisualElement(infoPopUp);
            this.gameObject.GetComponent<UIDocument>().sortingOrder = -1;
        };

        infoPopUpContinueBtn.clicked += delegate {
            CloseVisualElement(infoPanel);
            //Application.LoadLevel("FindTool");
           // SceneManager.UnloadScene("FindTool");
           // SceneManager.LoadScene("FindTool");
        };

    }

    public void CloseVisualElement(VisualElement element) {
        this.gameObject.GetComponent<UIDocument>().sortingOrder = -1;
        element.style.display = DisplayStyle.None;
    }

    public void DisplayPopUp() {
        this.gameObject.GetComponent<UIDocument>().sortingOrder = 1;
        infoPopUp.style.display = DisplayStyle.Flex;
    }
}
