using UnityEngine;

[System.Serializable]
public class HighlightData
{
    public int id;
    public HighlightDataAttributes attributes;

    public static HighlightData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<HighlightData>(jsonString);
    }
}

