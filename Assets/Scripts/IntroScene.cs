
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

public class IntroScene : VisualElement {
    SceneController sceneController;

     VisualElement m_Root;
     VisualElement sceneOne;
    
     VisualElement standtortScene;
     Label standortTitle;
     Button standorBtnDisable;
     Button standorBtnActivate;
     VisualElement kameraScene;
     Label kameraTitle;
     Button kameraBtnDisable;
     Button kameraBtnActivate;
     UIItemViewController uIItemViewControllerScript;

    // Start is called before the first frame update

    public new class UxmlFactory : UxmlFactory<IntroScene, UxmlTraits> {
    }

    public new class UxmlTraits : VisualElement.UxmlTraits {
    }

    public IntroScene() {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    
    }
    
    private void OnGeometryChange(GeometryChangedEvent evt) {

        sceneOne = this.Q<VisualElement>("sceneOne");
        SceneFadeOut();



        UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }



    private void SceneFadeOut() {
        standtortScene = this.Q<VisualElement>("standort");

        sceneOne.style.opacity = 0;
        standtortScene.style.opacity = 1;
        this.Q<VisualElement>("standort-btnsWrapper").style.display = DisplayStyle.Flex;
        //standtortScene.style.display = DisplayStyle.Flex;

    }


}

