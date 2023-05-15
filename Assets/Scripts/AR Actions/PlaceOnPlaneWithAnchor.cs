using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using System.Collections;
using System.Linq;
using UnityEngine.Animations;
using Unity.VisualScripting;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]



public class PlaceOnPlaneWithAnchor : MonoBehaviour {

    [SerializeField]
    private GameObject[] _prefabsToPlace;

    // Cache ARRaycastManager GameObject from XROrigin
    private ARRaycastManager _raycastManager;

    // Cache ARAnchorManager GameObject from XROrigin
    private ARAnchorManager _anchorManager;

    // Cache ARAnchorManager GameObject from XROrigin
    private ARPlaneManager _planeManager;
    [SerializeField]

    List<ARPlane> planes;
    ARPlane lowestPlane;

    public Camera _arCamera;

    // List for raycast hits is re-used by raycast manager
    private static readonly List<ARRaycastHit> Hits = new();

    public Material[] selectionIndicatorMaterial;

    //local variables
    private bool hasPlacedObject = false;
    private bool shouldPlaceObject = true;

    //should get CMS connection
    [SerializeField] public Vector2 dimensionsForBigPlane;
    [SerializeField] public float scale;
    [SerializeField] public float yOffSet;

    public int PlaneAmountBeforePlacement = 3;

    public bool ShouldShowPlaneDetection = true;
    public CrossGameManager crossGameManager;

    void Awake() {
        this._raycastManager = GetComponent<ARRaycastManager>();
        this._anchorManager = GetComponent<ARAnchorManager>();
        this._planeManager = GetComponent<ARPlaneManager>();


        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        if (Application.platform == RuntimePlatform.Android) {
            if (_arCamera.GetComponent<AROcclusionManager>()) {
                _arCamera.GetComponent<AROcclusionManager>().enabled = false;
            }
        } else {
            if (_arCamera.GetComponent<AROcclusionManager>()) {
                _arCamera.GetComponent<AROcclusionManager>().enabled = true;
            }
        }

        this._planeManager.planesChanged += OnPlanesChanged;

        crossGameManager.ErrorLog("wakes up AR");

    }
    private void OnDisable() {
        this._planeManager.planesChanged -= OnPlanesChanged;

    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args) {


        if (args.added != null && args.added.Count > 0 && !this.hasPlacedObject) {

            foreach (ARPlane plane in args.added.Where(plane => plane.extents.x * plane.extents.y >= this.dimensionsForBigPlane.x * this.dimensionsForBigPlane.y)) { //checks if ARPlane is bigger than 0.1F

                planes.Add(plane);
                
                if (lowestPlane == null) {
                    lowestPlane = plane;
                }
           
                foreach (ARPlane a_plane in planes) {
                    if (lowestPlane.extents.x * lowestPlane.extents.y > plane.extents.x * plane.extents.y) {
                        lowestPlane = plane;
                    }
                }

                crossGameManager.ErrorLog("planes: " + planes.Count);

            }

        }
    }

    void Update() {


        const TrackableType trackableTypes = TrackableType.PlaneWithinPolygon;

        ////checks it hasnt already been placed
        if (!this.hasPlacedObject && this.shouldPlaceObject && planes.Count >= PlaneAmountBeforePlacement && lowestPlane != null) {

            ////ensures it only places one object
            this.hasPlacedObject = true;
            this.shouldPlaceObject = false;

            Pose pose = new Pose(lowestPlane.centerInPlaneSpace, new Quaternion(0,0,0,0));
      
            crossGameManager.ErrorLog("has a pose:" + pose);

            int index = 0;
            foreach (GameObject prefab in this._prefabsToPlace) {
                index++;
                PlaceGameAndAnchorIt(prefab, lowestPlane, pose);
            }

        }

        // Only consider single-finger touches that are beginning
        if (Input.touchCount < 1 || Input.touches[0].phase != TouchPhase.Began) {
            return;
        }

        // Raycast against planes

        if (!TryGetTouchPosition(out Vector2 touchPosition)) {
            return;
        }

        if (!this.hasPlacedObject && this.shouldPlaceObject && this._raycastManager.Raycast(touchPosition, Hits, trackableTypes)) {

            // in place in the real world.
            if (Hits[0].trackable is ARPlane hitPlane) {

                this.hasPlacedObject = true;
                this.shouldPlaceObject = false;

  
                PlaceGameAndAnchorIt(this._prefabsToPlace[0], hitPlane, Hits[0].pose);
            }

        }

    }

    public void PlaceGameAndAnchorIt(GameObject _prefab, ARPlane plane, Pose pose) {
        GameObject prefab = new GameObject();
        if (_prefab.activeInHierarchy) {
            prefab = _prefab;
        } else {
            prefab = Instantiate(_prefab);
        }
        prefab.transform.position = new Vector3(plane.center.x, plane.center.y + this.yOffSet, plane.center.z);
        
        prefab.SetActive(true);
        prefab.transform.localScale = new Vector3(scale, scale, scale);
        prefab.AddComponent<ARAnchor>();

        ARAnchor anchor = this._anchorManager.AttachAnchor(plane, pose);

        Vector3 targetPostition = new Vector3(_arCamera.transform.position.x,
        anchor.transform.position.y,
         _arCamera.transform.position.z);

        //anchor.transform.LookAt(targetPostition);
        prefab.transform.LookAt(targetPostition);

        crossGameManager.ErrorLog("placegame4anchor" + targetPostition.ToString());

        TogglePlaneVisualisation(false);
        this._planeManager.planesChanged -= OnPlanesChanged;

    }

    public void PlaceOnAnchor(GameObject prefab, ARPlane plane, Pose pose) {

        crossGameManager.ErrorLog("placeOnAnchor" + prefab.activeSelf);

        var oldPrefab = this._anchorManager.anchorPrefab;
        
        prefab.transform.position = new Vector3(plane.centerInPlaneSpace.x, 0 + this.yOffSet, plane.centerInPlaneSpace.y);
 
        this._anchorManager.anchorPrefab = prefab;
        ////attaches anchor to plane
        ARAnchor anchor = this._anchorManager.AttachAnchor(plane, pose);
        

        if (!this._anchorManager.anchorPrefab.gameObject.activeSelf) {
            this._anchorManager.anchorPrefab.gameObject.SetActive(true);
            crossGameManager.ErrorLog("sets anchor to be true");
        }

        prefab.SetActive(true);

        crossGameManager.ErrorLog("anchorPrefab" + prefab.activeSelf);

        anchor.gameObject.transform.localPosition = new Vector3(this._anchorManager.anchorPrefab.gameObject.transform.position.x, this._anchorManager.anchorPrefab.gameObject.transform.position.y + this.yOffSet, this._anchorManager.anchorPrefab.gameObject.transform.position.z);

        crossGameManager.ErrorLog("sets position: " + this._anchorManager.anchorPrefab.gameObject.transform.position);

        anchor.gameObject.transform.localScale = new Vector3(scale, scale, scale);
        
        crossGameManager.ErrorLog("sets scale");


        Vector3 targetPostition = new Vector3(_arCamera.transform.position.x,
        this._anchorManager.anchorPrefab.gameObject.transform.position.y,
        _arCamera.transform.position.z);
        anchor.gameObject.transform.LookAt(targetPostition);

        crossGameManager.ErrorLog("no direction");



    }


    ARAnchor CreateAnchor(in ARRaycastHit hit, GameObject prefab, float scale) {
        ARAnchor anchor;
 

        crossGameManager.ErrorLog("createAnchor");

        // Note: the anchor can be anywhere in the scene hierarchy
        var instantiatedObject = Instantiate(prefab, hit.pose.position, new Quaternion(0,0,0,0));
        instantiatedObject.SetActive(true);
        instantiatedObject.transform.localScale = new Vector3(scale, scale, scale);
        Vector3 targetPostition = new Vector3(_arCamera.transform.position.x,
            instantiatedObject.transform.position.y,
            _arCamera.transform.position.z);
        instantiatedObject.transform.LookAt(targetPostition);

        // Make sure the new GameObject has an ARAnchor component
        anchor = instantiatedObject.GetComponent<ARAnchor>();

        if (anchor == null) {
            anchor = instantiatedObject.AddComponent<ARAnchor>();
        }

        crossGameManager.ErrorLog($"Created regular anchor (id: {anchor.nativePtr})." + instantiatedObject.transform.localScale);


        return anchor;
    }



    public void TogglePlaneVisualisation(bool flag) {
        if (flag) {
            SetAllPlanesActive(true);
        } else {
            SetAllPlanesActive(false);
        }
    }

    public void SetAllPlanesActive(bool flag) {


        crossGameManager.ErrorLog("AR planes active: " + flag);
        foreach (ARPlane plane in this._planeManager.trackables) {
            if (plane) {
                Renderer r = plane.GetComponent<Renderer>();
                ARPlaneMeshVisualizer t = plane.GetComponent<ARPlaneMeshVisualizer>();
                MeshCollider c = plane.GetComponent<MeshCollider>();
                if (r) {
                    r.enabled = flag;

                }
                if (t) {
                    t.enabled = flag;

                }
                if (c) {
                    c.enabled = flag;
                }

                plane.gameObject.SetActive(flag);
            }

        }
        this._planeManager.enabled = flag;
    }

    bool TryGetTouchPosition(out Vector2 touchPosition) {
        if (Input.touchCount > 0) {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    public bool PlacePrefabOnOnGameobject(GameObject Object, float scale, Vector3 pos) {

        GameObject newObject = Instantiate(Object, pos, new Quaternion(0,0,0,0));

        newObject.transform.localScale = new Vector3(scale, scale, scale);

        Vector3 targetPostition = new Vector3(_arCamera.transform.position.x,
         newObject.transform.position.y,
        _arCamera.transform.position.z);
        newObject.transform.LookAt(targetPostition);

        return true;
    }
    public bool PlacePrefabOnPlane(GameObject Object, float scale, Vector3 pos) {

        if (!TryGetTouchPosition(out Vector2 touchPosition)) {
            return false;
        }

        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit Rhit;

        const TrackableType trackableTypes = TrackableType.PlaneWithinPolygon;

        if (this._raycastManager.Raycast(touchPosition, Hits, trackableTypes)) {

            // in place in the real world.
            if (Hits[0].trackable is ARPlane hitPlane) {

                crossGameManager.ErrorLog("RC to ARP: " + Hits[0].trackable.gameObject.name);

                CreateAnchor(Hits[0], Object, scale);

                return true;

            } else {

                crossGameManager.ErrorLog("does not hit an ARPLane first");

                return false;

            }
                

        } else {

            crossGameManager.ErrorLog("fails all checks");

            return false;



        }
        

    }

    public void BeginWithStrapiData() {

        if (crossGameManager != null) {
            crossGameManager.ErrorLog("has strapi");
        }

        if (!this.ShouldShowPlaneDetection) {
            TogglePlaneVisualisation(false);
        }

    }





}