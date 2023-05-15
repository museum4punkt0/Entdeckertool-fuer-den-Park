using System.Collections.Generic;


[System.Serializable]
public class TourAttributes : BaseAttributes<TourAttributes> {
    
    public TourTeaser tourTeaser;
    public TourOverlayActive TourOverlayActive;
    public RelPOI point_of_interests;

}