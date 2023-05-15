using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StrapiItemResponse
{
    public List<Item> data;

    public static StrapiItemResponse CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<StrapiItemResponse>(jsonString);
    }
}