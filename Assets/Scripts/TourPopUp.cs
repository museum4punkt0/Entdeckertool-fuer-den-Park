
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using DG.Tweening;

using Mapbox.Unity.Location;


public class TourPopUp : MonoBehaviour
{
    public VisualElement overlay;
    public GroupBox OverlayContent;
    
    public Button Button1;
    public Button Button2;

    public Button ToggleOverlay;
    public VisualElement pin;
    public VisualElement checked_pin;
    public Label Headline;
    public Label Detail;
    public Label fund_amount;
    public Label coin_amount;
    public Label distance;
    public GroupBox coinscore;

    public GroupBox statusBar;
    public GroupBox TourStatusBar;

    public Label coinscore_text;

    public string Target;
    public string GameTarget;
    public Tour linkedTour;

    public string overrideType;
    public string Type;

    //Temporary Variables
    public string NewSceneName;
    public string MainSceneName;

    //Access GameController
    public GameObject GameController;
    public CrossGameManager crossGameManager;
    public ItemOnMap currentItemOnMap;
    public ConnectPoiToGame connectPoiToGame;

    //states
    public bool IsWithinRange = false;
    public bool hasBeenVisited = false;
    public bool isInARScene = false;

    public float AllowedDistanceToRange = 30;
   


    //UI Coloring
    //create override color in scenes to override overlay color defined by POI type
    public Color errorColor;
    public string overRideColor;
    private Color withinRangeColor;
    private string previousClassType;
    public Sprite pin_icon;
    public Sprite game_icon;
    public Sprite tour_icon;
    public Sprite AR_icon;
    public Sprite AR_icon_visited;

    public UIItemViewController uIItemViewController;
    public Sprite ARButtonImage;

    //Changing PopUp Capabilities if in Game Scene
    public bool IsInGame;

    
    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    /// Run start as enumerator to wait for first frame and be able to get all properties from UI elements
    private IEnumerator Start() {
        yield return new WaitForEndOfFrame();
        AssignPopUpUIElements();

        LocatePOItoGameConnector();

    }
    private void LocatePOItoGameConnector() {
        if (GameObject.FindGameObjectWithTag("GameController") && GameObject.FindGameObjectWithTag("GameController").GetComponent<ConnectPoiToGame>()) {

            Debug.Log("Has connected POI to Game");
            connectPoiToGame = GameObject.FindGameObjectWithTag("GameController").GetComponent<ConnectPoiToGame>();
        }
    }
    public void AssignPopUpUIElements() {

        var root = this.GetComponent<UIDocument>().rootVisualElement;

        this.overlay = root.Q<VisualElement>("GameOverlay");
        this.overlay.style.display = DisplayStyle.None;

        this.pin = root.Q<VisualElement>("pin");
        this.checked_pin = root.Q<VisualElement>("checked_pin");
        this.checked_pin.style.display = DisplayStyle.None;

        this.statusBar = root.Q<GroupBox>("StatusBar");
        this.TourStatusBar = root.Q<GroupBox>("TourStatusBar");
        this.Button1 = root.Q<Button>("ActionButton_ContinueTour");
        this.Button2 = root.Q<Button>("ActionButton_GoToTarget");
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

        LocatePOItoGameConnector();

        if (this.GameController != null && this.GameController.GetComponent<TourLoader>()) {
            this.GameController.GetComponent<TourLoader>().ChangeInteractionWithMap(false);
        }

        this.currentItemOnMap = ItemOnMap;
        this.Target = ItemOnMap.Target;
        this.statusBar.style.display = DisplayStyle.Flex;

        TourPoint TourPoint = ItemOnMap.Poi.attributes.tourPoint;
        //ToDo: _arLocationProvider is not set, nor checked if it is set. Exception needs to be handled
        
        
        double distance = crossGameManager.CalculateDistanceFromPinToPlayer(ItemOnMap);
        //double distance = 25;

        if (distance < AllowedDistanceToRange) {
            this.IsWithinRange = true;
        } else {
            this.IsWithinRange = false;
        }

        crossGameManager.ErrorLog(ItemOnMap.Name +"____inRange:" + this.IsWithinRange);

        this.distance.text = Math.Floor(distance).ToString() + " m";

        //this.Target = ItemOnMap.Target;

        string currentClassType = "";

        if (ItemOnMap.Poi.attributes.type != null && ItemOnMap.Poi.attributes.type != "") {
            currentClassType = ItemOnMap.Poi.attributes.type;

            Debug.Log("CHECKS CLASS/from poi.type" + currentClassType);
        } else {
            currentClassType = "spiel";
            Debug.Log("CHECKS CLASS/default" + currentClassType);

        }


        if (overRideColor == null || overRideColor == "") {
            if (ItemOnMap.Poi.attributes.type != null || ItemOnMap.Poi.attributes.type != "") {
                this.withinRangeColor = crossGameManager.colorToType(ItemOnMap.Poi.attributes.type);
            }

        } else {
            Color color;
            ColorUtility.TryParseHtmlString(overRideColor, out color);
            this.withinRangeColor = color;
        }

        if (overrideType == null || overrideType == "") {
            if (ItemOnMap.Poi.attributes.type != null || ItemOnMap.Poi.attributes.type != "") {
                Type = ItemOnMap.Poi.attributes.type;
            } else {
                Type = "fundObjekt";
            }

        } else {

            Type = overrideType;

        }



        this.coinscore_text.text = ItemOnMap.Poi.attributes.reward.ToString();
        this.hasBeenVisited = ItemOnMap.HasBeenVisited;

        this.Button2.style.display = DisplayStyle.None;
        this.Button1.style.display = DisplayStyle.None;
        if (this.TourStatusBar != null) {
            this.TourStatusBar.style.display = DisplayStyle.None;
        }

        if (!this.IsWithinRange) {
            this.Headline.text = TourPoint.TourPointOverlayInactive.headline;
            this.Detail.text = TourPoint.TourPointOverlayInactive.subHeadline;
          
            this.coinscore.style.opacity = 0.5f;

            this.overlay.style.backgroundColor = new Color(this.errorColor.r, this.errorColor.g, this.errorColor.b, 1f);

            //this.Button2.clicked -= GotoTarget("unknown");
            //this.Button1.clicked -= Hide;

            this.checked_pin.style.display = DisplayStyle.None;
            this.pin.style.display = DisplayStyle.Flex;


        } else if (this.IsWithinRange) {

            if (ItemOnMap.HasBeenVisited != true) {

                //crossGameManager.ShouldUpdateScore = true;
                //set ShouldUpdateScore to false to remove coinanimation, set Score manually from here
                crossGameManager.AddSecretlyToScore(crossGameManager.colorToType("tour"), this.currentItemOnMap.Poi.attributes.reward);
                crossGameManager.ShouldUpdateScore = false;

                crossGameManager.ScoreToAdd = this.currentItemOnMap.Poi.attributes.reward;

                this.currentItemOnMap.HasBeenVisited = true;
                if (this.GameController != null && this.GameController.GetComponent<TourLoader>()) {
                    this.GameController.GetComponent<TourLoader>().itemsVisited += 1;

                    this.currentItemOnMap.HasBeenVisitedBy = this.GameController.GetComponent<TourLoader>().name;
                } else {
                    this.currentItemOnMap.HasBeenVisitedBy = SceneManager.GetActiveScene().name;
                }
                crossGameManager.currentColor = crossGameManager.colorToType(currentItemOnMap.Poi.attributes.type);


            }


            this.overlay.style.backgroundColor = new Color(this.withinRangeColor.r, this.withinRangeColor.g, this.withinRangeColor.b, 1f);
            this.coinscore.style.opacity = 0.5f;
            this.Headline.text = TourPoint.TourPointOverlayActive.headline;


            this.Detail.text = TourPoint.TourPointOverlayActive.subHeadline;


            this.checked_pin.style.display = DisplayStyle.None;
            
            if (Type == "spiel") {
                this.pin.style.backgroundImage = new StyleBackground(game_icon);
                this.statusBar.style.display = DisplayStyle.None;


            }
            else if (Type == "panorama") {
                this.pin.style.backgroundImage = new StyleBackground(AR_icon);
                this.statusBar.style.display = DisplayStyle.Flex;


            } else if (Type == "tour") {
                this.pin.style.backgroundImage = new StyleBackground(tour_icon);
                this.statusBar.style.display = DisplayStyle.None;
                crossGameManager.tourIDtoStart = this.linkedTour.id;
                this.Target = "ParkTour";


                this.linkedTour = crossGameManager.Tours.Find(tour => tour.id.ToString() == ItemOnMap.Target);

                float netto_reward = 0;
                this.linkedTour.attributes.point_of_interests.data.ForEach((Poi point) => {
                    netto_reward += point.attributes.reward;
                });

                if (this.TourStatusBar != null) {
                    this.TourStatusBar.style.display = DisplayStyle.Flex;

                    this.TourStatusBar.Q<Label>("fund_amount").text = this.linkedTour.attributes.point_of_interests.data.Count + " funde";
                    this.TourStatusBar.Q<Label>("coin_amount").text = netto_reward + "";
                    this.TourStatusBar.Q<Label>("time").text = this.linkedTour.attributes.tourTeaser.duration.ToString();
                }






            } else {


                if (this.statusBar != null) {
                    this.statusBar.style.display = DisplayStyle.Flex;
                }

                if (this.TourStatusBar != null) {
                    this.TourStatusBar.style.display = DisplayStyle.None;
                }

                this.pin.style.backgroundImage = new StyleBackground(pin_icon);

            }

            this.pin.style.display = DisplayStyle.Flex;

            if (TourPoint.buttons.Count > 1) {
                this.Button2.Q<Label>("ButtonText").text = TourPoint.buttons[1].text;
                this.Button2.style.opacity = 1f;
                this.Button2.style.display = DisplayStyle.Flex;
                AssignFunctionToButton(Button2, TourPoint.buttons[1]);
                this.Button2.RemoveFromClassList(previousClassType);
                this.Button2.AddToClassList(currentClassType);

            } 
            
            if (TourPoint.buttons.Count > 0) {
                this.Button1.Q<Label>("ButtonText").text = TourPoint.buttons[0].text;
                this.Button1.style.opacity = 1f;
                this.Button1.style.display = DisplayStyle.Flex;
                this.Button1.clicked += Hide;

                Debug.Log("CHECK BUTTON STYLE___ prev:" + previousClassType + "_____current:" + currentClassType);
                this.Button1.RemoveFromClassList(previousClassType);
                this.Button1.AddToClassList(currentClassType);

                AssignFunctionToButton(Button1, TourPoint.buttons[0]);

            } else {

                this.Button1.Q<Label>("ButtonText").text = crossGameManager.configurationData.attributes.PopUpContinueDefaultButton;
                this.Button1.style.opacity = 1f;

                if (connectPoiToGame != null) {
                    if (Type == "spiel") {
                        this.Button1.clicked += Hide;
                        this.Button1.clicked += connectPoiToGame.StartGame;


                    } else {
                        this.Button1.clicked -= connectPoiToGame.StartGame;
                        this.Button1.clicked += Hide;

                    }

                } else {
                    this.Button1.clicked += Hide;
                }
          

                this.Button1.RemoveFromClassList(previousClassType);

                this.Button1.AddToClassList(currentClassType);

                Debug.Log("BUTTON CLASS____prev:" + previousClassType + "______next:" + currentClassType);

               
                this.Button2.Q<Label>("ButtonText").text = crossGameManager.configurationData.attributes.PopUpFundDefaultButton;
                this.Button2.style.opacity = 1f;
                this.Button2.clicked += GotoTarget;


                this.Button2.RemoveFromClassList(previousClassType);
                this.Button2.AddToClassList(currentClassType);

                this.Button2.style.display = DisplayStyle.Flex;
                this.Button1.style.display = DisplayStyle.Flex;
            }


    
        
        } 
        
        if (this.hasBeenVisited) {
                this.overlay.style.backgroundColor = new Color(this.withinRangeColor.r, this.withinRangeColor.g, this.withinRangeColor.b, 1f);

            if (TourPoint.buttons.Count > 1) {
                this.Button2.Q<Label>("ButtonText").text = TourPoint.buttons[1].text;
                this.Button2.style.opacity = 1f;
                AssignFunctionToButton(Button2, TourPoint.buttons[1]);

                //if (!isInARScene && TourPoint.buttons[1].type != "ArMode") {
                //    this.Button2.style.display = DisplayStyle.Flex;
                //} else {
                //    this.Button2.style.display = DisplayStyle.None;

                //}

            }

            if (TourPoint.buttons.Count > 0) {
                this.Button1.Q<Label>("ButtonText").text = TourPoint.buttons[0].text;
                this.Button1.style.opacity = 1f;
                this.Button1.clicked += Hide;

                this.Button1.RemoveFromClassList(previousClassType);
                this.Button1.AddToClassList(currentClassType);

                AssignFunctionToButton(Button1, TourPoint.buttons[0]);

                //if (!isInARScene && TourPoint.buttons[0].type != "ArMode") {
                //    this.Button1.style.display = DisplayStyle.Flex;
                //} else {
                //    this.Button1.style.display = DisplayStyle.None;

                //}



            } else {

                this.Button1.Q<Label>("ButtonText").text = crossGameManager.configurationData.attributes.PopUpContinueDefaultButton;

                Debug.Log("checks type" + Type);

                if (connectPoiToGame != null) {
                    if (Type == "spiel") {
                        this.Button1.clicked -= Hide;
                        this.Button1.clicked += connectPoiToGame.StartGame;
                        Debug.Log("should attach start game to poi");
                    } else {
                        this.Button1.clicked -= connectPoiToGame.StartGame;
                        this.Button1.clicked += Hide;

                    }
                } else {
                    this.Button1.clicked += Hide;
                }
                this.Button1.style.display = DisplayStyle.Flex;

                this.Button1.RemoveFromClassList(previousClassType);
                this.Button1.AddToClassList(currentClassType);

                this.Button2.Q<Label>("ButtonText").text = crossGameManager.configurationData.attributes.PopUpFundDefaultButton;
                this.Button2.style.display = DisplayStyle.Flex;

                this.Button2.clicked += GotoTarget;
            }


            this.Headline.text = TourPoint.TourPointOverlayActive.headline;

          
            this.coinscore.style.opacity = 1f;

            if (Type == "panorama") {
                this.checked_pin.style.backgroundImage = new StyleBackground(AR_icon_visited);
            }

            this.checked_pin.style.display = DisplayStyle.Flex;
            this.pin.style.display = DisplayStyle.None;  
        }

        if (this.GameController != null && this.GameController.GetComponent<TourLoader>()) {
      
            this.GameController.GetComponent<TourLoader>().updatePOIS();
        }


        this.overlay.style.display = DisplayStyle.Flex;
        previousClassType = ItemOnMap.Poi.attributes.type;
    }

    public void Hide() {

        Debug.Log("has Hide attached to button");

        if (this.GameController != null && this.GameController.GetComponent<TourLoader>()) {
            this.GameController.GetComponent<TourLoader>().ChangeInteractionWithMap(true);

            GameController.GetComponent<TourLoader>().CanInteractWithMap = true;

        }

        this.overlay.style.display = DisplayStyle.None;


    }



    public void AssignFunctionToButton(Button button, OverlayButton overlayButton) {

        print(overlayButton.type + overlayButton.target);
        button.clickable = null;
        button.style.display = DisplayStyle.Flex;
        button.Q<VisualElement>("Background").style.backgroundColor = new Color(0, 0, 0, 0);
        
        if (overlayButton.type == "fund") {
            ///button click redirects to 1) fundobject item page or 2) (in case of target override in TourPoint Object) to 
            if (overlayButton.target != "" && overlayButton.target != null) {
                button.clicked += delegate {
                    GotoCustomTarget(overlayButton.target);
                };
            } else {

                print("assigns GOTOTARGET to button" + overlayButton.type);
                this.GameTarget = currentItemOnMap.Target;
                button.clicked += GotoTarget;
            }

        } else if (overlayButton.type == "spiel") {
            ///button opens game (not to be used for pois inside game-scenes)
            
            if (!IsInGame) {
                //this.GameTarget = overlayButton.target;
                if (overlayButton.target != "" && overlayButton.target != null) {
                    button.clicked += delegate {
                        GotoCustomTarget(overlayButton.target);
                    };
                } else {
                    this.GameTarget = currentItemOnMap.Target;
                    button.clicked += GotoGameTarget;
                }
          

            } else {
                if (!connectPoiToGame) {
                
                } 
                button.clicked += connectPoiToGame.StartGame;
                button.clicked += Hide;
            }
        } else if (overlayButton.type == "tour") {
         
            crossGameManager.tourIDtoStart = this.linkedTour.id;
            this.GameTarget = "ParkTour";
            button.clicked += GotoGameTarget;

        } else if (overlayButton.type == "arMode") {

            button.Q<VisualElement>("ButtonIcon").style.backgroundImage = new StyleBackground(ARButtonImage);
            button.Q<VisualElement>("Background").style.unityBackgroundImageTintColor = crossGameManager.colorToType("panorama");

            print("assing button funciton" + overlayButton.target);

            if (isInARScene) {
                button.style.display = DisplayStyle.None;


            } else if (overlayButton.target == "Maske") {
                this.GameTarget = "ARFilterScene";
                //crossGameManager.IsVisitingFromTour = true;
                button.clicked += GotoGameTarget;


            } else if (overlayButton.target == "Maske+Helmet") {
                this.GameTarget = "ARFilterSceneHelmet";
                //crossGameManager.IsVisitingFromTour = true;
                button.clicked += GotoGameTarget;


            } else {
                if (this.currentItemOnMap.Poi.attributes.illustration.data.id != 0) {
                    crossGameManager.illustration = this.currentItemOnMap.Poi.attributes.illustration.data.attributes;
                    if (this.currentItemOnMap.Poi.attributes.illustration.data.id != 0) {
                        crossGameManager.illustration = this.currentItemOnMap.Poi.attributes.illustration.data.attributes;

                    }
                }

                crossGameManager.objectToViewInAR_ID = this.currentItemOnMap.ID;

                this.GameTarget = "360_Illustrations";
                //crossGameManager.IsVisitingFromTour = true;
                button.clicked += GotoGameTarget;
            }
      
           

        } else {
            button.clicked += Hide;
        }

        
    }
    public void GotoCustomTarget(string target) {
        if (target.Contains("item") && SceneManager.GetActiveScene().name == "MainScene" && uIItemViewController != null) {

            uIItemViewController.navigate(target.Split(":")[0], target.Split(":")[1]);
            print(target.Split(":")[0] + "________" + target.Split(":")[1]);
            Hide();
        } else {
            SceneManager.LoadScene(target, LoadSceneMode.Single);
        }
    }
    public void GotoTarget() {

        //crossGameManager.IsVisitingFromTour = true;

        crossGameManager.IsVisitingFromPOI = true;
        crossGameManager.LastPinVisited = this.currentItemOnMap.ID;
        
        if (SceneManager.GetActiveScene().name != "360_Illustrations") {
            crossGameManager.LastSceneVisited = SceneManager.GetActiveScene().name;
        }

        crossGameManager.FundObjektIDToView = currentItemOnMap.Poi.attributes.fundobjekt.data.id;


       
        if (this.Target != "" || this.Target != "unknown") {

            Debug.Log("NAVIGATING TO MAIN - target" + this.currentItemOnMap.Name + " targetID(fund):" + this.currentItemOnMap.TargetID +"___poi ID:"+ this.currentItemOnMap.Poi.id);
            if (this.currentItemOnMap.TargetID != 0) {
                crossGameManager.FundObjektIDToView = this.currentItemOnMap.TargetID;
            }

            if (SceneManager.GetActiveScene().name == "ParkTour") {
                crossGameManager.IsVisitingFromTour = true;
            }


            if (this.Target.Contains("item") && SceneManager.GetActiveScene().name == "MainScene" && uIItemViewController != null) {

                uIItemViewController.navigate(this.Target.Split(":")[0], this.Target.Split(":")[1]);
                print(this.Target.Split(":")[0] + "________" + this.Target.Split(":")[1]);
                Hide();
            } else {
                SceneManager.LoadScene(this.Target, LoadSceneMode.Single);
            }
        }

    }
    public void GotoGameTarget() {

        if (this.currentItemOnMap.HasBeenVisited != true) {

            crossGameManager.ShouldUpdateScore = true;
            crossGameManager.currentColor = crossGameManager.colorToType(currentItemOnMap.Poi.attributes.type);
            crossGameManager.ScoreToAdd = this.currentItemOnMap.Poi.attributes.reward;
            this.currentItemOnMap.HasBeenVisited = true;
            this.currentItemOnMap.HasBeenVisitedBy = this.GameController.GetComponent<TourLoader>().name;
        }

        if (SceneManager.GetActiveScene().name != "360_Illustrations") {
            crossGameManager.LastSceneVisited = SceneManager.GetActiveScene().name;
        }

        if (SceneManager.GetActiveScene().name == "ParkTour") {
            crossGameManager.IsVisitingFromTour = true;
        }


        if (this.GameTarget != "" || this.GameTarget != "unknown") {
            //mainscene:21
            SceneManager.LoadScene(this.GameTarget, LoadSceneMode.Single);
        }
    }


    public Action<Location> UpdateDistanceDisplayOnCGMDistanceUpdate() {

        double distance = crossGameManager.CalculateDistanceFromPinToPlayer(this.currentItemOnMap);
        AssignPopUpUIElements();
        this.distance.text = Math.Floor(distance).ToString() + " m";

        crossGameManager.ErrorLog("pos:" + crossGameManager.LocationProvider.CurrentLocation.LatitudeLongitude.ToString());



        return null;
        //distanceFunctions.Distance(LocationProvider.CurrentLocation, POI_Location);


    }









}
