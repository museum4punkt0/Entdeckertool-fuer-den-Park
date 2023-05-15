using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class onStart : MonoBehaviour
{
    public GameObject legend;
    public GameObject centerElement;
    public RadialWheel radialWheelScript;
    // Start is called before the first frame update
    void Start()
    {
        radialWheelScript.toolTextPlaceholder = centerElement.gameObject.transform.name.ToString();
    }

   
}
