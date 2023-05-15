using UnityEngine;

[System.Serializable]
public class HilfeMenuData {
    public int id;
    public HilfeAttributes attributes;

    public static HilfeMenuData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<HilfeMenuData>(jsonString);
    }
}


