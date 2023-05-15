using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.ARFoundation;
using System;


[RequireComponent(typeof(ARPlaneManager))]
public class PlaneDetection : MonoBehaviour
{

    [SerializeField] private Vector2 dimensionsForBigPlane;
    [SerializeField] private float scale;
    public ARPlaneManager arPlaneManager;
    private List<ARPlane> arPlanes;
    public GameObject ObjectToPlaceOnBigPlane;
    public GameObject checkIfPlaneChangeEventHappens;

    private bool hasPlacedObject = false;

    GameObject placedBigObject;
    GameObject placedObject;


    void OnEnable()
    {
        this.arPlanes = new List<ARPlane>();
        Debug.Log("hej this begins");
        this.hasPlacedObject = false;

        this.arPlaneManager.planesChanged += OnPlanesChanged;
    }
    void OnDisable() 
    {
        Debug.Log("hej this ends");
        this.arPlaneManager.planesChanged -= OnPlanesChanged;
    }
    private void OnPlanesChanged(ARPlanesChangedEventArgs args) {
        this.checkIfPlaneChangeEventHappens.gameObject.SetActive(true);

        if (args.added != null && args.added.Count > 0 && this.placedBigObject == null) { //
            this.arPlanes.AddRange(args.added);
            if (!this.hasPlacedObject) {
                PlaceObjects(args);


                ////testing to see if it helps to place it directly on the FIRST REGISTERED PLANE
                ARPlane arPlane = args.added[0];
                this.placedObject = Instantiate(this.ObjectToPlaceOnBigPlane, arPlane.transform.position, Quaternion.identity);

            }
        }


 
    }

    void PlaceObjects(ARPlanesChangedEventArgs args) {
        var biggestPlane = args.added[0];

        foreach (ARPlane plane in this.arPlanes.Where(plane => plane.extents.x * plane.extents.y >= 0.1f)) { //checks if ARPlane is bigger than 0.1F
            if (plane.extents.x * plane.extents.y >= this.dimensionsForBigPlane.x * this.dimensionsForBigPlane.y) { //checks if ARPlane is bigger than required area

                if (plane.extents.x * plane.extents.y > biggestPlane.extents.x * biggestPlane.extents.y) { //sorts through them all to find the biggest
                    biggestPlane = plane;
                }
            }
            
            
            ////testing to see if I can place ON ALL PLANES IN ARGS

            this.placedObject = Instantiate(this.ObjectToPlaceOnBigPlane, plane.transform.position, Quaternion.identity);
            this.placedObject.transform.LookAt(Camera.main.transform);
        }

        ////PLACING ON BIGGEST PLANE
        this.placedBigObject = Instantiate(this.ObjectToPlaceOnBigPlane, biggestPlane.transform.position, Quaternion.identity);
        this.placedBigObject.transform.LookAt(Camera.main.transform);
        this.placedBigObject.transform.localScale = new Vector3(this.scale, this.scale, this.scale);

        this.hasPlacedObject = true;
    }

}
