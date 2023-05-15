using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class BelohnungenPageAttributes {
    public string pageTitle;
    public string headline;
    public string totalCoinsHeadline;
    public string dropdownHeadline;
    public string maskButtonTextDisabled;
    public string helmetButtonTextDisabled;
    public string maskButtonTextEnabled;
    public string helmetButtonTextEnabled;

    public static ARPageAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<ARPageAttributes>(jsonString);
    }


}
