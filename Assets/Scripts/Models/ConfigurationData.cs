using UnityEngine;

[System.Serializable]
public class ConfigurationData {
    public int id;
    public ConfigurationAttributes attributes;

    public static ConfigurationData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ConfigurationData>(jsonString);
    }
}


