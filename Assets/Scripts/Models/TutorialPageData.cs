using UnityEngine;

[System.Serializable]
public class TutorialPageData {
    public int id;
    public TutorialPageAttributes attributes;

    public static TutorialPageData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<TutorialPageData>(jsonString);
    }
}


