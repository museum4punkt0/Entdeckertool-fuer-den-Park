using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class Game4DataContent {
    public int id;
    public StrapiMedia imageBefore;
    public StrapiMedia imageAfter;
    public string headline;
    public string material;
    public List<Game4ToolContent> tool;
    public bool completed = new bool();

    public static Game4DataContent CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Game4DataContent>(jsonString);
    }
}

