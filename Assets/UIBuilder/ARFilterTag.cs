using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Services;
using UIBuilder;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UIBuilder {
    public class ARFilterTag : VisualElement {
        private Button tag;
        private Label labelTag;
        private VisualElement closeIcon;
        public UIItemViewController uIItemViewControllerScript;

        public ARFilterTag(Button btn, string isRadioButton,Button dropdownIcon, VisualElement wrapper, Button filter, Button auswahlAnzeigen, Button allesAnzeigen, VisualElement filterTagsWrapper ,filterMenuView uIItemViewControllerScript) {

            tag = new Button();
            tag.AddToClassList("cms-filter-tag");


            closeIcon = new VisualElement();
            closeIcon.AddToClassList("cms-filter-tag-close-icon");

            labelTag = new Label();
            labelTag.text = btn.text;

            tag.Add(closeIcon);
            tag.Add(labelTag);
            filterTagsWrapper.Add(tag);

            allesAnzeigen.RemoveFromClassList("cms-filter-box-allesAnzeigen-enabled");
            allesAnzeigen.AddToClassList("cms-filter-box-allesAnzeigen-disabled");

            //enable auswahl anzeigen
            auswahlAnzeigen.RemoveFromClassList("cms-filter-header-auswahlAnzeigen-disabled");
            auswahlAnzeigen.AddToClassList("cms-filter-header-auswahlAnzeigen-enabled");

            //edit filter löschen
            filter.text = "filter löschen".ToUpper();
            filter.RegisterCallback<ClickEvent>(ev => uIItemViewControllerScript.gameObject.GetComponent<ARTagFilterController>().FilterLoschen(btn, dropdownIcon, wrapper, filter, filterTagsWrapper, auswahlAnzeigen, allesAnzeigen, uIItemViewControllerScript));

            uIItemViewControllerScript.m_Filter.clicked += delegate {

                if (uIItemViewControllerScript.resetFilter) {
                    uIItemViewControllerScript.gameObject.GetComponent<ARTagFilterController>().FilterLoschen(btn, dropdownIcon, wrapper, filter, filterTagsWrapper, auswahlAnzeigen, allesAnzeigen, uIItemViewControllerScript);

                }
            };

            tag.clicked += delegate {

                Debug.Log("is tag");

                uIItemViewControllerScript.gameObject.GetComponent<ARTagFilterController>().DestroyTag(tag, btn, filter, auswahlAnzeigen, allesAnzeigen, filterTagsWrapper, uIItemViewControllerScript);
                //uIItemViewControllerScript.gameObject.GetComponent<ARTagFilterController>().FilterLoschen(tag, dropdownIcon, wrapper, filter, filterTagsWrapper, auswahlAnzeigen, allesAnzeigen, uIItemViewControllerScript);

            };


        }

    
    }
}