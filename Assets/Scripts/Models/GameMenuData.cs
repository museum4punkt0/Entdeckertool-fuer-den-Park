using UnityEngine;

[System.Serializable]
public class GameMenuData
{
    public int id;
    public GameMenuDataAttributes attributes;

    public static GameMenuData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameMenuData>(jsonString);
    }
}

