using UnityEngine;
using UnityEngine.UIElements;

namespace UIBuilder
{
    public class CMSImage : CMSMediaItem {
        private VisualElement _imageBox;
        public CMSImage(string imgUrl) {
            this._imageBox = new VisualElement();
            //this._imageBox.style.backgroundImage = new StyleBackground(imageTexture);
            Davinci.get().load(imgUrl).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(_imageBox).start();
            this._imageBox.style.height = new StyleLength(100);
            this._imageBox.style.width = new StyleLength(200);
            this._imageBox.AddToClassList("cms-image");
            this.AddToClassList("cms-image-wrapper");
            this.Add(this._imageBox);
        }
    }
}