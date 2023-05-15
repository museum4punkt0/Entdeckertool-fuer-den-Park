using UnityEngine;

[System.Serializable]
public class Game3Data {
    public int id;
    public Game3Attributes attributes;

    public static Game3Data CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Game3Data>(jsonString);
    }
}


