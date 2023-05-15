using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using Services;

public class startGame3_AR_Ready : MonoBehaviour
{
    private StrapiService _strapiService;
    public GameObject part1;
    public GameObject part2;
    public GameObject part3;

    public int scores = 0;
    public int part1_goal = 8;
    public int part2_goal = 3;
    public int part3_goal = 1;

    public string state;
    private bool canChangeState = true;

    //Game Overlay Properties
    [Tooltip("INTRO TO GAME: Overlay Properties")]

    public GameObject gamePopUp;
    public GameObject FundInfo;

    public Game3Data _data;

    public CrossGameManager crossGameManager;
    public Game3Manager game3Manager;
    public RadialWheel radialWheel1;
    public RadialWheel radialWheel2;
    public RadialWheel radialWheel3;




    [SerializeField]
    public Color UIBackgroundColor;



    private IEnumerator Start() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        game3Manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game3Manager>();

        part1.SetActive(true);
        yield return new WaitForSeconds(1);

        radialWheel1.CanInstantiateDragableItem = true;

        part2.SetActive(false);
        part3.SetActive(false);

        gamePopUp.SetActive(true);
        gamePopUp.GetComponent<GamePopUp>().ShowAndUpdatePopUp(game3Manager.popupStep1_Headline, "", game3Manager.popupStep1_subheadline, game3Manager.popupStep1_ButtonText, "info");

    }


    void Update() {

        if (part1.activeSelf && this.scores == part1_goal) {            
            this.scores = 0;
            state = "state1";

            crossGameManager.AddToScore(crossGameManager.colorToType("spiel"), game3Manager.game3.attributes.reward/4);
            radialWheel1.EndRadialMenu("Das Grabungscamp ist aufgebaut.");

            StartCoroutine(ShowFundObjectOverlay());


        } else if (part2.activeSelf && this.scores == part2_goal-1 && this.state == "state2" && canChangeState) {

            //radialWheel2.gameObject.SetActive(false);
            crossGameManager.AddToScore(crossGameManager.colorToType("spiel"), game3Manager.game3.attributes.reward / 4);

            StartCoroutine(ShowFundObjectOverlay());

            radialWheel2.DisactivateRadialMenu("");


        } else if (part2.activeSelf && this.scores == part2_goal && this.state == "state3" && canChangeState) {

            this.scores = 0;

            crossGameManager.AddToScore(crossGameManager.colorToType("spiel"), game3Manager.game3.attributes.reward / 4);
            
            radialWheel2.EndRadialMenu("Der Fund ist ausgegraben.");

            StartCoroutine(ShowFundObjectOverlay());


        } else if (part3.activeSelf && this.scores == part3_goal && this.state == "state5" && canChangeState) {
            

            this.scores = 0;
            crossGameManager.AddToScore(crossGameManager.colorToType("spiel"), game3Manager.game3.attributes.reward / 4);
            radialWheel3.EndRadialMenu("Das ist ein Maultier!");

            StartCoroutine(ShowFundObjectOverlay());


        }



    }


    IEnumerator ShowFundObjectOverlay() {
        canChangeState = false;
        
        yield return new WaitForSeconds(3);

        if (state == "state1") {


            if (game3Manager.winimage1 != null) {
                SetImage(game3Manager.winimage1);

            }


            FundInfo.GetComponent<FundInfo>().Show(game3Manager.GlockeItem, false, game3Manager.winMessageP1_Headline, (game3Manager.game3.attributes.reward / 4).ToString(), game3Manager.winMessageP1_subheadline, game3Manager.winMessageP1_ButtonText, false);

 
            gamePopUp.SetActive(true);
           
            gamePopUp.GetComponent<GamePopUp>().ShowAndUpdatePopUp(game3Manager.popupStep2_Headline, "", game3Manager.popupStep2_subheadline, game3Manager.popupStep2_ButtonText, "info");

            state = "state2";

        } else if (state == "state2") {

            FundInfo.GetComponent<FundInfo>().Show(game3Manager.GlockeItem, true, game3Manager.winMessageP2_Headline, (game3Manager.game3.attributes.reward / 4).ToString(), game3Manager.winMessageP2_subheadline, game3Manager.winMessageP2_ButtonText, false);

            gamePopUp.SetActive(true);

            gamePopUp.GetComponent<GamePopUp>().ShowAndUpdatePopUp(game3Manager.popupStep3_Headline, "", game3Manager.popupStep3_subheadline, game3Manager.popupStep3_ButtonText, "info");

            //part2.SetActive(false);

            state = "state3";

        } else if (state == "state3") {
           
            gamePopUp.SetActive(true);

            //radialWheel2.EndRadialMenu("Der Fund ist ausgegraben.");

            gamePopUp.GetComponent<GamePopUp>().ShowAndUpdatePopUp(game3Manager.popupStep4_Headline, "", "", game3Manager.popupStep4_ButtonText, "info");
            state = "state4"; 

        } else if (state == "state4") {

            Debug.Log("toggles info for state4");
            
            gamePopUp.SetActive(true);
            gamePopUp.GetComponent<GamePopUp>().ShowAndUpdatePopUp(game3Manager.popupStep5_Headline, "", game3Manager.popupStep5_subheadline, game3Manager.popupStep5_ButtonText, "info");
            
            state = "state5";
            radialWheel3.gameObject.SetActive(true);


        } else if (state == "state5") {
            Debug.Log("toggles info for state5");

            if (game3Manager.winimage2 != null) {
                SetImage(game3Manager.winimage2);

            }

            yield return new WaitForSeconds(9);
            FundInfo.GetComponent<FundInfo>().Show(game3Manager.MaultierItem, true, game3Manager.winMessageP3_Headline, (game3Manager.game3.attributes.reward).ToString(), game3Manager.winMessageP3_subheadline, game3Manager.winMessageP3_ButtonText, true);

     

            radialWheel2.gameObject.SetActive(false);
            radialWheel3.gameObject.SetActive(false);


        }
        canChangeState = true;


    }


    async void SetImage(string imgUrl) {
        FundInfo.GetComponent<FundInfo>().setImage(imgUrl);
    }

    public void StartNextState() {
        gamePopUp.SetActive(false);
        if (state == "state2") {

            part1.SetActive(false);
            part2.SetActive(true);

            gamePopUp.GetComponent<GamePopUp>().StopInteractionWithUIElements(true);

            if (GameObject.FindGameObjectWithTag("ARSessionOrigin")) {
                GameObject.FindGameObjectWithTag("ARSessionOrigin").GetComponent<PlaceOnPlaneWithAnchor>().SetAllPlanesActive(false);
            } 
        
        } else if (state == "state3") {

            //part2.SetActive(true);

            //radialWheel2.gameObject.SetActive(true);
            radialWheel2.CanInstantiateDragableItem = true;

            crossGameManager.ErrorLog("returns to dig more" + radialWheel2.gameObject.activeSelf);


        } else if (state == "state4") {

            part2.SetActive(true);
            StartCoroutine(ShowFundObjectOverlay());

         
        } else if (state == "state5") {

            part2.SetActive(false);
            part3.SetActive(true);
            gamePopUp.GetComponent<GamePopUp>().StopInteractionWithUIElements(true);

        }
    }


    public void SetScore1() { ////keeps score and toggles New Part (teil2) when score 2 is met. 
        this.scores += 1;
    }









}
