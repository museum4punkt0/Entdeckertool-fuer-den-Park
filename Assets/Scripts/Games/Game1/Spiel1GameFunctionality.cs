using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services;
using UnityEngine.SceneManagement;

public class Spiel1GameFunctionality : MonoBehaviour
{
    
    bool roemisch = false;
    bool uiStroke = false;
    List<string> strokeValues = new List<string>();
    public GameObject nextPhase2;
    public GameObject nextPhase3;
    public GameObject win;
    public GameObject notificationRoemisch;
    public GameObject notificationNichtRoemisch;
    public GameObject notificationBlocker;
    public GameObject buttons;
    //GameObject tutorial = GameObject.FindGameObjectWithTag("gameTutorial").gameObject;
    CrossGameManager crossGameManagerScript;
    public bool showTutorial = true;

    int coinCount = 0;

    private void Start() {
        crossGameManagerScript = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        crossGameManagerScript.IsVisitingFromGame = true;
        print("inside level1");
        print("currentPhase: " + crossGameManagerScript.game1_currentPhase);

        
        if (crossGameManagerScript.game1_currentPhase == "Level2") {

            
            this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            foreach (Transform child in this.gameObject.transform.GetChild(1)) {
                Destroy(child.gameObject);
            }
            Destroy(this.gameObject.transform.GetChild(1).gameObject);
            

            print("inside level2");

        } else if (crossGameManagerScript.game1_currentPhase == "Level3") {
            
            //Destroy(GameObject.FindGameObjectWithTag("Game1_Level1").gameObject);
            //Destroy(GameObject.FindGameObjectWithTag("Game1_Level2").gameObject);
            //GameObject.FindGameObjectWithTag("Game1_Level3").gameObject.SetActive(true);
            //Destroy(this.gameObject.transform.GetChild(1).gameObject);
            //Destroy(this.gameObject.transform.GetChild(2).gameObject);
            //this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            //print("inside level3");
        }
        
    }

    public void CheckLevel() {
        print("inside CheckLevel");
        StartCoroutine(CheckIfLevelEnded());
    }

    public void CheckForElementType() {
        print("inside CheckForElementType");

        if (gameObject.transform.GetChild(1).GetChild(gameObject.transform.GetChild(1).childCount - 1).tag == "roemisch") {
            roemisch = true;
        } else if (gameObject.transform.GetChild(1).GetChild(gameObject.transform.GetChild(1).childCount - 1).tag != "roemisch") {
            roemisch = false;
        } 

    }

    public void OnCorrectAnswer() {
        print("inside OnCorrectAnswer");
        CheckForElementType();

        if (roemisch == true) {

            uiStroke = true;
            strokeValues.Add("true");
            UpdateFunde();

            notificationRoemisch.transform.GetChild(0).GetComponent<Text>().text = "Ja, das ist römisch!";
            notificationRoemisch.SetActive(true);
            notificationBlocker.SetActive(true);
            
        } else if(roemisch == false) {

            notificationNichtRoemisch.SetActive(true);
            notificationBlocker.SetActive(true);
            
        }
    }

    public void OnFalseAnswer() {

        //check if elements tag
        CheckForElementType();

        if (roemisch == false) {
            
            notificationRoemisch.transform.GetChild(0).GetComponent<Text>().text = "Genau, das ist nicht römisch!";
            notificationRoemisch.SetActive(true);
            notificationBlocker.SetActive(true);
           

        } else if (roemisch == true) {

            uiStroke = false;
            strokeValues.Add("false");
            UpdateFunde();
            notificationNichtRoemisch.transform.GetChild(0).GetComponent<Text>().text = "Nein, das ist römisch!";
            notificationNichtRoemisch.SetActive(true);
            notificationBlocker.SetActive(true);

        }

       

    }

    public void goToNextPhase() {
        print("inside goToNextPhase");
        
        if (gameObject.transform.GetChild(1).childCount == 0) {

            if (gameObject.transform.GetChild(1).gameObject.name == "Level1") {
                crossGameManagerScript.game1_currentPhase = "Level2";
                nextPhase2.SetActive(true);
               // crossGameManagerScript.AddToScore("#FD8300", coinCount);

                strokeValues.ToArray();
                #region Stroke Color
                if (strokeValues[0] == "true") {
                    nextPhase2.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    nextPhase2.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                    coinCount++;
                }
                if (strokeValues[0] == "false") {
                    nextPhase2.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);
                    nextPhase2.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                    nextPhase2.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Button>().enabled = false;
                }
                if (strokeValues[1] == "true") {
                    nextPhase2.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    nextPhase2.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                    coinCount++;
                }
                if (strokeValues[1] == "false") {
                    nextPhase2.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);
                    nextPhase2.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                    nextPhase2.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<Button>().enabled = false;
                }
                if (strokeValues[2] == "true") {
                    nextPhase2.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    nextPhase2.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                    nextPhase2.transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<Button>().enabled = true;
                    coinCount++;
                }
                if (strokeValues[2] == "false") {
                    nextPhase2.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);
                    nextPhase2.transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<Button>().enabled = false;
                    nextPhase2.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                }
                if (strokeValues[3] == "true") {
                    nextPhase2.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    nextPhase2.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                    coinCount++;
                }
                if (strokeValues[3] == "false") {
                    nextPhase2.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);
                    nextPhase2.transform.GetChild(2).GetChild(4).GetChild(0).GetComponent<Button>().enabled = false;
                    nextPhase2.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                }
                #endregion
                
                if (coinCount == 0) {
                    nextPhase2.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Versuche es noch einmal.";
                    nextPhase2.transform.GetChild(3).gameObject.SetActive(false);
                } else {
                    crossGameManagerScript.AddToScore(crossGameManagerScript.colorFromHex("#FD8300"), coinCount);
                }


                nextPhase2.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = coinCount.ToString();

                coinCount = 0;
                strokeValues.Clear();
            }

            if (gameObject.transform.GetChild(1).gameObject.name == "Level2") {
                crossGameManagerScript.game1_currentPhase = "Level3";
                nextPhase3.SetActive(true);
                //crossGameManagerScript.AddToScore("#FD8300", coinCount);

                strokeValues.ToArray();
                #region Stroke Color
                if (strokeValues[0] == "true") {
                    nextPhase3.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    nextPhase3.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                    coinCount++;
                }
                if (strokeValues[0] == "false") {
                    nextPhase3.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);
                    nextPhase3.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                    nextPhase3.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Button>().enabled = false;
                }
                if (strokeValues[1] == "true") {
                    nextPhase3.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    nextPhase3.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                    coinCount++;
                }
                if (strokeValues[1] == "false") {
                    nextPhase3.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);
                    nextPhase3.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                    nextPhase3.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<Button>().enabled = false;
                }
                if (strokeValues[2] == "true") {
                    nextPhase3.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    nextPhase3.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                    nextPhase3.transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<Button>().enabled = true;
                    coinCount++;
                }
                if (strokeValues[2] == "false") {
                    nextPhase3.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);
                    nextPhase3.transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<Button>().enabled = false;
                    nextPhase3.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                }
                if (strokeValues[3] == "true") {
                    nextPhase3.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    nextPhase3.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                    coinCount++;
                }
                if (strokeValues[3] == "false") {
                    nextPhase3.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);
                    nextPhase3.transform.GetChild(2).GetChild(4).GetChild(0).GetComponent<Button>().enabled = false;
                    nextPhase3.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                }
                #endregion
                
                if (coinCount == 0) {
                    nextPhase3.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Versuche es noch einmal.";
                    nextPhase3.transform.GetChild(3).gameObject.SetActive(false);
                } else {
                    crossGameManagerScript.AddToScore(crossGameManagerScript.colorFromHex("#FD8300"), coinCount);
                }

                nextPhase3.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = coinCount.ToString();

                coinCount = 0;
                strokeValues.Clear();
            }

            coinCount = 0;
            //empty funde
            Transform funde = gameObject.transform.GetChild(0).GetChild(1).transform;
            foreach (Transform child in funde) {
                child.GetComponent<Image>().sprite = null;
                child.GetChild(0).GetComponent<Image>().color = new Color32(146, 146, 146, 255);
            }

            Destroy(gameObject.transform.GetChild(1).gameObject);

            gameObject.transform.GetChild(2).gameObject.SetActive(true);

            if (gameObject.transform.GetChild(1).gameObject.name == "Level3") {
                crossGameManagerScript.game1_currentPhase = "Level1";
                win.SetActive(true);
                //crossGameManagerScript.AddToScore("#FD8300", coinCount);

                crossGameManagerScript.AddToScore(crossGameManagerScript.colorFromHex("#FD8300"), coinCount);

                strokeValues.ToArray();
                #region Stroke Color
                if (strokeValues[0] == "true") {
                    win.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    win.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                    coinCount++;
                }
                if (strokeValues[0] == "false") {
                    win.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);
                    win.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                    win.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Button>().enabled = false;
                }
                if (strokeValues[1] == "true") {
                    win.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    win.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                    coinCount++;
                }
                if (strokeValues[1] == "false") {
                    win.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);
                    win.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                    win.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<Button>().enabled = false;
                }
                if (strokeValues[2] == "true") {
                    win.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    win.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                    win.transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<Button>().enabled = true;
                    coinCount++;
                }
                if (strokeValues[2] == "false") {
                    win.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);
                    win.transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<Button>().enabled = false;
                    win.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                }
                if (strokeValues[3] == "true") {
                    win.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    win.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                    coinCount++;
                }
                if (strokeValues[3] == "false") {
                    win.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);
                    win.transform.GetChild(2).GetChild(4).GetChild(0).GetComponent<Button>().enabled = false;
                    win.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                }
                #endregion

                if (coinCount == 0) {
                    win.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Versuche es noch einmal.";
                    win.transform.GetChild(3).gameObject.SetActive(false);
                } else {
                    crossGameManagerScript.AddToScore(crossGameManagerScript.colorFromHex("#FD8300"), coinCount);
                }

                win.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = coinCount.ToString();

                coinCount = 0;
                strokeValues.Clear();
            }
        }

        print("coincount: " + coinCount);
    }

    void UpdateFunde() {

        int funde = gameObject.transform.GetChild(0).GetChild(1).transform.childCount;
       
        for (int i = 0; i < funde; ++i) {
            if (gameObject.transform.GetChild(0).GetChild(1).GetChild(i).GetComponent<Image>().sprite == null) {
                gameObject.transform.GetChild(0).GetChild(1).GetChild(i).GetComponent<Image>().sprite = gameObject.transform.GetChild(1).GetChild(gameObject.transform.GetChild(1).childCount - 1).GetChild(0).GetChild(1).GetComponent<Image>().sprite;

                //set stroke color
                if (uiStroke) {
                    gameObject.transform.GetChild(0).GetChild(1).GetChild(i).GetChild(0).GetComponent<Image>().color = new Color32(16, 159, 148, 255);
                    //coinCount++;

                } else if(!uiStroke)
                    gameObject.transform.GetChild(0).GetChild(1).GetChild(i).GetChild(0).GetComponent<Image>().color = new Color32(232, 108, 92, 255);

                break;
            }
            
        }

        print("coins: " + coinCount);

    }

    public void GoToMainScene() {
        SceneManager.LoadScene("MainScene");
    }

    public void ReStartGame() {
        showTutorial = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator CheckIfLevelEnded() {
            yield return new WaitForSeconds(0.1f);
            goToNextPhase();
    }

    IEnumerator Notification(GameObject notification) {

        yield return new WaitForSeconds(0.8f);
        notification.SetActive(false);
        
    }


    public void DelayDestroy() 
    {
        buttons.transform.GetChild(1).GetComponent<Button>().enabled = false;
        buttons.transform.GetChild(2).GetComponent<Button>().enabled = false;

        Destroy(gameObject.transform.GetChild(1).GetChild(gameObject.transform.GetChild(1).childCount - 1).gameObject);
        buttons.transform.GetChild(1).GetComponent<Button>().enabled = true;
        buttons.transform.GetChild(2).GetComponent<Button>().enabled = true;
    }
}
