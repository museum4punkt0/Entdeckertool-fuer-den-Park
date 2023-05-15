using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turntocamera : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");

        //if (player == null) {
        //    player = Camera.main.gameObject;
        //}

        //Vector3 relativePos = player.transform.position - transform.position;

        //relativePos.y = 0;

        //// the second argument, upwards, defaults to Vector3.up
        //Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        //transform.rotation = rotation;

        //Debug.Log("rotates" + rotation);


        //Vector3 targetPostition = new Vector3(Camera.main.transform.position.x,
        //    this.transform.position.y,
        //    Camera.main.transform.position.z);
        //this.transform.rotation = new Quaternion(0, 0, 0, 0);
        //this.transform.LookAt(targetPostition);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
