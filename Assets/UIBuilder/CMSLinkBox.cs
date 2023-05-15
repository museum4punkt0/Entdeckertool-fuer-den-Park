using UnityEngine;
using UnityEngine.UIElements;

namespace UIBuilder
{
    public class CMSLinkBox : VisualElement {
        private readonly UIItemViewController _itemViewController;
        private Label _headline;
        private Label _subheadline;
        public Button _linkButton;
        private GroupBox _groupBox;
        private Link link;
        private VisualElement _icon;

        public CMSLinkBox(Link link, UIItemViewController itemViewController) {
            this.link = link;
            this._itemViewController = itemViewController;
            this._headline = new Label(link.headline);
            
            this._subheadline = new Label(link.subheadline);
            this._subheadline.text = link.subheadline;
            this._linkButton = new Button();
            this._linkButton.clicked += HandleButtonClick;
            this._linkButton.text = link.ButtonText;
            this._icon = new VisualElement();

            this._groupBox = new GroupBox();
            this._groupBox.Add(this._headline);
            this._groupBox.Add(this._subheadline);
            this._groupBox.Add(this._linkButton);
            this._groupBox.Add(this._icon);

            this._linkButton.Add(this._icon);
            
            this._groupBox.AddToClassList("cms-groupbox");
            this._headline.AddToClassList("cms-box-headline");

            this._subheadline.AddToClassList("cms-box-sub-headline");
            this._linkButton.AddToClassList("cms-box-link-button");
            this._icon.AddToClassList("cms-box-link-icon");

            switch (link.type) {
                case "spiele":
                    this._groupBox.AddToClassList("spiele-background");
                    break;
                case "funde":
                    this._groupBox.AddToClassList("funde-background");
                    break;
                case "archaologie":
                    this._groupBox.AddToClassList("archaologie-background");
                    break;
                case "touren":
                    this._groupBox.AddToClassList("touren-background");
                    break;
            }



            Add(this._groupBox);

        }

        private void HandleButtonClick() {
            string[] targets = this.link.target.Split(':');
            string entity = targets[0];
            string id = targets[1];
            this._itemViewController.navigate(entity, id);
        }
    }
}   