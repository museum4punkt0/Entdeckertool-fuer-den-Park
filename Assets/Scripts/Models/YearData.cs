using UnityEngine;

[System.Serializable]
public class YearData
{
    public int id;
    public string headline;
    public string area;
    public string body;
    public string buttonHeadline;
    public string buttonTarget;
    public int year;

    public static YearData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<YearData>(jsonString);
    }
}

