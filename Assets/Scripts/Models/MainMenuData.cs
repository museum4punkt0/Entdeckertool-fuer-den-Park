using UnityEngine;

[System.Serializable]
public class MainMenuData
{
    public int id;
    public MainMenuDataAttributes attributes;

    public static MainMenuData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<MainMenuData>(jsonString);
    }
}

