using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class Header
{
    public int id;
    public string filter;
    public string filterDelete;
    public string display;
    public string inUse;

    public static Header CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Header>(jsonString);
    }

}