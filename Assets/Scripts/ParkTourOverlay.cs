using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class ParkTourOverlay: MonoBehaviour
{

    //Animation Properties
    public float AnimationSpeed = 0.5f;
    
    //UI elements
    public VisualElement overlay;
    public GroupBox OverlayContent;

    public Button Button;

    public Label Headline;
    public Label Detail;

    public Label time;
    public Label fund_amount;
    public Label coin_amount;

    public Label header_headline;
    public Button ToggleOverlay;

    public Label score;
    public Label goal;



    
    //Temporary Variables
    public string NewSceneName;
    public string MainSceneName;
    private float restingYposition_overlay;
    private float restingYposition_OverlayContent;

    //Access GameController
    public GameObject GameController;
    public CrossGameManager crossGameManager;


    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    /// Run start as enumerator to wait for first frame and be able to get all properties from UI elements
    private IEnumerator Start() {

        yield return new WaitForEndOfFrame();
        AssignUIElements();
    }

    public void AssignUIElements() {
       
        var root = GetComponent<UIDocument>().rootVisualElement;

        this.Headline = root.Q<Label>("headline");
        this.score = root.Q<Label>("currentScore");
        this.goal = root.Q<Label>("totalScore");



        //this.overlay = root.Q<VisualElement>("GameOverlay");
        //this.restingYposition_overlay = this.overlay.worldBound.position.y;

        //this.OverlayContent = root.Q<GroupBox>("OverlayContent");
        //this.restingYposition_OverlayContent = this.overlay.worldBound.position.y;


        //Debug.Log("resting postion" + this.restingYposition_overlay);
        //this.Button = root.Q<Button>("ActionButton");
        //this.Detail = root.Q<Label>("DETAIL");

        //this.time = root.Q<Label>("time_amount");
        //this.fund_amount = root.Q<Label>("fund_amount");
        //this.coin_amount = root.Q<Label>("coin_amount");

        //this.header_headline = root.Q<Label>("header_headline");
        //this.ToggleOverlay = root.Q<Button>("ToggleOverlay");




        //this.Button.clicked += GoToMainMenu;

        //if (crossGameManager.LastSceneVisited != SceneManager.GetActiveScene().name) {
        //    this.ToggleOverlay.clicked += MoveOverlayOutOfView;
        //} else {
        //    this.ToggleOverlay.clicked += MoveOverlayIntoView;
        //}

        //Debug.Log("Assigns Overlay elements" + this.ToggleOverlay);


    }


    public void UpdateHeader(string Headline, int score, int goal) {

        //check if UI elements have been assigned. If not: Assign them!
        if (this.Headline == null) {
            this.AssignUIElements();
        }

        this.Headline.text = Headline;

        this.score.text = score.ToString();
        this.goal.text = goal.ToString();

    }


    public void UpdateAndShowGameOverlay(string Headline, string Detail, int Fund_Amount, int Coin_Amount, int Duration) {

        //check if UI elements have been assigned. If not: Assign them!
        if (this.Headline == null) {
            this.AssignUIElements();
        }

        //this.overlay.style.backgroundColor = new Color(color.r, color.g, color.b, 1f);

        this.Headline.text = Headline;

        if (this.header_headline != null) {
            this.header_headline.text = Headline;
        }

        if (this.fund_amount != null) {
            this.fund_amount.text = Fund_Amount.ToString() + " Funde";
        }
        if (this.coin_amount != null) {
            this.coin_amount.text = Coin_Amount.ToString();
        }

        if (this.time != null) {
            this.time.text = Duration.ToString();
        }

        if (this.Detail != null) {
            this.Detail.text = Detail;
        }

        MoveOverlayIntoView();
    }

    public void UpdateGameOverlay(string Headline, string Detail, int Fund_Amount, int Coin_Amount, int Duration) {

        //check if UI elements have been assigned. If not: Assign them!
        this.AssignUIElements();

        //this.overlay.style.backgroundColor = new Color(color.r, color.g, color.b, 1f);

        this.Headline.text = Headline;


        if (this.Detail != null) {
            this.Detail.text = Detail;
        }

        if (this.header_headline != null) {
            this.header_headline.text = Headline;
        }

        if (this.fund_amount != null) {
            this.fund_amount.text = Fund_Amount.ToString() + " Funde";
        }
        if (this.coin_amount != null) {
            this.coin_amount.text = Coin_Amount.ToString();
        }

        if (this.time != null) {
            this.time.text = Duration.ToString();
        }
    }

    public void MoveOverlayIntoView() {

    

        DOTween.To(() => this.overlay.worldBound.position.y, y => this.overlay.style.top = y, 0, this.AnimationSpeed).SetEase(Ease.Linear);
        DOTween.To(() => this.restingYposition_OverlayContent, y => this.OverlayContent.style.top = y, 50, this.AnimationSpeed).SetEase(Ease.Linear);

        this.ToggleOverlay.Q<VisualElement>("ToggleOverlay_icon").transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Debug.Log("mOves overlay into view:" + this.restingYposition_overlay);

        this.header_headline.style.opacity = 0f;

        this.ToggleOverlay.clicked -= MoveOverlayIntoView;
        this.ToggleOverlay.clicked += MoveOverlayOutOfView;



    }

    public void MoveOverlayOutOfView() {
        DOTween.To(() => this.overlay.worldBound.position.y, y => this.overlay.style.top = y, this.restingYposition_overlay, this.AnimationSpeed).SetEase(Ease.Linear);

        DOTween.To(() => this.OverlayContent.worldBound.position.y, y => this.OverlayContent.style.top = y, this.restingYposition_OverlayContent, this.AnimationSpeed).SetEase(Ease.Linear);

        this.ToggleOverlay.Q<VisualElement>("ToggleOverlay_icon").transform.rotation = Quaternion.Euler(0f, 0f, 180f);

        this.header_headline.style.opacity = 1f;

        this.ToggleOverlay.clicked += MoveOverlayIntoView;
        this.ToggleOverlay.clicked -= MoveOverlayOutOfView;


    }

    public void ContinueSceneOnButtonPress() {
        //MoveOverlayIntoView();
    }

    public void StartGame() {
        //MoveOverlayIntoView();
    }
    public void GoToMainMenu() {

        if (this.MainSceneName != null) {
            SceneManager.LoadScene(this.MainSceneName);
        } else {
            SceneManager.LoadScene("MainScene");
        }
      
    }

}
