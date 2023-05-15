using ARLocation;
using Mapbox.Utils;
using UnityEngine;

[System.Serializable]
public class StrapiMediaDataAttributes : BaseAttributes<StrapiMediaDataAttributes> {
    public string name;
    public string alternativeText;
    public string caption;
    public int width;
    public int height;
    public string hash;
    public string ext;
    public string mime;
    public double size;
    public string url;
    public string previewUrl;
    public string provider;
    public string createdAt;
    public string updatedAt;

    public static StrapiMediaDataAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<StrapiMediaDataAttributes>(jsonString);
    }

    public string GetFullImageUrl() {
#if DEVELOPMENT_BUILD
        return "https://var-staging.xailabs.com" + this.url;
#else
        return "https://var-production.xailabs.com" + this.url;
#endif
    }
}