using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIBuilder {
    public class CMSMenuGamesCards : VisualElement {
        private UIItemViewController uiItemViewController;
        private TextElement _textElement = new TextElement();
        private TextElement _subTextElement = new TextElement();
        private Button _buttonElement = new Button();
        private VisualElement _btnIcon = new VisualElement();
        private VisualElement _boxElement = new VisualElement();
        private VisualElement _icon = new VisualElement();

        private VisualElement _footer = new VisualElement();
        private VisualElement _counter = new VisualElement();
        private VisualElement _counterIcon = new VisualElement();
        private TextElement _counterText = new TextElement();
        private VisualElement _funde = new VisualElement();
        private VisualElement _timer = new VisualElement();
        private VisualElement _timerWrapper = new VisualElement();
        private TextElement _timerText = new TextElement();
        private VisualElement _fundeWrapper = new VisualElement();
        private TextElement _fundeText = new TextElement();

        private VisualElement _cardArrowWrapper = new VisualElement();
        private VisualElement _arrowWrapper = new VisualElement();
        public Button _arrowLeft = new Button();
        private VisualElement _arrowLeftIcon = new VisualElement();
        public Button _arrowRight = new Button();
        private VisualElement _arrowRightIcon = new VisualElement();

        VisualElement _wrapper = new VisualElement();

        public CMSMenuGamesCards(GameMenuCard card, string menuType , UIItemViewController uiItemViewController) {

            this._textElement.AddToClassList("cms-menu-game-headline");
            this._textElement.text = card.title.ToUpper();

            this._subTextElement.text = card.subtitle;
            this._subTextElement.AddToClassList("cms-menu-game-subtitle");

            this._buttonElement.AddToClassList("cms-menu-game-button");

            this._buttonElement.clicked += delegate {
                string[] targets = card.target.Split(":");
                uiItemViewController.navigate(targets[0], targets[1]);
            };
           
            this._btnIcon.AddToClassList("cms-menu-game-button-icon");
            this._buttonElement.Add(this._btnIcon);

            if (menuType == "archaologie") {
                this._icon.AddToClassList("cms-menu-arch-icon");
                this._boxElement.AddToClassList("cms-menu-arch-cards");
                this._buttonElement.AddToClassList("cms-menu-arch-button");
                this._buttonElement.text = "Steckbrief der Ausgrabung";
                this._arrowLeft.style.unityBackgroundImageTintColor = new Color(0.1529412f, 0.1882353f, 1f);
                this._arrowRight.style.unityBackgroundImageTintColor = new Color(0.1529412f, 0.1882353f, 1f);
            } else if (menuType == "game") {
                this._icon.AddToClassList("cms-menu-game-icon");
                this._boxElement.AddToClassList("cms-menu-game-cards");
                this._buttonElement.text = "Spiel starten";
                this._arrowLeft.style.unityBackgroundImageTintColor = new Color(0.9921569f, 0.5137255f, 0f);
                this._arrowRight.style.unityBackgroundImageTintColor = new Color(0.9921569f, 0.5137255f, 0f);
            } else if (menuType == "touren") {
                this._icon.AddToClassList("cms-menu-touren-icon");
                this._boxElement.AddToClassList("cms-menu-touren-cards");
                this._buttonElement.text = "Los geht's!";
                this._arrowLeft.style.unityBackgroundImageTintColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                this._arrowRight.style.unityBackgroundImageTintColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
            } else if (menuType == "fragen") {
                this._icon.style.display = DisplayStyle.None; 
                this._subTextElement.style.display = DisplayStyle.None;
                this._boxElement.AddToClassList("cms-menu-fragen-cards");
                this._buttonElement.text = card.subtitle;
                this._textElement.AddToClassList("cms-menu-fragen-headline");

                switch (card.type) {
                    case "funde":
                        this._boxElement.style.borderBottomColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._boxElement.style.borderLeftColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._boxElement.style.borderRightColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._boxElement.style.borderTopColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._arrowLeft.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f);
                        this._arrowLeft.style.borderBottomColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._arrowLeft.style.borderLeftColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._arrowLeft.style.borderRightColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._arrowLeft.style.borderTopColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._arrowLeftIcon.style.unityBackgroundImageTintColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._arrowRight.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f);
                        this._arrowRight.style.borderBottomColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._arrowRight.style.borderLeftColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._arrowRight.style.borderRightColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._arrowRight.style.borderTopColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._arrowRightIcon.style.unityBackgroundImageTintColor = new Color(0.1098039f, 0.7019608f, 1f);
                        this._textElement.AddToClassList("funde-text");
                        this._buttonElement.AddToClassList("funde-text");
                        this._buttonElement.AddToClassList("cms-menu-fragen-button-funde");
                        this._btnIcon.AddToClassList("cms-menu-fragen-button-icon-funde");
                        break;
                    case "welt":
                        this._boxElement.style.borderBottomColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._boxElement.style.borderLeftColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._boxElement.style.borderRightColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._boxElement.style.borderTopColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._arrowLeft.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f);
                        this._arrowRight.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f);
                        this._arrowLeft.style.borderBottomColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._arrowLeft.style.borderLeftColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._arrowLeft.style.borderRightColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._arrowLeft.style.borderTopColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._arrowLeftIcon.style.unityBackgroundImageTintColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._arrowRight.style.borderBottomColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._arrowRight.style.borderLeftColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._arrowRight.style.borderRightColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._arrowRight.style.borderTopColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                        this._arrowRightIcon.style.unityBackgroundImageTintColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);

                        this._textElement.AddToClassList("welt-text");
                        this._buttonElement.AddToClassList("welt-text");
                        this._buttonElement.AddToClassList("cms-menu-fragen-button-welt");
                        this._btnIcon.AddToClassList("cms-menu-fragen-button-icon-welt");
                        break;
                    case "touren":
                        this._boxElement.style.borderBottomColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._boxElement.style.borderLeftColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._boxElement.style.borderRightColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._boxElement.style.borderTopColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._arrowLeft.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f);
                        this._arrowRight.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f);
                        this._arrowLeft.style.borderBottomColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._arrowLeft.style.borderLeftColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._arrowLeft.style.borderRightColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._arrowLeft.style.borderTopColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._arrowLeftIcon.style.unityBackgroundImageTintColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._arrowRight.style.borderBottomColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._arrowRight.style.borderLeftColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._arrowRight.style.borderRightColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._arrowRight.style.borderTopColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._arrowRightIcon.style.unityBackgroundImageTintColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                        this._textElement.AddToClassList("touren-text");
                        this._buttonElement.AddToClassList("touren-text");
                        this._buttonElement.AddToClassList("cms-menu-fragen-button-touren");
                        this._btnIcon.AddToClassList("cms-menu-fragen-button-icon-touren");
                        break;
                    case "ausgrabungen":
                        this._boxElement.style.borderBottomColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._boxElement.style.borderLeftColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._boxElement.style.borderRightColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._boxElement.style.borderTopColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._arrowLeft.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f);
                        this._arrowRight.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f);
                        this._arrowLeft.style.borderBottomColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._arrowLeft.style.borderLeftColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._arrowLeft.style.borderRightColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._arrowLeft.style.borderTopColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._arrowLeftIcon.style.unityBackgroundImageTintColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._arrowRight.style.borderBottomColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._arrowRight.style.borderLeftColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._arrowRight.style.borderRightColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._arrowRight.style.borderTopColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._arrowRightIcon.style.unityBackgroundImageTintColor = new Color(0.1529412f, 0.1882353f, 1f);
                        this._textElement.AddToClassList("ausgrabungen-text");
                        this._buttonElement.AddToClassList("ausgrabungen-text");
                        this._buttonElement.AddToClassList("cms-menu-fragen-button-ausgrabungen");
                        this._btnIcon.AddToClassList("cms-menu-fragen-button-icon-ausgrabungen");
                        break;
                    case "spiele":
                        this._boxElement.style.borderBottomColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._boxElement.style.borderLeftColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._boxElement.style.borderRightColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._boxElement.style.borderTopColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._arrowLeft.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f);
                        this._arrowRight.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f);
                        this._arrowLeft.style.borderBottomColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._arrowLeft.style.borderLeftColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._arrowLeft.style.borderRightColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._arrowLeft.style.borderTopColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._arrowLeftIcon.style.unityBackgroundImageTintColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._arrowRight.style.borderBottomColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._arrowRight.style.borderLeftColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._arrowRight.style.borderRightColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._arrowRight.style.borderTopColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._arrowRightIcon.style.unityBackgroundImageTintColor = new Color(0.9921569f, 0.5137255f, 0f);
                        this._textElement.AddToClassList("spiele-text");
                        this._buttonElement.AddToClassList("spiele-text");
                        this._buttonElement.AddToClassList("cms-menu-fragen-button-spiele");
                        this._btnIcon.AddToClassList("cms-menu-fragen-button-icon-spiele");
                        break;
                }

                this._buttonElement.clicked += delegate {
                    uiItemViewController.setEntedeckerFrageTitle(card.title, card.type);
                };

            }

            this._footer.AddToClassList("cms-coincounter-footer");

            this._counter.AddToClassList("cms-menu-game-coincounter-box");
            this._counterIcon.AddToClassList("cms-menu-game-coincounter-icon");
            this._counterText.AddToClassList("");
            this._counterText.text = card.reward.ToString();

            this._counter.Add(this._counterIcon);
            this._counter.Add(this._counterText);

            if (menuType == "touren") {
                this._funde.AddToClassList("cms-touren-footer-funde");
                this._fundeText.text = card.tourenFundAmount.ToString().ToUpper() + " FUNDE";
                this._fundeWrapper.AddToClassList("cms-menu-game-coincounter-box");
                this._fundeWrapper.Add(_funde);
                this._fundeWrapper.Add(_fundeText);
                
                this._timer.AddToClassList("cms-touren-footer-timer");
                this._timerText.text = card.tourenTime.ToUpper();
                this._timerWrapper.AddToClassList("cms-menu-game-coincounter-box");
                this._fundeWrapper.Add(_timer);
                this._fundeWrapper.Add(_timerText);

                if (card.target == "touren:1") {
                    this._fundeText.text = card.tourenFundAmount.ToString().ToUpper() + " FUNDE";
                    this._timerText.text = card.tourenTime.ToUpper();
                }
            }
            
            
            this._boxElement.Add(this._icon);
            this._boxElement.Add(this._textElement);
            this._boxElement.Add(this._subTextElement);
            this._boxElement.Add(this._buttonElement);

            if (menuType == "game")  {
                this._boxElement.Add(this._counter);
            } else if (menuType == "touren") {
                this._footer.Add(this._fundeWrapper);
                this._footer.Add(this._timerWrapper);
                this._footer.Add(this._counter);
                this._boxElement.Add(this._footer);
            }

            this._arrowWrapper.AddToClassList("cms-menu-cards-arrow-wrapper");
            this._arrowLeft.AddToClassList("cms-menu-cards-arrow-icon-rounded");
            this._arrowLeft.Add(_arrowLeftIcon);
            this._arrowLeftIcon.AddToClassList("cms-menu-cards-arrow-icon");
            this._arrowRight.AddToClassList("cms-menu-cards-arrow-icon-rounded");
            this._arrowRight.Add(_arrowRightIcon);
            this._arrowRightIcon.AddToClassList("cms-menu-cards-arrow-icon");
            this._arrowRightIcon.style.rotate = new Rotate(180);
            this._arrowLeft.style.paddingRight = 25;
            this._arrowRight.style.paddingLeft = 25;
            this._arrowWrapper.Add(this._arrowLeft);
            this._arrowWrapper.Add(this._arrowRight);

            this._wrapper.AddToClassList("cms-cards-arrow-wrapper");
            this._wrapper.Add(this._boxElement);
            this._wrapper.Add(this._arrowWrapper);


           Add(this._boxElement);
           // Add(this._wrapper);


            this._arrowLeft.clicked += delegate {
                Debug.Log("arrow left click");
                uiItemViewController.gameObject.GetComponent<CardsSwiperHelper>().direction = "left";
            };
        }


    }
}