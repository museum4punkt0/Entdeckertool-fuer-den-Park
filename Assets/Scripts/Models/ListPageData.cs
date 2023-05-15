using UnityEngine;

[System.Serializable]
public class ListPageData {
    public int id;
    public ListPageAttributes attributes;

    public static ListPageData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ListPageData>(jsonString);
    }
}


