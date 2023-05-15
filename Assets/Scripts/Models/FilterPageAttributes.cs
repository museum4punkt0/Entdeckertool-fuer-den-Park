using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class FilterPageAttributes {
    public string showAllOption;
    public string fundeOption;
    public string panoramaOption;
    public string spieleOption;
    public string parktourenOption;
    public string grabungenOption;
    public string landschaftOption;
    public Header header;

    public static FilterPageAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<FilterPageAttributes>(jsonString);
    }


}
