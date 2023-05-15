using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class ArchaeologieGrabungenAttributes {
    public List<YearData> years;


    public static ArchaeologieGrabungenAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<ArchaeologieGrabungenAttributes>(jsonString);
    }
}