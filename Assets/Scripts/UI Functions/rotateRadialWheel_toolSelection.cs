using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.EventSystems;
using System.Linq;

public class rotateRadialWheel_toolSelection : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {


    [Header("Recenter Speed")]
    [SerializeField]
    private float releaseSpeed = 350f;

    [Header("Size of ui overlap")]
    [SerializeField]
    public float overlap_size = 50f;

    public static float steeringInput;

    private List<RectTransform> ui_elements = new List<RectTransform>();
    private List<Transform> allChildren;
    private RectTransform wheel;

    public GameObject gameContainer;

    public RectTransform selectionField;


    private bool hasMoved = false;

    private RectTransform currentElement;

    public GameObject Resizer;

    public float targetScaleSize = 2;

    public float standardScaleSize;


    private void Start() {
        this.wheel = GetComponent<RectTransform>();
        this.allChildren = new List<Transform>(this.transform.GetComponentsInChildren<Transform>());
     
        foreach (Transform child in this.allChildren) {
            if (child.gameObject && child.gameObject.CompareTag("Radial_UI_Element")) {
                RectTransform ElementContainer = child.gameObject.transform.GetChild(0).GetComponent<RectTransform>();
                this.ui_elements.Add(ElementContainer);
                this.standardScaleSize = ElementContainer.transform.localScale.x;

            }
        }

        checkNewPosition();
        RescaleSelectedMenuItem();
    }

    #region Events

    public void OnPointerDown(PointerEventData eventData) {
        StartCalculatingWheelRotation(eventData);
        this.hasMoved = true;
    }

    public void OnDrag(PointerEventData eventData) {

        CalculateWheelRotation(eventData);

        UpdateWheelImage();

    }

    public void OnPointerUp(PointerEventData eventData) {
        OnDrag(eventData);
        checkNewPosition();

    }
    #endregion

    #region Helpers

    Rect GetWorldSpaceRect(RectTransform rt) {
        var r = rt.rect;
        r.center = rt.TransformPoint(r.center);
        r.size = new Vector2(this.overlap_size, this.overlap_size);
        return r;
    }

    #endregion

    #region Calculations

    private float wheelAngle = 0f;
    private float wheelPrevAngle = 0f;


    private Vector2 centerPoint;

    private void StartCalculatingWheelRotation(PointerEventData eventData) {
        this.centerPoint = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, this.wheel.position);
        this.wheelPrevAngle = Vector2.Angle(Vector2.up, eventData.position - this.centerPoint);
    }

    private void CalculateWheelRotation(PointerEventData eventData) {
        Vector2 pointerPos = eventData.position;

        float wheelNewAngle = Vector2.Angle(Vector2.up, pointerPos - this.centerPoint);

        // Do nothing if the pointer is too close to the center of the wheel
        if ((pointerPos - this.centerPoint).sqrMagnitude >= 400f) {
            if (pointerPos.x > this.centerPoint.x)
                this.wheelAngle += wheelNewAngle - this.wheelPrevAngle;

            else
                this.wheelAngle -= wheelNewAngle - this.wheelPrevAngle;
        }
        if (this.wheelAngle >= 360f || this.wheelAngle <= -360f) {
            this.wheelAngle = 0f;
        }
        // Make sure wheel angle never exceeds maximumSteeringAngle
        this.wheelAngle = Mathf.Clamp(this.wheelAngle, -360f, 360f);
        this.wheelPrevAngle = wheelNewAngle;
    }

    public void RemoveFromMenu() {
        if (this.currentElement != null) {
            Destroy(this.currentElement.GetChild(0).gameObject);
            this.ui_elements.Remove(this.currentElement);
        }
     }
    private void checkNewPosition() {

        //float newAngle = 0F;
        bool hasOverlap = false;
        
        foreach (RectTransform element in this.ui_elements) {

            if (GetWorldSpaceRect(this.selectionField).Overlaps(GetWorldSpaceRect(element))) {
                if (element.GetChild(0) != null) {

                    Debug.Log("Overlaps" + element.GetChild(0).name);
                 
                    this.currentElement = element;

                    this.gameContainer.GetComponent<start_teil2>().SetCurrentTool(element.GetChild(0).name);

                    //this.NewDragableObject.gameObject.SetActive(true);
                    //newAngle = this.availableAngles.OrderBy(v => Math.Abs((long)v - this.wheelAngle)).First();
                    //newAngle = this.wheelAngle;

                    //StartCoroutine(ReleaseWheel(newAngle));
                    hasOverlap = true;
                    return;

                }
       
            }
         
        }


        if (!hasOverlap && this.hasMoved) {
            //StartCoroutine(ReleaseWheel(newAngle));
        }

    }
    private void RescaleSelectedMenuItem() {
        foreach (RectTransform element in this.ui_elements) {

            if (GetWorldSpaceRect(this.Resizer.GetComponent<RectTransform>()).Overlaps(GetWorldSpaceRect(element))) {

                Debug.Log("SHOULD UPSCALE ELEMENT" + element.name);
                element.transform.localScale = new Vector3(this.targetScaleSize, this.targetScaleSize, this.targetScaleSize);


            } else {

                Debug.Log("SHOULD DOWNSIZE ELEMENT" + element.name);



            }


        }
    }

    private void UpdateWheelImage() {
        this.wheel.localEulerAngles = new Vector3(0f, 0f, -this.wheelAngle);


        foreach (RectTransform element in this.ui_elements) {

            if (GetWorldSpaceRect(this.Resizer.GetComponent<RectTransform>()).Overlaps(GetWorldSpaceRect(element))) {

                Debug.Log("SHOULD UPSCALE ELEMENT" + element.name);

                element.transform.localScale = Vector3.Lerp(element.transform.localScale, new Vector3(this.targetScaleSize, this.targetScaleSize, this.targetScaleSize), Time.deltaTime * 10);


            } else {

                Debug.Log("SHOULD DOWNSIZE ELEMENT" + element.name);

                element.transform.localScale = Vector3.Lerp(element.transform.localScale, new Vector3(this.standardScaleSize, this.standardScaleSize, this.standardScaleSize), Time.deltaTime * 10);


            }


        }
    }




    #endregion
}










