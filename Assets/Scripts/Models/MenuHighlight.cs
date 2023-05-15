using UnityEngine;

[System.Serializable]
public class MenuHighlight
{
    public int id;
    public string title;
    
    public static MenuHighlight CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<MenuHighlight>(jsonString);
    }
}

