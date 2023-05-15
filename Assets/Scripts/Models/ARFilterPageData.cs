using UnityEngine;

[System.Serializable]
public class ARFilterPageData {
    public int id;
    public ARFilterPageAttributes attributes;

    public static ARFilterPageData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ARFilterPageData>(jsonString);
    }
}


