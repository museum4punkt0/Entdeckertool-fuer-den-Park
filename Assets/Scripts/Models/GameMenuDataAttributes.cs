using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class GameMenuDataAttributes {
    public List<GameMenuCard> cards;
    public string pageTitle;

    public static GameMenuDataAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<GameMenuDataAttributes>(jsonString);
    }
}