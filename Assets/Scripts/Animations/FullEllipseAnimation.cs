using System;
using System.Collections;
using Mapbox.Unity.Location;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class FullEllipseAnimation : MonoBehaviour
{
    public GameObject detektorUIDocument;
    private float initialPos;


    private void Awake() {
        initialPos = this.gameObject.transform.position.y;

    }

    public void StartAnimationFadeIn()
    {
        //fade in ellipse
        this.transform.LeanMoveLocal(new Vector2(0, 1200), 1f).setEaseOutQuart();
        StartCoroutine(FadeIn());
    }

    public void StartAnimationMove()
    {
        this.transform.LeanMoveLocal(new Vector2(0, 0), 2).setEaseOutQuart();

        if (SceneManager.GetActiveScene().name == "MainScene")
            StartCoroutine(ScaleEllipse());

        if (SceneManager.GetActiveScene().name != "MainScene")
            StartCoroutine(DisplayText());
    }

    IEnumerator ScaleEllipse()
    {
        yield return new WaitForSeconds(2.5f);
        this.transform.LeanScale(new Vector2(6, 6), 2.5f);
    
    }

    IEnumerator DisplayText() {
        yield return new WaitForSeconds(2.5f);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);

    }

    IEnumerator FadeIn()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime) {
            // set color with i as alpha
            this.transform.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, i);
            yield return null;
        }
       
    }

    public void ReStartAnimation() {
        this.transform.LeanScale(new Vector2(1, 1), 0.1f);
        this.transform.localPosition = new Vector2(0, initialPos);
    }

}
