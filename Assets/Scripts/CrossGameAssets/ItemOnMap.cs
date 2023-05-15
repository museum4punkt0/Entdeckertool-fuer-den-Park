using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class ItemOnMap : BaseAttributes<ItemOnMap> {

    public bool HasBeenVisited;
    public string HasBeenVisitedBy;
    public int ID;
    public bool IsWithinReach;

    public double distanceToPlayer;

    public GameObject Pin;

    public TourPoint TourPoint;
    
    public Poi Poi;
    public Vector3 Position;
    
    public string Name;
    public string Target;
    public int TargetID;

    public List<Sprite> AssociatedMenuItems = new List<Sprite>();
}

