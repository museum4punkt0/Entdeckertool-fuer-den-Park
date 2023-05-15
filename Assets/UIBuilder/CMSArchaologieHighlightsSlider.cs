using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UIBuilder
{
    public class CMSArchaologieHighlightsSlider : VisualElement {
        private ScrollView _scrollView = new ScrollView();
        private TextElement _textElement = new TextElement();

        public CMSArchaologieHighlightsSlider(List<ArchaologieHighlitghData> highlights, string menuType, UIItemViewController uiItemViewController) {
            this._scrollView.Q<VisualElement>("unity-slider").visible = false;
            this._scrollView.Q<VisualElement>("unity-low-button").visible = false;
            this._scrollView.Q<VisualElement>("unity-high-button").visible = false;
            this._scrollView.Q<VisualElement>("unity-slider").visible = false;
            this._scrollView.Q<VisualElement>("unity-drag-container").visible = false;
            this._scrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;

            this._scrollView.mode = ScrollViewMode.Horizontal;

            foreach (var highlight in highlights) {
                this._scrollView.Add(new CMSArchaologieHighlightsCard(highlight, menuType, uiItemViewController));
            }

            Add(this._scrollView);
            this._scrollView.AddToClassList("cms-menu-highlights-slider");
        }
    }
}