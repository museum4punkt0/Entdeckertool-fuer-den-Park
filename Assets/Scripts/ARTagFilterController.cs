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

public class ARTagFilterController : MonoBehaviour
{
    CrossGameManager crossGameManager;
    UIItemViewController uIItemViewControllerScript;
    int counter = 0;

    //GameObject crossGameManager;

    // Start is called before the first frame update
    void Start()
    {
        uIItemViewControllerScript = this.gameObject.GetComponent<UIItemViewController>();
    }

    #region POIs




    public void DisactivatePin(string type) {

        for (int i = 0; i < crossGameManager.AllItemsOnMap.Count; i++) {
            if (crossGameManager.AllItemsOnMap[i].Poi.attributes.type == type) {
                crossGameManager.AllItemsOnMap[i].Pin.SetActive(false);
            }
        }
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

    public void DestroyTag(Button tag, Button btn, Button filter, Button auswahlAnzeigen, Button allesAnzeigen, VisualElement filterTagsWrapper, filterMenuView uIItemViewControllerScript) {
        filterTagsWrapper.Remove(tag);

        ARFilterDataInput filtertag = uIItemViewControllerScript.tagsInView.Find(item => item.name == btn.name);
        uIItemViewControllerScript.tagsInView.Remove(filtertag);

        btn.SetEnabled(true);
        btn.RemoveFromClassList("cms-filter-box");
        btn.AddToClassList("cms-filter-box-selected");

        btn.style.backgroundColor = new Color(0,0,0,0);

        //CountTags(filterTagsWrapper, filter, auswahlAnzeigen, allesAnzeigen, uIItemViewControllerScript);

    }



    public void CountTags(VisualElement filterTagsWrapper, Button filter, Button auswahlAnzeigen, Button allesAnzeigen, UIItemViewController uIItemViewControllerScript) {

        if (filterTagsWrapper.childCount > 0) {
            filter.text = "filter l?schen".ToUpper();
            auswahlAnzeigen.RemoveFromClassList("cms-filter-header-auswahlAnzeigen-disabled");
            auswahlAnzeigen.AddToClassList("cms-filter-header-auswahlAnzeigen-enabled");
            allesAnzeigen.RemoveFromClassList("cms-filter-box-allesAnzeigen-enabled");
            allesAnzeigen.AddToClassList("cms-filter-box-allesAnzeigen-disabled");

        } else if (filterTagsWrapper.childCount == 0) {
            filter.text = "filter".ToUpper();
            auswahlAnzeigen.RemoveFromClassList("cms-filter-header-auswahlAnzeigen-enabled");
            auswahlAnzeigen.AddToClassList("cms-filter-header-auswahlAnzeigen-disabled");
            allesAnzeigen.RemoveFromClassList("cms-filter-box-allesAnzeigen-disabled");
            allesAnzeigen.AddToClassList("cms-filter-box-allesAnzeigen-enabled");

            uIItemViewControllerScript.gameObject.GetComponent<filterController>().ActivateAllPins();
        }
    }

    public void FilterLoschen(Button btn, Button dropdownIcon, VisualElement wrapper, Button filter, VisualElement filterTagsWrapper, Button auswahlAnzeigen, Button allesAnzeigen, filterMenuView uIItemViewControllerScript) {

        print("attempts to remove filter" + btn );

        //filterTagsWrapper.Remove(btn);
        filterTagsWrapper.Clear();
        filter.text = "filter".ToUpper();

        auswahlAnzeigen.RemoveFromClassList("cms-filter-header-auswahlAnzeigen-enabled");
        auswahlAnzeigen.AddToClassList("cms-filter-header-auswahlAnzeigen-disabled");

      
        uIItemViewControllerScript.tagsInView.Clear();
        foreach (Button butn in uIItemViewControllerScript.buttons) {

 
            allesAnzeigen.RemoveFromClassList("cms-filter-box-allesAnzeigen-disabled");
            allesAnzeigen.AddToClassList("cms-filter-box-allesAnzeigen-enabled");
            butn.SetEnabled(true);

            butn.RemoveFromClassList("cms-filter-box-selected");
            butn.AddToClassList("cms-filter-box");
            butn.style.backgroundColor = new Color(255, 255, 255, 0);


        }
    }



    #endregion

    IEnumerator loadPOIS() {
      
        yield return new WaitForSeconds(2f);
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

    }


}
