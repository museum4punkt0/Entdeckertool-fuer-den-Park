using UnityEngine;

[System.Serializable]
public class Notification : BaseAttributes<Notification> {
    public int id;
    public NotificationAttributes attributes;
    public int IdentifierInt;
    public string IdentifierString;

    public static Poi CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Poi>(jsonString);
    }
}

