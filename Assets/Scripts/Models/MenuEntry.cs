using UnityEngine;

[System.Serializable]
public class MenuEntry
{
    public int id;
    public string headline;
    public string subheadline;
    public string ButtonText;
    public string target;
    public string type;

    public static MenuEntry CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<MenuEntry>(jsonString);
    }
}

