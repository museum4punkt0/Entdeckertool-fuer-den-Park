using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveAcrossField : MonoBehaviour
{
    public float duration = 30;
    public float xValue;
    public float xStartValue;
    public float xGoalValue;
    void Start()
    {
        StartCoroutine(SmoothLerp(duration));
    }


    private IEnumerator SmoothLerp(float time) {
        float timeElapsed = 0;
        while (timeElapsed < time) {
            xValue = Mathf.Lerp(xStartValue, xGoalValue, timeElapsed / time);
            Vector3 pos = new Vector3(xValue, transform.position.y, transform.position.z);
            transform.position = pos;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        xValue = xGoalValue;
    }
}
