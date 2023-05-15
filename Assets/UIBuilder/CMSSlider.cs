using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine;

using System.Collections;
using System;


namespace UIBuilder
{
    public class CMSSlider : VisualElement {
        private ScrollView _scrollView;
        private VisualElement _wrapper;
        private Button _zoomIcon;
        private TextElement _counter;
        private int currentImg;


        public CMSSlider(CMSMediaItem[] items, VisualElement scrollContent, UIItemViewController uIItemViewController) {
            this._scrollView = new ScrollView();
            this._wrapper = new VisualElement();
            this._zoomIcon = new Button();
            this._counter = new TextElement();

            

            this._scrollView.mode = ScrollViewMode.Horizontal;
            this._scrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            this._scrollView.AddToClassList("cms-slider");

            foreach (var item in items) {
                this._scrollView.Add(item);
                this._counter.text = "1" + "/" + items.Length.ToString();
            }

            this._counter.AddToClassList("cms-slider-counter");
            this.AddToClassList("cms-slider-wrapper");
            this._zoomIcon.AddToClassList("cms-slider-zoomIcon");
            
            this._zoomIcon.clicked += () => {
                Debug.Log("inside zoom");
                uIItemViewController.Zoom.SetActive(true);
                uIItemViewController.zoomController.LoadImage_static();
            };


            this._wrapper.style.flexDirection = FlexDirection.Row;
            this._wrapper.style.justifyContent = Justify.SpaceBetween;
            this._wrapper.style.alignItems = Align.Center;

            
            Add( this._scrollView);
            Add(this._wrapper);
            this._wrapper.Add(this._counter);
            this._wrapper.Add(this._zoomIcon);


            UpdateCounter(uIItemViewController);
        }

        public void UpdateCounter(UIItemViewController uIItemViewController) {

            int totalItems = this._scrollView.childCount;
            this._scrollView.horizontalScroller.valueChanged += (v) => {

                for (int i = 0; i < this._scrollView.childCount; i++) {
                    if ((int)this._scrollView.horizontalScroller.value > ((Screen.width * i) - (100 * i))) {
                        currentImg = i;
                        this._counter.text = (i + 1).ToString() + "/" + totalItems.ToString();
                        uIItemViewController.currentImageonSlider = currentImg;
                    }

                }

            };
        }
    }
}