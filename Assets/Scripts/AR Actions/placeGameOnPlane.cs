using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]

public class placeGameOnPlane : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject entireGame;

    [SerializeField]
    private ARPlaneManager aRPlaneManager;

    private bool hasPlacedGame = false;

    [SerializeField]
    private Camera _camera;

    private void Awake() {
        this.aRPlaneManager = GetComponent<ARPlaneManager>();
 
        this.aRPlaneManager.planesChanged += OnPlaneChanged;
    }
    private void OnPlaneChanged(ARPlanesChangedEventArgs args) {

        if (args.added != null) {

            var biggestPlane = args.added[0];
            for (var i = 1; i < args.added.Count; i++) {
                if (args.added[i].size.x * args.added[i].size.y > biggestPlane.size.x * biggestPlane.size.y) {
                    biggestPlane = args.added[i];
                }
            }
            Vector3 heading  = biggestPlane.gameObject.transform.position - this._camera.transform.position;
            float distance = Vector3.Dot(heading, this._camera.transform.forward);

            ARPlane aRPlane = biggestPlane;

            if (!this.hasPlacedGame && distance < 3) {
                GameObject placedGame = Instantiate(this.entireGame, aRPlane.transform.position, Quaternion.identity);

                this.entireGame.SetActive(false);
                placedGame.SetActive(true);
                this.hasPlacedGame = true;

            }

      


            //foreach (var plane in this.aRPlaneManager.trackables) {
              //  plane.gameObject.SetActive(false);
            //}
       
        
        }




    }

}
