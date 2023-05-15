namespace Mapbox.Examples
{
	using Mapbox.Unity.Location;
	using Mapbox.Unity.Map;
	using UnityEngine;

	public class ImmediatePositionWithLocationProvider : MonoBehaviour
	{

		public bool _isInitialized;

		ILocationProvider _locationProvider;
        CrossGameManager crossGameManager;
        //ILocationProvider LocationProvider
        //{
        //	get
        //	{
        //		if (_locationProvider == null)
        //		{
        //			_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
        //		}

        //		return _locationProvider;
        //	}
        //}

        public ILocationProvider LocationProvider {
            private get {
                if (_locationProvider == null) {
                    _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
                }

                return _locationProvider;
            }
            set {
                if (_locationProvider != null) {
                    //_locationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;

                }
                _locationProvider = value;
                //_locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
            }
        }

        Vector3 _targetPosition;

		void Start()
		{
			LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;

            crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
            crossGameManager.ErrorLog("has inititialised map" + _isInitialized);
        }

		void LateUpdate()
		{
			if (_isInitialized)
			{
				var map = LocationProviderFactory.Instance.mapManager;
				transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
                //crossGameManager.ErrorLog(_isInitialized + LocationProvider.CurrentLocation.LatitudeLongitude.ToString());
            }
            
		}
	}
}