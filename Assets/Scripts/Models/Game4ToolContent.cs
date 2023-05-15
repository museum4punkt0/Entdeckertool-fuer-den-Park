using UnityEngine;

[System.Serializable]
public class Game4ToolContent {
    public int id;
    public StrapiMedia image;
    public string headline;
    public int percentage;

    public static Game4ToolContent CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Game4ToolContent>(jsonString);
    }
}

