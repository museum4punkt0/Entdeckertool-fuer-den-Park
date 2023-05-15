using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UIBuilder;
using UnityEngine.EventSystems;
using Services;

namespace UIBuilder {
    public class CMSMenuGameSlider : VisualElement {
         Label category = new Label();
         Label counter = new Label();
         VisualElement headerWrapper = new VisualElement();
         ScrollView _scrollView = new ScrollView();
         TextElement _textElement = new TextElement();
         UIItemViewController uIItemViewController;
         CardsSwiperHelper CardSwiperScript;
         int incremment = 0;
        int value = 0;

        private VisualElement _cardArrowWrapper = new VisualElement();
        private VisualElement _arrowWrapper = new VisualElement();
        public Button _arrowLeft = new Button();
        private VisualElement _arrowLeftIcon = new VisualElement();
        public Button _arrowRight = new Button();
        private VisualElement _arrowRightIcon = new VisualElement();

        VisualElement _wrapper = new VisualElement();

        public CMSMenuGameSlider(List<GameMenuCard> _data, string pageTitle,string menuType, UIItemViewController uiItemViewController) {
            // this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);

            uIItemViewController = uiItemViewController.gameObject.GetComponent<UIItemViewController>();
            CardSwiperScript = uiItemViewController.gameObject.GetComponent<CardsSwiperHelper>();

            this._scrollView.mode = ScrollViewMode.Horizontal;
            this._scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;
            this._scrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;

            if (_data != null) {
                foreach (GameMenuCard card in _data) {
                    this._scrollView.Add(new CMSMenuGamesCards(card, menuType, uiItemViewController));

                    if (menuType == "funde") {
                        switch (card.type) {
                            case "funde":
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
                                break;
                            case "welt":
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
                                break;
                            case "touren":
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
                                break;
                            case "ausgrabungen":

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

                                break;
                            case "spiele":

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

                                break;
                        }
                    }
                }
            }

            incremment = 1;

            this._scrollView.RegisterCallback<PointerLeaveEvent>(ev => SnapToChildren(CardSwiperScript, null), TrickleDown.TrickleDown);
            this._scrollView.RegisterCallback<PointerEnterEvent>(ev => UpdateIncremment(null));
            this._scrollView.RegisterCallback<PointerOverEvent>(ev => StopSlider(), TrickleDown.TrickleDown);

            category.text = pageTitle.ToUpper();

            switch (menuType) {
                case "fragen":
                    headerWrapper.AddToClassList("cms-cards-header-wrapper-fragen");
                    category.style.fontSize = 48;
                    this._arrowLeft.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f);
                    this._arrowRight.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f);
                    this._arrowLeft.style.borderBottomColor = new Color(0.3607843f, 0.2156863f, 0.007843138f);
                    this._arrowLeft.style.borderLeftColor = new Color(0.3607843f, 0.2156863f, 0.007843138f);
                    this._arrowLeft.style.borderRightColor = new Color(0.3607843f, 0.2156863f, 0.007843138f);
                    this._arrowLeft.style.borderTopColor = new Color(0.3607843f, 0.2156863f, 0.007843138f);
                    this._arrowLeftIcon.style.unityBackgroundImageTintColor = new Color(0.3607843f, 0.2156863f, 0.007843138f);
                    this._arrowRight.style.borderBottomColor = new Color(0.3607843f, 0.2156863f, 0.007843138f);
                    this._arrowRight.style.borderLeftColor = new Color(0.3607843f, 0.2156863f, 0.007843138f);
                    this._arrowRight.style.borderRightColor = new Color(0.3607843f, 0.2156863f, 0.007843138f);
                    this._arrowRight.style.borderTopColor = new Color(0.3607843f, 0.2156863f, 0.007843138f);
                    this._arrowRightIcon.style.unityBackgroundImageTintColor = new Color(0.3607843f, 0.2156863f, 0.007843138f);
                    //this._arrowLeft.RegisterCallback<ClickEvent>(ev => ChangeArrowsColorinFragen(""));
                    //this._arrowRight.RegisterCallback<ClickEvent>(ev => ChangeArrowsColorinFragen(""));
                    break;
                case "game":
                    headerWrapper.AddToClassList("cms-cards-header-wrapper-spiele");
                    category.style.fontSize = 48;
                    this._arrowLeft.style.unityBackgroundImageTintColor = new Color(0.9921569f, 0.5137255f, 0f);
                    this._arrowRight.style.unityBackgroundImageTintColor = new Color(0.9921569f, 0.5137255f, 0f);
                    break;
                case "touren":
                    headerWrapper.AddToClassList("cms-cards-header-wrapper-touren");
                    category.style.fontSize = 48;
                    this._arrowLeft.style.unityBackgroundImageTintColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                    this._arrowRight.style.unityBackgroundImageTintColor = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                    break;
                case "archaologie":
                    headerWrapper.AddToClassList("cms-cards-header-wrapper-grabungen");
                    category.AddToClassList("cms-arch-headline");
                    this._arrowLeft.style.unityBackgroundImageTintColor = new Color(0.1529412f, 0.1882353f, 1f);
                    this._arrowRight.style.unityBackgroundImageTintColor = new Color(0.1529412f, 0.1882353f, 1f);
                    break;
            }

            
            if (menuType == "archaologie") {
                this._scrollView.style.width = Length.Percent(100);
                this._scrollView.style.height = 1200;
                
            } else {
                this._scrollView.AddToClassList("cms-menu-game-slider");
            }
            
            //this._scrollView.AddToClassList("cms-menu-game-slider");
           

            counter.text = "1" + " / " + this._scrollView.childCount.ToString();
            this._arrowLeft.style.opacity = 0;

            headerWrapper.Add(category);
            headerWrapper.Add(counter);


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


            Add(headerWrapper);

            this._wrapper.Add(this._scrollView);
            this._wrapper.Add(this._arrowWrapper);
            this._wrapper.style.justifyContent = Justify.Center;

            Add(this._wrapper);


            this._arrowLeft.RegisterCallback<ClickEvent>(ev => SnapToChildren(CardSwiperScript, "right"), TrickleDown.TrickleDown);
            this._arrowLeft.RegisterCallback<ClickEvent>(ev => UpdateIncremment("right"));
         
            this._arrowRight.RegisterCallback<ClickEvent>(ev => SnapToChildren(CardSwiperScript, "left"), TrickleDown.TrickleDown);
            this._arrowRight.RegisterCallback<ClickEvent>(ev => UpdateIncremment("left"));
           
        }




        public void UpdateIncremment(string dir) {

            this._arrowRight.style.opacity = 1;
            this._arrowLeft.style.opacity = 1;

            if ((CardSwiperScript.direction == "Left")|| (dir == "left")) {
                incremment = incremment + 1;
            } else if ((CardSwiperScript.direction == "Right")|| (dir == "right")) {
                incremment = incremment - 1;
            }

            if (incremment >= this._scrollView.childCount) {
                incremment = this._scrollView.childCount;
                this._arrowRight.style.opacity = 0;
            }

            if (incremment <= 1) {
                incremment = 1;
                this._arrowLeft.style.opacity = 0;
            }

            //Update Counter
            counter.text = incremment.ToString() + " / " + this._scrollView.childCount.ToString();
        }

       public void SnapToChildren(CardsSwiperHelper CardSwiperScript, string dir) {
           
            
            if ((CardSwiperScript.direction == "Left")|| (dir == "left")) {
                for (int i = 1; i < this._scrollView.childCount; i++) {
                    if (incremment == i) {
                        value = i * 1000;
                    }
                }

            } else if ((CardSwiperScript.direction == "Right") || (dir == "right")) {
                for (int i = 1; i < this._scrollView.childCount; i++) {
                    if (incremment == i) {
                        value = (i * 1000) - 2000;

                    } else if (incremment == this._scrollView.childCount) {
                        Debug.Log("inside last one");
                        value = (incremment * 1000) - 2000;
                    }
                }

            }

            MoveCard();
        }

        public void MoveCard() {
            this._scrollView.horizontalScroller.value = value;
        }

        
        public void StopSlider() {
            this._scrollView.horizontalScroller.valueChanged += (v) => {
                this._scrollView.horizontalScroller.value = value;
            };
        }
        
        void ChangeArrowsColorinFragen(string type) {

                        switch (type) {
                            case "funde":
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
                                break;
                            case "welt":
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
                                break;
                            case "touren":
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
                                break;
                            case "ausgrabungen":

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

                                break;
                            case "spiele":

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

                                break;
                        }

        }

       
    }
}