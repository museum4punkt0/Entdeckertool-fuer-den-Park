using UnityEngine;

[System.Serializable]
public class FunddetektorPageData {
    public int id;
    public FunddetektorPageAttributes attributes;

    public static FunddetektorPageData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<FunddetektorPageData>(jsonString);
    }
}


