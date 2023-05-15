using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class GrabungenPageAttributes {
    public string pageTitle;
    public string headline;

    public static GrabungenPageAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<GrabungenPageAttributes>(jsonString);
    }


}
