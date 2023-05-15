using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.EventSystems;

public class rotateRadialWheel_snapback : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {


    [Header("Size of ui overlap")]
    [SerializeField]
    public float overlap_size = 50f;

    public static float steeringInput;

    private List<RectTransform> ui_elements = new List<RectTransform>();
    private List<float> availableAngles = new List<float>();

    private List<MenuItem> menuItems = new List<MenuItem>();

    private List<Transform> allChildren;
    private RectTransform wheel;

    private RectTransform NewDragableObject;

    public RectTransform selectionField;

    public GameObject Resizer;

    public float targetScaleSize = 2;

    public float standardScaleSize;

    private bool hasMoved = false;

    private MenuItem currentMenuItem;

    private Transform DragableItemContainer;

    private bool IsRotatingTowardsOriginalPosition;



    public class MenuItem {
        private float angle;
        private RectTransform item;
        public float Angle {
            get {
                //Some other code
                return this.angle;
            }
            set {
                //Some other code
                this.angle = value;
            }
        }
        public RectTransform Item {
            get {
                return this.item;
            }
            set {
                this.item = value;
            }
        }
    }


    private void Start() {
        this.wheel = GetComponent<RectTransform>();
        this.allChildren = new List<Transform>(this.transform.GetComponentsInChildren<Transform>());
        this.DragableItemContainer = GameObject.FindGameObjectWithTag("DragContainer").transform;


        ///Tracks all UI elements in radial menu to later compare their intersection with SelectionField
        foreach (Transform child in this.allChildren) {
            if (child.gameObject && child.gameObject.CompareTag("Radial_UI_Element")) {
                RectTransform ElementContainer = child.gameObject.transform.GetChild(0).GetComponent<RectTransform>();
                this.ui_elements.Add(ElementContainer);
                float ElemenetZangle = child.gameObject.GetComponent<RectTransform>().eulerAngles.z;

                MenuItem menuItem = new MenuItem();
                menuItem.Angle = ElemenetZangle;
                menuItem.Item = ElementContainer;

                this.menuItems.Add(menuItem);

                if (ElementContainer.GetChild(0).GetComponent<DragDrop>()) {
                    ElementContainer.GetChild(0).GetComponent<DragDrop>().enabled = false;
                }
                if (ElementContainer.GetChild(0).GetComponent<DragDrop_spiel4>()) {
                    ElementContainer.GetChild(0).GetComponent<DragDrop_spiel4>().enabled = false;
                }
                if (ElementContainer.GetChild(0).GetComponent<DragPlace>()) {
                    ElementContainer.GetChild(0).GetComponent<DragPlace>().enabled = false;
                }
                this.standardScaleSize = ElementContainer.transform.localScale.x;
            }
        }
       

        ////rescales Menu Element that is in Selection Position
        RescaleSelectedMenuItem();
    }

    #region DragEvents

    public void OnPointerDown(PointerEventData eventData) {
        StartCalculatingWheelRotation(eventData);
        this.hasMoved = true;
    }

    public void OnDrag(PointerEventData eventData) {
        CalculateWheelRotation(eventData);
        UpdateWheelImage();

        ///deletes current instance of drag Item and sets the element containers child to show the selection item again
        if (this.NewDragableObject != null) {
            Destroy(this.NewDragableObject.gameObject);
            this.currentMenuItem.Item.GetChild(0).gameObject.SetActive(true);
        }
    }

    ///checks position when user is done dragging
    public void OnPointerUp(PointerEventData eventData) {


        OnDrag(eventData);
        this.IsRotatingTowardsOriginalPosition = true;

        CheckNewPosition();
    }


    #endregion

    #region Helpers

    ///get world position of UI elements to compare overlap
    Rect GetWorldSpaceRect(RectTransform rt) {
        var r = rt.rect;
        r.center = rt.TransformPoint(r.center);
        r.size = new Vector2(this.overlap_size, this.overlap_size);
        return r;
    }

    ///remove menu if element has been used (aka that there is no gameobject that needs it material
    public void RemoveFromMenu() {
        if (this.currentMenuItem != null) {


            
            Destroy(this.currentMenuItem.Item.GetChild(0).gameObject);
            this.menuItems.Remove(this.currentMenuItem);
       

            //this.currentElement.transform.localScale = Vector3.Lerp(this.currentElement.transform.localScale, new Vector3(this.standardScaleSize, this.standardScaleSize, this.standardScaleSize), Time.deltaTime * 20);

            StartCoroutine(ScaleDown(this.currentMenuItem.Item.transform, this.currentMenuItem.Item.transform.localScale.x, this.standardScaleSize));
        }
    }

    public IEnumerator ScaleDown(Transform transform, float startSize, float endSize) {
        float t = 0;

        Debug.Log("starts corourtine" + startSize + "" + endSize);

        while (transform.localScale.y >= endSize && t < 1f) {

            transform.localScale = Vector3.Lerp(new Vector3(startSize, startSize, startSize), new Vector3(endSize, endSize, endSize), t);
            Debug.Log("rescaling container:_ " + transform.name + " _starts at_ " + startSize + " _should end at_ " + endSize);
            yield return null;
            t += Time.deltaTime;
        }
    }

    public IEnumerator ScaleUp(Transform transform, float startSize, float endSize) {
        float t = 0;

        Debug.Log("starts corourtine" + startSize + "" + endSize);

        while (transform.localScale.y <= endSize && t < 1f) {

            transform.localScale = Vector3.Lerp(new Vector3(startSize, startSize, startSize), new Vector3(endSize, endSize, endSize), t);
            Debug.Log("rescaling container:_ " + transform.name + " _starts at_ " + startSize + " _should end at_ " + endSize);
            yield return null;
            t += Time.deltaTime;
        }
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

        if (pointerPos.x > this.centerPoint.x) {
            this.wheelAngle += wheelNewAngle - this.wheelPrevAngle;

        } else {
            this.wheelAngle -= wheelNewAngle - this.wheelPrevAngle;
        }

        if (this.wheelAngle >= 360f || this.wheelAngle <= -360f) {
            this.wheelAngle = 0f;
        }
        // Make sure wheel angle never exceeds maximumSteeringAngle
        this.wheelAngle = Mathf.Clamp(this.wheelAngle, -360f, 360f);
        this.wheelPrevAngle = wheelNewAngle;
    }


    public MenuItem OverlappingElement() {
        foreach (MenuItem menuItem in this.menuItems) {
            ///if there is an overlap, instantiate dragable UI in new DragContainer
            if (GetWorldSpaceRect(this.selectionField).Overlaps(GetWorldSpaceRect(menuItem.Item))) {
                if (menuItem.Item.transform.childCount > 0 && menuItem.Item.GetChild(0) != null) {
                    return menuItem;
                } 
            }
        }
        return null;    
    }

    private void CheckNewPosition() {
        MenuItem menuItemInFocus = this.OverlappingElement();
        if (menuItemInFocus != null) {
           
            if (this.NewDragableObject != null) {
                Destroy(this.NewDragableObject.gameObject);
            }

            ///Adjust the wheelPosition and Instantiates new Dragable Item
            StartCoroutine(AdjustWheelPosition(menuItemInFocus));
            
        } else {


            ///Currently does nothing
            float currentAngle = this.transform.eulerAngles.z;

            MenuItem closetAvailableMenuItem = this.menuItems[0];

            for (int i = 0; i < this.menuItems.Count; ++i) {
                if (Mathf.Abs(this.menuItems[i].Angle - currentAngle) < Mathf.Abs(closetAvailableMenuItem.Angle - currentAngle)) {
                    closetAvailableMenuItem = this.menuItems[i];
                    Debug.Log("updates closest available item" + closetAvailableMenuItem.Item + " item count" + this.menuItems.Count);

                }
            }
            Debug.Log(closetAvailableMenuItem.Item.name);
            //StartCoroutine(AdjustWheelPosition(closetAvailableMenuItem));
        }
    }

    void PlaceDragablePrefab(MenuItem menuItemInFocus) { 
        Vector3 NewPosition = menuItemInFocus.Item.position;

        this.NewDragableObject = (RectTransform)Instantiate(menuItemInFocus.Item.GetChild(0), NewPosition, Quaternion.identity);
        this.NewDragableObject.SetParent(this.DragableItemContainer);

        ///untoggles elementContainers version of dragable item (if not: the drag effect would be tangent to elementContainers position
        menuItemInFocus.Item.GetChild(0).gameObject.SetActive(false);

        this.currentMenuItem = menuItemInFocus;

        this.NewDragableObject.localScale = new Vector3(this.targetScaleSize, this.targetScaleSize, this.targetScaleSize);
        this.NewDragableObject.sizeDelta = new Vector2(this.NewDragableObject.rect.width, this.NewDragableObject.rect.height);

        if (this.NewDragableObject.GetComponent<DragDrop>()) {
            this.NewDragableObject.GetComponent<DragDrop>().enabled = true;
        }
        if (this.NewDragableObject.GetComponent<DragDrop_spiel4>()) {
            this.NewDragableObject.GetComponent<DragDrop_spiel4>().enabled = true;
        }

        if (this.NewDragableObject.GetComponent<DragPlace>()) {
            this.NewDragableObject.GetComponent<DragPlace>().enabled = true;
        }

        this.NewDragableObject.gameObject.SetActive(true);

    }

    private IEnumerator AdjustWheelPosition(MenuItem GoalMenuItem) {

        float elapsedTime = 0;
        float waitTime = 0.5f; 

        while (elapsedTime < waitTime) {
            this.wheel.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, 0, GoalMenuItem.Angle * -1), (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        PlaceDragablePrefab(GoalMenuItem);
    }




    private IEnumerator ReleaseWheel(MenuItem closestAvailableMenutItem) {


        Debug.Log("wheelAngle"+ this.wheelAngle + "closestAngle" + closestAvailableMenutItem.Angle);
        while (this.wheelAngle != closestAvailableMenutItem.Angle && this.IsRotatingTowardsOriginalPosition) {
            float deltaAngle = 100f * Time.deltaTime;

            if (Mathf.Abs(deltaAngle) > Mathf.Abs(this.wheelAngle))
                this.wheelAngle = closestAvailableMenutItem.Angle;

            else if (this.wheelAngle > closestAvailableMenutItem.Angle)
                this.wheelAngle -= deltaAngle;

            else
                this.wheelAngle += deltaAngle;


            UpdateWheelImage();

            yield return null;
        }
    }

    private void RescaleSelectedMenuItem() {
        foreach (MenuItem menuItem in this.menuItems) {
            if (GetWorldSpaceRect(this.Resizer.GetComponent<RectTransform>()).Overlaps(GetWorldSpaceRect(menuItem.Item))) {
                Debug.Log("has an overlap that should resize"+ menuItem.Item.name);
                this.currentMenuItem = menuItem;
                menuItem.Item.transform.localScale = new Vector3(this.targetScaleSize, this.targetScaleSize, this.targetScaleSize);
            } 
        }
    }

    private void UpdateWheelImage() {
        this.wheel.localEulerAngles = new Vector3(0f, 0f, -this.wheelAngle);



        ////Upscales items that go into the Resizer and downsizes when they go out
        foreach (MenuItem menuItem in this.menuItems) {

            if (GetWorldSpaceRect(this.Resizer.GetComponent<RectTransform>()).Overlaps(GetWorldSpaceRect(menuItem.Item))) {

                menuItem.Item.transform.localScale = Vector3.Lerp(menuItem.Item.transform.localScale, new Vector3(this.targetScaleSize, this.targetScaleSize, this.targetScaleSize), Time.deltaTime * 20);

            } else {

                menuItem.Item.transform.localScale = Vector3.Lerp(menuItem.Item.transform.localScale, new Vector3(this.standardScaleSize, this.standardScaleSize, this.standardScaleSize), Time.deltaTime * 20);
            }

        }


    }


    
       




    #endregion
}










