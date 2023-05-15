using ARLocation;
using Mapbox.Utils;
using UnityEngine;

[System.Serializable] 
public class StrapiMediaData : BaseAttributes<StrapiMediaData> {
    public StrapiMediaDataAttributes attributes;
    public int id;

    public static StrapiMediaData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<StrapiMediaData>(jsonString);
    }


}