
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

public class TourPointLoader : MonoBehaviour
{
	[SerializeField]
	AbstractMap _map;

	[SerializeField]
	[Geocode]
	string[] _locationStrings;
	Vector2d[] _locations;

	[SerializeField]
	float _spawnScale = 100f;

	[SerializeField]
	GameObject _bubblePrefab;
    
	public List<SpawnedPoi> _spawnedObjects;

    public CrossGameManager crossGameManager;

    public GameObject GameOverlay;
    public GameObject GamePopUp;

    [SerializeField]
    public List<ItemOnMap> ItemsOnMap = new List<ItemOnMap>();
    public int itemsVisited = 0;
    int netto_reward = 0;

    public Tour tour;
    public int ID;
    private bool hasGottenTour;


    private Camera _camera;

    public bool CanInteractWithMap = true;

    public GameObject Line;
    public int LinePositionIndex = 0;

    public TourPointLoader()
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
        Debug.Log("CScene is loaded");

        hasGottenTour = false;
        netto_reward = 0;
        _locations = new Vector2d[_locationStrings.Length];
        _spawnedObjects = new List<SpawnedPoi>();

        Debug.Log("CROSS GAME" + crossGameManager.AllItemsOnMap.Count);
    }



    public void GetTourAndPlacePOIs() {

        

        foreach (Tour one_of_the_tours in crossGameManager.Tours) {

            if (one_of_the_tours.id == ID) {
                tour = one_of_the_tours;
                hasGottenTour = true;

                List<Poi> points = tour.attributes.point_of_interests.data;


                int index = 0;
                points.ForEach((Poi point) => {
              
                    this.spawnPoint(point, index);
                    netto_reward += point.attributes.reward;                   
                    
                    index++;
                });



                if (this.GameOverlay.GetComponent<ParkTourOverlay>() != null) {
                    this.GameOverlay.GetComponent<ParkTourOverlay>().UpdateAndShowGameOverlay(tour.attributes.tourTeaser.headline, tour.attributes.tourTeaser.subheadline, tour.attributes.point_of_interests.data.Count, netto_reward, tour.attributes.tourTeaser.duration);

                } else {
                    Debug.Log("cant find overlay component");
                }

            }
        }


        if (crossGameManager.LastPinVisited != null) {

            int index = 0;
            foreach (ItemOnMap itemOnMap in this.ItemsOnMap) {
                if (itemOnMap.ID == crossGameManager.LastPinVisited) {
                    //itemOnMap.IsWithinReach = true;
                    if (this.GamePopUp.GetComponent<ParkTourPopUp>()) {
                        this.GamePopUp.GetComponent<ParkTourPopUp>().Show(itemOnMap);

                    }
                }
                index++;
            }

        }

    }
 


    private void spawnPoint(Poi point, int index) {

        GameObject instance = Instantiate(this._bubblePrefab);
    
        instance.transform.localPosition = _map.GeoToWorldPosition(point.attributes.getLatLng(), true);

        instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        
        _spawnedObjects.Add(new SpawnedPoi(instance, point));

        Debug.Log("SPAWNS TOURPOINT" + point + crossGameManager.AllItemsOnMap.Count);


        foreach (ItemOnMap item in crossGameManager.AllItemsOnMap) {
            //Debug.Log("CHECKS IDS___ = " + item.ID + "_=?_" + point.point_of_interest.data.id);
            Debug.Log("CHECKS ITEM___ = " + item);

        }

        ItemOnMap itemOnMap = crossGameManager.AllItemsOnMap.Find(item => item.ID == point.id);
        instance.name = point.id.ToString();
        itemOnMap.Pin = instance;
        itemOnMap.TourPoint = point.attributes.tourPoint;

        if (point.attributes.tourPoint.TourPointOverlayActive.Target != null || point.attributes.tourPoint.TourPointOverlayActive.Target != "") {
            itemOnMap.Target = point.attributes.tourPoint.TourPointOverlayActive.Target;
        } else if (point.attributes.fundobjekt.data.id != null || point.attributes.fundobjekt.data.id != 0) {
            itemOnMap.Target = point.attributes.fundobjekt.data.id.ToString();
        } else {
            itemOnMap.Target = "MainScene";
        }


        this.ItemsOnMap.Add(itemOnMap);

        if (itemOnMap.HasBeenVisitedBy != "" && itemOnMap.HasBeenVisited != false) {

            Debug.Log("has been visited" + itemOnMap.Pin);
            itemOnMap.Pin.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);

        }
    }







	private void Update()
	{

        if (crossGameManager.AllItemsOnMap.Count != 0 && !hasGottenTour) {
            Debug.Log("is cross game manAGER HERE?" + crossGameManager);
            GetTourAndPlacePOIs();
        }


        this._spawnedObjects.ForEach((SpawnedPoi spawnedPoi) => {
            spawnedPoi.gameObject.transform.localPosition = _map.GeoToWorldPosition(spawnedPoi.poi.attributes.getLatLng(), true);
            spawnedPoi.gameObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        });

        if (this.CanInteractWithMap) {
            if (Input.GetMouseButtonDown(0)) {
                HandleTouch(Input.mousePosition);
            }

            if (!TryGetTouchPosition(out Vector2 touchPosition)) {
                return;
            }

            if (Input.touchCount > 0 && Input.touchCount < 2) {
                Touch touch = Input.GetTouch(0);
                touchPosition = touch.position;

                if (touch.phase == TouchPhase.Began) {
                    HandleTouch(touchPosition);
                }
            }

        }

        if (this.ItemsOnMap.Count <= itemsVisited) {
            EndTour();
        }
    }

    public void ChangeInteractionWithMap(bool flag) {
        this.CanInteractWithMap = flag;
    }


    public void EndTour() {
        crossGameManager.AddToScore(crossGameManager.colorToType("#50C12A"), tour.attributes.tourTeaser.reward);
    }

    void HandleTouch(Vector3 pos) {
        Ray ray = this._camera.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {

            Debug.Log(hit.transform.gameObject.name);

            int index = 0;
            foreach (ItemOnMap itemOnMap in this.ItemsOnMap) {


                if (itemOnMap.ID.ToString() == hit.transform.gameObject.name) {
                    //itemOnMap.IsWithinReach = true;


                    if (this.GamePopUp.GetComponent<ParkTourPopUp>()) {
                        this.GamePopUp.GetComponent<ParkTourPopUp>().Show(itemOnMap);

                    }

                    Debug.Log("ItemOnMap__: " + itemOnMap.TourPoint.headline + "__ on index:_" + index + "___is within reach:" + itemOnMap.IsWithinReach + "");
                }
                index++;
            }

     


        }
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