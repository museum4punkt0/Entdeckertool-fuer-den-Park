using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public ItemAttributes attributes;


    public static Item CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Item>(jsonString);
    }
}

