using UnityEngine;

[System.Serializable]
public class GameMenuCard
{
    public int id;
    public string title;
    public string subtitle;
    public string type;
    public string target;
    public string reward;
    public string tourenTime;
    public int tourenFundAmount;

    public static GameMenuCard CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameMenuCard>(jsonString);
    }
}

