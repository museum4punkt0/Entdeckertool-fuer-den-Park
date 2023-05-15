using System.Collections.Generic;
using ARLocation;
using Mapbox.Utils;
using UnityEngine;

[System.Serializable]
public class AccordionItem
{
    public string headline;
    public string body;
    public List<VideoItem> video;

    public static AccordionItem CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<AccordionItem>(jsonString);
    }


}