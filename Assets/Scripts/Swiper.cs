using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Swiper : MonoBehaviour {
    UIItemViewController uIItemViewController;
    CrossGameManager crossGameManager;
    VisualElement mainButton;
    string prnt;
    private float timeTouchEnded;

    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition;
    public string direction;
    public string horizontalDirection;
    public bool allowSwiper;
    public bool FirstTimeUse;
    bool singleTime;

    float timeDelayThreshold = 1.0f;

    public float holdDuration = 0.5f;

    public List<int> touchesList = new List<int>();

    private void Start() {
        uIItemViewController = gameObject.GetComponent<UIItemViewController>();
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        mainButton = uIItemViewController.m_MainButton;
    }

    void Update() {

        /////////////////////
        int touches = 0;

        if (Input.touchCount > 0) {
            touches = 1;
            touchesList.Add(touches);

            theTouch = Input.GetTouch(0);

            
            if (!FirstTimeUse) {
           
                if ((uIItemViewController.isOpen &&
                   (Input.GetTouch(0).position.y <= Screen.height && Input.GetTouch(0).position.y >= Screen.height - 400)) ||
                    (!uIItemViewController.isOpen &&
                   (Input.GetTouch(0).position.y <= 400 && Input.GetTouch(0).position.y >= 0))) {

                    allowSwiper = true;

                } else {
                    allowSwiper = false;
                }
            
            } else if (FirstTimeUse) {

                singleTime = true;
                allowSwiper = true;
                FirstTimeUse = false;
            }
            

            if (allowSwiper) {

                //swipe core
                if (theTouch.phase == TouchPhase.Began) {
                    touchStartPosition = theTouch.position;

                } else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended) {
                    touchEndPosition = theTouch.position;

                    float x = touchEndPosition.x - touchStartPosition.x;
                    float y = touchEndPosition.y - touchStartPosition.y;

                    if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0) {
                        direction = "tapped";
                    } else if (Mathf.Abs(x) > Mathf.Abs(y)) {
                        direction = x > 0 ? "Right" : "Left";
                        
                    } else {
                        direction = y > 0 ? "Up" : "Down";

                        //block menu from opening when 360 view is on
                        if (uIItemViewController.panoramaSceneManager != null && !uIItemViewController.panoramaSceneManager.displaysAR) {
                            this.gameObject.GetComponent<UIItemViewController>().mainButtonOnClick(direction);
                        } 
                        
                    }
                }
                
            }

        } else if (Input.touchCount == 0) {
            touchesList.Clear();
        }

        if (singleTime) {
            if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended) {
                touchEndPosition = theTouch.position;

                float x = touchEndPosition.x - touchStartPosition.x;
                float y = touchEndPosition.y - touchStartPosition.y;

                if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0) {
                    direction = "tapped";
                } else if (Mathf.Abs(x) > Mathf.Abs(y)) {
                    direction = x > 0 ? "Right" : "Left";
                    
                } else {
                    direction = y > 0 ? "Up" : "Down";
                    this.gameObject.GetComponent<UIItemViewController>().mainButtonOnClick(direction);
                    singleTime = false;
                    FirstTimeUse = false;
                }
            }
            
        }

    }
}
