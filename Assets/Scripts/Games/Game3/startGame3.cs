using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class startGame3 : MonoBehaviour
{
    public ARRaycastManager arRaycastManager;
    private Vector2 touchPosition;
    static List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    private Camera _camera;
    private bool testingOnLapTop = true;
    public Camera ARCamera;
    public Camera GameCamera;

    public GameObject[] teil2_parts;
    public GameObject MenuPanel; ///menu item to toggle once


    //public GameObject teil1_endingUI;

    public int scores = 0;



    bool TryGetTouchPosition(out Vector2 touchPosition) {
      if (Input.touchCount > 0) {
          touchPosition = Input.GetTouch(0).position;
          return true;
      }
      touchPosition = default;
      return false;
    }

    // Start is called before the first frame update
    void Start()
    {

        this.MenuPanel.GetComponent<AnimateMenu>().ShowHideMenu();
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            this.testingOnLapTop = false;
            this._camera = this.ARCamera;
            this.GameCamera.enabled = false;
        } else {
            this._camera = this.GameCamera;
            this.ARCamera.enabled = false;
        }

    }

    void Update() {

        if (this.testingOnLapTop == true) {
            if (Input.GetMouseButtonDown(0)) {
                HandleTouch(Input.mousePosition);

            }

        } else {

            if (!TryGetTouchPosition(out Vector2 touchPosition)) {
                return;
            }


            ///currently does a redundant double check for touch position and Input.touch
            if (this.arRaycastManager.Raycast(touchPosition, m_Hits)) {
                //HandleRaycast(m_Hits[0]);

                if (Input.touchCount > 0 && Input.touchCount < 2) {
                    if (Input.GetTouch(0).phase == TouchPhase.Began) {
                        HandleTouch(Input.GetTouch(0).position);
                    }
                }

            }

        }


   }


    public void SetScore1() {
        this.scores += 1;
        Debug.Log("sets score");
        if (this.scores >= 2) {
            FinishTeil1();
        }
    }


    void HandleTouch(Vector3 pos) {

        Ray ray = this._camera.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)) {

            if (hit.transform.gameObject.CompareTag("digger")) {
                GameObject plane = hit.transform.gameObject;
                Debug.Log("dig");
                Debug.Log(plane.name);
  


            }


            if (hit.transform.gameObject.CompareTag("discoverable_object")) {
                GameObject Object = hit.transform.gameObject;
                Debug.Log(Object.name);
            }


        }

    }

    void FinishTeil1() {

        //Vector2 position = this.teil1_endingUI.transform.GetComponent<RectTransform>().anchoredPosition;
        //this.teil1_endingUI.transform.GetComponent<RectTransform>().position = new Vector2(position.x, position.y+100);
        GameObject[] teil1_parts = GameObject.FindGameObjectsWithTag("spiel2_teil1");
        foreach (var part in teil1_parts) {
            part.SetActive(false);
        }
        BeginTeil2();
    }
    void BeginTeil2() {
        Debug.Log("begin teil2");

        foreach (var part in this.teil2_parts) {
            Debug.Log(part);
            part.SetActive(true);
        }

    }



}
