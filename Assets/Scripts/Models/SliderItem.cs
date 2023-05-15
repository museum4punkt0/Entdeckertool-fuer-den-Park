using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class SliderItem
{
    public StrapiMedia media;
    public string id;

    public static SliderItem CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<SliderItem>(jsonString);
    }

}