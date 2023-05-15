using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Services
{
    public class Strapi_game3 : MonoBehaviour
    {

        [SerializeField]
        public GameObject ARSession;

        [SerializeField]
        public string url;

        public Strapi_game3()
        {
        }

        [System.Serializable]
        public class Game3VariableResponse {
            public Game3Variables data;
        }

        [System.Serializable]
        public class Game3Variables {
            public int id;
            public ActData attributes;
        }

        [System.Serializable]
        public class ActData {
            public bool PlaneDetectionDebug;
            public float tent_yOffSet;
            public float planeDetectionSizeParameter;
            public float tent_Scale;
        }

        private void Awake() {
            this.StartCoroutine(this.RequestRoutine(this.url+"/api/game-ausgrabung?=*", this.ResponseCallback));
        }

        private IEnumerator RequestRoutine(string url, Action<string> callback = null) {
            // Using the static constructor
            var request = UnityWebRequest.Get(url);
            // Wait for the response and then get our data
            yield return request.SendWebRequest();
            var data = request.downloadHandler.text;
      
            if (callback != null) {
                callback(data);
            }

        }

        // Callback to act on our response data
        private void ResponseCallback(string data) {

            var tempDataObject = JsonUtility.FromJson<Game3VariableResponse>(data);

            Debug.Log("has data" + tempDataObject.data.attributes.planeDetectionSizeParameter);


            this.ARSession.GetComponent<PlaceOnPlaneWithAnchor>().ShouldShowPlaneDetection = tempDataObject.data.attributes.PlaneDetectionDebug;
            this.ARSession.GetComponent<PlaceOnPlaneWithAnchor>().yOffSet = tempDataObject.data.attributes.tent_yOffSet;
            this.ARSession.GetComponent<PlaceOnPlaneWithAnchor>().scale = tempDataObject.data.attributes.tent_Scale;
            this.ARSession.GetComponent<PlaceOnPlaneWithAnchor>().dimensionsForBigPlane = new Vector2(tempDataObject.data.attributes.planeDetectionSizeParameter, tempDataObject.data.attributes.planeDetectionSizeParameter);


            this.ARSession.GetComponent<PlaceOnPlaneWithAnchor>().BeginWithStrapiData();
        }
    }
}