using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithCenter : MonoBehaviour
{
    [Header("Input Origin Point ei. Center Button In Menu -(to be used when center is outside prefab)-")]
    [SerializeField]
    private string tagname_center;

    [Header("Input Origin Point ei. Center Button In Menu")]
    [SerializeField]
    private GameObject gameobject_center;

    private GameObject CenterButton;

    private void Awake() {
        if (this.tagname_center != null && GameObject.FindGameObjectWithTag(this.tagname_center)) {
            this.CenterButton = GameObject.FindGameObjectWithTag(this.tagname_center);
        } else if (this.gameobject_center != null) {
            this.CenterButton = this.gameobject_center;
        }
    }
    void Start()
    {
        this.transform.position = this.CenterButton.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.CenterButton.transform.position;

    }
}
