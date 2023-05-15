using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class Game1Attributes {
    public List<Game1DataContent> content;
    public string correctAnswer;
    public string falseAnswer;
    public string correctFalseAnswer;
    public string falseCorrectAnswer;
    public string tryAgain;

    public static Game1Attributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<Game1Attributes>(jsonString);
    }
}