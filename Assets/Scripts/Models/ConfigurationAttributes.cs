using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class ConfigurationAttributes {
    public string read_more;
    public string read_less;
    public string highlights;
    public string PopUpFundDefaultButton;
    public string PopUpContinueDefaultButton;
    public string PopUpArModeDefaultButton;
    public StrapiMedia logo;


    public static ConfigurationAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<ConfigurationAttributes>(jsonString);
    }


}
