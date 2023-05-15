using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GrabungenTeaser {
    public string Headline;
    public string Subheadline;
    public string Body;

    public static GrabungenTeaser CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GrabungenTeaser>(jsonString);
    }
}