using UnityEngine;

[System.Serializable]
public class GrabungenPageData {
    public int id;
    public GrabungenPageAttributes attributes;

    public static GrabungenPageData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GrabungenPageData>(jsonString);
    }
}


