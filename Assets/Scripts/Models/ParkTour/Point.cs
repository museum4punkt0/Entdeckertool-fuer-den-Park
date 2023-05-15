using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TourPoint : BaseAttributes<TourPoint> {
    
    public TourPointOverlayActive TourPointOverlayInactive;
    public string headline;
    public TempPOI point_of_interest;
    public TourPointOverlayActive TourPointOverlayActive;
    public int id;
    public List<OverlayButton> buttons;
}


