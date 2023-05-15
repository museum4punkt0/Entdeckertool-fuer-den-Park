using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class Game3Attributes {
    public int tourID;
    public TempPOI point_of_interest;
    public TempPOI glocke;
    public TempPOI maultier;
    public string description;
    public int reward;

    public Game4PopUpStartContent popupStart;
    public Game4PopUpStartContent popupStep1;
    public Game4PopUpStartContent winMessageP1;
    public Game4PopUpStartContent popupStep2;
    public Game4PopUpStartContent winMessageP2;
    public Game4PopUpStartContent popupStep3;
    public Game4PopUpStartContent popupStep4;
    public Game4PopUpStartContent popupStep5;
    public Game4PopUpStartContent winMessageP3;

    public SliderItem winimage1;
    public SliderItem winimage2;

    public static Game3Attributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<Game3Attributes>(jsonString);
    }
}