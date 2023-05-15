using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCallsOnAnimationEnd: MonoBehaviour
{
    public GameObject gameContainer; 
    public void killmyself() {
        Debug.Log("object wants to kill itself");
        this.gameContainer.GetComponent<start_teil2>().ShowDropDownUIWithItem(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
