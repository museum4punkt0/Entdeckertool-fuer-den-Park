using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TurnCamera : MonoBehaviour
{
    [SerializeField] private ARCameraManager arCameraManager;
    [SerializeField] private ARFaceManager arFaceManager;
    [SerializeField] private GameObject arSessionOriginGameObject;
    [SerializeField] private GameObject arSessionGameObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Turn() {

        if (arCameraManager.currentFacingDirection != CameraFacingDirection.World) {
            arCameraManager.requestedFacingDirection = CameraFacingDirection.World;
        } else {
            arCameraManager.requestedFacingDirection = CameraFacingDirection.User;
        }
    }
}
