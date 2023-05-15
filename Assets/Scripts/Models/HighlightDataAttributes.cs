using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class HighlightDataAttributes {
    public string headline;
    public string target;

    public static HighlightDataAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<HighlightDataAttributes>(jsonString);
    }
}