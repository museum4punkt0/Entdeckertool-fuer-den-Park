using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using Services;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System;
using System.Linq;
using KDTree;
using Mapbox.Unity.Location;

[CreateAssetMenu]
public class CrossGameManager : MonoBehaviour
{
    public static CrossGameManager crossGameManager;
    public StrapiService strapiService;

    [SerializeField]
    public List<ItemOnMap> PinsFromPointOfInterest;

    [SerializeField]
    public List<Notification> notifications;

    [SerializeField]
    public bool IsVisitingFromPOI = false;

    [SerializeField]
    public bool IsVisitingFromTour = false;

    [SerializeField]
    public bool IsVisitingFromGame = false;

    [SerializeField]
    public bool IsVisitingFromGame4AfterCompleted = false;

    [SerializeField]
    public bool IsVisitingFromDetectorToFilter = false;

    [SerializeField]
    public bool IsVisitingFromDetectorToListe = false;

    [SerializeField]
    public bool IsVisitingFromDetector = false;

    [SerializeField]
    public bool IsVisitingFrom360 = false;

    [SerializeField]
    public bool IsVisitingFromMask = false;

    [SerializeField]
    public string currentContent = "";

    [SerializeField]
    public int LastPinVisited;

    [SerializeField]
    public string LastSceneVisited = "";

    [SerializeField]
    public int FundObjektIDToView;

    [SerializeField]
    public int tourIDtoStart;

    [SerializeField]
    public Illustration illustration;
    [SerializeField]
    public int objectToViewInAR_ID;

    public GameObject[] mapElementsToDestroy;

    [SerializeField]
    public int numberscore;

    [SerializeField]
    public int ScoreToAdd = 0;

    [SerializeField]
    public Color currentColor;
    
    [SerializeField]
    public bool ShouldUpdateScore = false;

    public GameObject CoinAnimation;

    public AbstractMap map;
    private bool hasMapAssets;


    [SerializeField]
    public List<Tour> Tours;

    [SerializeField]
    public List<ItemOnMap> AllItemsOnMap;

    [SerializeField]
    public bool hasCompletedGame1 = false;
    [SerializeField]
    public bool hasCompletedGame2= false;
    [SerializeField]
    public bool hasCompletedGame3 = false;
    [SerializeField]
    public bool hasCompletedGame4 = false;
    [SerializeField]
    public bool hasCompletedGame5 = false;
    [SerializeField]
    public bool isPlayingGame5 = false;


    [SerializeField]
    public ILocationProvider LocationProvider;
    public Vector2d _location;
    
    public float AllowedAccuracyMargin;
    public double accuracy;
    public bool hasARHeadingAccuracy;

    [SerializeField]
    public string game1_currentPhase;

    [SerializeField]
    public List<Game4DataContent> Game4PhaseStorage;
    public int Game4CurrentScore;

    [SerializeField]
    public int fundObjektIDtoImageZoom = 0;

    public Score score;

    [SerializeField]
    public ConfigurationData configurationData;


    [SerializeField]
    public GameObject locationProviderFactoryGO;


    public bool loadsWithNotification;
    public CrossGameManager () {
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake() {
        this.strapiService = new StrapiService(Application.persistentDataPath + "/cachedRequests");
        if (this.strapiService.cachingService.hasKey("score")) {
            this.score = Score.CreateFromJSON(this.strapiService.cachingService.getValue("score"));
        } 
        GetData();

    }

    private void Update() {
        if (Application.platform != RuntimePlatform.WindowsEditor) {

            if (SceneManager.GetActiveScene().name != "IntroScene") {

                if (LocationProvider != null) {
                    _location = LocationProvider.CurrentLocation.LatitudeLongitude;
                } else {
                    LocationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
                }
            }
        

        }

    }

    public void ReStart() {
        AllItemsOnMap.Clear();
        notifications.Clear();
        Tours.Clear();
        GetData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private void StartVariables() {

        PinsFromPointOfInterest = new List<ItemOnMap>();
        Tours = new List<Tour>();
        AllItemsOnMap = new List<ItemOnMap>();
        Game4PhaseStorage = new List<Game4DataContent>();
        IsVisitingFromPOI = false;
        IsVisitingFromTour = false;
        IsVisitingFromGame = false;
        IsVisitingFromGame4AfterCompleted = false;
        IsVisitingFromDetectorToFilter = false;
        IsVisitingFromDetectorToListe = false;
        IsVisitingFromDetector = false;
        IsVisitingFrom360 = false;
        IsVisitingFromMask = false;
        currentContent = "";
        LastSceneVisited = "";
        ScoreToAdd = 0;
        hasCompletedGame1 = false;
        hasCompletedGame2 = false;
        hasCompletedGame3 = false;
        hasCompletedGame4 = false;
        hasCompletedGame5 = false;
    }

    private void GetData() {
        if (crossGameManager == null) {
            crossGameManager = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }

        StartVariables();

        //get data
        StartCoroutine(this.strapiService.getPointOfInterests((StrapiResponse<Poi> res) => {


            List<Poi> Pois = res.data;


            int index = 0;
            foreach (Poi poi in Pois) {

                ItemOnMap itemOnMap = new ItemOnMap();
                itemOnMap.Name = poi.attributes.title;
                itemOnMap.ID = poi.id;
                itemOnMap.Poi = poi;

                itemOnMap.HasBeenVisited = false;
                if (this.score != null) {
                    this.score.visitedPois.ForEach(savedPoi => {
                        if (savedPoi.ID == itemOnMap.ID) {
                            itemOnMap.HasBeenVisited = savedPoi.HasBeenVisited;
                        }
                    });
                }

                if (poi.attributes.tourPoint.TourPointOverlayActive.Target != null && poi.attributes.tourPoint.TourPointOverlayActive.Target != "") {
                    itemOnMap.Target = poi.attributes.tourPoint.TourPointOverlayActive.Target;
                } else if (poi.attributes.fundobjekt.data.id != 0) {
                    itemOnMap.TargetID = poi.attributes.fundobjekt.data.id;
                    itemOnMap.Target = "MainScene";
                } else {
                    itemOnMap.Target = "MainScene";
                }



                if (poi.attributes.illustration != null && poi.attributes.illustration.data.id != 0) {
                    foreach (IllustrationPart illustrationPart in illustration.IllustrationParts) {

                        Debug.Log("CHECK Illustration part" + illustrationPart.image);
                        if (illustrationPart.image != null && illustrationPart.image.ConvertedSprite == null) {
                            illustrationPart.image.ToSprite();
                        }
                    }
                }

                index++;

                AllItemsOnMap.Add(itemOnMap);

            }

            ErrorLog("ItemsOnMap" + AllItemsOnMap.Count);



        }));
        StartCoroutine(this.strapiService.getTours((StrapiResponse<Tour> res) => {
            Tours = res.data;
        }));

        StartCoroutine(this.strapiService.getPushNotifications((StrapiResponse<Notification> res) => {
            notifications = res.data;

            if (GameObject.FindGameObjectWithTag("LocationProvider") && SceneManager.GetActiveScene().name == "MainScene") {

                locationProviderFactoryGO = GameObject.FindGameObjectWithTag("LocationProvider").gameObject;
                locationProviderFactoryGO.gameObject.transform.parent = this.transform;
            }

            GetComponent<PushNotificationHandler>().notifications.AddRange(notifications);

            print("has notifications" + notifications);
        }));


    }




    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (GameObject.FindGameObjectWithTag("LocationProvider") && SceneManager.GetActiveScene().name == "MainScene") {

            locationProviderFactoryGO = GameObject.FindGameObjectWithTag("LocationProvider").gameObject;
            locationProviderFactoryGO.gameObject.transform.parent = this.transform;
        }

        ErrorLog("clear");

        if (loadsWithNotification) {
            GetComponent<PushNotificationHandler>().showCurrentPopUp(System.DateTime.Now);
        }

        if (SceneManager.GetActiveScene().name != "IntroScene") {

            GetComponent<PushNotificationHandler>().enabled = true;

            LocationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            if (FindObjectOfType<AbstractMap>()) {
                LocationProviderFactory.Instance.mapManager = FindObjectOfType<AbstractMap>();
            }

            ErrorLog("sets location provider" + LocationProvider.CurrentLocation.LatitudeLongitude);

            if (LocationProvider != null) {

                if (GameObject.FindGameObjectWithTag("popup") && GameObject.FindGameObjectWithTag("popup").GetComponent<TourPopUp>()) {

                    ErrorLog("assigns update func to locationupdate");
                    this.LocationProvider.OnLocationUpdated += GameObject.FindGameObjectWithTag("popup").GetComponent<TourPopUp>().UpdateDistanceDisplayOnCGMDistanceUpdate();
                }

                this.LocationProvider.OnLocationUpdated += GetComponent<PushNotificationHandler>().evaluateDistance();


            }

        }

        if (ShouldUpdateScore && ScoreToAdd != 0) {
            AddToScore(currentColor, ScoreToAdd);
        }
    }

    public List<ItemOnMap> closestItemOnMap(List<ItemOnMap> itemsOnMap, int amount) {

        foreach (ItemOnMap itemOnMap in itemsOnMap) {
            double distance = CalculateDistanceFromPinToPlayer(itemOnMap);
            itemOnMap.distanceToPlayer = distance;

        }

        List<ItemOnMap> sortedItemsOnMap = itemsOnMap.OrderBy(i => i.distanceToPlayer).ToList();

        List<ItemOnMap> selectedItemsOnMap = new List<ItemOnMap>();

        for (int i = 0; i < amount; i++) {

            selectedItemsOnMap.Add(sortedItemsOnMap[i]);
        }
        return selectedItemsOnMap;
    }


    public double CalculateDistanceFromPinToPlayer(ItemOnMap poi) {
        double lat1 = this._location.x,
        lng1 = this._location.y,
        lat2 = poi.Poi.attributes.lat,
        lng2 = poi.Poi.attributes.lng;

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

    public double CalculateDistanceFromCenterToPlayer(float lat, float lng) {
        double lat1 = this._location.x,
        lng1 = this._location.y,
        lat2 = lat,
        lng2 = lng;

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


    public void GoBackToPreviousScene() {

        ShouldUpdateScore = false;
        ScoreToAdd = 0;
        IsVisitingFromTour = false;


        if (LastSceneVisited != null && LastSceneVisited != "" && LastSceneVisited != SceneManager.GetActiveScene().name) {
            SceneManager.LoadScene(LastSceneVisited);
        } else{
            SceneManager.LoadScene("MainScene");
        }
        
       
    }

    public void AddToScore(Color hex, int Amount) {
        this.score.coinsAmount += Amount;

        Debug.Log("adds to score");
        CoinAnimation.GetComponent<coinAnimation>().UpdateAndShowCoinAnimation(hex, Amount);
        
        ShouldUpdateScore = false;

        this.saveGame();
        
    }
    public void AddSecretlyToScore(Color hex, int Amount) {
        this.score.coinsAmount += Amount;

        ShouldUpdateScore = false;

        this.saveGame();

    }

    public void CountVisitedPois() {
        this.score.visitedPois.Clear();
        foreach (ItemOnMap item in AllItemsOnMap) {
            if (item.HasBeenVisited) {
                this.score.visitedPois.Add(item);

                print("ITEM: " + item.ID + "ADDED TO VISITED POIS");
            }
        }
        this.saveGame();
    }

    private void saveGame() {
        this.strapiService.cachingService.setValue("score", Score.ToJson(this.score));

    }

    public void ChangeStateOfItemOnMap(int ID, int ExperienceID) {
        ItemOnMap itemOnMap = AllItemsOnMap.Find(item => item.ID == ID);


        Debug.Log("check item on map in CGM" + itemOnMap);
        itemOnMap.HasBeenVisited = true;
        itemOnMap.HasBeenVisitedBy = ExperienceID.ToString();
        this.CountVisitedPois();
    }



    public void LetNextSceneKnowUserArrivesFromTourScene(bool flag) {
        this.IsVisitingFromTour = flag;
    }
    public void SetLastVisitedPinForUserReturn(int ID) {
        this.LastPinVisited = ID;
    }


    public Color colorToType(string type) {
       
        Color color;
        if (type == "spiel") {
            ColorUtility.TryParseHtmlString("#FD8300", out color);
        } else if (type == "tour") {
            ColorUtility.TryParseHtmlString("#50C12A", out color);
        } else if (type == "fundObjekt") {
            ColorUtility.TryParseHtmlString("#1CB3FF", out color);
        } else if (type == "panorama") {
            ColorUtility.TryParseHtmlString("#4D319A", out color);
        } else if (type == "grabung") {
                ColorUtility.TryParseHtmlString("#2730FF", out color);
        } else if (type == "illustration") {
            ColorUtility.TryParseHtmlString("#ED2E69", out color);
        } else if (type == "pin") {
            ColorUtility.TryParseHtmlString("#1CB3FF", out color);
        } else if (type == "KriegUndKampfen") {
            ColorUtility.TryParseHtmlString("#D43B58", out color);
        } else if (type == "EindruckMachenUndImponieren") {
            ColorUtility.TryParseHtmlString("#E05436", out color);
        } else if (type == "AlltagUndUnterwegsSein") {
            ColorUtility.TryParseHtmlString("#5DCAD2", out color);
        } else if (type == "LeidenSterbenTod") {
            ColorUtility.TryParseHtmlString("#D5D5D5", out color);
        } else if (type == "HoffnungUndGlaube") {
            ColorUtility.TryParseHtmlString("#8827A6", out color);
        } else if (type == "Germanen") {
            ColorUtility.TryParseHtmlString("#47621F", out color);
        } else if (type == "UbersForschen") {
            ColorUtility.TryParseHtmlString("#424DAC", out color);
        } else if (type == "Abschalten") {
            ColorUtility.TryParseHtmlString("#4BA48A", out color);
        } else {     
            ColorUtility.TryParseHtmlString("#50C12A", out color);
        }


        return color;
    }

    public Color colorFromHex(string hex) {

        Color color;
        ColorUtility.TryParseHtmlString(hex, out color);
  
        return color;
    }

    public void ErrorLog(string message) {

        if (GameObject.FindGameObjectWithTag("errorlogging")) {
            
            if (message == "clear" || GameObject.FindGameObjectWithTag("errorlogging").GetComponent<Text>().text.Length > 700) {
                GameObject.FindGameObjectWithTag("errorlogging").GetComponent<Text>().text = "";
            } else {
                GameObject.FindGameObjectWithTag("errorlogging").GetComponent<Text>().text += "/  :" + message;
            }

        }
    }





}
