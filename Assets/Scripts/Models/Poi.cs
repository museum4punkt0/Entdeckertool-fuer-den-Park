using UnityEngine;

[System.Serializable]
public class Poi
{
    public int id;
    public PoiAttributes attributes;

    public static Poi CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Poi>(jsonString);
    }
}

