using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Linq;




[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]

public class PlaneDetectionPlacement: MonoBehaviour
{

    // GAME OBJECTS USED IN TO PLACE
    public GameObject[] gameObjectsToInstance;
    private ARRaycastManager _aRRaycastManager;
    private ARPlaneManager _arPlaneManager;

    private ARAnchorManager _arAnchorManager;
    private List<ARAnchor> anchors = new List<ARAnchor>();


    public bool ShouldShowPlaneDetection = true;

    //touch position for and Razcast hits for testing with touch placement
    private Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    //local gameObject Variables
    private GameObject spawnedObject;
    private ARPlane appropiatePlane;


    //local variables
    private bool hasPlacedObject = false;

    //should get CMS connection
    [SerializeField] public Vector2 dimensionsForBigPlane;
    [SerializeField] public float scale;
    [SerializeField] public float yOffSet;


    public Material[] selectionIndicatorMaterial;

    private void Awake() {

        Application.targetFrameRate = 60; ///set Target Frame Rate for optimal use for ARKit
        this._aRRaycastManager = GetComponent<ARRaycastManager>();
        this._arPlaneManager = GetComponent<ARPlaneManager>();
        this._arAnchorManager = GetComponent<ARAnchorManager>();

        this._arPlaneManager.planesChanged += OnPlanesChanged;

    }


    public void BeginWithStrapiData() {
        OnTogglePlanes(this.ShouldShowPlaneDetection); 
    }
    private void OnDisable() {
        this._arPlaneManager.planesChanged -= OnPlanesChanged;

    }




    private void OnPlanesChanged(ARPlanesChangedEventArgs args) {

        if (args.added != null && args.added.Count > 0 && !this.hasPlacedObject) { //
           

            foreach (ARPlane plane in args.added.Where(plane => plane.extents.x * plane.extents.y >= this.dimensionsForBigPlane.x * this.dimensionsForBigPlane.y)) { //checks if ARPlane is bigger than 0.1F
        
                //plane.gameObject.GetComponent<MeshRenderer>().material = this.selectionIndicatorMaterial[2]; ///colors big planes green

                if (!this.hasPlacedObject) {
                    this.appropiatePlane = plane;
                    placeObject(this.appropiatePlane);
                }
          
           
           }

        }
    }

    public void placeObject(ARPlane plane) {
        this.hasPlacedObject = true;
       
        plane.gameObject.GetComponent<MeshRenderer>().material = this.selectionIndicatorMaterial[1]; ///colors chosen plane Red

        Pose center = new Pose(plane.transform.position, Quaternion.identity);


      

        // Vector3 position = new Vector3(plane.transform.position.x, plane.transform.position.y + this.yOffSet, plane.transform.position.z);
        Vector3 position = new Vector3(0, this.yOffSet, 0);



        foreach (GameObject GameObjectToInstance in this.gameObjectsToInstance) {

            //ARAnchor aRAnchor = plane.gameObject.AddComponent<ARAnchor>();
            ////ARAnchor aRAnchor = this._arAnchorManager.AttachAnchor(plane, center);
            //GameObjectToInstance.transform.SetParent(aRAnchor.transform);
            //ARAnchor anchor = GameObjectToInstance.AddComponent<ARAnchor>();

            Pose pose = new Pose(plane.transform.position, Quaternion.identity);

            var oldPrefab = this._arAnchorManager.anchorPrefab;
            this._arAnchorManager.anchorPrefab = GameObjectToInstance;
            ARAnchor anchor = this._arAnchorManager.AttachAnchor(plane, pose);
            this._arAnchorManager.anchorPrefab = oldPrefab;

            //GameObjectToInstance.transform.position = position;

            if (GameObjectToInstance.name == "teil1" || GameObjectToInstance.gameObject.name == "teil1") {
                GameObjectToInstance.gameObject.SetActive(true);
                GameObjectToInstance.SetActive(true);
            }

            GameObjectToInstance.transform.localScale = new Vector3(this.scale, this.scale, this.scale);

        }

        //remove plane detection handler when game has been placed
        this._arPlaneManager.planesChanged -= OnPlanesChanged;
        this._arPlaneManager.enabled = !this._arPlaneManager.enabled;


        OnTogglePlanes(false);

    }




    public void OnTogglePlanes(bool flag) {
        foreach (GameObject plane in GameObject.FindGameObjectsWithTag("AR_Plane")) {
            Renderer r = plane.GetComponent<Renderer>();
            ARPlaneMeshVisualizer t = plane.GetComponent<ARPlaneMeshVisualizer>();
            r.enabled = flag;
            t.enabled = flag;
        }
    }

    //bool TryGetTouchPosition(out Vector2 touchPosition) {
    //    if (Input.touchCount > 0) {
    //        touchPosition = Input.GetTouch(0).position;
    //        return true;
    //    }
    //    touchPosition = default;
    //    return false;
    //}


    void Update()
    {
        //if (!TryGetTouchPosition(out Vector2 touchPosition)) {
        //    return;
        //}
        //if (this._aRRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon)) {
        //    var hitPose = hits[0].pose;
        //    var plane = this._arPlaneManager.GetPlane(hits[0].trackableId);
        //    plane.gameObject.GetComponent<MeshRenderer>().material = this.selectionIndicatorMaterial[0];

        //    if (this.spawnedObject == null) {
        //        this.spawnedObject = Instantiate(this.testObjectToInstance, hitPose.position, hitPose.rotation);
        //        this.spawnedObject.SetActive(true);
        //    } else {
        //        this.spawnedObject.transform.position = hitPose.position;
        //    }
        //}
    }




}
