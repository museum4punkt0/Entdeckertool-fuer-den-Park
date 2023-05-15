using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

    private Canvas canvas;
    public GameObject dirtyObject;
    public GameObject border;
    public GameObject image;
    Image img;
    public Game4Controller game4Controller;

    RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData eventData) {
        if (this.gameObject.transform.parent.name == "Tuch" + "(Clone)") {
            border.gameObject.GetComponent<Image>().enabled = true;
            border.gameObject.GetComponent<Image>().color = new Color32(232, 108, 92,255);
            
        }
    }
    public void OnDrag(PointerEventData eventData) {
        if (this.gameObject.transform.parent.name == "Tuch" + "(Clone)") {
            return;
        } else {
            rectTransform.anchoredPosition += eventData.delta;
            rectTransform.transform.LeanScaleX(0.8f, 0.1f);
            rectTransform.transform.LeanScaleY(0.8f, 0.1f);
        }

    }
    public void OnBeginDrag(PointerEventData eventData) {

    }

    public void OnEndDrag(PointerEventData eventData) {
        if ((this.gameObject.name == "dust") && 
            (this.gameObject.transform.parent.name != "Tuch" + "(Clone)")) {
            rectTransform.gameObject.GetComponent<Image>().enabled = false;
            border.gameObject.GetComponent<Image>().enabled = true;
            image.gameObject.GetComponent<Image>().enabled = true;
            img = dirtyObject.GetComponent<Image>();
            var tempColor = img.color;
            tempColor.a -= 0.5f;
            img.color = tempColor;
            game4Controller.CheckIfObjectIsClean();
        }
    }
}

