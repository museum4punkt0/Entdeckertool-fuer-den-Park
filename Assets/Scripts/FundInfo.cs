
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using DG.Tweening;
using ARLocation;

public class FundInfo: MonoBehaviour
{

    public string Target;
    public string NextScene;
    
    public VisualElement overlay;
    
    //standard greeting elements
    public Label Headline;
    public Label Greeting;
    public GroupBox coinscore;
    public Label coinscore_text;
    public Button Button1;


    //content elements
    public GroupBox Content; 
    public Button Button2;
    public VisualElement displayimage;
    public GamePopUp gamePopUp;

    //Access GameController
    public GameObject GameController;
    public CrossGameManager crossGameManager;
    public ItemOnMap currentItemOnMap;
    public ConnectPoiToGame connectPoiToGame;

    //go to fundobject
    public bool IsVisitingFromTour = true;
    public bool IsVisitingFromPOI = true;


    //UI Coloring
    //create override color in scenes to override overlay color defined by POI type
    public Color errorColor;
    private Color withinRangeColor;
    private string previousClassType;




    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
   
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

        
        this.displayimage = root.Q<VisualElement>("image");
      
        this.Button1 = root.Q<Button>("ActionButton_ContinueTour");
        this.Button2 = root.Q<Button>("ActionButton_GoToFundObjekt");
        this.Button2.clicked += GotoTarget;

        this.Headline = root.Q<Label>("HEADLINE");
        this.Greeting = root.Q<Label>("Greeting");
        this.Content = root.Q<GroupBox>("Content");

        //coinscore
        this.coinscore = root.Q<GroupBox>("CoinScore");
        this.coinscore_text = root.Q<Label>("CoinScore");


        Hide();

    }


    public void setImage(string imgUrl) {
        
        //this.displayimage.style.backgroundImage = new StyleBackground(image);
        Davinci.get().load(imgUrl).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(this.displayimage).start();
    }

    public void Show(ItemOnMap ItemOnMap, bool ShouldShowContent, string Greeting, string Reward, string Headline, string Button1Text, bool IsLastButtonInGame) {


        if (gamePopUp != null) {
            gamePopUp.StopInteractionWithUIElements(false);
        }

        string currentClassType = "";
        if (ItemOnMap.ID != 0) {
            if (ItemOnMap.Poi.attributes.type != null || ItemOnMap.Poi.attributes.type != "") {
                currentClassType = ItemOnMap.Poi.attributes.type;
            } else {
                currentClassType = "spiel";
            }
            this.currentItemOnMap = ItemOnMap;

            //if (this.currentItemOnMap.Poi.attributes.fundobjekt.data.id != 0) {
            //    crossGameManager.FundObjektIDToView = this.currentItemOnMap.Poi.attributes.fundobjekt.data.id;
            //}
           

            TourPoint TourPoint = ItemOnMap.Poi.attributes.tourPoint;

            this.Target = ItemOnMap.Target;


        }




        this.coinscore_text.text = Reward;
        this.Headline.text = Headline;
        this.Greeting.text = Greeting;
        this.Button1.Q<Label>("ButtonText").text = Button1Text;
        this.Button1.style.opacity = 1f;


        if (IsLastButtonInGame) {
            this.Button1.clicked -= Hide;
            this.Button1.clicked += GoToNextScene;
        } else {
            this.Button1.clicked += Hide;
            this.Button1.clicked -= GoToNextScene;
        }

        if (!ShouldShowContent) {
            this.Content.style.display = DisplayStyle.None;
        } else {
            this.Content.style.display = DisplayStyle.Flex;
        }

        this.overlay.style.display = DisplayStyle.Flex;
    }

    public void Hide() {
        crossGameManager.ErrorLog("hides element");
        this.overlay.style.display = DisplayStyle.None;

        if (gamePopUp != null) {

            if (gamePopUp.GameController.GetComponent<startGame3_AR_Ready>() && gamePopUp.GameController.GetComponent<startGame3_AR_Ready>().state == "state3") {
                gamePopUp.GameController.GetComponent<startGame3_AR_Ready>().part2.SetActive(true);
            }


        }
        if (GameController != null && GameController.GetComponent<startGame3_AR_Ready>()) {
            if (GameController.GetComponent<startGame3_AR_Ready>().state == "state3") {
                GameController.GetComponent<startGame3_AR_Ready>().radialWheel2.ActivateRadialMenu("");
            }
        }
    }


    public void GotoTarget() {

        print("goes towards target");
        crossGameManager.IsVisitingFromTour = IsVisitingFromTour;
        crossGameManager.IsVisitingFromPOI = IsVisitingFromPOI;
        crossGameManager.LastPinVisited = this.currentItemOnMap.ID;
        crossGameManager.LastSceneVisited = SceneManager.GetActiveScene().name;

        if (this.currentItemOnMap.Poi.attributes.fundobjekt.data.id != 0) {
            crossGameManager.FundObjektIDToView = this.currentItemOnMap.Poi.attributes.fundobjekt.data.id;
        }

        if (this.currentItemOnMap.HasBeenVisited != true) {

            crossGameManager.ShouldUpdateScore = true;
            crossGameManager.ScoreToAdd = this.currentItemOnMap.Poi.attributes.reward;

            if (currentItemOnMap.Name != "") {
                crossGameManager.currentColor = crossGameManager.colorToType(currentItemOnMap.Poi.attributes.type);
            } else {
                crossGameManager.currentColor = crossGameManager.colorToType("spiel");

            }

        }


        if (this.Target != "" || this.Target != "unknown") {
            SceneManager.LoadScene(this.Target, LoadSceneMode.Single);

            Debug.Log("Heading to mainscene with  FUNDID" + crossGameManager.FundObjektIDToView);
        }
    }


    public void GoToNextScene() {

        if (this.NextScene != "" || this.NextScene != "unknown") {
            SceneManager.LoadScene(this.NextScene, LoadSceneMode.Single);
        }

    }







}
