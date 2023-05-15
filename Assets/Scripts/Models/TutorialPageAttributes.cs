using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class TutorialPageAttributes {
    public string introText;
    public string buttonText;
    public string restartButton;
    public SliderItem[] images;

    public static TutorialPageAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<TutorialPageAttributes>(jsonString);
    }
}