using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Services;
using UnityEngine.SceneManagement;

public class DetectorFundPopUp : MonoBehaviour
{
    public VisualElement popUpPanel;
    public VisualElement m_Root;
    public Button closePopUp;
    public Button weitereBtn;
    public Button mehrUberBtn;

    public UIItemViewController uIItemViewControllerScript;
    public FindToolController findToolControllerScript;
    public CrossGameManager crossGameManagerScript;
    public FullEllipseAnimation fullEllipseAnimationScript;
    public AnimationControllerFundDetektor animationControllerFundDetektor;

    public CrossGameManager crossGameManager;


    public DetectorFundPopUp() {
    }

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    // Start is called before the first frame update
    async void Start()
    {
        m_Root = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<UIDocument>().rootVisualElement;
        popUpPanel = m_Root.Q<VisualElement>("FundPopUp");
        closePopUp = popUpPanel.Q<VisualElement>("Panel").Q<VisualElement>("header").Q<Button>("closeBtn");
        weitereBtn = popUpPanel.Q<Button>("weitereBtn");
        mehrUberBtn = popUpPanel.Q<Button>("mehrUberBtn");

        uIItemViewControllerScript = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<UIItemViewController>();
        findToolControllerScript = gameObject.GetComponent<FindToolController>();
        crossGameManagerScript = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        animationControllerFundDetektor = gameObject.GetComponent<AnimationControllerFundDetektor>();

        ButtonClickManager();
    }

    public void ButtonClickManager() {
        closePopUp.clicked += delegate {
            print("inside close pop up");
            popUpPanel.style.display = DisplayStyle.None;
            animationControllerFundDetektor.RestartFunddetektor();
            findToolControllerScript.triggerAnimationPart2 = false;
            findToolControllerScript.triggerAnimationPart1 = false;
            findToolControllerScript.enabled = true;
        };

        weitereBtn.clicked += delegate {
            print("inside weitere btn");
            //restart
            popUpPanel.style.display = DisplayStyle.None;
            animationControllerFundDetektor.RestartFunddetektor();
            findToolControllerScript.triggerAnimationPart2 = false;
            findToolControllerScript.triggerAnimationPart1 = false;
            findToolControllerScript.enabled = true;
        };

        mehrUberBtn.clicked += delegate {
            uIItemViewControllerScript.navigate("item", findToolControllerScript.itemOnMapCurrentlyClosestToPlayer.Poi.attributes.fundobjekt.data.id.ToString());
            findToolControllerScript.itemOnMapCurrentlyClosestToPlayer.HasBeenVisited = true;

            popUpPanel.style.display = DisplayStyle.None;
            crossGameManagerScript.IsVisitingFromDetector = true;

            animationControllerFundDetektor.RestartFunddetektor();
            findToolControllerScript.triggerAnimationPart2 = false;
            findToolControllerScript.triggerAnimationPart1 = false;

            if (SceneManager.GetActiveScene().name != "MainScene") {
                crossGameManagerScript.AddToScore(crossGameManagerScript.colorFromHex("#1CB3FF"), 12);
            }
        };
    }

    public void Show() {
        popUpPanel.style.display = DisplayStyle.Flex;
        popUpPanel.Q<Label>("text").text = findToolControllerScript.itemOnMapCurrentlyClosestToPlayer.Poi.attributes.fundobjekt.data.attributes.headline.ToString();
        findToolControllerScript.enabled = false;
    }
}
