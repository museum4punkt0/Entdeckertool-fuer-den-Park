using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateEllipse : MonoBehaviour
{
    public int speed;
    public GameObject pivotObject;

    void Update()
    {
        transform.RotateAround(pivotObject.transform.position, new Vector3(0, 0, 1), speed * Time.deltaTime);
    }

}
