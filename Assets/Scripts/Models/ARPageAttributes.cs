using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class ARPageAttributes {
    public PopUp firstPopUp;
    public PopUp stopARPopUp;

    public static ARPageAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<ARPageAttributes>(jsonString);
    }


}
