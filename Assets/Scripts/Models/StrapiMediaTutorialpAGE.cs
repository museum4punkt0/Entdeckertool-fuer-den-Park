using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using Tasks;
using UIBuilder;
using UnityEngine;


[System.Serializable]
public class StrapiMediaTutorialPage {

    public List<StrapiMediaDataTutorialPage> data;

    public static StrapiMediaTutorialPage CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<StrapiMediaTutorialPage>(jsonString);
    }

}