using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score {
    public int coinsAmount;
    public List<ItemOnMap> visitedPois;

    public Score() {
        this.visitedPois = new List<ItemOnMap>();
    }
    public static Score CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<Score>(jsonString);
    }
    public static string ToJson(Score score) {
        return JsonUtility.ToJson(score);
    }
}
