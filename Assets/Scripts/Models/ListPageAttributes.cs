using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class ListPageAttributes {
    public string pageTitle;
    public string buttonSubHeadline;

    public static ListPageAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<ListPageAttributes>(jsonString);
    }


}
