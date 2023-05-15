using System;
using ARLocation;
using Mapbox.Unity.Location;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using ILocationProvider = Mapbox.Unity.Location.ILocationProvider;
using System.Collections;

public class CompassHeadingController : MonoBehaviour {
    //public GameObject compassNeedle;
    private VisualElement compassNeedle;
    bool _isInitialized;
    
    ILocationProvider _locationProvider;
    private ARLocationProvider _arLocationProvider;
    float speed = 2f;
    
    ILocationProvider LocationProvider {
        get {
            if (_locationProvider == null) {
                _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            }

            return _locationProvider;
        }
    }

    void OnEnable() {

        compassNeedle = this.gameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("compassNeedle");

        if (SceneManager.GetActiveScene().name == "MainScene") {
            LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;
        } else if (SceneManager.GetActiveScene().name == "ARScene" || SceneManager.GetActiveScene().name == "360_Illustrations") {
            this._arLocationProvider = ARLocationProvider.Instance;
        }
    }

    void OnDisable() {

        if (this._arLocationProvider != null) {
            Destroy(this._arLocationProvider.gameObject);
        }

    }



    private void Update() {
        float currentHeading = 0;
        if (gameObject.GetComponent<UIItemViewController>()  && gameObject.GetComponent<UIItemViewController>().menuType == "menuOnClosedPosition") {

            
            if (SceneManager.GetActiveScene().name == "MainScene") {
                currentHeading = this.LocationProvider.CurrentLocation.UserHeading;
                this.compassNeedle.transform.rotation = Quaternion.Euler(0, 0, (float)currentHeading);
                //this.compassNeedle.transform.rotation *= Quaternion.Euler(0, 0, (float)currentHeading);

            } else if (SceneManager.GetActiveScene().name == "ARScene" || SceneManager.GetActiveScene().name == "360_Illustrations") {
                currentHeading = (float)this._arLocationProvider.CurrentHeading.heading;
            }
            // ToDo: smooth/tween values
            this.compassNeedle.transform.rotation = Quaternion.Euler(0, 0, (float)currentHeading);

        } else if (SceneManager.GetActiveScene().name == "ARScene" || SceneManager.GetActiveScene().name == "360_Illustrations") {
            currentHeading = (float)this._arLocationProvider.CurrentHeading.heading;
            this.compassNeedle.transform.rotation = Quaternion.Euler(0, 0, (float)currentHeading);

        } else {
            //this.compassNeedle.transform.rotation = Quaternion.Euler(0, 0, 90f);
            this.compassNeedle.transform.rotation = Quaternion.Slerp(this.compassNeedle.transform.rotation, Quaternion.Euler(0, 0, 90f), speed * Time.deltaTime);
        }

    }

}