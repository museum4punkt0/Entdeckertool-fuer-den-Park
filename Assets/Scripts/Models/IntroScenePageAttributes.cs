using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class IntroScenePageAttributes {
    public PermissionObject locassionPermission;
    public PermissionObject cameraPermission;
    public string locationError;
    public string cameraError;

    public static IntroScenePageAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<IntroScenePageAttributes>(jsonString);
    }
}