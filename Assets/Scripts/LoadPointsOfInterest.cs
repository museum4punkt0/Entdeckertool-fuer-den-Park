
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using Services;
using TMPro;

public class LoadPointsOfInterest : MonoBehaviour
{
	[SerializeField]
	AbstractMap _map;

	[SerializeField]
	[Geocode]
	string[] _locationStrings;
	Vector2d[] _locations;

	[SerializeField]
	float _spawnScale = 100f;

	[SerializeField]
	GameObject _bubblePrefab;
    
	public List<SpawnedPoi> _spawnedObjects;

	public CrossGameManager crossGameManager;


	public LoadPointsOfInterest()
    {
    }

    private void Awake() {
		crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
	}

	void Start()
	{
		_locations = new Vector2d[_locationStrings.Length];
		_spawnedObjects = new List<SpawnedPoi>();

		StartCoroutine(this.crossGameManager.strapiService.getPointOfInterests((StrapiResponse<Poi> res) =>
			{
				Debug.Log("strapi response" + res);
				res.data.ForEach((Poi poi)=>
				{
					this.spawnFromPoi(poi);
				});
            }
		));

	}
	private void spawnFromPoi(Poi poi) {
        GameObject instance;
        switch (poi.attributes.type) {
            case "type1":
            default:
                //bubble
                instance = Instantiate(this._bubblePrefab);
                break;
        }
		instance.transform.localPosition = _map.GeoToWorldPosition(poi.attributes.getLatLng(), true);
		instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        if (instance.GetComponentInChildren<TMP_Text>()) {
            instance.GetComponentInChildren<TMP_Text>().SetText(poi.attributes.title);
        }
        _spawnedObjects.Add(new SpawnedPoi(instance, poi));
	}


	private void Update()
	{
		this._spawnedObjects.ForEach((SpawnedPoi spawnedPoi) =>
		{
			spawnedPoi.gameObject.transform.localPosition = _map.GeoToWorldPosition(spawnedPoi.poi.attributes.getLatLng(), true);
			spawnedPoi.gameObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
		}); 
		int count = _spawnedObjects.Count;
		
	}
}