using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Services;

public class introSceneButtons : MonoBehaviour {
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

    AppPermissions permissionrequesthandler;

    introSceneContentLoader introSceneContentLoader;
    CrossGameManager crossGameManager;
    StrapiService strapiService;

    private void Awake() {

        //crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        permissionrequesthandler = GetComponent<AppPermissions>();
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        sceneOne = m_Root.Q<VisualElement>("sceneOne");

        standorBtnActivate = m_Root.Q<Button>("standort-btnActivate");
        standorBtnActivate?.RegisterCallback<ClickEvent>(ev => RquestLocation());

        standorBtnDisable = m_Root.Q<Button>("standort-btnDisable");
        standorBtnDisable?.RegisterCallback<ClickEvent>(ev => DenyLocation());

        kameraBtnActivate = m_Root.Q<Button>("kamera-btnActivate");
        kameraBtnActivate?.RegisterCallback<ClickEvent>(ev => RquestCam());

        kameraBtnDisable = m_Root.Q<Button>("kamera-btnDisable");
        kameraBtnDisable?.RegisterCallback<ClickEvent>(ev => DenyCamera());

        introSceneContentLoader = gameObject.GetComponent<introSceneContentLoader>();
        introSceneContentLoader.LoadContent_static();
    }



    public void Populate() {
        standtortScene = m_Root.Q<VisualElement>("standort");
        standortTitle = standtortScene.Q<Label>("standort-title");
        standortTitle.text = introSceneContentLoader.locationPermission_text;
        if (standortTitle.text == null) {
            standortTitle.text = "Zum Aktivieren aller Funktionen für den Park-Entdecker benötigt unsere App die Standortfreigabe deines Gerätes.";
        }

        standorBtnDisable = standtortScene.Q<Button>("standort-btnDisable");
        standorBtnDisable.text = "NEIN, KEINE STANDORT-FREIGABE ERLAUBEN";
        standorBtnDisable.text = introSceneContentLoader.locationPermission_denyBtn;

        standorBtnActivate = standtortScene.Q<Button>("standort-btnActivate");
        standorBtnActivate.text = "Ja, die Standort-Freigabe aktivieren.";
        standorBtnActivate.text = introSceneContentLoader.locationPermission_allowBtn;


        kameraScene = m_Root.Q<VisualElement>("kamera");
        kameraTitle = kameraScene.Q<Label>("kamera-text");
        kameraTitle.text = "Das digitale Entdecken unseres Parkes mit dieser App ist nur mit Freigabe der Kamerafunktionen deines Ger?tes m?glich.";
        kameraTitle.text = introSceneContentLoader.cameraPermission_text;

        kameraBtnDisable = m_Root.Q<Button>("kamera-btnDisable");
        kameraBtnDisable.text = "NEIN, KEINEN KAMERA-ZUGRIFF ERLAUBEN";
        kameraBtnDisable.text = introSceneContentLoader.cameraPermission_denyBtn;

        kameraBtnActivate = m_Root.Q<Button>("kamera-btnActivate");
        kameraBtnActivate.text = "Ja, den Kamera-Zugriff erlauben.";
        kameraBtnActivate.text = introSceneContentLoader.cameraPermission_allowBtn;

    }

    public void DenyLocation() {
        standortTitle = standtortScene.Q<Label>("standort-title");
        standortTitle.text = introSceneContentLoader.locationError;
    }

    public void DenyCamera() {
        kameraTitle = kameraScene.Q<Label>("kamera-text");
        kameraTitle.text = introSceneContentLoader.cameraError;
    }

    public void RquestLocation() {
        permissionrequesthandler.RquestLocationAccess();
    }

    public void RquestCam() {
        permissionrequesthandler.RquestCameraAccess();
    }
    public void SceneStandortFadeOut() {
        standtortScene = m_Root.Q<VisualElement>("standort");
        kameraScene = m_Root.Q<VisualElement>("kamera");
        standtortScene.style.display = DisplayStyle.None;
        kameraScene.style.display = DisplayStyle.Flex;
    }


    public void SceneKamera() {
        this.m_Root.Q<VisualElement>("IntroScene").style.display = DisplayStyle.None;
        gameObject.GetComponent<TutorialController>().ShowImage_static();
    }
}