
using UnityEngine;

[System.Serializable]
public class GrabungItem {
    public string date;
    public string description;
    public string location;
    public string size;
    public string ButtonText;

    public static GrabungItem CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<GrabungItem>(jsonString);
    }


}