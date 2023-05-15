using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPlace : MonoBehaviour, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;

    public Material MaterialToApplyToMesh;

    private Vector2 position;
    public GameObject GameController;
    public GameObject RadialMenu;

    public float scale;

    //private GameObject highlightedMesh;
    //public Material hightLightMaterial;
    //public Material notHighLightMaterial;

    public GameObject ARController;

    public GameObject ObjectToPlace;

    public GameObject error_logging;

    private void Awake() {

        this.ARController = GameObject.FindGameObjectWithTag("ARSessionOrigin");

        this.rectTransform = GetComponent<RectTransform>();

        this.error_logging = GameObject.FindGameObjectWithTag("errorlogging");

        this.position = this.rectTransform.transform.position;
    }
    public void OnDrag(PointerEventData eventData) {
        this.rectTransform.anchoredPosition += eventData.delta /this.canvas.scaleFactor;

        HandleDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData) {

        if (eventData.position.y > Screen.height / 4) {
            HandleDrop(eventData.position);
        } else {
            this.rectTransform.position = this.position;
        }
         
    }


    void HandleDrop(Vector3 pos) {
        Ray ray = Camera.main.ScreenPointToRay(pos);

        
        RaycastHit hit;


        ////placing object on AR Plane
        if (this.ARController && this.ARController.GetComponent<PlaceOnPlaneWithAnchor>() ) {

            bool placingObject = this.ARController.GetComponent<PlaceOnPlaneWithAnchor>().PlacePrefabOnPlane(this.ObjectToPlace, scale, pos);

            ErrorLog("placing object" + placingObject);



            ///either place on AR Plane, or
            if (placingObject == true) {

                this.Destroy();

                Destroy(this.gameObject);

                this.RadialMenu.GetComponent<RadialWheel>().RemoveFromMenu();

                if (this.GameController.GetComponent<startGame3_AR_Ready>()) {
                    this.GameController.GetComponent<startGame3_AR_Ready>().scores++;
                }



            // place on collider object if user hits something
            } else if (Physics.Raycast(ray, out hit, 100)) {


                ///check if that object is tagged AR Plane
                if (hit.transform.gameObject.tag == "AR_Plane") {

                    bool placingObjectLoseInSpace = this.ARController.GetComponent<PlaceOnPlaneWithAnchor>().PlacePrefabOnOnGameobject(this.ObjectToPlace, scale, hit.point);


                    this.Destroy();

                    Destroy(this.gameObject);

                    this.RadialMenu.GetComponent<RadialWheel>().RemoveFromMenu();

                    if (this.GameController.GetComponent<startGame3_AR_Ready>()) {
                        this.GameController.GetComponent<startGame3_AR_Ready>().scores++;
                    }
                
                //else return to menu
                } else {

                    this.rectTransform.position = this.position;
                }


            //treturn to emnu if user doesnt hit anything
            } else {
                this.rectTransform.position = this.position;

            } 

        

        //else simply destroy (for desktoip testing)
        } else {
            this.Destroy();

            Destroy(this.gameObject);


            if (this.RadialMenu.GetComponent<RadialWheel>()) {
                this.RadialMenu.GetComponent<RadialWheel>().RemoveFromMenu();

            }

            if (this.GameController.GetComponent<startGame3_AR_Ready>()) {
                this.GameController.GetComponent<startGame3_AR_Ready>().scores++;
            }
        }


    }


    void HandleDrag(Vector3 pos) {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)) {

            ///if dragable UI rect transform hits specified tag (either hardcoded or because it is tagged with the same as the object name
            if (hit.transform.gameObject.tag == "AR_Plane") {
                //ErrorLog("hovers over " + hit.transform.gameObject.name);
            }
        }
    }





    // Update is called once per frame
    void ChangeMaterialsOnMeshWithTag(string tag)
    {
        GameObject[] meshes = GameObject.FindGameObjectsWithTag(tag);
        foreach (var mesh in meshes) {
            if (mesh.GetComponent<Renderer>()) {
                mesh.GetComponent<Renderer>().material = this.MaterialToApplyToMesh;

            }
        }
     
    }


    void HighLightOnHover(GameObject highlightedMesh, bool shouldHighLight) {

        //if (shouldHighLight) {
        //    Debug.Log("Tries to Highlight" + highlightedMesh.name);
        //    highlightedMesh.GetComponent<Renderer>().material = this.hightLightMaterial;
        //} 
        //else if(this.highlightedMesh != null) {
        //    Debug.Log("Tries to deselect" + highlightedMesh.name);
        //    //Debug.Log("Tries to remove highlight" + highlightedMesh.GetComponent<Renderer>().sharedMaterial);
        //    highlightedMesh.GetComponent<Renderer>().material = this.notHighLightMaterial;
        //    this.highlightedMesh = null;
        //}
  
    }


    public void ErrorLog(string message) {
        if (GameObject.FindGameObjectWithTag("errorlogging")) {
            GameObject.FindGameObjectWithTag("errorlogging").GetComponent<UnityEngine.UI.Text>().text += message;
        }

    }

}
