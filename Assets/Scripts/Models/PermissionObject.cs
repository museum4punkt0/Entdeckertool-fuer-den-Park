using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class PermissionObject
{
    public int id;
    public string bodyText;
    public string allowButton;
    public string denyButton;

    public static PermissionObject CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PermissionObject>(jsonString);
    }

}