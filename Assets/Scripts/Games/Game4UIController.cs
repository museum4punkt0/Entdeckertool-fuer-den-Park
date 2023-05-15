using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using System.Threading.Tasks;
using DG.Tweening;
using Services;
using UIBuilder;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Game4UIController : MonoBehaviour
{
    public VisualElement m_Root;
    public VisualElement m_Game;
    public VisualElement m_Tutorial;
    public GameObject gameUI;
    public RectTransform imgBeforeContent;
    public RectTransform imgAfterContent;
    public GameObject dragContainer;
    public Game4Controller gameController;
    public RadialWheel radialWheelMenu;
    UnityEngine.UIElements.Button material01_Row01_vorher;
    UnityEngine.UIElements.Button material01_Row01_nachher;
    bool objectIsCleaned = false;
    List<string> itemsObj = new List<string>();
    int totalCoins = 0;
    CrossGameManager crossGameManager;

    public Game4UIController() {
    }

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

        this.m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_Game = this.m_Root.Q<VisualElement>("Game");
        m_Tutorial = this.m_Root.Q<VisualElement>("Tutorial");

        StartGame();
           
    }

    public void StartGame() {
        Debug.Log("starts game");
        UnityEngine.UIElements.Button startButton;
        startButton = m_Tutorial.Q<UnityEngine.UIElements.Button>("startBtn");

        startButton.clicked += delegate {
            m_Tutorial.style.display = DisplayStyle.None;
            m_Game.style.display = DisplayStyle.Flex;
            //StartCoroutine(ShowPopUp());
        };
    }

    public void CardClick() {
        //delete
        
        material01_Row01_vorher = m_Game.Q<VisualElement>("materialWrapper01").Q<VisualElement>("materiaRow01").Q<UnityEngine.UIElements.Button>("vorher");
        
        material01_Row01_vorher.clicked += delegate {
            if (!objectIsCleaned) {
                m_Game.Q("PopUpStart").style.display = DisplayStyle.Flex;
                objectIsCleaned = true;
            } else
                return;
        };
    }

    IEnumerator ShowPopUp() {
        yield return new WaitForSeconds(5);
        m_Game.Q("PopUpStart").style.display = DisplayStyle.Flex;
    }

    void ClosePopUp() {
        
        m_Game.Q("PopUpStart").Q<UnityEngine.UIElements.Button>("popup-close").clicked += delegate {
            m_Game.Q("PopUpStart").style.display = DisplayStyle.None;
           // objectIsCleaned = false;
        };

        m_Game.Q("PopUpStart").Q<UnityEngine.UIElements.Button>("popup-button").clicked += delegate {
            m_Game.Q("PopUpStart").style.display = DisplayStyle.None;
            m_Game.style.display = DisplayStyle.None;
            gameUI.SetActive(true);
        };
    }

    public void StartGameFunc(Texture2D imgBefore, Texture2D imgAfter) {
      //  gameController.enabled = true;
        
        
        var tempColor = imgBeforeContent.GetComponent<UnityEngine.UI.Image>().color;
        tempColor.a = 1f;
        imgBeforeContent.GetComponent<UnityEngine.UI.Image>().color = tempColor;

        imgBeforeContent.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(imgBefore, new Rect(0, 0, imgBefore.width, imgBefore.height), new Vector2(0.5f, 0.5f));
        imgAfterContent.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(imgAfter, new Rect(0, 0, imgAfter.width, imgAfter.height), new Vector2(0.5f, 0.5f));

        ClosePopUp();
        m_Game.style.display = DisplayStyle.None;
        gameUI.SetActive(true);
       
        dragContainer.SetActive(true);
        
        
    }

    IEnumerator RestartRadialWheel() {
        yield return new WaitForSeconds(1);
        radialWheelMenu.StartRadialMenu();
    }

    public void AfterObjectIsCleaned(int coinAmount) {
        totalCoins += coinAmount;
        crossGameManager.Game4CurrentScore += coinAmount;

        StartCoroutine(DisplayGameMenu());

        Label coins;
        coins = m_Game.Q<Label>("coins-label");
        coins.text = crossGameManager.Game4CurrentScore.ToString();

        this.gameObject.GetComponent<game4contentLoader>().CheckObjs(totalCoins);

    }

    IEnumerator DisplayGameMenu() {
        yield return new WaitForSeconds(5);

        gameUI.SetActive(false);
        gameController.isObjectCleaned = false;
        //gameController.enabled = false;


        dragContainer.SetActive(false);
        m_Game.style.display = DisplayStyle.Flex;
    }

}
