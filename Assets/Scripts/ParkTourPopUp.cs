
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using DG.Tweening;
using ARLocation;

public class ParkTourPopUp : MonoBehaviour
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


    public string Target;




    //Temporary Variables
    public string NewSceneName;
    public string MainSceneName;


    //Access GameController
    public GameObject GameController;
    public CrossGameManager crossGameManager;
    public ItemOnMap currentItemOnMap;

    public bool IsWithinRange = false;
    public bool hasBeenVisited = false;

    public Color errorColor;
    public Color withinRangeColor;


    private ARLocationProvider _arLocationProvider;
    public List<ItemOnMap> allItemsOnTour = new List<ItemOnMap>();


    
    
    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        allItemsOnTour = GameController.GetComponent<TourPointLoader>().ItemsOnMap;
        this._arLocationProvider = ARLocationProvider.Instance;

    }
    /// Run start as enumerator to wait for first frame and be able to get all properties from UI elements
    private IEnumerator Start() {

        Debug.Log("pop up awakes");

        yield return new WaitForEndOfFrame();
        AssignPopUpUIElements();

    }

    public void AssignPopUpUIElements() {

        Debug.Log("begins pop up assignment");


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


    }

    public void Show(ItemOnMap ItemOnMap) {
        ////neeeds to be configured when we have the point_of_interest data correctly in
        ///
        double distance = CalculateDistanceFromPinToPlayer(ItemOnMap.TourPoint);
   

        if (distance < 25) {
            //this.IsWithinRange = true;
        }
        TourPoint TourPoint = ItemOnMap.TourPoint;
        this.currentItemOnMap = ItemOnMap;

        this.coinscore_text.text = ItemOnMap.TourPoint.point_of_interest.data.attributes.reward.ToString();

        this.hasBeenVisited = ItemOnMap.HasBeenVisited;

        if (!this.IsWithinRange) {
            this.Headline.text = TourPoint.TourPointOverlayInactive.headline;
            this.Detail.text = TourPoint.TourPointOverlayInactive.subHeadline;
            this.InvestigateButton.style.opacity = 0f;
            this.ContinueButton.style.opacity = 0f;
            this.coinscore.style.opacity = 0.5f;
            this.overlay.style.backgroundColor = new Color(this.errorColor.r, this.errorColor.g, this.errorColor.b, 1f);
            this.InvestigateButton.clicked -= GotoTarget;
            this.ContinueButton.clicked -= Hide;


        } else if (this.IsWithinRange) {
            this.overlay.style.backgroundColor = new Color(this.withinRangeColor.r, this.withinRangeColor.g, this.withinRangeColor.b, 1f);
            this.InvestigateButton.style.opacity = 1f;
            this.ContinueButton.style.opacity = 1f;
            this.coinscore.style.opacity = 0.5f;
            this.InvestigateButton.clicked += GotoTarget;
            this.ContinueButton.clicked += Hide;
            this.Headline.text = TourPoint.TourPointOverlayInactive.headline;
            this.Detail.style.display = DisplayStyle.None;
        }

        if (this.hasBeenVisited) {
            this.overlay.style.backgroundColor = new Color(this.withinRangeColor.r, this.withinRangeColor.g, this.withinRangeColor.b, 1f);
            this.Headline.text = TourPoint.TourPointOverlayActive.headline;

            this.InvestigateButton.clicked += GotoTarget;
            this.ContinueButton.clicked += Hide;
            this.InvestigateButton.style.opacity = 1f;
            this.ContinueButton.style.opacity = 1f;
            this.coinscore.style.opacity = 1f;
            this.checked_pin.style.display = DisplayStyle.Flex;
            this.pin.style.display = DisplayStyle.None;

        }

        this.distance.text = distance.ToString() + " m";
        this.overlay.style.display = DisplayStyle.Flex;

        this.Target = ItemOnMap.Target;
 
        if (this.GameController.GetComponent<TourPointLoader>()) {
            this.GameController.GetComponent<TourPointLoader>().ChangeInteractionWithMap(false);
        }
    }

    public void Hide() {

        Debug.Log("hides element");

        if (this.GameController.GetComponent<TourPointLoader>()) {
            this.GameController.GetComponent<TourPointLoader>().ChangeInteractionWithMap(true);
        }

        this.overlay.style.display = DisplayStyle.None;

        GameController.GetComponent<TourPointLoader>().DrawLineToNextPin(this.currentItemOnMap);

    }

    public void GotoTarget() {

        if (this.currentItemOnMap.HasBeenVisited != true) {
            crossGameManager.ChangeStateOfItemOnMap(this.currentItemOnMap.ID, this.GameController.GetComponent<TourPointLoader>().ID);
            crossGameManager.ShouldUpdateScore = true;
            crossGameManager.ScoreToAdd = this.currentItemOnMap.TourPoint.point_of_interest.data.attributes.reward;
        } else {
            crossGameManager.ShouldUpdateScore = false;
            crossGameManager.ScoreToAdd = 0;
        }
      


        crossGameManager.IsVisitingFromTour = true;
        crossGameManager.LastPinVisited = this.currentItemOnMap.ID;
        crossGameManager.LastSceneVisited = SceneManager.GetActiveScene().name;



    


        Debug.Log("GO TO TARGET" + this.Target);


        if (this.Target != "" || this.Target != "unknown") {
            SceneManager.LoadScene(this.Target, LoadSceneMode.Single);
        }
    }


    public double CalculateDistanceFromPinToPlayer(TourPoint TourPoint) {
        double lat1 = this._arLocationProvider.CurrentLocation.latitude,
        lng1 = this._arLocationProvider.CurrentLocation.longitude,
        lat2 = 52.407341,
        lng2 = 8.127440;
        //lat2 = TourPoint.point_of_interest.attributes.lat;
        //lng2 = TourPoint.point_of_interest.attributes.lng;

        double R = 6371000;
        double p1 = lat1 * Math.PI / 180;
        double p2 = lat2 * Math.PI / 180;
        double dp = (lat2 - lat1) * Math.PI / 180;
        double dL = (lng2 - lng1) * Math.PI / 180;
        double a = Math.Sin(dp / 2) * Math.Sin(dp / 2) +
                   Math.Cos(p1) * Math.Cos(p2) *
                   Math.Sin(dL / 2) * Math.Sin(dL / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = R * c;

        return distance;
    }




}
