using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIBuilder {
    public class CMSListeButton : VisualElement {
        Button btn = new Button();
        VisualElement labelWrapper = new VisualElement();
        Label headline;
        Label subHeadline;
        VisualElement icon = new VisualElement();

        
        private UIItemViewController uIItemViewControllerScript;

        public CMSListeButton(string type, string target_type,string target, UIItemViewController uIItemViewControllerScript) {

            headline = new Label();
            headline.name = "headline";
            subHeadline = new Label();
            subHeadline.name = "subheadline";

            switch (type) {
                case "spiel":
                    btn.AddToClassList("cms-liste-button-spiele");
                    icon.AddToClassList("cms-liste-button-icon-spiele");

                    break;
                case "fund":
                    btn.AddToClassList("cms-liste-button-fund");
                    icon.AddToClassList("cms-liste-button-icon-fund");

                    break;
                case "touren":
                    btn.AddToClassList("cms-liste-button-touren");
                    icon.AddToClassList("cms-liste-button-icon-touren");

                    break;
                case "panorama":
                    btn.AddToClassList("cms-liste-button-panorama");
                    icon.AddToClassList("cms-liste-button-icon-panorama");

                    break;
                case "ausgrabung":
                    btn.AddToClassList("cms-liste-button-ausgrabung");
                    icon.AddToClassList("cms-liste-button-icon-ausgrabung");

                    break;
            }

            headline.AddToClassList("cms-liste-button-headline");
            subHeadline.AddToClassList("cms-liste-button-subheadline");
           // labelWrapper.AddToClassList("cms-liste-button-labelwrapper");

            labelWrapper.Add(headline);
            labelWrapper.Add(subHeadline);
            btn.Add(labelWrapper);
            btn.Add(icon);

            Add(btn);

            if (target_type == "spiel") {
                btn.clicked += delegate {
                    string[] targets = target.Split(":");
                    uIItemViewControllerScript.navigate(targets[0], targets[1]);
                };
            } else {
                btn.clicked += delegate {
                    uIItemViewControllerScript.m_PanelListe.style.display = DisplayStyle.None;
                    uIItemViewControllerScript.ListeClickBlocker.gameObject.SetActive(false);
                    uIItemViewControllerScript.isListePanelOpen = false;
                    //uIItemViewControllerScript.SetIconState("liste", "innactive");
                    uIItemViewControllerScript.gameObject.GetComponent<listeController>().ShowPOI(target);
                };

            }

        }

     }
}