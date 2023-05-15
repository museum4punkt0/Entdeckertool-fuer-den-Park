using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panZoom : MonoBehaviour
{
    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    public float minX, minY, maxX, maxY;

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2) {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;
            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.01f);

        } else if (Input.GetMouseButton(0)) {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            /*direction.x = Mathf.Clamp(0, -0.39f, 0.42f);
            direction.y = Mathf.Clamp(0, -0.51f, 0.41f);*/
            transform.position += direction;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), -10);
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float increment) {
        //maxX = maxX * 2;
        //minX = minX * 2;
        
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
        print("incfrement: " + increment);

    }

}

