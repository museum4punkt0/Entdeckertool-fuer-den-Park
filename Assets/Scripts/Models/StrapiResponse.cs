using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StrapiResponse<T>
{
    public List<T> data;

    public static StrapiResponse<T> CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<StrapiResponse<T>>(jsonString);
    }
}