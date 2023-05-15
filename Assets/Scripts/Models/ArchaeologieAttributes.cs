using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class ArchaeologieAttributes {
    public string FAQHeadline;
    public string NewsHeadline;
    public string HighlightsHeadline;
    public List<AccordionItem> FAQAccordionItems;
    public List<GameMenuCard> cards;
    public List<VideoItem> video;
    public List<ArchaologieHighlitghData> Highlights;
    public string pageTitle;
    public string cardsHeadline;

    public static ArchaeologieAttributes CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<ArchaeologieAttributes>(jsonString);
    }
}