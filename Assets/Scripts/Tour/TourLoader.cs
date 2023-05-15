
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using Services;
using TMPro;
using System.Collections;
using ARLocation;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TourLoader : MonoBehaviour
{

    [SerializeField]
    [Header("update ID to change tour to display") ]
    public int ID = 2;

    [SerializeField]
    [Header("set name of scene on tourloader to know which scene visits what poi")]
    public string name;

    [SerializeField]
    [Header("stop poi's from moving on pc")]
    private bool isTestingOnPC;



    public List<ItemOnMap> ItemsOnMap = new List<ItemOnMap>();
    public int itemsVisited = 0;
    public List<TourPoint> points;
    public Tour tour;
    private bool hasGottenPoints;

    [Header("change if manager is ready so that a manager has the opportunity to query for id's in strapi")]
    public bool ManagerIsReady;
    [SerializeField]
	AbstractMap _map;

	[SerializeField]
	[Geocode] string[] _locationStrings;
	Vector2d[] _locations;
    public List<SpawnedPoi> _spawnedObjects;
    public GameObject Line;
    public int LinePositionIndex = 0;


    [Header("set to true to load all points in all tours")]
    public bool shouldLoadAllPois;

    //stzling
    [SerializeField]
	float _spawnScale = 50f;
	[SerializeField]
	GameObject _bubblePrefab;
    [SerializeField]
    GameObject _ARBubblePrefab;


    //UI
    public GameObject GamePopUp;
    public GameObject GameManager;
    public bool CanInteractWithMap = true;
    public string overRideColor;
    public GameObject[] colored_ui_elements;
    public GameObject mapPoiParentContainer;
    public bool blockMapInteractionBelowMenu = true;


    //CrossGameAssets
    public CrossGameManager crossGameManager;
    private Camera _camera;
    public int netto_reward;


    public TourLoader()
    {

    }

    void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        if (!Camera.main) {
            this._camera = Camera.current;
        } else {
            this._camera = Camera.main;
        }
    }
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        hasGottenPoints = false;
        _locations = new Vector2d[_locationStrings.Length];
        _spawnedObjects = new List<SpawnedPoi>();


        if (GamePopUp.GetComponent<TourPopUp>()) {
            GamePopUp.GetComponent<TourPopUp>().AssignPopUpUIElements();
        }

        if (crossGameManager.tourIDtoStart != null && crossGameManager.tourIDtoStart != 0) {
            ID = crossGameManager.tourIDtoStart;
        }
        Debug.Log("Loads scene");

    }


    public IEnumerator GetTour() {

        hasGottenPoints = true;

        foreach (Tour one_of_the_tours in crossGameManager.Tours) {



                if (one_of_the_tours.id == ID) {
                    tour = one_of_the_tours;

                    List<Poi> points = tour.attributes.point_of_interests.data;
                    int index = 0;
                    points.ForEach((Poi point) => {
                        ItemOnMap itemOnMap = crossGameManager.AllItemsOnMap.Find(item => item.ID == point.id);
                        this.spawnPoint(itemOnMap, index);
                        index++;
                        netto_reward += point.attributes.reward;

                        this.ItemsOnMap.Add(itemOnMap);
                    });


                //if (crossGameManager.LastSceneVisited != SceneManager.GetActiveScene().name) {

                //    if (GameManager.GetComponent<TourManager>()) {
                //        GameManager.GetComponent<TourManager>().updateTourOverlay(tour, netto_reward);
                //    }

                //} else {

                //    if (GameManager.GetComponent<TourManager>()) {
                //        GameManager.GetComponent<TourManager>().updateTourOverlay(tour, netto_reward);
                //    }

                //}

                if (GameManager.GetComponent<TourManager>()) {
                    GameManager.GetComponent<TourManager>().updateTourHeader(tour, itemsVisited);
                }


            }

            

        }

        
        yield return null;

    }

    public IEnumerator GetAllPois() {

        hasGottenPoints = true;
        int index = 0;
        foreach (ItemOnMap itemOnMap in crossGameManager.AllItemsOnMap) {


            this.spawnPoint(itemOnMap, index);
            index++;
            netto_reward += itemOnMap.Poi.attributes.reward;

            this.ItemsOnMap.Add(itemOnMap);



        }


   

        yield return null;


    }


    public void spawnPoints() {
        hasGottenPoints = true;
        int index = 0;
        foreach (ItemOnMap itemOnMap in ItemsOnMap) {
            spawnPoint(itemOnMap, index);
            index++;
        }
    }

    public void spawnPoint(ItemOnMap itemOnMap, int pinNumber) {

        GameObject instance;

        if (itemOnMap.Poi.attributes.type == "panorama") {
            instance = Instantiate(this._ARBubblePrefab, _map.gameObject.transform);

        } else {
            instance = Instantiate(this._bubblePrefab, _map.gameObject.transform);
        }

        instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        instance.transform.localPosition = _map.GeoToWorldPosition(itemOnMap.Poi.attributes.getLatLng(), true);
        itemOnMap.Position = _map.GeoToWorldPosition(itemOnMap.Poi.attributes.getLatLng(), true);
    
        instance.name = itemOnMap.Poi.id.ToString();

        itemOnMap.Pin = instance;

        
        if (itemOnMap.HasBeenVisitedBy != "" && itemOnMap.HasBeenVisited != false) {
            if (itemOnMap.Poi.attributes.type != "panorama") {
                itemOnMap.Pin.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                itemOnMap.Pin.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
            } else {
                itemOnMap.Pin.transform.GetChild(0).gameObject.SetActive(false);
                itemOnMap.Pin.transform.GetChild(1).gameObject.SetActive(true);
            }
            itemsVisited += 1;

            if (GameManager != null && GameManager.GetComponent<TourManager>()) {
                GameManager.GetComponent<TourManager>().updateTourHeader(tour, itemsVisited);
            }
        }

        /////increase sort order slightly
        ///
        SpriteRenderer pinSprite = itemOnMap.Pin.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        pinSprite.sortingOrder += pinNumber;


        if (pinSprite.gameObject.transform.childCount > 1  && pinSprite.gameObject.transform.GetChild(1) != null) {
            pinSprite.gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder += pinNumber;
        }
        if (pinSprite.gameObject.transform.childCount > 0 && pinSprite.gameObject.transform.GetChild(0) != null && pinSprite.gameObject.transform.GetChild(0).transform.GetChild(0) != null) {
            pinSprite.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder += pinNumber;
        }

        //itemOnMap.Pin.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder += pinNumber;


        if (overRideColor == null || overRideColor == "") {

            if (itemOnMap.Poi.attributes.type == "grabung" || itemOnMap.Poi.attributes.type == "panorama") {
                ChangeColorOfChildren(instance.transform, crossGameManager.GetComponent<CrossGameManager>().colorToType(itemOnMap.Poi.attributes.type));


            } else  {
                ChangeColorOfChildren(instance.transform, crossGameManager.GetComponent<CrossGameManager>().colorToType("fundObjekt"));

            }
            //ChangeColorOfChildren(instance.transform, crossGameManager.GetComponent<CrossGameManager>().colorToType(itemOnMap.Poi.attributes.type));
        } else {
            Color uiColor;
            ColorUtility.TryParseHtmlString(overRideColor, out uiColor);
            ChangeColorOfChildren(instance.transform, uiColor);

        }


        _spawnedObjects.Add(new SpawnedPoi(instance, itemOnMap.Poi));

    }

    void ChangeColorOfChildren(Transform transform, Color uiColor) {

        foreach (Transform child in transform) {
            if (child.gameObject.CompareTag("ui_element_to_change_color")) {
                    if (child.GetComponent<Image>()) {
                    child.GetComponent<Image>().color = uiColor;
                    } else if (child.GetComponent<TextMeshProUGUI>()) {
                        GetComponent<TextMeshProUGUI>().color = uiColor;
                    } else if (child.GetComponent<SpriteRenderer>()) {
                        child.GetComponent<SpriteRenderer>().color = uiColor;
                    }
            }
            if (child.childCount > 0) {
                ChangeColorOfChildren(child, uiColor);
            }
        }
    }



    private void Update()
	{
        if (crossGameManager == null) {
            crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        }

        if (crossGameManager != null && crossGameManager.AllItemsOnMap.Count != 0 && !hasGottenPoints && ManagerIsReady) {
            if (!shouldLoadAllPois) {
                StartCoroutine(GetTour());
            } else {
                StartCoroutine(GetAllPois());
            }
        }

        if (ManagerIsReady && !hasGottenPoints && ItemsOnMap.Count > 0) {
            spawnPoints();
        }



        if (!isTestingOnPC && _map != null) {
            this._spawnedObjects.ForEach((SpawnedPoi spawnedPoi) => {
                spawnedPoi.gameObject.transform.localPosition = _map.GeoToWorldPosition(spawnedPoi.poi.attributes.getLatLng(), true);
                spawnedPoi.gameObject.transform.localPosition = new Vector3(spawnedPoi.gameObject.transform.localPosition.x, 3f, spawnedPoi.gameObject.transform.localPosition.z);
                spawnedPoi.gameObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            });
        }


     
        
        
         
        if (!TryGetTouchPosition(out Vector2 touchPosition)) {
             return;
        }

        if (Input.touchCount > 0 && Input.touchCount < 2) {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;
                
            if (touch.phase == TouchPhase.Began) {

           
                if (this.CanInteractWithMap) {
                    HandleTouch(touchPosition);

                }
            }
         
        }

  
    }


    public void updatePOIS() {

        print("updates pois");
        if (GameManager != null && GameManager.GetComponent<TourManager>()) {
            GameManager.GetComponent<TourManager>().updateTourHeader(tour, itemsVisited);
        }
        foreach (ItemOnMap itemOnMap in ItemsOnMap) {
            if (itemOnMap.HasBeenVisitedBy != "" && itemOnMap.HasBeenVisited != false) {
                if (itemOnMap.Poi.attributes.type != "panorama") {
                    itemOnMap.Pin.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    itemOnMap.Pin.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                } else {
                    itemOnMap.Pin.transform.GetChild(0).gameObject.SetActive(false);
                    itemOnMap.Pin.transform.GetChild(1).gameObject.SetActive(true);
                }

            }

        }
    }

    public void ChangeInteractionWithMap(bool flag) {
        this.CanInteractWithMap = flag;
    }



    void HandleTouch(Vector3 pos) {
        Ray ray = this._camera.ScreenPointToRay(pos);
        RaycastHit hit;

        if (pos.y < 300 && blockMapInteractionBelowMenu) {
            return;
        }

        if (Physics.Raycast(ray, out hit)) {

            Debug.Log("clicks map" + hit.transform.gameObject.name);

            int index = 0;

            foreach (ItemOnMap itemOnMap in this.ItemsOnMap) {

                
                if (itemOnMap.ID.ToString() == hit.transform.gameObject.name) {
                    //print("inside raycast");

                    crossGameManager.ErrorLog("click pin" + hit.transform.gameObject.name);

                    if (this.GamePopUp != null) {

                        if (this.GamePopUp.GetComponent<TourPopUp>()) {
                            this.GamePopUp.GetComponent<TourPopUp>().Show(itemOnMap);

                        }
                        if (this.GamePopUp.GetComponent<RomanTourPopUp>()) {
                            this.GamePopUp.GetComponent<RomanTourPopUp>().Show(itemOnMap);
                        }

                    }
          
                }
                index++;
            }

     


        }
    }

    public void DeletePOIs() {
        foreach (ItemOnMap itemOnMap in this.ItemsOnMap) {
            Destroy(itemOnMap.Pin);
        }
        Debug.Log("destroys pins");
    }


    public void ChangeItemOnMapToBeInReach(SpawnedPoi itemOnMapPoi) {

        int index = 0;
        foreach (ItemOnMap itemOnMap in this.ItemsOnMap) {
            if (itemOnMap.Pin = itemOnMapPoi.gameObject) {
                itemOnMap.IsWithinReach = true;
                Debug.Log("ItemOnMap__: " + itemOnMap + "__ on index:_" + index + "___is within reach:" + itemOnMap.IsWithinReach + "");
            }
            index++;
        }
    }


    bool TryGetTouchPosition(out Vector2 touchPosition) {
        if (Input.touchCount > 0) {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }



    public void DrawLineToNextPin(ItemOnMap currentItemOnMap) {
        ItemOnMap NextitemOnMap = FindNextPin(currentItemOnMap);
        if (!Line.activeSelf) {
            Line.transform.localPosition = currentItemOnMap.Position;
            Line.SetActive(true);
        }

        Line.GetComponent<LineRenderer>().SetPosition(LinePositionIndex, NextitemOnMap.Position);


        Debug.Log("Current ITEM" + currentItemOnMap.Position);

        Debug.Log("NEXT ITEM" + NextitemOnMap.Position);

        LinePositionIndex++;

    }

    public ItemOnMap FindNextPin(ItemOnMap currentItemOnMap) {
        int index = ItemsOnMap.IndexOf(currentItemOnMap) + 1;

        if (index > ItemsOnMap.Count) {
            index = 0;
        }

        ItemOnMap NextItemOnMap = ItemsOnMap[index];
        return NextItemOnMap;
    }

}