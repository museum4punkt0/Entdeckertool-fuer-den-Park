using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnimateMenu : MonoBehaviour
{

    float screenHeight;
    float idealHeight;
    public GameObject PanelMenu;
    public bool isOpen = true;


    // Start is called before the first frame update
    void Awake()
    {
        SetIdealHeight();

        //check active scene
        if (SceneManager.GetActiveScene().name == "FindTool") {
            isOpen = false;
            transform.LeanMoveLocal(new Vector2(0, idealHeight), 0f);
            PanelMenu.GetComponent<Image>().enabled = false;
            PanelMenu.transform.GetChild(0).LeanMoveLocal(new Vector2(0, -210), 0);
        }
    }

    private void SetIdealHeight() {
       
        screenHeight = Screen.height;

        if (screenHeight >= 2532) {
            idealHeight = -1940;
        } else if (screenHeight >= 2436) {
            idealHeight = -1940;
        } else if (screenHeight >= 2388) {
            idealHeight = -1540;
        } else if (screenHeight >= 2340) {
            idealHeight = -1940;
        } else if (screenHeight >= 1920) {
            idealHeight = -1540;
        } else if (screenHeight >= 1792) {
            idealHeight = -1940;
        } else if (screenHeight >= 1536) {
            idealHeight = -1540;
        } else if (screenHeight >= 1366) {
            idealHeight = -1940;
        } else if (screenHeight >= 1136) {
            idealHeight = -1540;
        } else if (screenHeight >= 360) {
            idealHeight = -1540;

        } else {
            idealHeight = -1540;
        }
    }

    public void ShowHideMenu() {

        if (this.idealHeight == 0) {
            SetIdealHeight();
        }


        if (PanelMenu != null)
        {
            if (isOpen) {

                Debug.Log("MOVES MENU PANEL TO" + idealHeight);
                transform.LeanMoveLocal(new Vector2(0, idealHeight), 1).setEaseOutQuart();

                PanelMenu.GetComponent<Image>().enabled = false;
                PanelMenu.transform.GetChild(0).LeanMoveLocal(new Vector2(0, -210), 1f).setEaseOutQuart();

                isOpen = false;

            } else if(!isOpen) {
                transform.LeanMoveLocal(new Vector2(0, 0), 1).setEaseOutQuart();

                PanelMenu.GetComponent<Image>().enabled = true;
                PanelMenu.transform.GetChild(0).LeanMoveLocal(new Vector2(0, 0), 1f).setEaseOutQuart();

                isOpen = true;
            }
            
        }
    }

}
