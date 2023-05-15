using UnityEngine;

[System.Serializable]
public class ArchaologieHighlitghData
{
    public int id;
    public string headline;
    public string target;
    public StrapiMedia image;

    public static ArchaologieHighlitghData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ArchaologieHighlitghData>(jsonString);
    }
}

