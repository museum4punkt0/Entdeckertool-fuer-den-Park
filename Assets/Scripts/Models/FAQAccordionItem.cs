using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FAQAccordionItem
{
    public string headline;
    public string body;

    public static FAQAccordionItem CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<FAQAccordionItem>(jsonString);
    }
}