using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class MainMenuDataAttributes {
    public MenuHighlight[] highlights;
    public MenuEntry[] links;

    public static MainMenuDataAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<MainMenuDataAttributes>(jsonString);
    }
}