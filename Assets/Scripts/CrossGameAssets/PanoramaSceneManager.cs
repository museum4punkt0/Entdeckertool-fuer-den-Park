using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Mapbox;
using UnityEngine;
using ARLocation;

public class PanoramaSceneManager : MonoBehaviour
{
    public int amountOfVisiblePois;
    public double allowedDistanceToPOI = 30;
    public bool onlyShowPoisWRelationTo3D;
    public CrossGameManager crossGameManager;
    public UIItemViewController uiItemViewController;


    public List<int> selectableItemsIDs = new List<int>();

    public List<ItemOnMap> selectableItemsOnMap = new List<ItemOnMap>();
    public List<Illustration> illustrations = new List<Illustration>();

    public List<GameObject> ARPinsInScene = new List<GameObject>();


    public GameObject animatedPinPrefab;

    public GameObject placeAtLocationPrefab;

    private bool hasGottenAllItemsOnMap;

    public GameObject map;
    public GameObject arAssets;

    public bool displaysAR = false;

    public TourPopUp tourPopUp;

    public bool hasPlacedARPins;

    public bool ARButtonIsActive;

    public bool firstTimeOPeningCamera = true;
    public GameObject popUp;

   




    


    void Start()
    {
        GameObject crossGameManagerObject = GameObject.FindGameObjectWithTag("CrossGameManager");
        if (crossGameManagerObject != null) {
            crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        }
    }

    private void Update() {
        if (crossGameManager.AllItemsOnMap.Count > 0 && !hasGottenAllItemsOnMap && !hasPlacedARPins) {
            hasGottenAllItemsOnMap = true;
            crossGameManager.ErrorLog("has items in cgm");

            //StartAR();
        }


        if (!ARButtonIsActive) {
       

            ARButtonIsActive = true;
            uiItemViewController.isARLocationReady = true;
            uiItemViewController.SetARButtonToInactive(true);

            Debug.Log("sets button to active");
            crossGameManager.ErrorLog("should toggle ARBtn");

        } 
    }

    public void StartAR() {

        map.SetActive(false);
        arAssets.SetActive(true);
        crossGameManager.ErrorLog("starts ar");

        tourPopUp.isInARScene = true;

        if (!firstTimeOPeningCamera) {
            popUp.SetActive(false);
        } else {
            popUp.SetActive(true);
      
            firstTimeOPeningCamera = false;
        }

        displaysAR = true;

        StartCoroutine(WaitForCameraToChange());
       
    }

    IEnumerator WaitForCameraToChange() {

        yield return new WaitForEndOfFrame();

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {

            SelectPoisToInstantiate(crossGameManager.AllItemsOnMap);

            crossGameManager.ErrorLog("starts selection");



        } else {


            foreach (int id in selectableItemsIDs) {

                foreach (ItemOnMap itemOnMap in crossGameManager.AllItemsOnMap) {
                    if (itemOnMap.ID == id) {

                        Debug.Log("HAS POI" + itemOnMap.Name + "___" + crossGameManager.AllItemsOnMap.Count);

                        selectableItemsOnMap.Add(itemOnMap);
                        illustrations.Add(itemOnMap.Poi.attributes.illustration.data.attributes);
                        InsertItemOnMapInScene(itemOnMap);
                    }

                }
            }

        }


    }

    public void StopAR() {

        print("stops AR mode");

        map.SetActive(true);
        arAssets.SetActive(false);
        displaysAR = false;
        selectableItemsOnMap.Clear();

        tourPopUp.isInARScene = false;


        illustrations.Clear();

        StopAllCoroutines();

        hasPlacedARPins = false;

        if (ARPinsInScene.Count > 0) {

            foreach (GameObject pin in ARPinsInScene) {
                Destroy(pin);

                crossGameManager.ErrorLog("deletes" + pin.name);

            }
            ARPinsInScene.Clear();

        }

        if (uiItemViewController != null) {
            uiItemViewController.View360("close");
        }


    
    }


    void SelectPoisToInstantiate(List<ItemOnMap> itemsOnMap) {


        crossGameManager.ErrorLog("checks for items" + itemsOnMap.Count);

        int index = 0;
    
        foreach (ItemOnMap item in itemsOnMap) {

            double distance = crossGameManager.CalculateDistanceFromPinToPlayer(item);

  
            if (onlyShowPoisWRelationTo3D) {
                if (item.Poi.attributes.illustration != null || item.Poi.attributes.pin != null) {
                    if (distance < allowedDistanceToPOI) {
                        item.distanceToPlayer = distance;
                        selectableItemsOnMap.Add(item);
                    }
                }
            } else {
                if (distance < allowedDistanceToPOI) {
                    item.distanceToPlayer = distance;
                    selectableItemsOnMap.Add(item);
                }
            }
            index++;

            if (index >= itemsOnMap.Count) {


                selectableItemsOnMap = selectableItemsOnMap.OrderBy(i => i.distanceToPlayer).ToList();


                for (int i = 0; i < amountOfVisiblePois; i++) {


                    InsertItemOnMapIntoAR(selectableItemsOnMap[i]);

                    crossGameManager.ErrorLog(selectableItemsOnMap[i].Name);
                }

               

            }

        }
        hasPlacedARPins = true;
  


    }

    public void InsertItemOnMapIntoAR(ItemOnMap selectedItemOnMap) {


        GameObject instance = Instantiate(placeAtLocationPrefab, this.transform);
     
        instance.name = selectedItemOnMap.Name;
        //instance.transform.position = new Vector3(selectedItemOnMap.ID * 2, 0, 0);

        if (!instance.GetComponent<PlaceAtLocation>()) {
            instance.AddComponent<PlaceAtLocation>();
        }

        instance.GetComponent<PlaceAtLocation>()._Latitude = (float)selectedItemOnMap.Poi.attributes.lat; //in the 50s
        instance.GetComponent<PlaceAtLocation>()._Longitude = (float)selectedItemOnMap.Poi.attributes.lng; //around 13

        GameObject ArObject;

        ArObject = Instantiate(animatedPinPrefab, instance.transform);

        print("AROBJECT" + ArObject.name);
        crossGameManager.ErrorLog("place pin" + instance.name + instance.GetComponent<PlaceAtLocation>()._Latitude +"_"+ instance.GetComponent<PlaceAtLocation>()._Longitude);

        ArObject.GetComponent<IllustrationContainerController>().shouldCreateIllustration = false;
        StartCoroutine(ArObject.GetComponent<IllustrationContainerController>().CreatePin(selectedItemOnMap));


        //ArObject.GetComponent<PinCostumiser>().ChangeColorOfChildren(ArObject.transform, crossGameManager.colorToType(selectedItemOnMap.Poi.attributes.type), true);
        //ArObject.GetComponent<PinCostumiser>().CreatePinContent(selectedItemOnMap);
        ArObject.transform.localPosition = new Vector3(0, 0, 0);

        ARPinsInScene.Add(instance);




    }

    public void InsertItemOnMapInScene(ItemOnMap selectedItemOnMap) {

        Debug.Log(selectedItemOnMap.Poi.attributes.illustration.data.attributes);
        
        GameObject instance = Instantiate(placeAtLocationPrefab, transform);
        instance.name = selectedItemOnMap.Name;
        instance.transform.position = new Vector3(selectedItemOnMap.ID * 2, 0, 0);
        
        //if (!instance.GetComponent<PlaceAtLocation>()) {
        //    instance.AddComponent<PlaceAtLocation>();
        //}


        //instance.GetComponent<PlaceAtLocation>()._Latitude = (float)selectedItemOnMap.Poi.attributes.lat; //in the 50s
        //instance.GetComponent<PlaceAtLocation>()._Longitude = (float)selectedItemOnMap.Poi.attributes.lng; //around 13

    


        GameObject ArObject;
        ArObject = Instantiate(animatedPinPrefab, instance.transform);


        print("AROBJECT" + ArObject.name);
        ArObject.GetComponent<IllustrationContainerController>().shouldCreateIllustration = false;
        StartCoroutine(ArObject.GetComponent<IllustrationContainerController>().CreatePin(selectedItemOnMap));

        //ArObject.GetComponent<PinCostumiser>().ChangeColorOfChildren(ArObject.transform, crossGameManager.colorToType(selectedItemOnMap.Poi.attributes.type), true);
        //ArObject.GetComponent<PinCostumiser>().CreatePinContent(selectedItemOnMap);

        ArObject.transform.localPosition = new Vector3(0, 0, 0);

        ARPinsInScene.Add(instance);


    }




}
