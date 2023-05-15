using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class Game4Attributes {
    public List<Game4DataContent> content;
    public string title;
    public string subTitle;
    public Game4PopUpStartContent popupStart;
    public Game4PopUpEndContent popupEnd;
    public string winButtonHeadline;


    public static Game4Attributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<Game4Attributes>(jsonString);
    }
}