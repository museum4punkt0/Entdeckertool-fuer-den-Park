using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;


namespace UIBuilder
{
    public class CMSAccordionItem : VisualElement {
        private Foldout _foldout;
        private TextElement _text;
        private VisualElement icon;
        private VisualElement wrapper;
         

        public CMSAccordionItem(AccordionItem item, UIItemViewController UIItemViewControllerScript) {

            this._foldout = new Foldout();
            this.icon = new VisualElement();
            this.wrapper = new VisualElement();

            this.wrapper.Add(this.icon);
            this.wrapper.AddToClassList("cms-accordionWrapper");

            this._foldout.text = item.headline;
            this.icon.AddToClassList("cms-accordionIconClose");


            this._text = new TextElement();
            this._text.text = item.body;
            this._foldout.contentContainer.Add(this._text);
            this._foldout.value = false;

            this._foldout.Q<Label>().style.whiteSpace = WhiteSpace.Normal;
            this._foldout.Q<Label>().style.width = Length.Percent(70);

            Add(this.wrapper);
            Add(this._foldout);
            
            this._foldout.AddToClassList("cms-accordion");
            this._foldout.Q<VisualElement>("unity-checkmark").visible = false;

            this._foldout?.RegisterCallback<ClickEvent>(ev => ChangeIcon());

            

            foreach (VideoItem videoElement in item.video)
                this._foldout.contentContainer.Add(new CMSVideo(videoElement, UIItemViewControllerScript));
           

        }
        
        

        public void ChangeIcon() {
            if (this._foldout.value == false) {
                this.icon.RemoveFromClassList("cms-accordionIconOpen");
                this.icon.AddToClassList("cms-accordionIconClose");
            } else if(this._foldout.value == true) {
                this.icon.RemoveFromClassList("cms-accordionIconClose");
                this.icon.AddToClassList("cms-accordionIconOpen");
            }

        }
    }
}