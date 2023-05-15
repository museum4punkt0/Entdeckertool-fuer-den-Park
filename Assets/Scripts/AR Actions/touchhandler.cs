using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchhandler : MonoBehaviour
{
   
    public List<GameObject> CollectedCoins = new List<GameObject>();
    void Update() {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            if (Input.touchCount > 0 && Input.touchCount < 2) {
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    checkTouch(Input.GetTouch(0).position);
                }
            }
        } else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
            if (Input.GetMouseButtonDown(0)) {
                checkTouch(Input.mousePosition);
            }
        }

        if (this.CollectedCoins.Count == GameObject.FindGameObjectsWithTag("coin").Length) {
            Debug.Log("finish game");
        }
    }


    // Start is called before the first frame update
    private void checkTouch(Vector3 pos) {
   
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)) {
            if (hit.transform.gameObject.CompareTag("coin")) {
                GameObject coin = hit.transform.gameObject;
                Debug.Log("Clicked on Coin");
                if (!this.CollectedCoins.Contains(coin)){
                    this.CollectedCoins.Add(hit.transform.gameObject);
                }

            }
        }
 

       

    }

}
