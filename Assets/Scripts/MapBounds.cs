using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using UnityEngine;

public class MapBounds : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;
    bool _isInitialized;

    ILocationProvider _locationProvider;
    ILocationProvider LocationProvider
    {
        get
        {
            if (_locationProvider == null)
            {
                _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            }

            return _locationProvider;
        }
    }

    Vector3 _targetPosition;

    
    // Start is called before the first frame update
    void Start() {
        this._map = this.GetComponent<AbstractMap>();
        LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;


    }

    // Update is called once per frame
    void Update()
    {
        //this._map.SetZoom(16.0f);
    }
    void LateUpdate()
    {
        if (_isInitialized)
        {
            var map = LocationProviderFactory.Instance.mapManager;
            //map.SetCenterLatitudeLongitude(LocationProvider.CurrentLocation.LatitudeLongitude);
           map.UpdateMap(LocationProvider.CurrentLocation.LatitudeLongitude);

        }
    }
}
