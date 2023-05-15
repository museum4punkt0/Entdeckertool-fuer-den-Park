using UnityEngine;

[System.Serializable]
public class Game1Data {
    public int id;
    public Game1Attributes attributes;

    public static Game1Data CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Game1Data>(jsonString);
    }
}


