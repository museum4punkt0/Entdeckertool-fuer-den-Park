using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePopUp : MonoBehaviour {


    Label title;
    Label subText;
    VisualElement popup;
    public GameObject popupContainer;
    Label popupHeadline;
    Label popupExtraheadline;
    Label popupSubheadline;
    Button popupButton;
    Button popupClose;

    public GameObject GameController;

    public List<GameObject> UIElementsToStopInteractionWith;

  


    private IEnumerator Start() {
        yield return new WaitForEndOfFrame();
        AssignPopUpUIElements();
        Debug.Log("STARTS GAME POP UP");
    }

    public void AssignPopUpUIElements() {

        Debug.Log("Assigns elements");

        var root = this.GetComponent<UIDocument>().rootVisualElement;

        popup = root.Q<VisualElement>("PopUp");
        popupClose = root.Q<Button>("popup-close");
        //popupClose?.RegisterCallback<ClickEvent>(ev => popup.style.display = DisplayStyle.None);
        //
        //
        popupClose?.RegisterCallback<ClickEvent>(ev => ButtonClick());

        title = root.Q<Label>("title");
        subText = root.Q<Label>("subText");
        popupHeadline = root.Q<Label>("popup-headline");
        popupExtraheadline = root.Q<Label>("popup-extraheadline");
        
        
        popupSubheadline = root.Q<Label>("popup-subHeadline");
        popupButton = root.Q<Button>("popup-button");
        popupButton?.RegisterCallback<ClickEvent>(ev => popup.style.display = DisplayStyle.None);
        root.Q<Button>("popup-button").RegisterCallback<ClickEvent>(ev => ButtonClick());

 
        popupButton.clicked += ButtonClick;

        //if (GameController.GetComponent<startGame3_AR_Ready>()) {
        //    popupClose.clicked += ButtonClick;
        //}
        //if (GameController.GetComponent<startGame2>()) {
        //    popupClose.clicked += ButtonClick;
        //}

        popup.style.display = DisplayStyle.None;
        StopInteractionWithUIElements(false);
    }

    private void ButtonClick() {

        if (GameController.GetComponent<startGame3_AR_Ready>()) {
            GameController.GetComponent<startGame3_AR_Ready>().StartNextState();
        } else if (GameController.GetComponent<startGame2>()) {
            GameController.GetComponent<startGame2>().StartNextState();
        } else if (GameController.GetComponent<TourManager>()) {
            GameController.GetComponent<TourManager>().LeaveTour();
        }

        StopInteractionWithUIElements(true);

        popup.style.display = DisplayStyle.None;
    }

    public void ShowAndUpdatePopUp(string Title, string SubTitle, string Subheadline, string ButtonText, string Type) {

        AssignPopUpUIElements();
        StopInteractionWithUIElements(false);

        if (Type == "Error") {
            popup.RemoveFromClassList("inFokus");
            popup.AddToClassList("error");
        } else if (Type == "info") {
            popup.RemoveFromClassList("error");
            popup.AddToClassList("inFokus");
        }
  
        popupHeadline.text = Title;
        popupExtraheadline.text = SubTitle;
        if (SubTitle == "") {
            popupExtraheadline.style.display = DisplayStyle.None;
        } else {
            popupExtraheadline.style.display = DisplayStyle.Flex;
        }
        if (Subheadline == "") {
            popupSubheadline.style.display = DisplayStyle.None;
        } else {
            popupSubheadline.style.display = DisplayStyle.Flex;
        }
        popupSubheadline.text = Subheadline;
        popupButton.text = ButtonText;

        popup.style.display = DisplayStyle.Flex;
       

        Debug.Log("should show pop up" + popup.style.display);

        
    }

    public void hidePopUp() {
        if (popup != null) {
            popup.style.display = DisplayStyle.None;
            //StopInteractionWithUIElements(true);
        }
    }

    public void StopInteractionWithUIElements(bool flag) {

        foreach (GameObject uiElement in UIElementsToStopInteractionWith) {
            if (uiElement.activeSelf) {
                Debug.Log("set radial wheel to interact" + flag);
                if (uiElement.GetComponent<RadialWheel>()) {

                    //if (GameController.GetComponent<startGame3_AR_Ready>() && GameController.GetComponent<startGame3_AR_Ready>().state == "state3") {
                    //    uiElement.GetComponent<RadialWheel>().CanInstantiateDragableItem = false;
                    //} else {
                    //    uiElement.GetComponent<RadialWheel>().CanInstantiateDragableItem = flag;

                    //}
                    uiElement.GetComponent<RadialWheel>().CanInstantiateDragableItem = flag;
                }
            }
        }

    }




}
