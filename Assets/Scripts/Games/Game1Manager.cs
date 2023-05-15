using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.UIElements;

public class Game1Manager : MonoBehaviour {

    public CrossGameManager crossGameManager;
    public GameObject tutorial;
    public TextElement tutorialText;
    Button gameStart;
    public bool gameHasStarted = false;

    public Game1Manager() {

    }

    public Game game1;

    //public GameObject tuturialScene;
    private bool hasAssignedDescriptionToField;

    public GameObject textContainer;

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    private void Start() {

        StartCoroutine(LoadContent());
    }

    private IEnumerator LoadContent() {
        yield return new WaitForSeconds(1f);
        tutorial = GameObject.FindGameObjectWithTag("Game1Tutorial");
        tutorialText = tutorial.GetComponent<UIDocument>().rootVisualElement.Q<TextElement>("tutorial-text");
        gameStart = tutorial.GetComponent<UIDocument>().rootVisualElement.Q<Button>("btnStartGame");

        gameStart.clicked += delegate {
            gameHasStarted = true;
	    };

        StartCoroutine(GetGameContent());

    }


    private IEnumerator GetGameContent() {

        StartCoroutine(this.crossGameManager.strapiService.getSpiel1TutorialContent((StrapiSingleResponse<Game> res) => {
            game1 = res.data;

            if (!hasAssignedDescriptionToField && game1.attributes.description != "" && game1.attributes.description != null) {

                //textContainer.GetComponent<TMPro.TextMeshProUGUI>().text = game1.attributes.description;
                tutorialText.text = game1.attributes.description;

                hasAssignedDescriptionToField = true;

                print("GetGameContent");
            }
        }));

        yield return null;
    }
}
