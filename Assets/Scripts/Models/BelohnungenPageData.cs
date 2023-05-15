using UnityEngine;

[System.Serializable]
public class BelohnungenPageData {
    public int id;
    public BelohnungenPageAttributes attributes;

    public static BelohnungenPageData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<BelohnungenPageData>(jsonString);
    }
}


