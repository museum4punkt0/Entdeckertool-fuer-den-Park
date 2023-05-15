using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StrapiSingleResponse<T>
{
    public T data;

    public static StrapiSingleResponse<T> CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<StrapiSingleResponse<T>>(jsonString);
    }
}