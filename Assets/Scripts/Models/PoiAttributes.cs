using ARLocation;
using Mapbox.Utils;
using UnityEngine;

[System.Serializable]
public class PoiAttributes : BaseAttributes<PoiAttributes> {
    public string title;
    public double lat;
    public double lng;
    public string type;
    public int reward;
    public TourPoint tourPoint;
    
    public FundObject fundobjekt;

    public Pin pin;

    public Grabung grabung;

    public illustrationContainer illustration;

    //public static PoiAttributes CreateFromJSON(string jsonString)
    //{
    //    return JsonUtility.FromJson<PoiAttributes>(jsonString);
    //}

    public Vector2d getLatLng()
    {
        return new Vector2d(lat,lng);
    }

    public Location getARLocation() {
        return new Location() {
            Latitude = this.lat,
            Longitude = this.lng,
            Altitude = 0,
            AltitudeMode = AltitudeMode.DeviceRelative
        };
    }
}