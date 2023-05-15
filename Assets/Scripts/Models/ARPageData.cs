using UnityEngine;

[System.Serializable]
public class ARPageData {
    public int id;
    public ARPageAttributes attributes;

    public static ARPageData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ARPageData>(jsonString);
    }
}


