using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NotificationAttributes : BaseAttributes<NotificationAttributes> {
    public string Title;
    public string Subtitle;
    public string ButtonText;
    //public DateTime Timing;
    public string Timing;
    public bool Repeats;
    public bool onlyOnLocation;
    public string Target;

    public static Poi CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<Poi>(jsonString);
    }
}
