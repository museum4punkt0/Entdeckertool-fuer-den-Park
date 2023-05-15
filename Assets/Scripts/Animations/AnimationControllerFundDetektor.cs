using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class AnimationControllerFundDetektor : MonoBehaviour {
    public GameObject script;
    public FullEllipseAnimation FullEllipseAnimation_script;
    public AnimateMenu AnimateMenu_script;
    public GameObject ellipse1;
    public GameObject ellipse2;
    public GameObject ellipse3;
    public GameObject FullEllipse;
    public GameObject screenFundEntdeckt;
    public AudioSource audioSource;
    public DetectorFundPopUp detectorFundPopUpScript;

    private void Start() {
        detectorFundPopUpScript = gameObject.GetComponent<DetectorFundPopUp>();
    }

    public void StopFunddetektor() {
        if (AnimateMenu_script.isOpen) {
            FullEllipse.SetActive(false);
        } else {
            FullEllipse.SetActive(true);
        }
    }


    public void AlignEllipses() {

        ellipse1.transform.LeanMoveLocal(new Vector2(0, 0), 1).setEaseOutQuart();
        ellipse2.transform.LeanMoveLocal(new Vector2(0, 0), 1).setEaseOutQuart();
        ellipse3.transform.LeanMoveLocal(new Vector2(0, 0), 1).setEaseOutQuart();

        StartCoroutine(ShowPopUp());
    }

    public void ResetEllipses() {

        ellipse1.transform.LeanMoveLocal(new Vector2(40, 0), 1).setEaseOutQuart();
        ellipse2.transform.LeanMoveLocal(new Vector2(80, 80), 1).setEaseOutQuart();
        ellipse3.transform.LeanMoveLocal(new Vector2(0, -80), 1).setEaseOutQuart();

    }

    public void RestartFunddetektor() {
        //go to animation
        StartCoroutine(Restart());
        //reset distance
    }

    IEnumerator LoadScene() {

        yield return new WaitForSeconds(5f);
       // SceneManager.LoadScene("ARScene");
    }

    IEnumerator ShowPopUp() {

        yield return new WaitForSeconds(5f);
        //    screenFundEntdeckt.SetActive(true);
        // SceneManager.LoadScene("ARGPSLocationTest");
        if (SceneManager.GetActiveScene().name == "MainScene") {
            detectorFundPopUpScript.Show();
            audioSource.mute = true;
        }

    }

    IEnumerator Restart() {
        yield return new WaitForSeconds(1f);
        FullEllipseAnimation_script.ReStartAnimation();
        ResetEllipses();

    }

}
