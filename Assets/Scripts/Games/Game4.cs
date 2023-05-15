using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Game4 : VisualElement {
    public new class UxmlFactory : UxmlFactory<Game4, UxmlTraits> {
    }
    public new class UxmlTraits : VisualElement.UxmlTraits {
    }

    public Game4() {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);


    }

    private void OnGeometryChange(GeometryChangedEvent evt) {

        PopulateCards();




        UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void PopulateCards() {

        
        this.Q<ScrollView>().Q("materialWrapper01").Q<Label>().text = "Material: Eisen/Blech";
        this.Q<ScrollView>().Q("materialWrapper02").Q<Label>().text = "Material: Bronze";
        this.Q<ScrollView>().Q("materialWrapper03").Q<Label>().text = "Material: Silber";
        this.Q<ScrollView>().Q("materialWrapper04").Q<Label>().text = "Material: Gold";


        this.Q<ScrollView>().Q<VisualElement>("unity-slider").style.opacity = 0;
        this.Q<ScrollView>().Q<VisualElement>("unity-low-button").style.opacity = 0;
        this.Q<ScrollView>().Q<VisualElement>("unity-high-button").style.opacity = 0;
        this.Q<ScrollView>().Q<VisualElement>("unity-slider").style.opacity = 0;
        this.Q<ScrollView>().mode = ScrollViewMode.Vertical;
    }
}
