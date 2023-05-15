using UnityEngine.UIElements;

namespace UIBuilder {
    public class CMSDivisionBar: VisualElement {

        private VisualElement bar;

        public CMSDivisionBar(string barType) {
            this.bar = new VisualElement();
            Add(this.bar);
            this.bar.AddToClassList("cms-DivisionBar");

            if (barType != "thickBar") {
                this.bar.style.height = 10;
            }

            
        }
    }
}