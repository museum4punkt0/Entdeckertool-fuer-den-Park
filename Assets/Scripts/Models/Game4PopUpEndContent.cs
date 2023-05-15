using UnityEngine;

[System.Serializable]
public class Game4PopUpEndContent {
    public int id;
    public string headline;
    public string subHeadline;
    public string buttonText;

    public static Game4PopUpEndContent CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Game4PopUpEndContent>(jsonString);
    }
}

