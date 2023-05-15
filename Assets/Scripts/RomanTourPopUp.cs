
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using DG.Tweening;
using ARLocation;

public class RomanTourPopUp : MonoBehaviour
{
    public VisualElement overlay;
    public GroupBox OverlayContent;
    public Button InvestigateButton;
    public Button ContinueButton;
    public Button ToggleOverlay;
    public VisualElement pin;
    public VisualElement checked_pin;
    public Label Headline;
    public Label Detail;
    public Label fund_amount;
    public Label coin_amount;
    public Label distance;
    public GroupBox coinscore;
    public Label coinscore_text;

    public float allowedDistance = 30;

    public string Target;

    //Temporary Variables
    public string NewSceneName;
    public string MainSceneName;


    //Access GameController
    public GameObject GameController;
    public CrossGameManager crossGameManager;
    public ItemOnMap currentItemOnMap;

    //Ui
    public UIGame5Controller GameAssetMenu;
    public GameObject GameAssetMenuPopUp;
    


    //states
    public bool IsWithinRange = false;
    public bool hasBeenVisited = false;


    //UI Coloring
    //create override color in scenes to override overlay color defined by POI type
    public Color errorColor;
    public string overRideColor;
    private Color withinRangeColor;

    private ARLocationProvider _arLocationProvider;
    private string previousClassType;

    
    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        this._arLocationProvider = ARLocationProvider.Instance;
        Debug.Log("CHECK AWAKE IN UI");
      
    }

    /// Run start as enumerator to wait for first frame and be able to get all properties from UI elements
    private IEnumerator Start() {
        yield return new WaitForEndOfFrame();
        AssignPopUpUIElements();
    }

    public void AssignPopUpUIElements() {

        var root = this.GetComponent<UIDocument>().rootVisualElement;

        this.overlay = root.Q<VisualElement>("GameOverlay");
        this.overlay.style.display = DisplayStyle.None;

        this.pin = root.Q<VisualElement>("pin");
        this.checked_pin = root.Q<VisualElement>("checked_pin");
        this.checked_pin.style.display = DisplayStyle.None;



        this.ContinueButton = root.Q<Button>("ActionButton_ContinueTour");
        this.InvestigateButton = root.Q<Button>("ActionButton_GoToTarget");
        this.ToggleOverlay = root.Q<Button>("ClosingPopUp");


        this.Detail = root.Q<Label>("DETAIL");
        this.Headline = root.Q<Label>("HEADLINE");

      

        this.distance = root.Q<Label>("Distance");
        this.coinscore = root.Q<GroupBox>("CoinScore");
        this.coinscore_text = root.Q<Label>("CoinScore");

        this.ToggleOverlay.clicked += Hide;

        Hide();
    }

    public void Show(ItemOnMap ItemOnMap) {
     
        TourPoint TourPoint = ItemOnMap.Poi.attributes.tourPoint;
        string currentClassType = ItemOnMap.Poi.attributes.type;

        this.currentItemOnMap = ItemOnMap;

        Debug.Log("SHOWS POPS UP" + this.currentItemOnMap.Name);

        if (overRideColor == null || overRideColor == "") {
            if (ItemOnMap.Poi.attributes.type != null || ItemOnMap.Poi.attributes.type != "") {
                this.withinRangeColor = crossGameManager.colorToType(ItemOnMap.Poi.attributes.type);
            }
        } else {
            Color color;
            ColorUtility.TryParseHtmlString(overRideColor, out color);
            this.withinRangeColor = color;
        }
    

        double distance = crossGameManager.CalculateDistanceFromPinToPlayer(ItemOnMap);
        if (distance < allowedDistance) {
            this.IsWithinRange = true;
        }

        this.coinscore_text.text = ItemOnMap.Poi.attributes.reward.ToString();

        this.hasBeenVisited = ItemOnMap.HasBeenVisited;

        if (!this.IsWithinRange) {
            this.Headline.text = TourPoint.TourPointOverlayInactive.headline;
            this.Detail.text = TourPoint.TourPointOverlayInactive.subHeadline;
            this.InvestigateButton.style.opacity = 0f;
            this.ContinueButton.style.opacity = 0f;
            this.coinscore.style.opacity = 0.5f;
            this.overlay.style.backgroundColor = new Color(this.errorColor.r, this.errorColor.g, this.errorColor.b, 1f);
            this.InvestigateButton.clicked -= GotoTarget;
            this.ContinueButton.clicked -= ShowGameAssetMenu;

            this.checked_pin.style.display = DisplayStyle.None;
            this.pin.style.display = DisplayStyle.Flex;


        } else if (this.IsWithinRange) {

            this.overlay.style.backgroundColor = new Color(this.withinRangeColor.r, this.withinRangeColor.g, this.withinRangeColor.b, 1f);

            this.ContinueButton.RemoveFromClassList("inFokus");
            this.ContinueButton.AddToClassList("spiel");

            this.InvestigateButton.style.opacity = 1f;
            this.ContinueButton.style.opacity = 1f;
            this.coinscore.style.opacity = 0.5f;
            this.InvestigateButton.clicked += GotoTarget;
            this.ContinueButton.clicked += ShowGameAssetMenu;
            this.ContinueButton.clicked += Hide;
            this.Headline.text = TourPoint.TourPointOverlayInactive.headline;
            this.Detail.style.display = DisplayStyle.None;

            this.checked_pin.style.display = DisplayStyle.None;
            this.pin.style.display = DisplayStyle.Flex;
        }

        if (this.hasBeenVisited) {
            this.overlay.style.backgroundColor = new Color(this.withinRangeColor.r, this.withinRangeColor.g, this.withinRangeColor.b, 1f);
            this.ContinueButton.style.backgroundColor = new Color(this.withinRangeColor.r, this.withinRangeColor.g, this.withinRangeColor.b, 1f);

            this.ContinueButton.RemoveFromClassList("spiel");
            this.ContinueButton.AddToClassList("inFokus");

            this.Headline.text = TourPoint.TourPointOverlayActive.headline;

            this.InvestigateButton.clicked += GotoTarget;
            this.ContinueButton.clicked += ShowGameAssetMenu;
            this.InvestigateButton.style.opacity = 1f;
            this.ContinueButton.style.opacity = 1f;
            this.coinscore.style.opacity = 1f;
            this.checked_pin.style.display = DisplayStyle.Flex;
            this.pin.style.display = DisplayStyle.None;

        }

        this.distance.text = Math.Floor(distance).ToString() + " m";


        this.overlay.style.display = DisplayStyle.Flex;

        this.Target = ItemOnMap.Target;
 
        if (this.GameController.GetComponent<TourLoader>()) {
            this.GameController.GetComponent<TourLoader>().ChangeInteractionWithMap(false);
        }


        previousClassType = ItemOnMap.Poi.attributes.type;
    }

    public void Hide() {

        

        if (this.GameController.GetComponent<TourLoader>()) {
            this.GameController.GetComponent<TourLoader>().ChangeInteractionWithMap(true);
        }

        this.overlay.style.display = DisplayStyle.None;


        Debug.Log("hides element under menu" + this.overlay.style.display);
        //GameController.GetComponent<TourPointLoader>().DrawLineToNextPin(this.currentItemOnMap);

    }

    public void GotoTarget() {

        //crossGameManager.IsVisitingFromGame = true;
        crossGameManager.LastPinVisited = this.currentItemOnMap.ID;
        crossGameManager.LastSceneVisited = SceneManager.GetActiveScene().name;
        crossGameManager.isPlayingGame5 = true;

        if (this.currentItemOnMap.HasBeenVisited != true) {

            crossGameManager.ShouldUpdateScore = false;
            crossGameManager.ScoreToAdd = this.currentItemOnMap.Poi.attributes.reward;
            //crossGameManager.Score += this.currentItemOnMap.Poi.attributes.reward;
            crossGameManager.AddToScore(crossGameManager.colorToType("spiel"), this.currentItemOnMap.Poi.attributes.reward);

            crossGameManager.FundObjektIDToView = currentItemOnMap.Poi.attributes.fundobjekt.data.id;

            this.currentItemOnMap.HasBeenVisited = true;
            this.currentItemOnMap.HasBeenVisitedBy = this.GameController.GetComponent<TourLoader>().name;
            crossGameManager.currentColor = crossGameManager.colorToType(currentItemOnMap.Poi.attributes.type);


        }


        if (this.Target != "" || this.Target != "unknown") {
            if (this.currentItemOnMap.TargetID != 0) {
                crossGameManager.FundObjektIDToView = this.currentItemOnMap.TargetID;
            }
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        }
    }



    public void ShowGameAssetMenu() {


        print("shows menu");
        //if (!GameAssetMenu.activeSelf) {

        //    print("opens menu ");
        //    GameAssetMenu.SetActive(true);


        //    if (this.currentItemOnMap.HasBeenVisited != true) {
        //        this.currentItemOnMap.HasBeenVisited = true;
        //        this.currentItemOnMap.HasBeenVisitedBy = this.GameController.GetComponent<TourLoader>().name;
        //        crossGameManager.AddToScore(crossGameManager.colorToType("spiel"), currentItemOnMap.Poi.attributes.reward);
        //        this.GameController.GetComponent<TourLoader>().itemsVisited += 1;
        //    }


        //} else {
        //    GameAssetMenu.SetActive(false);
        //}


        //if (this.GameController.GetComponent<TourLoader>().itemsVisited > this.GameController.GetComponent<TourLoader>().ItemsOnMap.Count - 1) {
        //    Debug.Log("ready to create button for next step");
        //    GameAssetMenuPopUp.SetActive(true);

        //}



        if (this.currentItemOnMap.HasBeenVisited != true) {
            this.currentItemOnMap.HasBeenVisited = true;
            this.currentItemOnMap.HasBeenVisitedBy = this.GameController.GetComponent<TourLoader>().name;

            crossGameManager.AddToScore(crossGameManager.colorToType("spiel"), currentItemOnMap.Poi.attributes.reward);
            this.GameController.GetComponent<TourLoader>().itemsVisited += 1;
        }

        GameAssetMenu.ClosePopUpOPenUbersicht();
        GameAssetMenu.PopulateCards();

        this.GameController.GetComponent<TourLoader>().updatePOIS();

    }





}
