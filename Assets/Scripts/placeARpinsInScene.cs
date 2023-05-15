using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARLocation;

public class placeARpinsInScene : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ARPinPrefab;
    public List<Vector2> positions;

    public CrossGameManager crossGameManager;
    
    void Start()
    {

        GameObject crossGameManagerObject = GameObject.FindGameObjectWithTag("CrossGameManager");
        if (crossGameManagerObject != null) {
            crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        }

        int index = 0;
        foreach (Vector2 position in positions) {
            GameObject newPin = Instantiate(ARPinPrefab);

            if (!newPin.GetComponent<PlaceAtLocation>()) {
                newPin.AddComponent<PlaceAtLocation>();
            }
          
        
            newPin.GetComponent<PlaceAtLocation>()._Latitude = position.x; //in the 50s
            newPin.GetComponent<PlaceAtLocation>()._Longitude = position.y; //around 13
            crossGameManager.ErrorLog("placed pin" +index+ newPin.transform.position);
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
