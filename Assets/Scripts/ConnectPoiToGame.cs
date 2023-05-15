using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectPoiToGame : MonoBehaviour
{

    public GameObject game;
    public GameObject map;

    public void StartGame() {

        Debug.Log("Starts game in connect poi to game");
        map.SetActive(false);
        game.SetActive(true);

        if (game.GetComponents<MonoBehaviour>().Length > 0) {
            MonoBehaviour[] comps = game.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour c in comps) {
                c.enabled = true;
            }
        }

        GameObject[] MapMarkers = GameObject.FindGameObjectsWithTag("MapMarker");

        foreach (GameObject MapMarker in MapMarkers) {
            MapMarker.SetActive(false);
        }

    }
}
