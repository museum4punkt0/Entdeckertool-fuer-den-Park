using UnityEngine.UIElements;

namespace UIBuilder
{
    public class CMSItemContent : VisualElement {
        public Image Icon;
        public string ItemGuid = "";

        public CMSItemContent() {
            //Create a new Image element and add it to the root
            this.Icon = new Image();
            Add(this.Icon);
            //Add USS style properties to the elements
            this.Icon.AddToClassList("slotIcon");
            AddToClassList("slotContainer");
        }
    }
}