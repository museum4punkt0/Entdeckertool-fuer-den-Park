using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class detektorUI : VisualElement {

    Button introStartBtn;
    Button introCloseBtn;
    Button infoPanelCloseBtn;
    VisualElement introPopup;
    VisualElement infoPanel;
    public new class UxmlFactory : UxmlFactory<detektorUI, UxmlTraits> {
    }

    public new class UxmlTraits : VisualElement.UxmlTraits {
    }

    public detektorUI() {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void OnGeometryChange(GeometryChangedEvent evt) {

        /*
        infoPanel = this.Q<VisualElement>("info-panel");
        introStartBtn = this.Q<Button>("intro-start-btn");
        introCloseBtn = this.Q<Button>("intro-close-btn");
        introPopup = this.Q<VisualElement>("intro-popup");
        infoPanelCloseBtn = infoPanel.Q<Button>("intro-close-btn");

        introStartBtn.clicked += delegate {
            this.style.display = DisplayStyle.None;
        };

        introCloseBtn.clicked += delegate {
            this.style.display = DisplayStyle.None;
        };

        infoPanelCloseBtn.clicked += delegate {
            this.style.display = DisplayStyle.None;
        };
        */

    }

}
