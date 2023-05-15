using UnityEngine;

[System.Serializable]
public class Game4PopUpStartContent {
    public int id;
    public string headline;
    public string subHeadline;
    public string buttonText;

    public static Game4PopUpStartContent CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Game4PopUpStartContent>(jsonString);
    }
}

