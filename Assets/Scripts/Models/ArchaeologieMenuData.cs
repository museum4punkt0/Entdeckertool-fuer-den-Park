using UnityEngine;

[System.Serializable]
public class ArchaeologieMenuData {
    public int id;
    public ArchaeologieAttributes attributes;

    public static ArchaeologieMenuData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ArchaeologieMenuData>(jsonString);
    }
}


