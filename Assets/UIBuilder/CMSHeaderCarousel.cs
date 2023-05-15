using UnityEngine.UIElements;

namespace UIBuilder
{
    public class CMSHeaderCarousel : VisualElement {
        private ScrollView _scrollView;
        public CMSHeaderCarousel() {
            this._scrollView.mode = ScrollViewMode.Horizontal;
            for (int i = 0; i < 10; i++) {
                var image = new VisualElement();
                image.style.backgroundImage = new StyleBackground();
                this._scrollView.Add(image);

            }
            Add(this._scrollView);
            this._scrollView.AddToClassList("cms-header-carousel");
        }
    }
}