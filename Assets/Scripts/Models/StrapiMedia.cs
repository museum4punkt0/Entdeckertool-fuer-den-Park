using System.Threading.Tasks;
using ARLocation;
using Mapbox.Utils;
using Tasks;
using UIBuilder;
using UnityEngine;


[System.Serializable]
public class StrapiMedia : BaseAttributes<StrapiMedia> {

    public StrapiMediaData data;
    public Sprite ConvertedSprite;

    public static StrapiMedia CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<StrapiMedia>(jsonString);
    }

    public async Task<CMSImage> GetMediaImageTextureFromStrapiMedia() {

        //CMSImage image =  new CMSImage(await CommonTasks.GetRemoteTexture(this.data.attributes.GetFullImageUrl()));
        CMSImage image = new CMSImage(this.data.attributes.GetFullImageUrl());
        return image;
    }


    public async Task<Texture2D> GetTexture2D() {
        return await CommonTasks.GetRemoteTexture(this.data.attributes.GetFullImageUrl());
    }

    public async Task<Sprite> ToSprite() {

        Texture2D texture = await CreateTexture();
   
        this.ConvertedSprite = Sprite.Create(texture, new Rect(0, 0, this.data.attributes.width, this.data.attributes.height), new Vector2(0.5f,0.5f));

        return this.ConvertedSprite;
    }

    public async Task<Texture2D> CreateTexture() {

        Texture2D texture = await CommonTasks.GetRemoteTexture(this.data.attributes.GetFullImageUrl());

        return texture;
    }





}