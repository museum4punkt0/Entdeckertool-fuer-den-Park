using System.Collections;
using Services;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(TourLoader))]
public class Game3Manager : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField]
    //CrossGameAssets
    public CrossGameManager crossGameManager;
    private TourLoader tourLoader;
    public int currentNetto_Reward;

    public bool isFinished;
    public bool canFinish;
    private bool hasAssociatedMenuItems;

    public ItemOnMap itemOnMap;
    public ItemOnMap GlockeItem;
    public ItemOnMap MaultierItem;

    public bool shouldPlaceItemOnMap = true;
    public bool isConnectedToMap = true;

    public Game3Data game3;

    public GameObject tuturialScene;
    private bool hasAssignedDescriptionToField;
    public VisualElement m_Root_Tutorial;

    public GameObject gamePopUp;

    public string popupStart_Headline;
    public string popupStart_subHeadline;
    public string popupStart_ButtonText;
    public string popupStep1_Headline;
    public string popupStep1_subheadline;
    public string popupStep1_ButtonText;
    public string winMessageP1_Headline;
    public string winMessageP1_subheadline;
    public string winMessageP1_ButtonText;
    public string popupStep2_Headline;
    public string popupStep2_subheadline;
    public string popupStep2_ButtonText;
    public string winMessageP2_Headline;
    public string winMessageP2_subheadline;
    public string winMessageP2_ButtonText;
    public string popupStep3_Headline;
    public string popupStep3_subheadline;
    public string popupStep3_ButtonText;
    public string popupStep4_Headline;
    public string popupStep4_subheadline;
    public string popupStep4_ButtonText;
    public string popupStep5_Headline;
    public string popupStep5_subheadline;
    public string popupStep5_ButtonText;
    public string winMessageP3_Headline;
    public string winMessageP3_subheadline;
    public string winMessageP3_ButtonText;
    public string winimage1;
    public string winimage2;


    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        m_Root_Tutorial = tuturialScene.GetComponent<UIDocument>().rootVisualElement;

        tourLoader = transform.GetComponent<TourLoader>();

        Debug.Log("has loaded scene" + crossGameManager);
    }



    private IEnumerator GetRelevantPois() {
        shouldPlaceItemOnMap = false;

        Debug.Log("should get single poi");
        StartCoroutine(this.crossGameManager.strapiService.getSpiel3Content((StrapiSingleResponse<Game3Data> res) => {
            game3 = res.data;

            var _data = res.data;


            int poiID = res.data.attributes.point_of_interest.data.id;

            int glockeID = res.data.attributes.glocke.data.id;
            int maultierID = res.data.attributes.maultier.data.id;

            popupStart_Headline = _data.attributes.popupStart.headline;
            popupStart_subHeadline = _data.attributes.popupStart.subHeadline;
            popupStart_ButtonText = _data.attributes.popupStart.buttonText;

            popupStep1_Headline = _data.attributes.popupStep1.headline;
            popupStep1_subheadline = _data.attributes.popupStep1.subHeadline;
            popupStep1_ButtonText = _data.attributes.popupStep1.buttonText;

            winMessageP1_Headline = _data.attributes.winMessageP1.headline;
            winMessageP1_subheadline = _data.attributes.winMessageP1.subHeadline;
            winMessageP1_ButtonText = _data.attributes.winMessageP1.buttonText;

            popupStep2_Headline = _data.attributes.popupStep2.headline;
            popupStep2_subheadline = _data.attributes.popupStep2.subHeadline;
            popupStep2_ButtonText = _data.attributes.popupStep2.buttonText;

            winMessageP2_Headline = _data.attributes.winMessageP2.headline;
            winMessageP2_subheadline = _data.attributes.winMessageP2.subHeadline;
            winMessageP2_ButtonText = _data.attributes.winMessageP2.buttonText;

            popupStep3_Headline = _data.attributes.popupStep3.headline;
            popupStep3_subheadline = _data.attributes.popupStep3.subHeadline;
            popupStep3_ButtonText = _data.attributes.popupStep3.buttonText;

            popupStep4_Headline = _data.attributes.popupStep4.headline;
            popupStep4_subheadline = _data.attributes.popupStep4.subHeadline;
            popupStep4_ButtonText = _data.attributes.popupStep4.buttonText;

            popupStep5_Headline = _data.attributes.popupStep5.headline;
            popupStep5_subheadline = _data.attributes.popupStep5.subHeadline;
            popupStep5_ButtonText = _data.attributes.popupStep5.buttonText;

            winMessageP3_Headline = _data.attributes.winMessageP3.headline;
            winMessageP3_subheadline = _data.attributes.winMessageP3.subHeadline;
            winMessageP3_ButtonText = _data.attributes.winMessageP3.buttonText;

            LoadImage();


            itemOnMap = crossGameManager.AllItemsOnMap.Find(item => item.ID == poiID);

            MaultierItem = crossGameManager.AllItemsOnMap.Find(item => item.ID == maultierID);
            GlockeItem = crossGameManager.AllItemsOnMap.Find(item => item.ID == glockeID);


            if (isConnectedToMap) {
                tourLoader.spawnPoint(itemOnMap, 0);
                tourLoader.ItemsOnMap.Add(itemOnMap);
            }


            gamePopUp.GetComponent<GamePopUp>().ShowAndUpdatePopUp(popupStart_Headline, "", popupStart_subHeadline, popupStart_ButtonText, "info");




        }));

        yield return null;
    }

    async void LoadImage() {
        //winimage1 = (Sprite)(await game3.attributes.winimage1.media.ToSprite());
        //winimage2 = (Sprite)(await game3.attributes.winimage2.media.ToSprite());

        winimage1 = game3.attributes.winimage1.media.data.attributes.GetFullImageUrl();
        winimage2 = game3.attributes.winimage2.media.data.attributes.GetFullImageUrl();

    }

    void Update() {

        if (shouldPlaceItemOnMap && crossGameManager.AllItemsOnMap.Count > 1) {
       
            StartCoroutine(GetRelevantPois());

        }

        if (!hasAssignedDescriptionToField && game3.attributes.description != "" && game3.attributes.description != null) {
            m_Root_Tutorial.Q<TextElement>("tutorial-text").text = game3.attributes.description;
            hasAssignedDescriptionToField = true;
        }

    }



    //public IEnumerator EndTour() {
    //        isFinished = true;
    //        crossGameManager.AddToScore(crossGameManager.colorToType("tour"), tourLoader.tour.attributes.tourTeaser.reward);

    //        yield return new WaitForSeconds(1);

    //        showTourOverlay(currentTour, currentNetto_Reward);

    // }

    // public void showTourOverlay(Tour tour, int netto_reward) {

    //        currentTour = tour;

    //        if (this.GameOverlayContainer.GetComponent<ParkTourOverlay>() != null) {
    //            this.GameOverlayContainer.GetComponent<ParkTourOverlay>().UpdateAndShowGameOverlay(tour.attributes.tourTeaser.headline, tour.attributes.tourTeaser.subheadline, tour.attributes.points.Count, netto_reward, tour.attributes.tourTeaser.duration);

    //        } else {
    //            Debug.Log("cant find overlay component");
    //        }

    //    }

    //public void BeginDressingRomanPerson() {
    //    isFinished = true;
        
    //    part1.SetActive(false);

    //    tourLoader.DeletePOIs();

    //    part2.SetActive(true);
    //}


    }
