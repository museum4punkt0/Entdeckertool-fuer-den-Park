using UnityEngine;

[System.Serializable]
public class Tour
{
    public int id;
    public TourAttributes attributes;
    public static Tour CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Tour>(jsonString);
    }
}

