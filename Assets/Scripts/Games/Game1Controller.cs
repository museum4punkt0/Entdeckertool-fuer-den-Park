
using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.UIElements;
using UIBuilder;
using UnityEngine.SceneManagement;


public class Game1Controller : MonoBehaviour
{
    public VisualElement m_Root;
    public VisualElement Game;
    public VisualElement Level1;
    public VisualElement Level2;
    public VisualElement Level3;
    public VisualElement nextPhase2;
    public VisualElement nextPhase3; 
    public VisualElement win;
    public VisualElement notifications;
    public VisualElement notificationCorrect;
    public VisualElement notificationFalse;
    public Button notificationCorrectCloseBtn;
    public Button notificationFalseCloseBtn;
    public VisualElement fundeImages;
    public VisualElement nextPhase2Images;
    public VisualElement nextPhase3Images;
    public VisualElement winImages;
    public Button goToLevel2Btn;
    public Button goToLevel3Btn;
    public VisualElement closeGamePanel;
    public Button continuePlaying;
    public Button saveGame;
    public Button closeSavePanel;
    public Button restartGame;

    public Button correctBtn;
    public Button falseBtn;

    CrossGameManager crossGameManager;
    public VisualElement gameHeader;
    public Button closeGameHeader;

    int coinCount = 0;
    bool roemisch = false;
    bool uiStroke = false;
    string htmlCorrectValue = "#109F94";
    string htmlFalseValue = "#E86C5C";
    string htmlemptyValue = "#929292";
    Color borderColor;
    List<string> strokeValues = new List<string>();
    string currentPhase;

    bool checkLoadedContent = false;

    public Game1Controller() {
    }

    private void Awake() {
        this.crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    async void Start() {
        #region DefineVariables
        this.m_Root = GetComponent<UIDocument>().rootVisualElement;
        this.Game = this.m_Root.Q<VisualElement>("Game");
        this.Level1 = this.m_Root.Q<VisualElement>("Level1");
        this.Level2 = this.m_Root.Q<VisualElement>("Level2");
        this.Level3 = this.m_Root.Q<VisualElement>("Level3");
        this.correctBtn = this.m_Root.Q<Button>("CorrectButton");
        this.falseBtn = this.m_Root.Q<Button>("FalseButton");
        this.nextPhase2 = this.m_Root.Q<VisualElement>("NextPhase2");
        this.nextPhase3 = this.m_Root.Q<VisualElement>("NextPhase3");
        this.win = this.m_Root.Q<VisualElement>("Win");
        this.notifications = this.m_Root.Q<VisualElement>("Notifications");
        this.notificationCorrect = this.m_Root.Q<VisualElement>("Notifications").Q<VisualElement>("Correct");
        this.notificationFalse = this.m_Root.Q<VisualElement>("Notifications").Q<VisualElement>("False");
        this.notificationCorrectCloseBtn = this.notificationCorrect.Q<VisualElement>("header").Q<Button>("close");
        this.notificationFalseCloseBtn = this.notificationFalse.Q<VisualElement>("header").Q<Button>("close");
        this.fundeImages = this.m_Root.Q<VisualElement>("RomischeFunde").Q<VisualElement>("Images");
        this.nextPhase2Images = this.m_Root.Q<VisualElement>("NextPhase2").Q<VisualElement>("Content").Q<VisualElement>("Images");
        this.nextPhase3Images = this.m_Root.Q<VisualElement>("NextPhase3").Q<VisualElement>("Content").Q<VisualElement>("Images");
        this.winImages = this.m_Root.Q<VisualElement>("Win").Q<VisualElement>("Content").Q<VisualElement>("Images");
        this.goToLevel2Btn = this.m_Root.Q<VisualElement>("NextPhase2").Q<VisualElement>("Footer").Q<Button>();
        this.goToLevel3Btn = this.m_Root.Q<VisualElement>("NextPhase3").Q<VisualElement>("Footer").Q<Button>();
        this.closeGamePanel = this.m_Root.Q<VisualElement>("CloseGame");
        this.continuePlaying = this.m_Root.Q<VisualElement>("CloseGame").Q<Button>("continue");
        this.saveGame = this.m_Root.Q<VisualElement>("CloseGame").Q<Button>("save");
        this.closeSavePanel = this.m_Root.Q<VisualElement>("CloseGame").Q<VisualElement>("header").Q<Button>("close");
        this.gameHeader = GameObject.FindGameObjectWithTag("GameHeader").GetComponent<UIDocument>().rootVisualElement;
        this.closeGameHeader = this.gameHeader.Q<Button>("close");
        this.restartGame = this.m_Root.Q<VisualElement>("Win").Q<VisualElement>("Footer").Q<Button>();
        #endregion

        this.crossGameManager.IsVisitingFromGame = true;

        checkLoadedContent = true;

        if (crossGameManager.game1_currentPhase == "") {
            crossGameManager.game1_currentPhase = "Level1";
            this.Level1.name = "Level1";
            this.Level2.name = "Level2";
            this.Level3.name = "Level3";
        }

        if (crossGameManager.game1_currentPhase == "Level2") {
            this.nextPhase2.style.display = DisplayStyle.None;
            this.Game.style.display = DisplayStyle.Flex;
            this.Level1.style.display = DisplayStyle.None;
            this.Level2.style.display = DisplayStyle.Flex;
        }
        else if (crossGameManager.game1_currentPhase == "Level3") {
            this.nextPhase3.style.display = DisplayStyle.None;
            this.Game.style.display = DisplayStyle.Flex;
            this.Level1.style.display = DisplayStyle.None;
            this.Level2.style.display = DisplayStyle.None;
            this.Level3.style.display = DisplayStyle.Flex;
        }

        ButtonClick();

    }

    private void Update() {

        if (checkLoadedContent) {
            if (this.Level3.childCount > 0) {
                ContentIsLoaded(crossGameManager.game1_currentPhase);
                checkLoadedContent = false;
            }
        }

    }

    async void ButtonClick() {
        this.falseBtn.clicked += delegate {
            OnFalseAnswer();
            CheckLevel();
        };

        this.correctBtn.clicked += delegate {
            OnCorrectAnswer();
            CheckLevel();
        };

        this.notificationCorrectCloseBtn.clicked += delegate {
            this.notifications.style.display = DisplayStyle.None;
            this.notificationCorrect.style.display = DisplayStyle.None;
            DelayDestroy();
            CheckLevel();
        };

        this.notificationFalseCloseBtn.clicked += delegate {
            this.notifications.style.display = DisplayStyle.None;
            this.notificationFalse.style.display = DisplayStyle.None;
            DelayDestroy();
            CheckLevel();
        };

        this.goToLevel2Btn.clicked += delegate {
            this.nextPhase2.style.display = DisplayStyle.None;
            this.Game.style.display = DisplayStyle.Flex;
            this.Level1.style.display = DisplayStyle.None;
            this.Level2.style.display = DisplayStyle.Flex;
            this.Level2[0].style.display = DisplayStyle.Flex;
        };

        this.goToLevel3Btn.clicked += delegate {
            this.nextPhase3.style.display = DisplayStyle.None;
            this.Game.style.display = DisplayStyle.Flex;
            this.Level1.style.display = DisplayStyle.None;
            this.Level2.style.display = DisplayStyle.None;
            this.Level3.style.display = DisplayStyle.Flex;
            this.Level3[0].style.display = DisplayStyle.Flex;
        };

        this.restartGame.clicked += delegate {
            SceneManager.LoadScene("Game1");
        };

        /*
        this.closeGameHeader.clicked += delegate {
            this.closeGamePanel.style.display = DisplayStyle.Flex;
        };

        this.continuePlaying.clicked += delegate {
            this.closeGamePanel.style.display = DisplayStyle.None;
        };

        this.closeSavePanel.clicked += delegate {
            this.closeGamePanel.style.display = DisplayStyle.None;
        };
        */

        this.saveGame.clicked += delegate {
            SceneManager.LoadScene("MainScene");
        };

    }

    public void CheckLevel() {
        StartCoroutine(CheckIfLevelEnded());
    }

    public void CheckForElementType() {

        if (crossGameManager.game1_currentPhase == "Level1") {

            for (int i = 0; i < this.Level1.childCount; i++) {
                if (this.Level1[i].style.display == DisplayStyle.Flex) {
                    if (this.Level1[i].name == "True") {
                        roemisch = true;
                    } else if (this.Level1[i].name == "False") {
                        roemisch = false;
                    }
                }
            }
        }

        if (crossGameManager.game1_currentPhase == "Level2") {
            for (int i = 0; i < this.Level2.childCount; i++) {
                if (this.Level2[i].style.display == DisplayStyle.Flex) {
                    if (this.Level2[i].name == "True") {
                        roemisch = true;
                    } else if (this.Level2[i].name == "False") {
                        roemisch = false;
                    }
                }
            }
        }

        if (crossGameManager.game1_currentPhase == "Level3") {
            for (int i = 0; i < this.Level3.childCount; i++) {
                if (this.Level3[i].style.display == DisplayStyle.Flex) {
                    if (this.Level3[i].name == "True") {
                        roemisch = true;
                    } else if (this.Level3[i].name == "False") {
                        roemisch = false;
                    }
                }
            }
        }
    }

    public void OnCorrectAnswer() {
        CheckForElementType();

        if (roemisch == true) {

            uiStroke = true;
            strokeValues.Add("true");
            UpdateFunde();

            this.notifications.style.display = DisplayStyle.Flex;
            this.notificationCorrect.style.display = DisplayStyle.Flex;
            this.notificationCorrect.Q<Label>().text = this.gameObject.GetComponent<Game1ContentLoader>().onCorrectAnswer;
            //this.notificationCorrect.Q<Label>().text = "Ja, das ist römisch!";

        } else if (roemisch == false) {

            this.notifications.style.display = DisplayStyle.Flex;
            this.notificationFalse.style.display = DisplayStyle.Flex;
            this.notificationFalse.Q<Label>().text = this.gameObject.GetComponent<Game1ContentLoader>().onFalseCorrectAnswer;
            //False Correct
        }
    }

    public void OnFalseAnswer() {
        CheckForElementType();

        if (roemisch == false) {

            this.notifications.style.display = DisplayStyle.Flex;
            this.notificationCorrect.style.display = DisplayStyle.Flex;
            this.notificationCorrect.Q<Label>().text = this.gameObject.GetComponent<Game1ContentLoader>().onFalseAnswer;
            //this.notificationCorrect.Q<Label>().text = "Genau, das ist nicht römisch!";

        } else if (roemisch == true) {

            uiStroke = false;
            strokeValues.Add("false");
            UpdateFunde();

            this.notifications.style.display = DisplayStyle.Flex;
            this.notificationFalse.style.display = DisplayStyle.Flex;
            this.notificationFalse.Q<Label>().text = this.gameObject.GetComponent<Game1ContentLoader>().onCorrectFalseAnswer;
            //this.notificationFalse.Q<Label>().text = "Nein, das ist römisch!";
        }
    }

    public void goToNextPhase() {
            
         if ((this.Level1.childCount == 0) && (this.Level1.name == "Level1:Completed")) {
                crossGameManager.game1_currentPhase = "Level2";
                this.Game.style.display = DisplayStyle.None;
                this.nextPhase2.style.display = DisplayStyle.Flex;

            
            for (int i = 0; i < strokeValues.Count; i++) {
                if (strokeValues[i] == "true") {
                    nextPhase2Images[i].Q<Button>("correct").style.display = DisplayStyle.Flex;
                    nextPhase2Images[i].Q<VisualElement>("false").style.display = DisplayStyle.None;
                    coinCount++;
                    } else if (strokeValues[i] == "false") {
                    nextPhase2Images[i].Q<Button>("correct").style.display = DisplayStyle.None;
                    nextPhase2Images[i].Q<VisualElement>("false").style.display = DisplayStyle.Flex;
                }
            }

            if (coinCount == 0) {
                    this.nextPhase2.Q<VisualElement>("Header").Q<Label>("Gratulation").text = this.gameObject.GetComponent<Game1ContentLoader>().onTryAgain;
                    //this.nextPhase2.Q<VisualElement>("Header").Q<Label>("Gratulation").text = "Versuche es noch einmal.";
                    this.nextPhase2.Q<VisualElement>("Content").Q<Label>().style.display = DisplayStyle.None;
                } else {
                    crossGameManager.AddToScore(crossGameManager.colorFromHex("#FD8300"), coinCount);
                }

                this.nextPhase2.Q<VisualElement>("Header").Q<VisualElement>("coin").Q<Label>("coinCount").text = coinCount.ToString();

                coinCount = 0;
                strokeValues.Clear();
                this.Level1.name = "Done";
                EmptyFunde();
            }

         if ((this.Level2.childCount == 0) && (this.Level2.name == "Level2:Completed"))  {
                crossGameManager.game1_currentPhase = "Level3";
                this.Game.style.display = DisplayStyle.None;
                this.nextPhase3.style.display = DisplayStyle.Flex;

                for (int i = 0; i < strokeValues.Count; i++) {
                    if (strokeValues[i] == "true") {
                        nextPhase3Images[i].Q<Button>("correct").style.display = DisplayStyle.Flex;
                        nextPhase3Images[i].Q<VisualElement>("false").style.display = DisplayStyle.None;
                        coinCount++;
                    } else if (strokeValues[i] == "false") {
                        nextPhase3Images[i].Q<Button>("correct").style.display = DisplayStyle.None;
                        nextPhase3Images[i].Q<VisualElement>("false").style.display = DisplayStyle.Flex;
                    }
                }

                if (coinCount == 0) {
                    this.nextPhase3.Q<VisualElement>("Header").Q<Label>("Gratulation").text = this.gameObject.GetComponent<Game1ContentLoader>().onTryAgain;
                    //this.nextPhase3.Q<VisualElement>("Header").Q<Label>("Gratulation").text = "Versuche es noch einmal.";
                    this.nextPhase3.Q<VisualElement>("Content").Q<Label>().style.display = DisplayStyle.None;
                } else {
                    crossGameManager.AddToScore(crossGameManager.colorFromHex("#FD8300"), coinCount);
                }

                this.nextPhase3.Q<VisualElement>("Header").Q<VisualElement>("coin").Q<Label>("coinCount").text = coinCount.ToString();

                coinCount = 0;
                strokeValues.Clear();
                this.Level2.name = "Done";
                EmptyFunde();
            }

         if ((this.Level3.childCount == 0) && (this.Level3.name == "Level3:Completed")) {
                crossGameManager.game1_currentPhase = "Level1";
                this.Game.style.display = DisplayStyle.None;
                win.style.display = DisplayStyle.Flex;

                crossGameManager.AddToScore(crossGameManager.colorFromHex("#FD8300"), coinCount);

                for (int i = 0; i < strokeValues.Count; i++) {
                    if (strokeValues[i] == "true") {
                        winImages[i].Q<Button>("correct").style.display = DisplayStyle.Flex;
                        winImages[i].Q<VisualElement>("false").style.display = DisplayStyle.None;
                        coinCount++;
                    } else if (strokeValues[i] == "false") {
                        winImages[i].Q<Button>("correct").style.display = DisplayStyle.None;
                        winImages[i].Q<VisualElement>("false").style.display = DisplayStyle.Flex;
                    }
                }

                if (coinCount == 0) {
                this.win.Q<VisualElement>("Header").Q<Label>("Gratulation").text = this.gameObject.GetComponent<Game1ContentLoader>().onTryAgain;
                //this.win.Q<VisualElement>("Header").Q<Label>("Gratulation").text = "Versuche es noch einmal.";
                this.win.Q<VisualElement>("Content").Q<Label>().style.display = DisplayStyle.None;
                } else {
                    crossGameManager.AddToScore(crossGameManager.colorFromHex("#FD8300"), coinCount);
                }

                this.win.Q<VisualElement>("Header").Q<VisualElement>("coin").Q<Label>("coinCount").text = coinCount.ToString();

                coinCount = 0;
                strokeValues.Clear();
                this.Level3.name = "Done";
                EmptyFunde();
            }

    }

    void UpdateFunde() {
        
        for (int i = 0; i < this.fundeImages.childCount; i++) {
            if (this.fundeImages[i].name == "empty") {
                this.fundeImages[i].name = "image";

                if(crossGameManager.game1_currentPhase == "Level1") {
                    this.fundeImages[i].style.backgroundImage = this.Level1[0].style.backgroundImage;
                }
                else if (crossGameManager.game1_currentPhase == "Level2") {
                    this.fundeImages[i].style.backgroundImage = this.Level2[0].style.backgroundImage;
                } else if (crossGameManager.game1_currentPhase == "Level3") {

                    this.fundeImages[i].style.backgroundImage = this.Level3[0].style.backgroundImage;
                }

                if (uiStroke) {
                    if (ColorUtility.TryParseHtmlString(htmlCorrectValue, out borderColor)) {
                        this.fundeImages[i].style.borderTopColor = borderColor;
                        this.fundeImages[i].style.borderRightColor = borderColor;
                        this.fundeImages[i].style.borderLeftColor = borderColor;
                        this.fundeImages[i].style.borderBottomColor = borderColor;
                    }

                } else if (!uiStroke) {
                    if (ColorUtility.TryParseHtmlString(htmlFalseValue, out borderColor)) {
                        this.fundeImages[i].style.borderTopColor = borderColor;
                        this.fundeImages[i].style.borderRightColor = borderColor;
                        this.fundeImages[i].style.borderLeftColor = borderColor;
                        this.fundeImages[i].style.borderBottomColor = borderColor;
                    }
                }

                break;
            }
        
        }

    }

    void EmptyFunde() {
        for (int i = 0; i < this.fundeImages.childCount; i++) {
            this.fundeImages[i].style.backgroundImage = null;
            this.fundeImages[i].name = "empty";

            if (ColorUtility.TryParseHtmlString(htmlemptyValue, out borderColor)) {
                this.fundeImages[i].style.borderTopColor = borderColor;
                this.fundeImages[i].style.borderRightColor = borderColor;
                this.fundeImages[i].style.borderLeftColor = borderColor;
                this.fundeImages[i].style.borderBottomColor = borderColor;
            }
        }
    }

    public void DelayDestroy() {

        if (crossGameManager.game1_currentPhase == "Level1") {

            for (int i = 0; i < this.Level1.childCount; i++) {
                if (this.Level1[i].style.display == DisplayStyle.Flex) {
                    this.Level1.Remove(this.Level1[i]);
                }
            }

            if (this.Level1.childCount > 0) {
                this.Level1[0].style.display = DisplayStyle.Flex;

                if (this.Level1.childCount == 1) {
                    this.Level1.name = "Level1:Completed";
                }
            }
        }

        if (crossGameManager.game1_currentPhase == "Level2") {

            for (int i = 0; i < this.Level2.childCount; i++) {
                if (this.Level2[i].style.display == DisplayStyle.Flex) {
                    this.Level2.Remove(this.Level2[i]);
                }
            }

            if (this.Level2.childCount > 0) {
                this.Level2[0].style.display = DisplayStyle.Flex;

                if (this.Level2.childCount == 1) {
                    this.Level2.name = "Level2:Completed";
                }
            }
        }

        if (crossGameManager.game1_currentPhase == "Level3") {

            for (int i = 0; i < this.Level3.childCount; i++) {
                if (this.Level3[i].style.display == DisplayStyle.Flex) {
                    this.Level3.Remove(this.Level3[i]);
                }
            }

            if (this.Level3.childCount > 0) {
                this.Level3[0].style.display = DisplayStyle.Flex;

                if (this.Level3.childCount == 1) {
                    this.Level3.name = "Level3:Completed";
                }
            }
        }

    }


    IEnumerator CheckIfLevelEnded() {
        yield return new WaitForSeconds(0.1f);
        goToNextPhase();
    }

    void ContentIsLoaded(string level) {
        if (level == "Level1") {
            this.Level1[0].style.display = DisplayStyle.Flex;
        }
        if (level == "Level2") {
            this.Level2[0].style.display = DisplayStyle.Flex;
            this.Level1.name = "Done";
        }
        if (level == "Level3") {
            this.Level3[0].style.display = DisplayStyle.Flex;
            this.Level1.name = "Done";
            this.Level2.name = "Done";
        }


    }
}
