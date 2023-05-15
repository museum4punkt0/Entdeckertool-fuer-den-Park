using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class PopUp
{
    public int id;
    public string headline;
    public string subHeadline;
    public string buttonText;

    public static PopUp CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PopUp>(jsonString);
    }

}