using System;
using System.Collections;
using Services;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Game4Controller : MonoBehaviour
{

    public GameObject objectClean;
    public GameObject objectDirty;
    public UnityEngine.UI.Image objectDirty_Image;
    public CrossGameManager crossGameManager;
    public Game4UIController game4UIController;
    int coinAmount = 1;

    public GameObject image;
    UnityEngine.UI.Image img;

    public Game game4;
    public bool isObjectCleaned;
    public GameObject tuturialScene;
    private bool hasAssignedDescriptionToField;
    public VisualElement m_Root_Tutorial;

    public float currentAlpha = 1;
    public Game4Controller(){

    }

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        m_Root_Tutorial = tuturialScene.GetComponent<UIDocument>().rootVisualElement;

        isObjectCleaned = false;

        StartCoroutine(GetGameContent());
    }

    private IEnumerator GetGameContent() {
        
        StartCoroutine(this.crossGameManager.strapiService.getSpiel4TutorialContent((StrapiSingleResponse<Game> res) => {
            game4 = res.data;

            StartCoroutine(placeTutorialContent(game4));

        }));
        
        yield return null;
    }

    IEnumerator placeTutorialContent(Game game) {
        yield return new WaitForEndOfFrame();
        if (!hasAssignedDescriptionToField && game.attributes.description != "" && game.attributes.description != null) {
            m_Root_Tutorial.Q<TextElement>("tutorial-text").text = game.attributes.description;
            hasAssignedDescriptionToField = true;
        }
    }


    public float CleanObject() {

        objectDirty_Image = objectDirty.GetComponent<UnityEngine.UI.Image>();
        img = objectDirty_Image;
        var tempColor = img.color;

        tempColor.a -= 0.005f;

        img.color = tempColor;

        return tempColor.a;
    }

    public void CheckIfObjectIsClean() {
        
        objectDirty_Image = objectDirty.GetComponent<UnityEngine.UI.Image>();

        print("checks if img is clean" + isObjectCleaned);

        if (objectDirty_Image.color.a < 0 && !isObjectCleaned) {

            isObjectCleaned = true;

            print("img is clean");
            this.currentAlpha = 1;

            crossGameManager.AddToScore(crossGameManager.colorFromHex("#FD8300"), coinAmount);
            game4UIController.AfterObjectIsCleaned(coinAmount);

            
        }

    }

}
