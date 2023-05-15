using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Services;
using UIBuilder;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;

public class filterController : MonoBehaviour
{
    CrossGameManager crossGameManager;
    UIItemViewController uIItemViewControllerScript;
    int counter = 0;
    //GameObject crossGameManager;

    // Start is called before the first frame update
    void Start()
    {
        uIItemViewControllerScript = this.gameObject.GetComponent<UIItemViewController>();
        StartCoroutine(loadPOIS());
    }

    #region POIs

    public void loadPoi(string type) {
        for (int i = 0; i < crossGameManager.AllItemsOnMap.Count; i++) {
            if (counter == 0) {
                crossGameManager.AllItemsOnMap[i].Pin.SetActive(false);

                if (crossGameManager.AllItemsOnMap[i].Poi.attributes.type == type) {                 
                    crossGameManager.AllItemsOnMap[i].Pin.SetActive(true);
                }

            } else if (counter > 0) {
                if (crossGameManager.AllItemsOnMap[i].Poi.attributes.type == type) {
                    crossGameManager.AllItemsOnMap[i].Pin.SetActive(true);
                }
            }
        }

        counter++;
    }

    public void ActivateAllPins() {
        for (int i = 0; i < crossGameManager.AllItemsOnMap.Count; i++) {
            crossGameManager.AllItemsOnMap[i].Pin.SetActive(true);
        }

        this.gameObject.GetComponent<UIItemViewController>().m_BGFilterListe.Query<RadioButton>().ForEach((radioButton) =>
        {
            radioButton.value = false;
            radioButton.AddToClassList("cms-filter-radiobutton-disabled");
        });

        counter = 0;
    }

    public void DisctivateAllPins() {
        for (int i = 0; i < crossGameManager.AllItemsOnMap.Count; i++) {
            crossGameManager.AllItemsOnMap[i].Pin.SetActive(false);
        }

        counter = 0;
    }

    public void DisactivatePin(string type) {

        for (int i = 0; i < crossGameManager.AllItemsOnMap.Count; i++) {
            if (crossGameManager.AllItemsOnMap[i].Poi.attributes.type == type) {
                crossGameManager.AllItemsOnMap[i].Pin.SetActive(false);
            }
        }
    }

    public void LoadDropdownFund() {
        string type = "";
        string target = "";
        List<string> targetGroup = new List<string>();
        string[] deleteDuplicates;
        target = "";

        for (int i = 0; i < crossGameManager.AllItemsOnMap.Count; i++) {
            if (crossGameManager.AllItemsOnMap[i].Poi.attributes.fundobjekt.data.attributes.category != null) {
                type = "Funde";

                //add category titles to list
                targetGroup.Add(crossGameManager.AllItemsOnMap[i].Poi.attributes.fundobjekt.data.attributes.category.ToString());
            }
        }

        deleteDuplicates = targetGroup.Distinct().ToArray();

        CMSDropdownBox allItems = new CMSDropdownBox(type, "Alle Funde", deleteDuplicates, this.gameObject.GetComponent<UIItemViewController>().funde, this.gameObject.GetComponent<UIItemViewController>().fundeWrapper, this.gameObject.GetComponent<UIItemViewController>().fundeDropdownIcon, this.gameObject.GetComponent<UIItemViewController>());
        this.gameObject.GetComponent<UIItemViewController>().m_BGFilterListe.Q<GroupBox>("fundeDropdown").Add(allItems);


        //creates radio button
        for (int j = 0; j < deleteDuplicates.Length; j++) {

            target = deleteDuplicates[j].ToString();

            CMSDropdownBox opt = new CMSDropdownBox(type, target, deleteDuplicates, this.gameObject.GetComponent<UIItemViewController>().funde, this.gameObject.GetComponent<UIItemViewController>().fundeWrapper, this.gameObject.GetComponent<UIItemViewController>().fundeDropdownIcon, this.gameObject.GetComponent<UIItemViewController>());
            this.gameObject.GetComponent<UIItemViewController>().m_BGFilterListe.Q<GroupBox>("fundeDropdown").Add(opt);

        }

    }

    public void LoadRadioButton(string title, string type) {

        for (int i = 0; i < crossGameManager.AllItemsOnMap.Count; i++) {
            if (counter == 0) {
                crossGameManager.AllItemsOnMap[i].Pin.SetActive(false);

                if (crossGameManager.AllItemsOnMap[i].Poi.attributes.fundobjekt.data.attributes.category == title && crossGameManager.AllItemsOnMap[i].Poi.attributes.type == type) {
                    crossGameManager.AllItemsOnMap[i].Pin.SetActive(true);
                }

            } else if (counter > 0) {
                if (crossGameManager.AllItemsOnMap[i].Poi.attributes.fundobjekt.data.attributes.category == title && crossGameManager.AllItemsOnMap[i].Poi.attributes.type == type) {
                    crossGameManager.AllItemsOnMap[i].Pin.SetActive(true);
                }
            }
        }

        counter++;
        
    }

    public void DisableCheckBox(string title) {

        for (int i = 0; i < crossGameManager.AllItemsOnMap.Count; i++) {
            if (crossGameManager.AllItemsOnMap[i].Poi.attributes.fundobjekt.data.attributes.category != null) {
                //crossGameManager.AllItemsOnMap[i].Pin.SetActive(false);

                if (crossGameManager.AllItemsOnMap[i].Poi.attributes.fundobjekt.data.attributes.category == title) {
                    crossGameManager.AllItemsOnMap[i].Pin.SetActive(false);
                } else if (crossGameManager.AllItemsOnMap[i].Poi.attributes.fundobjekt.data.attributes.category != title) {
                   // crossGameManager.AllItemsOnMap[i].Pin.SetActive(false);

                }
            }
        }

        counter++;
    }

    #endregion

    #region FilterUI

    public void DestroyTag(Button tag, Button btn, Button filter, Button auswahlAnzeigen, Button allesAnzeigen, VisualElement filterTagsWrapper, UIItemViewController uIItemViewControllerScript) {
        filterTagsWrapper.Remove(tag);
        string type = "";

        switch (btn.text) {
            case "Panorama-Ansichten":
                btn.RemoveFromClassList("cms-filter-box-panorama-selected");
                btn.AddToClassList("cms-filter-box-panorama");
                type = "panorama";
                break;
            case "Spiele":
                btn.RemoveFromClassList("cms-filter-box-spiele-selected");
                btn.AddToClassList("cms-filter-box-spiele");
                type = "spiel";
                break;
            case "Park-Touren":
                btn.RemoveFromClassList("cms-filter-box-touren-selected");
                btn.AddToClassList("cms-filter-box-touren");
                type = "tour";
                break;
            case "Landschaft":
                btn.RemoveFromClassList("cms-filter-box-landschaft-selected");
                btn.AddToClassList("cms-filter-box-landschaft");
                break;
        }


        btn.SetEnabled(true);
        uIItemViewControllerScript.gameObject.GetComponent<filterController>().DisactivatePin(type);
        CountTags(filterTagsWrapper, filter, auswahlAnzeigen, allesAnzeigen, uIItemViewControllerScript);

    }

    public void DestroyTagWithDropdown(Button tag, Button btn, Button dropdownIcon, VisualElement wrapper, Button filter,Button auswahlAnzeigen, Button allesAnzeigen, VisualElement filterTagsWrapper, UIItemViewController uIItemViewControllerScript) {

        filterTagsWrapper.Remove(tag);
        string type = "";

        if (btn.text == "Funde") {

            btn.RemoveFromClassList("cms-filter-box-funde-button-selected");
            btn.AddToClassList("cms-filter-box-funde-button");
            wrapper.RemoveFromClassList("cms-filter-box-funde-selected");
            wrapper.AddToClassList("cms-filter-box-funde");

            if (uIItemViewControllerScript.fundeDropdownValue) {
                uIItemViewControllerScript.fundeDropdownIcon.RemoveFromClassList("cms-filter-box-funde-dropdown-icon-opened-selected");
                uIItemViewControllerScript.fundeDropdownIcon.AddToClassList("cms-filter-box-funde-dropdown-icon-opened");

            } else if (!uIItemViewControllerScript.fundeDropdownValue) {
                uIItemViewControllerScript.fundeDropdownIcon.RemoveFromClassList("cms-filter-box-funde-dropdown-icon-closed-selected");

                uIItemViewControllerScript.fundeDropdownIcon.AddToClassList("cms-filter-box-funde-dropdown-icon-closed");

            }
            
            uIItemViewControllerScript.fundeBoxValue = false;

            uIItemViewControllerScript.fundeDropdown.Query<Toggle>().ForEach((checkbox) => {
                checkbox.value = false;
                checkbox.RemoveFromClassList("cms-filter-radiobutton-funde-selected");
                checkbox.Q("unity-checkmark").RemoveFromClassList("cms-filter-radiobutton-checkmark-funde-selected");
            });

            type = "fundObjekt";

        } else if (btn.text == "Grabungen") {
            btn.RemoveFromClassList("cms-filter-box-grabungen-button-selected");
            btn.AddToClassList("cms-filter-box-grabungen-button");
            wrapper.RemoveFromClassList("cms-filter-box-grabungen-selected");
            wrapper.AddToClassList("cms-filter-box-grabungen");
            dropdownIcon.RemoveFromClassList("cms-filter-box-grabungen-dropdown-icon-closed-selected");
            dropdownIcon.AddToClassList("cms-filter-box-grabungen-dropdown-icon-closed");
            uIItemViewControllerScript.grabungenDropdownValue = true;
            uIItemViewControllerScript.grabungenBoxValue = false;

            type = "grabung";
        }

        btn.SetEnabled(true);
        uIItemViewControllerScript.gameObject.GetComponent<filterController>().DisactivatePin(type);
        CountTags(filterTagsWrapper, filter, auswahlAnzeigen, allesAnzeigen, uIItemViewControllerScript);
    }

    public void CountTags(VisualElement filterTagsWrapper, Button filter, Button auswahlAnzeigen, Button allesAnzeigen, UIItemViewController uIItemViewControllerScript) {

        if (filterTagsWrapper.childCount > 0) {
            filter.text = uIItemViewControllerScript.filterConfig_delete.ToUpper();
            auswahlAnzeigen.RemoveFromClassList("cms-filter-header-auswahlAnzeigen-disabled");
            auswahlAnzeigen.AddToClassList("cms-filter-header-auswahlAnzeigen-enabled");
            allesAnzeigen.RemoveFromClassList("cms-filter-box-allesAnzeigen-enabled");
            allesAnzeigen.AddToClassList("cms-filter-box-allesAnzeigen-disabled");

        } else if (filterTagsWrapper.childCount == 0) {
            filter.text = uIItemViewControllerScript.filterConfig_title.ToUpper();
            auswahlAnzeigen.RemoveFromClassList("cms-filter-header-auswahlAnzeigen-enabled");
            auswahlAnzeigen.AddToClassList("cms-filter-header-auswahlAnzeigen-disabled");
            allesAnzeigen.RemoveFromClassList("cms-filter-box-allesAnzeigen-disabled");
            allesAnzeigen.AddToClassList("cms-filter-box-allesAnzeigen-enabled");

            uIItemViewControllerScript.gameObject.GetComponent<filterController>().ActivateAllPins();
        }
    }

    public void FilterLoschen(Button btn, Button dropdownIcon, VisualElement wrapper, Button filter, VisualElement filterTagsWrapper, Button auswahlAnzeigen, Button allesAnzeigen, UIItemViewController uIItemViewControllerScript) {
        filterTagsWrapper.Clear();
        filter.text = uIItemViewControllerScript.filterConfig_title.ToUpper();
        auswahlAnzeigen.RemoveFromClassList("cms-filter-header-auswahlAnzeigen-enabled");
        auswahlAnzeigen.AddToClassList("cms-filter-header-auswahlAnzeigen-disabled");
        allesAnzeigen.RemoveFromClassList("cms-filter-box-allesAnzeigen-disabled");
        allesAnzeigen.AddToClassList("cms-filter-box-allesAnzeigen-enabled");

        switch (btn.text) {
            case "Panorama-Ansichten":
                btn.RemoveFromClassList("cms-filter-box-panorama-selected");
                btn.AddToClassList("cms-filter-box-panorama");
                break;
            case "Spiele":
                btn.RemoveFromClassList("cms-filter-box-spiele-selected");
                btn.AddToClassList("cms-filter-box-spiele");
                break;
            case "Park-Touren":
                btn.RemoveFromClassList("cms-filter-box-touren-selected");
                btn.AddToClassList("cms-filter-box-touren");
                break;
            case "Landschaft":
                btn.RemoveFromClassList("cms-filter-box-landschaft-selected");
                btn.AddToClassList("cms-filter-box-landschaft");
                break;
            case "Funde":
                btn.RemoveFromClassList("cms-filter-box-funde-button-selected");
                btn.AddToClassList("cms-filter-box-funde-button");
                wrapper.RemoveFromClassList("cms-filter-box-funde-selected");
                wrapper.AddToClassList("cms-filter-box-funde");

                if (uIItemViewControllerScript.fundeDropdownValue) {
                    uIItemViewControllerScript.fundeDropdownIcon.RemoveFromClassList("cms-filter-box-funde-dropdown-icon-opened-selected");
                    uIItemViewControllerScript.fundeDropdownIcon.AddToClassList("cms-filter-box-funde-dropdown-icon-opened");

                } else if (!uIItemViewControllerScript.fundeDropdownValue) {
                    uIItemViewControllerScript.fundeDropdownIcon.RemoveFromClassList("cms-filter-box-funde-dropdown-icon-closed-selected");

                    uIItemViewControllerScript.fundeDropdownIcon.AddToClassList("cms-filter-box-funde-dropdown-icon-closed");

                }

                uIItemViewControllerScript.fundeBoxValue = false;

                uIItemViewControllerScript.fundeDropdown.Query<Toggle>().ForEach((checkbox) => {
                    checkbox.value = false;
                    checkbox.RemoveFromClassList("cms-filter-radiobutton-funde-selected");
                    checkbox.Q("unity-checkmark").RemoveFromClassList("cms-filter-radiobutton-checkmark-funde-selected");
                });

                break;
            case "Grabungen":
                btn.RemoveFromClassList("cms-filter-box-grabungen-button-selected");
                btn.AddToClassList("cms-filter-box-grabungen-button");
                wrapper.RemoveFromClassList("cms-filter-box-grabungen-selected");
                wrapper.AddToClassList("cms-filter-box-grabungen");
                /*
                wrapper.RemoveFromClassList("cms-filter-box-grabungen-selected");
                wrapper.AddToClassList("cms-filter-box-grabungen");
                dropdownIcon.RemoveFromClassList("cms-filter-box-grabungen-dropdown-icon-closed-selected");
                dropdownIcon.AddToClassList("cms-filter-box-grabungen-dropdown-icon-closed");
                uIItemViewControllerScript.grabungenDropdownValue = true;
                uIItemViewControllerScript.grabungenBoxValue = false;
                */
                break;
        }
        btn.SetEnabled(true);
    }

    public void UpdateButtonDropdownUI() {

            uIItemViewControllerScript.funde.RemoveFromClassList("cms-filter-box-funde-button");
            uIItemViewControllerScript.funde.AddToClassList("cms-filter-box-funde-button-selected");
            uIItemViewControllerScript.fundeWrapper.RemoveFromClassList("cms-filter-box-funde");
            uIItemViewControllerScript.fundeWrapper.AddToClassList("cms-filter-box-funde-selected");

        if (uIItemViewControllerScript.fundeDropdownValue) {
            uIItemViewControllerScript.fundeDropdownIcon.RemoveFromClassList("cms-filter-box-funde-dropdown-icon-closed-selected");
            uIItemViewControllerScript.fundeDropdownIcon.AddToClassList("cms-filter-box-funde-dropdown-icon-opened-selected");
        } else if (!uIItemViewControllerScript.fundeDropdownValue) {
            uIItemViewControllerScript.fundeDropdownIcon.RemoveFromClassList("cms-filter-box-funde-dropdown-icon-closed");
            uIItemViewControllerScript.fundeDropdownIcon.AddToClassList("cms-filter-box-funde-dropdown-icon-closed-selected");
        }

        //disable alles anzeigen and button
        uIItemViewControllerScript.funde.SetEnabled(false);

        
    }

    #endregion

    IEnumerator loadPOIS() {
      
        yield return new WaitForSeconds(2f);
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        LoadDropdownFund();

    }

   
}
