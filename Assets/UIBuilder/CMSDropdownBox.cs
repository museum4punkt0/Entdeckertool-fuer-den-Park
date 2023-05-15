
using UnityEngine;
using UnityEngine.UIElements;
using UIBuilder;
using System.Collections.Generic;
using System.Collections;

namespace UIBuilder
{
    public class CMSDropdownBox : VisualElement {
        private Toggle opt;
        private UIItemViewController UIItemViewControllerScript;

        public CMSDropdownBox(string type, string target, string[] radioButtonsTitles, Button fundeBtn, VisualElement fundeWrapper, Button fundeDropdownIcon, UIItemViewController UIItemViewControllerScript) {
            opt = new Toggle();
            opt.AddToClassList("cms-filter-radiobutton-wrapper");
            opt.text = target;
            opt.Q("unity-checkmark").AddToClassList("cms-filter-radiobutton");
            opt.RegisterCallback<ClickEvent>(ev => CheckBox(type, target, radioButtonsTitles, fundeBtn, fundeWrapper, fundeDropdownIcon, UIItemViewControllerScript));
            Add(opt);
            ToggleLabel();
        }

        public void ToggleLabel() {
            opt.Query<Label>().ForEach((label) => {
                opt.Q<Label>().AddToClassList("toggle-label");
            });
        }

        public void CheckBox(string btn, string text, string[] radioButtonsTitles, Button fundeBtn, VisualElement fundeWrapper, Button fundeDropdownIcon, UIItemViewController UIItemViewControllerScript) {

            UIItemViewControllerScript.gameObject.GetComponent<filterController>().UpdateButtonDropdownUI();
            UIItemViewControllerScript.fundeBoxValue = true;



            if (opt.value) {
                if (btn == "Funde") {
                    opt.AddToClassList("cms-filter-radiobutton-funde-selected");
                    opt.Q("unity-checkmark").AddToClassList("cms-filter-radiobutton-checkmark-funde-selected");
                    Button filterTag = new Button();
                    filterTag.text = "Funde";
                    string radioButtonType = "funde-radio-button";


                    //removes the tag if already exists
                    UIItemViewControllerScript.filterTagsWrapper.Query<Label>().ForEach((label) => {
                        if (label.text == "Funde") {
                            UIItemViewControllerScript.filterTagsWrapper.Remove(label.parent);
                        }
                    });

                    CMSFilterTag tag = new CMSFilterTag(UIItemViewControllerScript.funde, radioButtonType, UIItemViewControllerScript.fundeDropdownIcon, UIItemViewControllerScript.fundeWrapper, UIItemViewControllerScript.filter, UIItemViewControllerScript.auswahlAnzeigen, UIItemViewControllerScript.allesAnzeigen, UIItemViewControllerScript.filterTagsWrapper, UIItemViewControllerScript);

                    if (opt.text == "Alle Funde") {
                        UIItemViewControllerScript.gameObject.GetComponent<filterController>().loadPoi("fundObjekt");

                        for (int i = 1; i < UIItemViewControllerScript.fundeDropdown.childCount; i++) {
                            UIItemViewControllerScript.fundeDropdown[i].Q<Toggle>().value = false;
                            UIItemViewControllerScript.fundeDropdown[i].Q<Toggle>().RemoveFromClassList("cms-filter-radiobutton-funde-selected");
                            UIItemViewControllerScript.fundeDropdown[i].Q<Toggle>().Q("unity-checkmark").RemoveFromClassList("cms-filter-radiobutton-checkmark-funde-selected");
                        }

                    } else {
                        if (UIItemViewControllerScript.fundeDropdown[0].Q<Toggle>().value) {
                            UIItemViewControllerScript.fundeDropdown[0].Q<Toggle>().value = false;
                            UIItemViewControllerScript.fundeDropdown[0].Q<Toggle>().RemoveFromClassList("cms-filter-radiobutton-funde-selected");
                            UIItemViewControllerScript.fundeDropdown[0].Q<Toggle>().Q("unity-checkmark").RemoveFromClassList("cms-filter-radiobutton-checkmark-funde-selected");
                            UIItemViewControllerScript.gameObject.GetComponent<filterController>().DisactivatePin("fundObjekt");
                        }

                        UIItemViewControllerScript.gameObject.GetComponent<filterController>().LoadRadioButton(opt.text, "fundObjekt");
                    }

                } else if (btn == "Grabungen") {
                    opt.AddToClassList("cms-filter-radiobutton-grabungen-selected");
                    opt.Q("unity-checkmark").parent.AddToClassList("cms-filter-radiobutton-background-grabungen-selected");
                    opt.Q("unity-checkmark").AddToClassList("cms-filter-radiobutton-checkmark-grabungen-selected");
                }


            }
            else if (!opt.value) {
                if (btn == "Funde") {
                    opt.RemoveFromClassList("cms-filter-radiobutton-funde-selected");
                    opt.Q("unity-checkmark").RemoveFromClassList("cms-filter-radiobutton-checkmark-funde-selected");

                    string radioButtonType = "funde-radio-button";
                    UIItemViewControllerScript.gameObject.GetComponent<filterController>().DisableCheckBox(opt.text);

                    int checkboxes = 0;

                    for (int i = 0; i < UIItemViewControllerScript.fundeDropdown.childCount; i++) {
                        UIItemViewControllerScript.fundeDropdown.Query<Toggle>().ForEach((checkbox) => {
                            if (!checkbox.value) {
                                checkboxes += 1;
                            }
                        });

                        if (checkboxes == UIItemViewControllerScript.fundeDropdown.childCount) {

                            for (int j = 0; j < UIItemViewControllerScript.filterTagsWrapper.childCount; j++) {
                                if (UIItemViewControllerScript.filterTagsWrapper[j].Q<Button>().Q<Label>().text == "Funde") {
                                    UIItemViewControllerScript.gameObject.GetComponent<filterController>().DestroyTagWithDropdown(UIItemViewControllerScript.filterTagsWrapper[j].Q<Button>(), fundeBtn, fundeDropdownIcon, UIItemViewControllerScript.fundeWrapper, UIItemViewControllerScript.filter, UIItemViewControllerScript.auswahlAnzeigen, UIItemViewControllerScript.allesAnzeigen, UIItemViewControllerScript.filterTagsWrapper, UIItemViewControllerScript);

                                }
                            }

                        }

                    }



                }
            }


        }

    }
}