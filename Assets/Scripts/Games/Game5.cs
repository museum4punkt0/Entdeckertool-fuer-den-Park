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

public class Game5 : VisualElement {

    Label title;
    Label coins;
    Label subText;
    VisualElement popup;
    Label popupHeadline;
    Label popupSubheadline;
    Button popupButton;
    Button popupClose;
    Button card01;
    VisualElement card01Image;
    VisualElement card01Pin;
    Label card01Label;
    Button card02;
    VisualElement card02Image;
    VisualElement card02Pin;
    Label card02Label;
    Button card03;
    VisualElement card03Image;
    VisualElement card03Pin;
    Label card03Label;
    Button card04;
    VisualElement card04Image;
    VisualElement card04Pin;
    Label card04Label;
    GameObject game5UI;

    public new class UxmlFactory : UxmlFactory<Game5, UxmlTraits> {
    }
    public new class UxmlTraits : VisualElement.UxmlTraits {
    }

    public Game5() {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void OnGeometryChange(GeometryChangedEvent evt) {
        title = this.Q<Label>("title");
        coins = this.Q<Label>("coins-label");
        subText = this.Q<Label>("subText");
        popupHeadline = this.Q<Label>("popup-headline");
        popupSubheadline = this.Q<Label>("popup-subHeadline");
        popupButton = this.Q<Button>("popup-button");

        Populate();
        //PopulateCards();

        popup = this.Q<VisualElement>("PopUp");
        popupClose = this.Q<Button>("popup-close");
        popupClose?.RegisterCallback<ClickEvent>(ev => popup.style.display = DisplayStyle.None);
        this.Q<Button>("popup-close").RegisterCallback<ClickEvent>(ev => popup.style.display = DisplayStyle.None);




        //this.Q<Button>("card01").RegisterCallback<ClickEvent>(ev => popup.style.display = DisplayStyle.Flex);

        //ActivateCard();
        //WinPopUp();

        UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void Populate() {
        ScrollView scroller = this.Q<ScrollView>();


        if (scroller != null) {
            if (scroller.Q<VisualElement>("unity-slider") != null) {
                scroller.Q<VisualElement>("unity-slider").style.opacity = 0;
            }

            if (scroller.Q<VisualElement>("unity-low-button") != null) {
                scroller.Q<VisualElement>("unity-low-button").style.opacity = 0;
            }

            if (scroller.Q<VisualElement>("unity-high-button") != null) {
                scroller.Q<VisualElement>("unity-high-button").style.opacity = 0;
            }

            if (scroller.Q<VisualElement>("unity-slider") != null) {
                scroller.Q<VisualElement>("unity-slider").style.opacity = 0;
            }

        }

        if (SceneManager.GetActiveScene().name == "Game5") {
            if (title != null) {
                title.text = "Die Legion?rs-ausstattung";
            }

            if (coins != null) {
                coins.text = "2";
            }

            if (subText != null) {
                subText.text = "Finde 9 Ausstattungsteile der Legion?rsausr?stung: ";
            }

            if (popupHeadline != null) {
                popupHeadline.text = "W?hle je ein Bild und finde das passende Ausstattungsteil im Park.";
                popupSubheadline.text = "Wenn du alle 9 Teile gefunden hast, kannst du den Legion?r ausstatten und zum Leben erwecken.";
                popupButton.text = "Schritt 1: Funde im Park suchen";
            }
        }
 


  

    }


    private void PopulateCards() {

        //CARD 01
        card01 = new Button();
        card01.name = "card01";
        card01.AddToClassList("cms-game5-card-wrapper");
        card01Image = new VisualElement();
        card01Image.AddToClassList("cms-game5-card01-image");
        card01Pin = new VisualElement();
        card01Pin.AddToClassList("cms-game5-card-pinIcon");
        card01Label = new Label();
        card01Label.name = "card01-label";
        //card01Label.text = "oi";
        card01Image.Add(card01Pin);
        card01.Add(card01Image);
        card01.Add(card01Label);
        this.Q<ScrollView>().Add(card01);

        card01.clicked += delegate {
            //popup.style.display = DisplayStyle.Flex;
        }; 

        //CARD 02
        card02 = new Button();
        card02.name = "card02";
        card02.AddToClassList("cms-game5-card-wrapper");
        card02Image = new VisualElement();
        card02Image.AddToClassList("cms-game5-card02-image");
        card02Pin = new VisualElement();
        card02Pin.AddToClassList("cms-game5-card-pinIcon");
        card02Label = new Label();
        card02Label.text = "Ausstattungsteil 2";
        card02Image.Add(card02Pin);
        card02.Add(card02Image);
        card02.Add(card02Label);
        this.Q<ScrollView>().Add(card02);

        card02.clicked += delegate {
            popup.style.display = DisplayStyle.Flex;
        };

        //CARD 03
        card03 = new Button();
        card03.name = "card03";
        card03.AddToClassList("cms-game5-card-wrapper");
        card03Image = new VisualElement();
        card03Image.AddToClassList("cms-game5-card03-image");
        card03Pin = new VisualElement();
        card03Pin.AddToClassList("cms-game5-card-pinIcon");
        card03Label = new Label();
        card03Label.text = "Ausstattungsteil 3";
        card03Image.Add(card03Pin);
        card03.Add(card03Image);
        card03.Add(card03Label);
        this.Q<ScrollView>().Add(card03);

        card03.clicked += delegate {
            popup.style.display = DisplayStyle.Flex;
        };

        //CARD 04
        card04 = new Button();
        card04.name = "card04";
        card04.AddToClassList("cms-game5-card-wrapper");
        card04Image = new VisualElement();
        card04Image.AddToClassList("cms-game5-card04-image");
        card04Pin = new VisualElement();
        card04Pin.AddToClassList("cms-game5-card-pinIcon");
        card04Label = new Label();
        card04Label.text = "Ausstattungsteil 4";
        card04Image.Add(card04Pin);
        card04.Add(card04Image);
        card04.Add(card04Label);
        this.Q<ScrollView>().Add(card04);

        card04.clicked += delegate {
            popup.style.display = DisplayStyle.Flex;
        };

    }

    private void ActivateCard() {

         
        card01Image.RemoveFromClassList("cms-game5-card01-image");
        card01Image.AddToClassList("cms-game5-card01-image-active");
        card01Pin.RemoveFromClassList("cms-game5-card-pinIcon");
        card01Pin.AddToClassList("cms-game5-card-pinIcon-active");
        card01Label.text = "Schienenpanzer";
        card01.SetEnabled(false);
    }

    private void WinPopUp() {
        popupHeadline.text = "Bravo! Du hast alle Teile der Legion?rs-ausstattung im Park gefunden.";
        popupSubheadline.text = "";
        popupButton.text = "Schrit2: Legion?r anziehen";
    }
}
