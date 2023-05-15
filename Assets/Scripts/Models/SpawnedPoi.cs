using System.Collections;
using UnityEngine;
public class SpawnedPoi
{
    public GameObject gameObject { get; set; }
    public Poi poi { get; set; }

    public SpawnedPoi(GameObject gameObject, Poi poi)
    {
        this.gameObject = gameObject;
        this.poi = poi;
    }
}