using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleGameCameraInEditor : MonoBehaviour
{
    // Start is called before the first frame updat

    private Camera _camera;
    private bool testingOnLapTop = true;

    public Camera GameCamera;
    public GameObject ARSessionOrigin;
    public GameObject GamePrefabToBePLacedInARSession;

    public CrossGameManager crossGameManager;


    private void Awake() {

        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            this.testingOnLapTop = false;
            this.GameCamera.gameObject.SetActive(false);
            this.ARSessionOrigin.SetActive(true);


            if (this.GamePrefabToBePLacedInARSession != null) {
                this.GamePrefabToBePLacedInARSession.SetActive(false);
            }


        } else {
            this.testingOnLapTop = true;
            this.GameCamera.gameObject.SetActive(true);
            this.ARSessionOrigin.SetActive(false);


            if (this.GamePrefabToBePLacedInARSession != null) {
                this.GamePrefabToBePLacedInARSession.SetActive(true);
            }


        }

        if (!Camera.main) {
            this._camera = Camera.current;
        } else {
            this._camera = Camera.main;
        }
    }
}
