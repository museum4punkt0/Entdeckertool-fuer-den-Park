using UnityEngine;

[System.Serializable]
public class Game1DataContent {
    public int id;
    public StrapiMedia image;
    public bool roman;
    public string level;
    public string title;

    public static YearData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<YearData>(jsonString);
    }
}

