using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AppHeader : VisualElement
{
    Button close;
    Label headline;
    VisualElement background;
    CrossGameManager crossGameManager;

    public new class UxmlFactory : UxmlFactory<AppHeader, UxmlTraits> {
    }
    public new class UxmlTraits : VisualElement.UxmlTraits {
    }

    public AppHeader() {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void OnGeometryChange(GeometryChangedEvent evt) {

        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        close = this.Q<Button>("close");

        headline = this.Q<Label>("headline");


        background = this;


        if (crossGameManager.IsVisitingFromTour) {

            Debug.Log("assigns tour loader to back button");
            close.RegisterCallback<ClickEvent>(ev => SceneManager.LoadScene("Parktour"));
            headline.text = "";
            background.style.backgroundColor = crossGameManager.colorToType("tour");

            crossGameManager.IsVisitingFromTour = false;


        } else if (SceneManager.GetActiveScene().name.Contains("Game6")) {
            headline.text = "Panorama-Ansichten";
            background.style.backgroundColor = crossGameManager.colorToType("panorama");
            close.RegisterCallback<ClickEvent>(ev => crossGameManager.GoBackToPreviousScene());


        } else if (SceneManager.GetActiveScene().name != "Game1") {
            Debug.Log("assigns mainscene to back button");



            background.style.backgroundColor = crossGameManager.colorToType("spiel");

            if (SceneManager.GetActiveScene().name != "ParkTour") {
                headline.text = headline.text = "spiel beenden".ToUpper();
                close.RegisterCallback<ClickEvent>(ev => crossGameManager.GoBackToPreviousScene());

            } else {

                Debug.Log("assigns pop up to button");
                TourManager tm = GameObject.FindObjectOfType<TourManager>();
                close.RegisterCallback<ClickEvent>(ev => tm.OpenEndTourPopUp());


                //close.RegisterCallback<ClickEvent>(ev => crossGameManager.GoBackToPreviousScene());
            }




        } 





        UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}
