using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using Tasks;
using UIBuilder;
using UnityEngine;

[System.Serializable] 
public class StrapiMediaDataTutorialPage {

    public int id;
    public StrapiMediaDataAttributes attributes;
    public Sprite ConvertedSprite;

    public static StrapiMediaDataTutorialPage CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<StrapiMediaDataTutorialPage>(jsonString);
    }

    public async Task<CMSImage> GetMediaImageTextureFromStrapiMedia() {

        CMSImage image = new CMSImage(this.attributes.GetFullImageUrl());
        return image;
    }

    public async Task<Texture2D> GetTexture2D() {
        return await CommonTasks.GetRemoteTexture(this.attributes.GetFullImageUrl());
    }

    public async Task<Sprite> ToSprite() {

        Texture2D texture = await CreateTexture();

        this.ConvertedSprite = Sprite.Create(texture, new Rect(0, 0, this.attributes.width, this.attributes.height), new Vector2(0.5f, 0.5f));

        return this.ConvertedSprite;
    }

    public async Task<Texture2D> CreateTexture() {

        Texture2D texture = await CommonTasks.GetRemoteTexture(this.attributes.GetFullImageUrl());

        return texture;
    }
}