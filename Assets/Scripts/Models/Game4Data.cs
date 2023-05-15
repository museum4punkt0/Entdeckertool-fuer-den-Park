using UnityEngine;

[System.Serializable]
public class Game4Data {
    public int id;
    public Game4Attributes attributes;

    public static Game4Data CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Game4Data>(jsonString);
    }
}


