using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class HilfeAttributes {
    public List<AccordionItem> accordionItem;
    public List<AccordionItem> AccordionItem;
    public string imprintText;
    public string headline;
    public string text;
    public string imprint;
    public string pageTitle;
    public string restartButton;
    public string locationAdmisionButton;
    public string cameraAdmissionButton;
    public string tutorialButton;
    public string FAQHeadline;
    public string impressum;

    public static HilfeAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<HilfeAttributes>(jsonString);
    }
}