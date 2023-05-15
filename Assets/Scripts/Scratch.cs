using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scratch : MonoBehaviour
{
    public GameObject mask;
    public Camera cam;
    bool pressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        if (pressed == true) {
            GameObject ob = Instantiate(mask, pos, Quaternion.identity);
            ob.transform.SetParent(this.gameObject.transform);
        }
        if (Input.GetMouseButtonDown(0)) {
            pressed = true;
        } else if (Input.GetMouseButtonUp(0)) {
            pressed = false;
        }
    }
}
