using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInChildren : MonoBehaviour
{
    public List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    void Start()
    {

        getChildrenRenderers(transform);


    }

    void getChildrenRenderers(Transform GoT) {

        for (int i = 0; i < GoT.childCount; i++) {
            GameObject child = GoT.GetChild(i).gameObject;
            if (child.activeInHierarchy && child.GetComponent<SpriteRenderer>()) {
                renderers.Add(child.GetComponent<SpriteRenderer>());
                SetOpacityToZero(child.GetComponent<SpriteRenderer>());
            }
            if (child.transform.childCount > 0) {
                getChildrenRenderers(child.transform);
            }
        }
    }

    private void SetOpacityToZero(SpriteRenderer sr) {
        Color tmp = sr.color;
        tmp.a = 0;
        sr.color = tmp;

    }
    public void appearFade() {

        print("runs appearfade");
        foreach (SpriteRenderer sr in renderers) {
            StartCoroutine(FadeIn(sr, 10f));
        }
    }


    private IEnumerator FadeIn(SpriteRenderer sr, float duration) {
        float alphaVal = sr.color.a;
        Color tmp = sr.color;

        //while (sr.color.a < 100) {
        //    alphaVal += 0.2f;
        //    tmp.a = alphaVal;
        //    sr.color = tmp;

        //    yield return new WaitForSeconds(duration); // update interval
        //}
 

        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            alphaVal += 0.001f;
            tmp.a = alphaVal;
            sr.color = tmp;
            yield return new WaitForEndOfFrame();
        }

    }

}
