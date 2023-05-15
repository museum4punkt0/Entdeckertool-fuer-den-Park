using UnityEngine;

[System.Serializable]
public class IntroScenePageData {
    public int id;
    public IntroScenePageAttributes attributes;

    public static IntroScenePageData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<IntroScenePageData>(jsonString);
    }
}


