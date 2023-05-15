using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class ArPinClickHandler : MonoBehaviour
{
    // Cache ARRaycastManager GameObject from XROrigin
    public ARRaycastManager _raycastManager;
    List<ARRaycastHit> Hits = new List<ARRaycastHit>();
    public TourPopUp popup;

    void Start()
    {
        
    }

    bool TryGetTouchPosition(out Vector2 touchPosition) {
        if (Input.touchCount > 0) {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    // Update is called once per frame
    void Update() {
        // Only consider single-finger touches that are beginning
        if (Input.touchCount < 1 || Input.touches[0].phase != TouchPhase.Began) {
            return;
        }

        // Raycast against planes

        if (!TryGetTouchPosition(out Vector2 touchPosition)) {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(touchPosition);


        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)) {


            ///check if that object is tagged AR Plane
            if (hit.transform.gameObject.tag == "MapMarker") {


                if (hit.transform.parent.gameObject.GetComponent<IllustrationContainerController>()) {
                    hit.transform.parent.gameObject.GetComponent<IllustrationContainerController>().openPinPopUp(popup);
                }
            
        }
        }
    }

}
