using UnityEngine;

[System.Serializable]
public class Game5Data {
    public int id;
    public Game5Attributes attributes;

    public static Game5Data CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Game5Data>(jsonString);
    }
}


