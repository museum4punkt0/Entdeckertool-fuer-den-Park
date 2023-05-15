using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using UIBuilder;
using UnityEngine;

[System.Serializable]
public class ItemAttributes
{
    public string headline;
    public string subHeadline;
    public string shortText;
    public string longText;
    public string bodyText;
    public string captionShort;
    public string captionLong;
    public string buttonText;
    public OverlayButton button;
    public AccordionItem[] accordionItems;
    public Link[] links;
    public SliderItem[] sliderItems;
    public OverlayButton[] buttons;
    public OverlayButton[] button1;
    public OverlayButton[] button2;
    
    public string fundTitle;

    public string date;
    public string description;
    public string location;
    public string size;
    public string buttonHeadline;
    public StrapiMedia grabungskarte;

    public static ItemAttributes CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ItemAttributes>(jsonString);
    }

    public async Task<CMSMediaItem[]> GetSliderItemsWithMedia() {
        if (this.sliderItems != null) {
            CMSMediaItem[] list = new CMSMediaItem[this.sliderItems.Length];
            for (int i = 0; i < this.sliderItems.Length; i++) {
                list[i] = await this.sliderItems[i].media.GetMediaImageTextureFromStrapiMedia();
            }

            return list;
        }

        return null;

    }


}