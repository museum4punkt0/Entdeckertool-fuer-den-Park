using UnityEngine;

[System.Serializable]
public class VideoItem
{
    public int id;
    public string headline;
    public StrapiMedia video;
    public string orientation;

    public static VideoItem CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<VideoItem>(jsonString);
    }
}

