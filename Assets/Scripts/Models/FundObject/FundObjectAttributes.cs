using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIBuilder;
using System.Threading.Tasks;


[System.Serializable]

public class FundObjectAttributes : BaseAttributes<FundObjectAttributes> {

    public string headline;
    public double subHeadline;
    public string shortText;
    public string longText;
    public SliderItem[] sliderItems;
    public Poi point_of_interest;
    public StrapiMedia mainImage;
    public string category;

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
