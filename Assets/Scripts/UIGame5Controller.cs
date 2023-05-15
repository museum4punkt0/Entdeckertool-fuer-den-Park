using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Services;
using UIBuilder;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIGame5Controller : MonoBehaviour {

    public GameObject romanTourManager;
    public GameObject ubersichtScene;
    public VisualElement m_Root_Ubersicht;
    public VisualElement m_Root_romanTourPopUp;
    public GameObject game5UI;
    public GameObject mapAssets;
    public GameObject romanTourPopUp;
    public CrossGameManager crossGameManager;
    public GameObject game5SecondPart;
    public TourLoader tourloader;
    public RadialWheel radialWheel;

    public GameObject tutorialManager;

    public VisualElement cardContainer;

    public int score;

    public Game5Data _data;

    private string popupStart_Headline;
    private string popupStart_subHeadline;
    private string popupStart_ButtonText;
    private string popupStep1_Headline;
    private string popupStep1_subheadline;
    private string popupStep1_ButtonText;
    private string popupStep2_Headline;
    private string popupStep2_subheadline;
    private string popupStep2_ButtonText;
    private string winMessage_Headline;
    private string winMessage_subheadline;
    private string winMessage_ButtonText;


    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

        m_Root_Ubersicht = ubersichtScene.GetComponent<UIDocument>().rootVisualElement;
        m_Root_romanTourPopUp = romanTourPopUp.GetComponent<UIDocument>().rootVisualElement;
        //PopulateCards();

        romanTourManager.GetComponent<TourLoader>().CanInteractWithMap = false;

        StartCoroutine(Populate());
        ButtonClick();

        if (crossGameManager.hasCompletedGame5) {
            Debug.Log("should load other roman guy");
            ubersichtScene.SetActive(false);
            game5SecondPart.SetActive(true);
            mapAssets.SetActive(false);
        }

        StartCoroutine(crossGameManager.strapiService.getSpiel5Content(LoadPopUpContent));
    }

    public async void LoadPopUpContent(StrapiSingleResponse<Game5Data> res) {
        _data = res.data;

        popupStart_Headline = _data.attributes.popupStart.headline;
        popupStart_subHeadline = _data.attributes.popupStart.subHeadline;
        popupStart_ButtonText = _data.attributes.popupStart.buttonText;

        popupStep1_Headline = _data.attributes.popupStep1.headline;
        popupStep1_subheadline = _data.attributes.popupStep1.subHeadline;
        popupStep1_ButtonText = _data.attributes.popupStep1.buttonText;
    }

    public void PopulatePopUp() {
        m_Root_Ubersicht.Q<Label>("title").text = _data.attributes.gameTitle;
        m_Root_Ubersicht.Q<Label>("subText").text = _data.attributes.popupStart.subHeadline;
        m_Root_Ubersicht.Q<Label>("popup-subHeadline").text = popupStart_subHeadline;
        m_Root_Ubersicht.Q<Label>("popup-headline").text = popupStart_Headline;
        cardContainer = m_Root_Ubersicht.Q<VisualElement>("cardContainer");


    }

    public void PopulateCards() {


        this.cardContainer.Clear();

        List<ItemOnMap> itemsOnMap = romanTourManager.GetComponent<TourLoader>().ItemsOnMap;
        //m_Root_Ubersicht.Q<Label>("subText").text = "Finde " + itemsOnMap.Count + " Ausstattungsteile der Legion?rsausrstung: ";
        m_Root_Ubersicht.Q<Label>("subText").text = _data.attributes.popupStart.subHeadline;
;
        for (int i = 0; i < itemsOnMap.Count;i++) {
            print("itemsonmap: " + i + itemsOnMap[i].HasBeenVisited);

            Button card = new Button();
            card.name = "card0" + i;
            card.AddToClassList("cms-game5-card-wrapper");
            VisualElement cardImage = new VisualElement();
            cardImage.AddToClassList("cms-game5-card01-image");

            if (itemsOnMap[i].AssociatedMenuItems.Count >= 1) {
                cardImage.style.backgroundImage = new StyleBackground(itemsOnMap[i].AssociatedMenuItems[1]);
            } else {
                cardImage.style.backgroundImage = new StyleBackground(romanTourManager.GetComponent<Game5Manager>().unfilled_images[i]);
            }

            print("cards: " + itemsOnMap[i].AssociatedMenuItems[1]);
            //Davinci.get().load(itemsOnMap[i].AssociatedMenuItems[1].).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(cardImage).start();


            VisualElement cardPin = new VisualElement();
            cardPin.AddToClassList("cms-game5-card-pinIcon");

            Label cardLabel = new Label();
            cardLabel.name = "card01-label";
            //cardLabel.text =  "Ausstattungsteil " + (i+1).ToString() + " " + itemsOnMap[i].Name; 
            cardLabel.text = itemsOnMap[i].Name;
            cardImage.Add(cardPin);
            card.Add(cardImage);
            card.Add(cardLabel);

            cardContainer.Add(card);

            m_Root_Ubersicht.Q<Button>("card0" + i).clicked += delegate {
                m_Root_Ubersicht.style.display = DisplayStyle.None;
                //game5UI.SetActive(true);
                //game5UI.transform.GetChild(0).GetComponent<RomanTourPopUp>()
                romanTourManager.GetComponent<TourLoader>().updatePOIS();

                romanTourManager.GetComponent<TourLoader>().CanInteractWithMap = true;
            };
           
           
            //romanTourManager.GetComponent<TourLoader>().ItemsOnMap[1].HasBeenVisited = true;

            if (itemsOnMap[i].HasBeenVisited) {
                cardImage.RemoveFromClassList("cms-game5-card01-image");
                cardImage.AddToClassList("cms-game5-card01-image-active");
                cardPin.RemoveFromClassList("cms-game5-card-pinIcon");
                cardPin.AddToClassList("cms-game5-card-pinIcon-active");
                cardImage.style.backgroundImage = new StyleBackground(itemsOnMap[i].AssociatedMenuItems[0]);

                cardLabel.text = itemsOnMap[i].Name.ToString();
                card.SetEnabled(false);
                score++;
                m_Root_Ubersicht.Q<Label>("coins-label").text = score.ToString();

                if (score >= itemsOnMap.Count) {
                    //show win pop up
                    StartCoroutine(Win());

                }
                
            }

            Debug.Log("ubersicht-item:" + itemsOnMap[i].Name.ToString() + itemsOnMap[i].AssociatedMenuItems[0]);
        }

        m_Root_Ubersicht.Q<Label>("coins-label").text = score.ToString();

        print(score + " score");
    }

    private IEnumerator Win() {
        m_Root_Ubersicht.Q<Label>("popup-headline").text = popupStep1_Headline;
        m_Root_Ubersicht.Q<Label>("popup-subHeadline").text = "";
        m_Root_Ubersicht.Q<Button>("popup-button").text = popupStep1_ButtonText;

        yield return new WaitForSeconds(2f);
        m_Root_Ubersicht.Q<VisualElement>("PopUp").style.display = DisplayStyle.Flex;
        radialWheel.CanInstantiateDragableItem = true;

        ubersichtScene.SetActive(false);
        game5SecondPart.SetActive(true);
        mapAssets.SetActive(false);



    }

    public void ButtonClick() {
        //Button backToCardsPanel;
        //backToCardsPanel = m_Root_romanTourPopUp.Q<Button>("ActionButton_ContinueTour");
        //backToCardsPanel.clicked += delegate {
        //    //crossgamemanager
        //    crossGameManager.AddToScore(crossGameManager.colorFromHex("#1CB3FF"), 1);
        //    //disableui
        //    //game5UI.SetActive(false);
        //    game5UI.transform.GetChild(0).GetComponent<RomanTourPopUp>().Hide();
        //    //enablepanel
        //    m_Root_Ubersicht.style.display = DisplayStyle.Flex;
        //    romanTourManager.GetComponent<TourLoader>().CanInteractWithMap = false;
        //    //update ui panel
        //    PopulateCards();


        //};
        if (crossGameManager.hasCompletedGame5) {
            m_Root_Ubersicht.Q<Button>("popup-button").clicked += delegate {
                ubersichtScene.SetActive(false);
                game5SecondPart.SetActive(true);
                mapAssets.SetActive(false);
            };
        }


    }

    private IEnumerator Populate() {
        yield return new WaitForSeconds(2f);
        PopulatePopUp();
        PopulateCards();
    }

    public void ClosePopUpOPenUbersicht() {
        //crossGameManager.AddToScore(crossGameManager.colorFromHex("#1CB3FF"), 1);
        //disableui
        //game5UI.SetActive(false);
        game5UI.transform.GetChild(0).GetComponent<RomanTourPopUp>().Hide();
        //enablepanel
        m_Root_Ubersicht.style.display = DisplayStyle.Flex;
        romanTourManager.GetComponent<TourLoader>().CanInteractWithMap = false;
    }

    public void returnToGameFromItemView() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        m_Root_Ubersicht = ubersichtScene.GetComponent<UIDocument>().rootVisualElement;
        crossGameManager.IsVisitingFromGame = false;
        crossGameManager.IsVisitingFromTour = false;
        
        tutorialManager.SetActive(false);
        


        m_Root_Ubersicht.Q<VisualElement>("PopUp").style.display = DisplayStyle.None;

    }

}
