using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
public class PlaneAreaBehaviour : MonoBehaviour
{

    public TextMeshProUGUI areaText;
    public ARPlane arPlane;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void ArPlane_BoundaryChanged(ARPlaneBoundaryChangedEventArgs obj) {
        this.areaText.text = CalculatePlaneArea(this.arPlane).ToString();
        this.areaText.text += "@" + this.arPlane.center.ToString();
    }
    private float CalculatePlaneArea(ARPlane plane) {
        return plane.size.x * plane.size.y;
    }
    private void ShowPosition() {
    
    }
    public void ToggleAreaView() {
        if (this.areaText.enabled) {
            this.areaText.enabled = false;

        } else {
            this.areaText.enabled = true;

        }
    }

    private void Update() {
        this.areaText.transform.rotation =
           Quaternion.LookRotation(this.areaText.transform.position -
              Camera.main.transform.position);
    }


}
