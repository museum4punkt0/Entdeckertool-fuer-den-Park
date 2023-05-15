using UnityEngine;

[System.Serializable]
public class ArchaeologieGrabungen {
    public int id;
    public ArchaeologieGrabungenAttributes attributes;

    public static ArchaeologieGrabungen CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ArchaeologieGrabungen>(jsonString);
    }
}


