using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ARMenu : MonoBehaviour
{
    Button closeButton;
    Button infoButton;
    VisualElement m_Root;

    CrossGameManager crossGameManager;

    public PanoramaSceneManager panoramaSceneManager;

    public IllustrationContainerController illustrationContainerController;

    public TourPopUp popup;

    public ItemOnMap currentItemOnMap;

    public bool hasPopUp = true;


    void OnEnable() {
        AssignButtons();
        }
    void OnDisable() {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void AssignButtons() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        this.m_Root = GetComponent<UIDocument>().rootVisualElement;
        this.closeButton = this.m_Root.Q<Button>("closeButton");
        this.infoButton = this.m_Root.Q<Button>("infoButton");



        this.closeButton.clicked += delegate {
            GoBack();
        };


        this.infoButton.clicked += delegate {
            OpenClosestPinPopUp();
        };
        if (!hasPopUp) {
            this.infoButton.style.visibility = Visibility.Hidden;
        }

   

    }
    void Start()
    {
        AssignButtons();

    }

    // Update is called once per frame
    public void GoBack() {


        print("AR MENU CLICK go back");
        if (panoramaSceneManager == null) {
            crossGameManager.GoBackToPreviousScene();

        } else {
            panoramaSceneManager.StopAR();
        }

    }


    public void OpenClosestPinPopUp() {
        crossGameManager.ErrorLog("opens something");

        if (panoramaSceneManager != null) {

            crossGameManager.ErrorLog("opens wrong item");

            List<ItemOnMap> itemsOnMap = crossGameManager.closestItemOnMap(panoramaSceneManager.selectableItemsOnMap, 1);
            popup.Show(itemsOnMap[0]);

        } else if (illustrationContainerController != null && illustrationContainerController.itemOnMap.Poi.id != 0) {
            crossGameManager.ErrorLog("POPUP-Item:" + illustrationContainerController.itemOnMap.Poi.id);
            popup.Show(illustrationContainerController.itemOnMap);

        } else if (currentItemOnMap != null && currentItemOnMap.ID != 0) {

            crossGameManager.ErrorLog("CurrentItem:" + illustrationContainerController.itemOnMap.Poi.id);

            popup.Show(currentItemOnMap);

        } else {
            crossGameManager.ErrorLog("opens nothing");

        }


    }
}
