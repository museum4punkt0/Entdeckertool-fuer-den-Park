using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class ARFilterController : MonoBehaviour {
    CrossGameManager crossGameManager;
    [SerializeField] private ARCameraManager arCameraManager;

    public ARFaceManager arFaceManager;
    public GameObject maskPrefab;
    public GameObject helmetPrefab;
    public GameObject ImageTexture;
    public GameObject popup;

    private void Awake() {
        if (arCameraManager.currentFacingDirection == CameraFacingDirection.World) {
            arCameraManager.requestedFacingDirection = CameraFacingDirection.User;
        }
    }

    void Start() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        //crossGameManager.IsVisitingFromMask = true;
        crossGameManager.ErrorLog("opens filter" + ImageTexture.gameObject.activeSelf);
    }

    public void TakeScreenShot() {
        crossGameManager.ErrorLog("takes screenshot");
        screnshoot.TakeScreenshot_static(Screen.width, Screen.height, ImageTexture);
    }

    public void ShareSchreenshot() {
        screnshoot.ShareScreenshot_static();
    }

    public void SaveSchreenshot() {
        screnshoot.SaveScreenshot_static();
        popup.SetActive(true);

    }

    public void TrashSchreenshot() {
        screnshoot.TrashScreenshot_static();
    }


    public void MaskScene() {
        SceneManager.LoadScene("ARFilterScene");
    }
    
    public void HelmetScene() {
        SceneManager.LoadScene("ARFilterSceneHelmet");
    }


}