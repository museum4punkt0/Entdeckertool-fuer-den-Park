using ARLocation;
using Mapbox.Utils;
using UnityEngine;

[System.Serializable]
public class Link
{
    public string headline;
    public string subheadline;
    public string ButtonText;
    public string type;
    public string target;

    public static Link CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Link>(jsonString);
    }


}