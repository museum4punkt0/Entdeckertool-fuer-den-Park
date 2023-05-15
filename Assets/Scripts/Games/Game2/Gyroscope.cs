using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope : MonoBehaviour
{
    // Start is called before the first frame update

    private UnityEngine.Gyroscope gyro; // Gyroscope
    private Quaternion rot; // Base quaternion to get a rotation
    private bool isGyroReady;

    private CrossGameManager crossGameManager;

    public float minXangle = 10;
    public float maxXangle = 100;

    private int nextUpdate = 1;


    void Start()
    {

        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        EnableGyro();
    }


    private void EnableGyro() {
        if (SystemInfo.supportsGyroscope) {
            gyro = Input.gyro;
            gyro.enabled = true;
            rot = new Quaternion(0, 0, 1, 0);
            isGyroReady = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGyroReady) {
            transform.localRotation = gyro.attitude * rot;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);

          
            if (Time.time >= nextUpdate) {
                Debug.Log(Time.time + ">=" + nextUpdate);
                // Change the next update (current second+1)
                nextUpdate = Mathf.FloorToInt(Time.time) + 1;

                // Call your fonction
                crossGameManager.ErrorLog("X rotation" + transform.eulerAngles.x.ToString().Split("."));
                crossGameManager.ErrorLog("Y rotation" + transform.eulerAngles.y.ToString().Split("."));


            }


            //if (transform.eulerAngles.x > minXangle && transform.eulerAngles.x < maxXangle) {
            //    crossGameManager.ErrorLog("points in direction");

            //}

        }

    }
}
