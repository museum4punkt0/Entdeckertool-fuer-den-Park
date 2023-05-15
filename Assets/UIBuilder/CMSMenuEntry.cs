using UnityEngine.UIElements;

namespace UIBuilder
{
    public class CMSMenuEntry : VisualElement {
        //private TextElement _textElement = new TextElement();
        //private VisualElement _boxElement  = new VisualElement();
        public Button _button = new Button();
        private VisualElement _icon = new VisualElement();
        private UIItemViewController uiItemViewController;
        private MenuEntry entry;
        private TextElement coins = new TextElement();
        public CMSMenuEntry(MenuEntry entry, int score, UIItemViewController uiItemViewController) {
            this.uiItemViewController = uiItemViewController;
            this.entry = entry;
            //this._textElement.AddToClassList("cms-menu-entry-headline");
            this._icon.AddToClassList("cms-menu-entry-icon");
            this._button.AddToClassList("cms-menu-entry-container");

            switch (entry.type) {
                case "game":
                case "spiele":
                    this._icon.AddToClassList("cms-menu-entry-icon-spiele");
                    break;
                case "fund":
                    this._button.AddToClassList("funde-background");
                    break;
                case "archaologie":
                    this._button.AddToClassList("archaologie-background");
                    this._icon.AddToClassList("cms-menu-entry-icon-arch");
                    break;
                case "touren":
                    this._button.AddToClassList("touren-background");
                    this._icon.AddToClassList("cms-menu-entry-icon-touren");
                    break;
                case "belohnungen":

                    if (score == 0) {
                        this._button.AddToClassList("belohnungen-background");
                        this._icon.AddToClassList("cms-menu-entry-icon-belohnungen");
                    }
                    if (score > 0) {
                        this._button.AddToClassList("belohnungen-background-inverted");
                        this._icon.AddToClassList("cms-menu-entry-icon-belohnungen-inverted");
                    }
                    //this.coins.text = "0";
                    this.coins.text = score.ToString();
                    this.coins.style.paddingLeft = 130;
                    this._icon.Add(this.coins);
                    break;
            }
            
            //this._textElement.text = entry.headline;
            //this._button.text = entry.ButtonText;
            this._button.text = entry.headline.ToUpper();
            this._button.clicked += click;

           //this._boxElement.Add(this._textElement);
           this._button.Add(this._icon);

            //Add(this._boxElement);
            Add(this._button);
        }

        private void click() {
            string[] targets = this.entry.target.Split(":");
            this.uiItemViewController.navigate(targets[0], targets[1]);
        }
    }
}