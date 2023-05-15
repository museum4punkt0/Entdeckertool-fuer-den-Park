using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using Services;

public class readContet : MonoBehaviour
{
    public GameObject root;
    public GameObject spielPrefab;

    public readContet() {
    }


    void loadGame1(StrapiSingleResponse<Game1Data> res) {
        Game1Data _data = res.data;

        /*
        foreach (Game1DataContent content in _data.attributes.content) {
            print("data: " + new CMSGame1Content(content));
        }
        */
    }


}
