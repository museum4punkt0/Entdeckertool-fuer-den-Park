using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateInGameUI : MonoBehaviour
{

    float screenHeight;
    float idealHeight;
    bool isOpen = true;
    private Vector2 position;


    void Awake() {
        this.position = this.gameObject.GetComponent<RectTransform>().anchoredPosition;
        this.gameObject.SetActive(false);
    }

    public void DropDownElement() {
     
            if (this.isOpen) {
                this.gameObject.SetActive(true);
                //this.gameObject.GetComponent<RectTransform>().LeanMoveLocal(new Vector2(0, 0), 1).setEaseOutQuart();
                this.isOpen = false;

            } else if (!this.isOpen) {
                this.gameObject.SetActive(false);

                // this.gameObject.GetComponent<RectTransform>().LeanMoveLocal(this.position, 1).setEaseOutQuart();
                this.isOpen = true;
            }

        
    }
}
