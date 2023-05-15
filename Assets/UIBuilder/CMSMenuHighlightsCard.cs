using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIBuilder
{
    public class CMSMenuHighlightsCard : VisualElement {
        private TextElement _textElement = new TextElement();
        private Button _linkTextElement = new Button();
        private Button _boxElement  = new Button();
        private UIItemViewController uiItemViewController;
        public CMSMenuHighlightsCard(HighlightDataAttributes highlight, string menuType, UIItemViewController uiItemViewController) {
            this.uiItemViewController = uiItemViewController;
            this._textElement.AddToClassList("cms-menu-highlights-box-headline");
            this._linkTextElement.AddToClassList("cms-menu-highlights-box-link");

           
            this._boxElement.AddToClassList("cms-menu-highlights-box");

            this._boxElement.clicked += delegate {
                string[] targets = highlight.target.Split(":");
                this.uiItemViewController.navigate(targets[0], targets[1]);
            };

            this._textElement.text = highlight.headline;
            this._linkTextElement.text = "Zum Fund".ToUpper();
            this._boxElement.Add(this._textElement);
            this._boxElement.Add(this._linkTextElement);
            Add(this._boxElement);
        }
    }
}