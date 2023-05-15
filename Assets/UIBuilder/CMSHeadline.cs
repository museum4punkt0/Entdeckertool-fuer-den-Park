using UnityEngine.UIElements;

namespace UIBuilder
{
    public class CMSHeadline : VisualElement {
        private Label label;

        public CMSHeadline(string headline, string type) {

            if (type == "highlights") {
                this.label = new Label(headline.ToUpper());
                Add(this.label);
                this.label.AddToClassList("cms-headline");

                this.label.AddToClassList("headline-highlights");
            }  else if (type == "spiele") {
                this.label = new Label(headline.ToUpper());
                Add(this.label);
                this.label.AddToClassList("cms-headline");
                this.label.AddToClassList("game-headline");

            } else if (type != "highlights") {
                this.label = new Label(headline);
                Add(this.label);
                this.label.AddToClassList("cms-headline");
                
            }





        }

    }

}
