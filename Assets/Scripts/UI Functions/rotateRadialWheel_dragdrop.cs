using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class rotateRadialWheel_dragdrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {


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

    private RectTransform currentElement;

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
                float elemenetZangle = child.gameObject.GetComponent<RectTransform>().eulerAngles.z;

                MenuItem menuItem = new MenuItem();
                menuItem.Angle = elemenetZangle;
                menuItem.Item = ElementContainer;

                this.menuItems.Add(menuItem);

                this.availableAngles.Add(elemenetZangle * -1f);

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
        

       //this.transform.eulerAngles = new Vector3(this.availableAngles[3].x, this.availableAngles[3].y, this.transform.eulerAngles.y + this.availableAngles[3].z);

        Debug.Log("WHEEL ROTATION" + this.transform.eulerAngles + "from:_ " + this.ui_elements[3].name);

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
            this.currentElement.GetChild(0).gameObject.SetActive(true);
        }
    }

    ///checks position when user is done dragging
    public void OnPointerUp(PointerEventData eventData) {
        OnDrag(eventData);
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
        if (this.currentElement != null) {

            //int index = this.ui_elements.IndexOf(this.currentElement);

            
            Destroy(this.currentElement.GetChild(0).gameObject);
            this.ui_elements.Remove(this.currentElement);
       

            //this.currentElement.transform.localScale = Vector3.Lerp(this.currentElement.transform.localScale, new Vector3(this.standardScaleSize, this.standardScaleSize, this.standardScaleSize), Time.deltaTime * 20);

            StartCoroutine(ScaleDown(this.currentElement.transform, this.currentElement.transform.localScale.x, this.standardScaleSize));
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


    public RectTransform OverlappingElement() {
        foreach (RectTransform element in this.ui_elements) {
            ///if there is an overlap, instantiate dragable UI in new DragContainer
            if (GetWorldSpaceRect(this.selectionField).Overlaps(GetWorldSpaceRect(element))) {
                if (element.transform.childCount > 0 && element.GetChild(0) != null) {
                    return element;
                } 
            }
        }
        return null;    
    }

    private void CheckNewPosition() {
        RectTransform element = this.OverlappingElement();

        if (element != null) {
            ///old placement was relative to Element ei. 
            Vector3 NewPosition = element.position;
            ///instantiates UI element
            this.NewDragableObject = (RectTransform)Instantiate(element.GetChild(0), NewPosition, Quaternion.identity);
            this.NewDragableObject.SetParent(this.DragableItemContainer);

            ///untoggles elementContainers version of dragable item (if not: the drag effect would be tangent to elementContainers position
            element.GetChild(0).gameObject.SetActive(false);

            this.currentElement = element;

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


        } else {

            this.IsRotatingTowardsOriginalPosition = true;
            Debug.Log("should move to better position");
            //StartCoroutine(ReleaseWheel());
        }
    }


    private IEnumerator ReleaseWheel() {
        while (this.wheelAngle != 0f && this.IsRotatingTowardsOriginalPosition) {
            float deltaAngle = 100f * Time.deltaTime;

            if (Mathf.Abs(deltaAngle) > Mathf.Abs(this.wheelAngle))
                this.wheelAngle = 0f;

            else if (this.wheelAngle > 0f)
                this.wheelAngle -= deltaAngle;

            else
                this.wheelAngle += deltaAngle;


            UpdateWheelImage();

       
            yield return null;
        }
    }

    private void RescaleSelectedMenuItem() {
        foreach (RectTransform element in this.ui_elements) {

            if (GetWorldSpaceRect(this.Resizer.GetComponent<RectTransform>()).Overlaps(GetWorldSpaceRect(element))) {
                Debug.Log("rescales selcted Item");
                element.transform.localScale = new Vector3(this.targetScaleSize, this.targetScaleSize, this.targetScaleSize);
            } 
        }
    }


    private IEnumerator ScalingMenuItemUp(RectTransform element) {

        while (element.transform.localScale.y <= this.targetScaleSize) {
            element.transform.localScale = Vector3.Lerp(element.transform.localScale, new Vector3(this.targetScaleSize, this.targetScaleSize, this.targetScaleSize), Time.deltaTime * 20);

            yield return null;
        }
     
    }

    private IEnumerator ScalingMenuItemDown(RectTransform element) {

        while (element.transform.localScale.y >= this.standardScaleSize) {
            element.transform.localScale = Vector3.Lerp(element.transform.localScale, new Vector3(this.standardScaleSize, this.standardScaleSize, this.standardScaleSize), Time.deltaTime * 20);

            yield return null;
        }

    }

    private void handleScaling(RectTransform element) {
        List<RectTransform> other_ui_elements = new List<RectTransform>(this.ui_elements);
        
        other_ui_elements.Remove(element);

        StartCoroutine(ScalingMenuItemUp(element));

        foreach (RectTransform other_element in this.ui_elements) {

            //other_element.transform.localScale = Vector3.Lerp(other_element.transform.localScale, new Vector3(this.standardScaleSize, this.standardScaleSize, this.standardScaleSize), Time.deltaTime * 20);
            other_element.transform.localScale = new Vector3(this.standardScaleSize, this.standardScaleSize, this.standardScaleSize);
            Debug.Log(other_element.name);

        }

    }


    void Update() {
        //foreach (RectTransform element in this.ui_elements) {
        //    if (element == this.currentElement) {
        //        Debug.Log("there is one current element" + element.name + "" + this.currentElement.name);
        //        if (element.localScale.x < this.targetScaleSize) {
        //            element.transform.localScale = Vector3.Lerp(element.transform.localScale, new Vector3(this.targetScaleSize, this.targetScaleSize, this.targetScaleSize), Time.deltaTime * 20);
        //        }
        //    } else {

        //        if (element.localScale.x >= this.standardScaleSize) {
        //            element.transform.localScale = Vector3.Lerp(element.transform.localScale, new Vector3(this.standardScaleSize, this.standardScaleSize, this.standardScaleSize), Time.deltaTime * 20);
        //        }
        //        element.localScale = new Vector3(1, 1, 1);
        //    }
        //}
    }

    private void UpdateWheelImage() {
        this.wheel.localEulerAngles = new Vector3(0f, 0f, -this.wheelAngle);

        foreach (RectTransform element in this.ui_elements) {
            
            
            if (GetWorldSpaceRect(this.Resizer.GetComponent<RectTransform>()).Overlaps(GetWorldSpaceRect(element))) {

                element.transform.localScale = Vector3.Lerp(element.transform.localScale, new Vector3(this.targetScaleSize, this.targetScaleSize, this.targetScaleSize), Time.deltaTime * 20);

            } else {

                element.transform.localScale = Vector3.Lerp(element.transform.localScale, new Vector3(this.standardScaleSize, this.standardScaleSize, this.standardScaleSize), Time.deltaTime * 20);
            }








        }


    }


    
       




    #endregion
}










