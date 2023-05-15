using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class Game5Attributes {
    public int tourID;
    public string description;
    public int reward;

    public RelPOI point_of_interests;

    public Game4PopUpStartContent popupStart;
    public Game4PopUpStartContent popupStep1;
    public Game4PopUpStartContent popupStep2;
    public Game4PopUpStartContent winMessage;
    public string gameTitle;

    public static Game5Attributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<Game5Attributes>(jsonString);
    }
}