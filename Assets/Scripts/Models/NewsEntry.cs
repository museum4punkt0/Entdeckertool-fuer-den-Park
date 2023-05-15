using UnityEngine;

[System.Serializable]
public class NewsEntry {
    //public int id;
    public string headline;
    public string target;
    
    public static NewsEntry CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<NewsEntry>(jsonString);
    }
}

