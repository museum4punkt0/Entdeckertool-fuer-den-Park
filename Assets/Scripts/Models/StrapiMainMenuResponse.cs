using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StrapiMainMenuResponse
{
    public MainMenuData data;

    public static StrapiMainMenuResponse CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<StrapiMainMenuResponse>(jsonString);
    }
}