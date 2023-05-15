using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OverlapHandler : MonoBehaviour
{
    public List<GameObject> objectsToControl;
    public List<GameObject> objectsToObserve;

    private void Update() {
        foreach (GameObject objectToObserve in objectsToObserve) {
            if (objectToObserve.activeSelf) {
                foreach (GameObject objectToControl in objectsToControl) {
                    if (objectToControl.activeSelf) {
                        objectToControl.SetActive(false);
                    }
                }
            } else {
                foreach (GameObject objectToControl in objectsToControl) {
                    if (objectToControl.activeSelf) {
                        objectToControl.SetActive(true);
                    }
                }
            }
        }
    }
}
