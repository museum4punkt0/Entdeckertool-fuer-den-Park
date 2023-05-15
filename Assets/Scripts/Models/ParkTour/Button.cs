using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class OverlayButton : BaseAttributes<OverlayButton> {

    public int id;
    public string text;
    public string type;
    public string target;


    public static OverlayButton CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<OverlayButton>(jsonString);
    }

}
