using UnityEngine.UIElements;

namespace UIBuilder
{
    public class CMSMenuEntries : VisualElement {
        private VisualElement _container = new VisualElement();
       // private VisualElement _icon = new VisualElement();
        public CMSMenuEntries(MenuEntry[] entries, int score,UIItemViewController uiItemViewController) {

            foreach (MenuEntry menuEntry in entries) {
                CMSMenuEntry m = new CMSMenuEntry(menuEntry, score, uiItemViewController);
                this._container.Add(m); 
            }

            Add(this._container);
            //this._container.AddToClassList("cms-menu-entries-container");
            //this._icon.AddToClassList("cms-menu-entries-container-icon");
        }
   
    }
}