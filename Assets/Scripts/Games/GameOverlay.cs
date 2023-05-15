using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class GameOverlay : MonoBehaviour
{

    //Animation Properties
    public float AnimationSpeed = 0.5f;
    
    //UI elements
    public VisualElement overlay;
    public Button Button;
    public Label Headline;
    public Label Detail;
    public Label ButtonText;
    public VisualElement Image;
    public Button EndGameButton;
    
    
    //Temporary Variables
    private bool ShouldExitScene = false;
    public string NewSceneName;
    public string MainSceneName;
    private float restingYposition;

    //Access GameController
    public GameObject GameController;


    private void Awake() {
   
    }

    /// Run start as enumerator to wait for first frame and be able to get all properties from UI elements
    private IEnumerator Start() {
        yield return new WaitForEndOfFrame();
        AssignUIElements();
    }

    public void AssignUIElements() {
        Debug.Log("Assigns elements");
        var root = GetComponent<UIDocument>().rootVisualElement;

        this.overlay = root.Q<VisualElement>("GameOverlay");
        this.restingYposition = this.overlay.worldBound.position.y;

        Debug.Log("resting postion" + this.restingYposition);
        this.Image = root.Q<VisualElement>("CONTENT-IMAGE");
        this.Button = root.Q<Button>("ActionButton");
        this.Detail = root.Q<Label>("DETAIL");
        this.Headline = root.Q<Label>("HEADLINE");
        this.ButtonText = root.Q<Label>("ButtonText");

        this.EndGameButton = root.Q<Button>("ExitButton");
        this.EndGameButton.clicked += GoToMainMenu;

    }

    public void UpdateAndShowGameOverlay(string Headline, string Detail, string ButtonText, bool ShouldExitScene, bool Startgame, string NewSceneName, Sprite sprite, Color color) {

        //check if UI elements have been assigned. If not: Assign them!
        if (this.Headline == null) {
            this.AssignUIElements();
        }

        this.overlay.style.backgroundColor = new Color(color.r, color.g, color.b, 1f);

        this.Headline.text = Headline;


        if (this.Detail != null) {
            this.Detail.text = Detail;
        }
   

        this.ButtonText.text = ButtonText;
        this.Image.style.backgroundImage = new StyleBackground(sprite);

        if (ShouldExitScene) {
            this.ShouldExitScene = true;
            this.NewSceneName = NewSceneName;
            this.Button.clicked += ExitSceneOnButtonPress;

        } else if (Startgame) {
            this.ShouldExitScene = false;
            this.Button.clicked += StartGame;

        } else {
            this.ShouldExitScene = false;
            this.Button.clicked += ContinueSceneOnButtonPress;
        }

     
        MoveOverlayIntoView(true);
    }

    public void MoveOverlayIntoView(bool ShouldAnimateIn) {

        Debug.Log("moves into view");
        if (ShouldAnimateIn) {
            DOTween.To(() => this.restingYposition, y => this.overlay.style.top = y, 0, this.AnimationSpeed).SetEase(Ease.Linear);
        } else {
            DOTween.To(() => this.overlay.worldBound.position.y, y => this.overlay.style.top = y, this.restingYposition, this.AnimationSpeed).SetEase(Ease.Linear);
        }




    }
    void ExitSceneOnButtonPress() {
        Debug.Log("PRESSES BUTTON TO END GAME" + this.NewSceneName);
        if (this.ShouldExitScene) {
            if (this.NewSceneName != null) {
                SceneManager.LoadScene(this.NewSceneName);
            } else {
                SceneManager.LoadScene("MainScene");

            }
        }

    }
    public void ContinueSceneOnButtonPress() {
        Debug.Log("PRESSES BUTTON TO CONTINUE GAME");
        MoveOverlayIntoView(false);
    }

    public void StartGame() {
        Debug.Log("PRESSES BUTTON TO start GAME");

        this.GameController.GetComponent<startGame2>().startGame();
        MoveOverlayIntoView(false);
    }
    public void GoToMainMenu() {

        Debug.Log("end scene button clicked");

        if (this.MainSceneName != null) {
            SceneManager.LoadScene(this.MainSceneName);
        } else {
            SceneManager.LoadScene("MainScene");
        }
      
    }

}
