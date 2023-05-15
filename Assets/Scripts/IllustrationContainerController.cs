using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Services;

public class IllustrationContainerController : MonoBehaviour
{


    public CrossGameManager crossGameManager;

    public int FallBackIllustrationID;
    public bool isInPanoramaScene;

    public bool shouldCreateIllustration = false;

    public PinCostumiser pinCostumiser;

    public ItemOnMap itemOnMap;

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    IEnumerator Start() {

        GameObject crossGameManagerObject = GameObject.FindGameObjectWithTag("CrossGameManager");

        yield return null;

    }

    private void OnDisable() {

        print("disables illustration controller and stops coroutines");
        StopAllCoroutines();
    }

    private void OnDestroy() {

        print("destroy illustration controller and stops coroutines");
        StopAllCoroutines();
    }


    private void Update() {
        if (crossGameManager != null && crossGameManager.AllItemsOnMap.Count > 0 && shouldCreateIllustration) {
            shouldCreateIllustration = false;

            StartCoroutine(loadStart());
        }
    }

    public void openPinPopUp(TourPopUp popup) {
        popup.Show(itemOnMap);

    }

    IEnumerator loadStart() {

        crossGameManager.ErrorLog("load starts");



        if (!isInPanoramaScene) {
            if (crossGameManager != null && crossGameManager.objectToViewInAR_ID != null && crossGameManager.objectToViewInAR_ID != 0) {

                //crossGameManager.ErrorLog("has ItemOnMap in manager" + crossGameManager.objectToViewInAR_ID);

                itemOnMap = crossGameManager.AllItemsOnMap.Find(item => item.ID == crossGameManager.objectToViewInAR_ID);

                print("has item from CGM" + crossGameManager.AllItemsOnMap.Find(item => item.ID == crossGameManager.objectToViewInAR_ID).Name);

                StartCoroutine(pinCostumiser.CreatePinContent(itemOnMap));

                if (GameObject.FindObjectOfType<ARMenu>()) {
                    GameObject.FindObjectOfType<ARMenu>().currentItemOnMap = itemOnMap;
                }

            } else if (crossGameManager.AllItemsOnMap.Count > 0) {

                itemOnMap = crossGameManager.AllItemsOnMap.Find(item => item.ID == FallBackIllustrationID);
             
                StartCoroutine(pinCostumiser.CreatePinContent(itemOnMap));

            } else {
           
                crossGameManager.ErrorLog("no pin");

            }
            if (GameObject.FindObjectOfType<ARMenu>()) {
                GameObject.FindObjectOfType<ARMenu>().currentItemOnMap = itemOnMap;
            }
        }
        yield return null;
    }


    public IEnumerator CreatePin(ItemOnMap item) {

        if (crossGameManager == null) {
            GameObject crossGameManagerObject = GameObject.FindGameObjectWithTag("CrossGameManager");

            if (crossGameManagerObject != null) {
                crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
            }

        }
        crossGameManager.ErrorLog("CREATE PIN FOR" + item.ID);
        itemOnMap = item;

        StartCoroutine(pinCostumiser.CreatePinContent(itemOnMap));

        yield return null;
    }









 



    void ChangeColorOfChildren(Transform transform, Color CustomColor) {


        Debug.Log("CHNAGE TO COLOR" + transform.gameObject.name);
        foreach (Transform child in transform) {
            if (child.gameObject.CompareTag("custom_color")) {

                child.GetComponent<SpriteRenderer>().color = CustomColor;
            }

            if (child.childCount > 0) {

                Debug.Log("looks for more children in" +child.name);

                ChangeColorOfChildren(child, CustomColor);
            }
        }
    }






}
