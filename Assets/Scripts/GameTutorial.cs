using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameTutorial : VisualElement
{
    Button btn = new Button();
    VisualElement icon = new VisualElement();
    UITutorialController uITutorialControllerScript;
    public string descriptionText;

    public new class UxmlFactory : UxmlFactory<GameTutorial, UxmlTraits> {
    }
    public new class UxmlTraits : VisualElement.UxmlTraits {
    }

    public GameTutorial() {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void OnGeometryChange(GeometryChangedEvent evt) {

        PopulateContent();
       // PopulateHeader();

        if (SceneManager.GetActiveScene().name != "Game4") {
            PopulateHeader();
        }
            

        UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void PopulateHeader() {
        //Debug.Log("inside populatehead");
        //btn.name = "btnStartGame";
        //btn.AddToClassList("cms-gametutorial-button");
        //btn.text = "Spiel starten";
        //this.Q("header").Add(btn);

        //icon.AddToClassList("cms-gametutorial-button-icon");
        //btn.Add(icon);
        btn = this.Q<Button>("btnStartGame");
        btn.clicked += delegate {
            this.style.display = DisplayStyle.None;
        };
    }
    
    private void PopulateContent() {

        Label subtitle = new Label();
        subtitle.AddToClassList("cms-gametutorial-subtitle");
        subtitle.text = "So Funktioniert's".ToUpper();
        this.Q("content").Add(subtitle);

        Label title = new Label();
        title.AddToClassList("cms-gametutorial-title");
        Label subHeadline = new Label();
        subHeadline.AddToClassList("cms-gametutorial-subheadline");
        ScrollView scroller = new ScrollView();
        scroller.AddToClassList("cms-gametutorial-scroller");
        TextElement scrollText = new TextElement();
        scrollText.name = "tutorial-text";
        title.name = "title";
        subHeadline.name = "subheadline";

        //GAME 1 - ROEMISCH NICHT ROEMISCH
        if (SceneManager.GetActiveScene().name == "Game1") {
            title.text = "Römisch oder nicht römisch?".ToUpper();
            subHeadline.text = "Teste dein Expertenwissen.";
        }

        //GAME 2 - SUCHEN UND FINDEN
        if (SceneManager.GetActiveScene().name == "game2") {

            title.text = "wir Suchen und Finden".ToUpper();
            subHeadline.text = "Komm mit uns!";
            scrollText.text = "Was schimmert denn da? Münze oder Kronkorken? Mit dem Handy kannst du den Boden scannen und hineinschauen. So ein Zaubergerät hätten Archäologen auch gerne, denn in Wirklichkeit ist das alles nicht so einfach. Los geht’s! Scanne den Boden und finde die Münzen. Doch Vorsicht! Es ist nicht alles Geld, was glänzt.";
        }

        //GAME 4 - MIT SCHAUFEL UND KELLE
        if (SceneManager.GetActiveScene().name == "Game3") {

            title.text = "MIT SCHAUFEL UND KELLE".ToUpper();
            subHeadline.text = "Komm ins Grabungsteam!";
            //scrollText.text = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

        }

        //GAME 4 - MIT SANDSTRAHLER UND SKALPELL
        if (SceneManager.GetActiveScene().name == "Game4") {

            title.text = "Mit sandstrahler und skalpell!".ToUpper();
            subHeadline.text = "Ab in die Werkstatt!";
            //scrollText.text = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

        }

        //GAME 5 - STATTE DEN LEGIONAR AUS
        if (SceneManager.GetActiveScene().name == "Game5") {

            title.text = "statte den legionaer aus!".ToUpper();
            subHeadline.text = "Von Kopf bis Fuss von dir ausgeruestet.";
            //scrollText.text = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

        }


        this.Q("content").Add(title);
        this.Q("content").Add(subHeadline);
        this.Q("content").Add(scroller);
        scroller.Add(scrollText);
        scroller.Q<VisualElement>("unity-slider").style.opacity = 0;
        scroller.Q<VisualElement>("unity-low-button").style.opacity = 0;
        scroller.Q<VisualElement>("unity-high-button").style.opacity = 0;
        scroller.Q<VisualElement>("unity-slider").style.opacity = 0;
    }
}
