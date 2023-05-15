using UnityEngine;

[System.Serializable]
public class FilterPageData {
    public int id;
    public FilterPageAttributes attributes;

    public static FilterPageData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<FilterPageData>(jsonString);
    }
}


