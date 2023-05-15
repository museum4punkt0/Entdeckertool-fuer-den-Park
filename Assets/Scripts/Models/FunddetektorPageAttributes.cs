using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class FunddetektorPageAttributes {
    public string headline;
    public PopUp firstPopUp;

    public static FunddetektorPageAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<FunddetektorPageAttributes>(jsonString);
    }


}
