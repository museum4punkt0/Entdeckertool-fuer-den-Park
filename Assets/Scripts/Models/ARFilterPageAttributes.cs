using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class ARFilterPageAttributes {
    public string maskButtonText;
    public string helmetButtonText;
    public string popUpText;
    public string popUpButton;

    public static ARFilterPageAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<ARFilterPageAttributes>(jsonString);
    }
}