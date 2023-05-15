using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class RadialWheel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {


    [Header("radial Wheel")]
    [SerializeField]
    [Header("size of overlapping RectTransform")]
    public float overlap_size = 50f;
    public RectTransform selectionField;
    public GameObject Resizer;

    [Header("High Light State")]
    public Material HighLightMaterial;

    public float targetScaleSize = 2.5f;
    public float standardScaleSize = 1f;

    [Header("Elements in Wheel")]
    public List<RectTransform> ui_elements = new List<RectTransform>();
    public List<RadialMenuItem> menuItems = new List<RadialMenuItem>();
    private List<Transform> allChildren;
    private RectTransform wheel;
    private RectTransform NewDragableObject;
    public RadialMenuItem currentMenuItem;

    private Transform DragableItemContainer;

    public bool CanInstantiateDragableItem = false;

    public int chronologyIndex = 0;

    public ParticleSystem particleEffect;

    public string toolTextPlaceholder;

    bool fingerIsdown;

    public RectTransform particleContainer;



    /// <summary>
    /// calculation variables
    /// </summary>
    public static float steeringInput;
    private float wheelAngle = 90f;
    private float wheelPrevAngle = 0f;
    private Vector2 centerPoint;


    public Label toolName;
    public VisualElement toolNameContainer;
    public float currentAngle;
    public float lastAngle;


    public List<Transform> availableSlots;


    private void Start() {
        
        this.wheel = GetComponent<RectTransform>();
        this.allChildren = new List<Transform>(this.transform.GetChild(0).transform.GetComponentsInChildren<Transform>());
        this.DragableItemContainer = GameObject.FindGameObjectWithTag("DragContainer").transform;

        var root = GetComponent<UIDocument>().rootVisualElement;
        this.toolName = root.Q<Label>("toolname");

        this.toolName.text = toolTextPlaceholder;
        this.toolNameContainer = root.Q<VisualElement>("ToolNameContainer");

        print("start radialwheel");

        if (SceneManager.GetActiveScene().name != "Game4") {
            StartRadialMenu();
        }
       
        
    }

    public void StartRadialMenu() {

        menuItems.Clear();
        ///Tracks all UI elements in radial menu to later compare their intersection with SelectionField
        foreach (Transform child in this.allChildren) {
            if (child.gameObject && child.gameObject.CompareTag("Radial_UI_Element")) {
                RectTransform ElementContainer = child.gameObject.transform.GetChild(0).GetComponent<RectTransform>();
                this.ui_elements.Add(ElementContainer);
                float ElemenetZangle = child.gameObject.GetComponent<RectTransform>().eulerAngles.z;

                RadialMenuItem menuItem = new RadialMenuItem();
                menuItem.Angle = ElemenetZangle;
                menuItem.Item = ElementContainer;
                menuItem.Name = child.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).name;

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


        if (transform.GetComponent<RadialMenuCustomiser>()) {
            transform.GetComponent<RadialMenuCustomiser>().CostumiseRadialWheel();
        }
    }

    public void ChangeCustomMenu(List<Game4ToolContent> tools) {

        this.transform.eulerAngles = new Vector3(0,0,-90f);
        chronologyIndex = 0;

        wheelPrevAngle = 0f;
        currentAngle = 0f;
  
        lastAngle = 0f;

        wheelAngle = 90f;
        wheelPrevAngle = 0f;

        menuItems.Clear();
        Debug.Log("new tools" + tools.Count);

        for (int i = 0; i < availableSlots.Count; i++) {
            Transform child = this.availableSlots[i];
            child.gameObject.SetActive(false);
        }

    
        if (availableSlots.Count > 0 && availableSlots.Count > tools.Count) {


            float startRot = 45f;
            float startRot2 = 30f;

            for (int i = 0; i < tools.Count; i++) {

                Transform child = this.availableSlots[i];
                child.gameObject.SetActive(true);

                RectTransform ElementContainer = child.gameObject.transform.GetChild(0).GetComponent<RectTransform>();
                ElementContainer.transform.localScale = new Vector3(1.122738f, 1.122738f, 1.122738f);
                this.ui_elements.Add(ElementContainer);

             
                //if (tools.Count <= 3) {
                    
                //    ElementContainer.parent.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, startRot);
                //    startRot += 45f;
                //} else {
                //    ElementContainer.parent.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, startRot2);
                //    startRot2 += 45f;
                //}

                float ElemenetZangle = child.gameObject.GetComponent<RectTransform>().eulerAngles.z;

                RadialMenuItem menuItem = new RadialMenuItem();
                menuItem.Angle = ElemenetZangle;
                menuItem.Item = ElementContainer;
                menuItem.Name = tools[i].headline;
                menuItem.Image = tools[i].image.ConvertedSprite;

                this.menuItems.Add(menuItem);

                menuItem.Item.GetChild(0).GetComponent<Image>().sprite = menuItem.Image;



                ///change high light value
                menuItem.Item.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);

                if (ElementContainer.GetChild(0).GetComponent<DragDrop>()) {
                    ElementContainer.GetChild(0).GetComponent<DragDrop>().enabled = false;

                    if (tools[i].percentage != 0 && tools[i].percentage != null) {

                        print("has percentage for tool:"+ i + "__:" + tools[i].percentage);
                        ElementContainer.GetChild(0).GetComponent<DragDrop>().swipeQouta = tools[i].percentage;
                    }
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
        

    }

    #region DragEvents

    public void OnPointerDown(PointerEventData eventData) {
        StartCalculatingWheelRotation(eventData);
        lastAngle = this.transform.eulerAngles.z;
        fingerIsdown = true;

    }
    public void OnDrag(PointerEventData eventData) {
        ///deletes current instance of drag Item and sets the element containers child to show the selection item again
        if (this.NewDragableObject != null) {
            Destroy(this.NewDragableObject.gameObject);
            //this.currentMenuItem.Item.GetChild(0).gameObject.SetActive(true);
            foreach (RadialMenuItem menuItem in this.menuItems) {
                menuItem.Item.GetChild(0).gameObject.SetActive(true);
            }
        }
        CalculateWheelRotation(eventData);
        UpdateWheelImage();
    }

    ///checks position when user is done dragging
    public void OnPointerUp(PointerEventData eventData) {
        if (this.menuItems.Count > 0) {
            CheckNewPosition();
        }
        fingerIsdown = false;
    }


    #endregion

    #region Helpers

    ///get world position of UI elements to compare overlap
    public Rect GetWorldSpaceRect(RectTransform rt) {
        var r = rt.rect;
        r.center = rt.TransformPoint(r.center);
        r.size = new Vector2(this.overlap_size, this.overlap_size);
        return r;
    }

    ///remove menu if element has been used (aka that there is no gameobject that needs it material
    public void RemoveFromMenu() {

        if (this.currentMenuItem != null) {

            this.currentMenuItem.Item.GetChild(0).gameObject.SetActive(true);
            this.currentMenuItem.Item.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);

            //Destroy(this.currentMenuItem.Item);
            chronologyIndex++;

            fingerIsdown = false;

            this.currentMenuItem.IsUsed = true;
         
            //this.menuItems.Remove(this.currentMenuItem);

            //this.currentElement.transform.localScale = Vector3.Lerp(this.currentElement.transform.localScale, new Vector3(this.standardScaleSize, this.standardScaleSize, this.standardScaleSize), Time.deltaTime * 20);
            StartCoroutine(ScaleDown(this.currentMenuItem.Item.transform, this.currentMenuItem.Item.transform.localScale.x, this.standardScaleSize));
        }
    }

    public IEnumerator ScaleDown(Transform transform, float startSize, float endSize) {
        float t = 0;

        while (transform.localScale.y >= endSize && t < 1f) {
            transform.localScale = Vector3.Lerp(new Vector3(startSize, startSize, startSize), new Vector3(endSize, endSize, endSize), t);
            yield return null;
            t += Time.deltaTime;
        }
    }


    #endregion

    #region Calculations

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

    private IEnumerator AdjustWheelPosition(RadialMenuItem GoalMenuItem) {

        float elapsedTime = 0;
        float waitTime = 0.5f;



        while (elapsedTime < waitTime) {
            this.wheel.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, 0, GoalMenuItem.Angle * -1), (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        if (CanInstantiateDragableItem) {

            if (SceneManager.GetActiveScene().name != "Game4") {
                if (!GoalMenuItem.isUsed) {
                    PlaceDragablePrefab(GoalMenuItem);
                }
            } else if (chronologyIndex == this.menuItems.IndexOf(this.menuItems.Find(item => item.Name == GoalMenuItem.Name))){
                if (!GoalMenuItem.isUsed) {
                    PlaceDragablePrefab(GoalMenuItem);
                }
            }


        }

        //this.wheelPrevAngle = GoalMenuItem.Angle * -1f;
        this.wheelAngle = GoalMenuItem.Angle;
        

    }

    private void RescaleSelectedMenuItem() {
        foreach (RadialMenuItem menuItem in this.menuItems) {
            if (GetWorldSpaceRect(this.Resizer.GetComponent<RectTransform>()).Overlaps(GetWorldSpaceRect(menuItem.Item)) && !menuItem.isUsed) {
                this.currentMenuItem = menuItem;
                menuItem.Item.transform.localScale = Vector3.Lerp(menuItem.Item.transform.localScale, new Vector3(this.targetScaleSize, this.targetScaleSize, this.targetScaleSize), Time.deltaTime * 20);
            } else {
                menuItem.Item.transform.localScale = Vector3.Lerp(menuItem.Item.transform.localScale, new Vector3(this.standardScaleSize, this.standardScaleSize, this.standardScaleSize), Time.deltaTime * 20);
            }
        }
    }


    private void UpdateWheelImage() {
        this.wheel.localEulerAngles = new Vector3(0f, 0f, -this.wheelAngle);

    }


    void Update() {

        //if (fingerIsdown) {
        //    RescaleSelectedMenuItem();
        //}

        RescaleSelectedMenuItem();


        currentAngle = this.transform.eulerAngles.z;
        
   
    }



    public RadialMenuItem OverlappingElement() {
        foreach (RadialMenuItem menuItem in this.menuItems) {
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
        RadialMenuItem menuItemInFocus = this.OverlappingElement();
        Debug.Log("check position");

        if (menuItemInFocus != null) {

            if (this.NewDragableObject != null) {
                Destroy(this.NewDragableObject.gameObject);
            }
            Debug.Log("has amenu item in focus" + menuItemInFocus.name);

            ///Adjust the wheelPosition and Instantiates new Dragable Item
            StartCoroutine(AdjustWheelPosition(menuItemInFocus));

        } else if (currentMenuItem != null) {


            //RadialMenuItem closetAvailableMenuItem = this.menuItems[0];
            //for (int i = 0; i < this.menuItems.Count; ++i) {
            //    if (Mathf.Abs(this.menuItems[i].Angle - currentAngle) < Mathf.Abs(closetAvailableMenuItem.Angle - currentAngle)) {
            //        closetAvailableMenuItem = this.menuItems[i];

            //    }
            //}

            int nextIndex = menuItems.IndexOf(currentMenuItem);
            if (lastAngle > currentAngle) {
                nextIndex = nextIndex + 1;
            } else {
                nextIndex = nextIndex - 1;
            }
            if (nextIndex > menuItems.Count - 1) {
                nextIndex = 0;
            } 
            if (nextIndex < 0) {
                nextIndex = menuItems.Count - 1;
            }
            RadialMenuItem nextAvailableMenuItem = this.menuItems[nextIndex];

            Debug.Log("tries to get next item" + nextAvailableMenuItem.name);

            StartCoroutine(AdjustWheelPosition(nextAvailableMenuItem));
        } else {
            StartCoroutine(AdjustWheelPosition(this.menuItems[0]));

        }


    }

    void PlaceDragablePrefab(RadialMenuItem menuItemInFocus) {
        Vector3 NewPosition = menuItemInFocus.Item.position;
        print("PlaceDragablePrefab");

        toolName.text = menuItemInFocus.Name;

        this.NewDragableObject = (RectTransform)Instantiate(menuItemInFocus.Item.GetChild(0), NewPosition, Quaternion.identity);
        this.NewDragableObject.SetParent(this.DragableItemContainer);

        ///untoggles elementContainers version of dragable item (if not: the drag effect would be tangent to elementContainers position
        menuItemInFocus.Item.GetChild(0).gameObject.SetActive(false);

        //this.NewDragableObject.GetChild(0).gameObject.SetActive(true);

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

        //this.NewDragableObject.GetComponent<CanvasRenderer>().SetMaterial(this.HighLightMaterial, 1);

        this.NewDragableObject.GetComponent<UnityEngine.UI.Image>().material = this.HighLightMaterial;

        //if (SceneManager.GetActiveScene().name == "Game4") {
        //    string[] targets = this.DragableItemContainer.transform.GetChild(0).transform.gameObject.name.Split("(");
        //    toolName.text = targets[0];
        //}
    }


    public void EndRadialMenu(string message) {
        toolName.text = message;

        print("closing radial menu: " + message + toolName); 

        toolNameContainer.RemoveFromClassList("neutral");
        toolNameContainer.AddToClassList("inFokus");

        this.transform.GetComponent<UnityEngine.UI.Image>().enabled = false;

        Transform[] children = this.transform.GetComponentsInChildren<Transform>();

        foreach (Transform child in children) {
            if (child != this.transform) {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void DisactivateRadialMenu(string message) {
        toolName.text = message;
        CanInstantiateDragableItem = false;
    }

    public void ActivateRadialMenu(string message) {
        toolName.text = toolTextPlaceholder;
        CanInstantiateDragableItem = true;
    }
    #endregion
}










