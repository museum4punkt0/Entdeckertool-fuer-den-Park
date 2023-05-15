using UnityEngine.UIElements;

namespace UIBuilder
{
    public class CMSGame1Content : VisualElement {

        public TextElement headline;
        public CMSGame1Content(Game1DataContent content) {
            headline.text = content.level;
            Add(headline);
        }
        
        
    }
}