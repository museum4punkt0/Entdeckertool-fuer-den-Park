using UnityEngine.UIElements;
using UnityEngine;

namespace UIBuilder
{
    public class CMSButton : VisualElement {

        public Button button;
        private VisualElement container;
        private readonly UIItemViewController _itemViewController;
        private OverlayButton item;

        public CMSButton(OverlayButton item, UIItemViewController itemViewController) {
            this._itemViewController = itemViewController;
            this.item = item;
            this.button = new Button();
            this.button.text = item.text;
            this.button.clicked += HandleButtonClick;

            Add(this.button);

            if (item.type == "fund") {
                this.button.AddToClassList("cms-buttonFund");
                this.container = new VisualElement();
                this.button.Add(this.container);
                this.button.text = item.text;
                this.container.AddToClassList("cms-fund-link-icon");
            }
            else {
                this.button.AddToClassList("cms-button");
            }
        }

        private void HandleButtonClick() {
            string[] targets = this.item.target.Split(':');
            string entity = targets[0];
            string id = targets[1];
            Debug.Log(entity+ id);
            this._itemViewController.navigate(entity, id);
        }
    }
}