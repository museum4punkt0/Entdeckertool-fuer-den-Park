using UnityEngine.UIElements;

namespace UIBuilder
{
    public class CMSText : VisualElement {
        private Label label;
        public CMSText(string headline) {
            this.label = new Label(headline);
            Add(this.label);
            this.label.AddToClassList("cms-text");
        }
    }
}