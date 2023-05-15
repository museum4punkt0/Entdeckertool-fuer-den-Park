using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;

public class ContentRenderer : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public GameObject linkPrefab;
    public CrossGameManager crossGameManager;


    void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        this.loadContent();
    }

    void loadContent() {
        foreach(Transform child in this.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(crossGameManager.strapiService.getItems((StrapiItemResponse res) =>
            {
                foreach (var item in res.data) {
                    GameObject contentElement = getContentElementFromItem(item, this.gameObject);
                    contentElement.transform.parent = this.gameObject.transform;
                }
            }
        ));
    }
    GameObject getContentElementFromItem(Item item, GameObject root) {
        GameObject go = new GameObject();
        //header slider
        //headline
        //CTA
        //text
        GameObject introText = Instantiate(this.textBoxPrefab, root.transform);
        introText.transform.Find("textBox/title").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = item.attributes.headline;
        introText.transform.Find("textBox/longText").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = item.attributes.longText;
        introText.transform.Find("textBox/shortText").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = item.attributes.shortText;
        introText.transform.parent = go.transform;
        //accordions
        //links
        
        foreach (Link link in item.attributes.links)
        {
            GameObject linkGameObject = Instantiate(this.linkPrefab, root.transform);
            linkGameObject.transform.Find("linkBox/headline").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = link.headline;
            linkGameObject.transform.Find("linkBox/subheadline").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = link.subheadline;
            linkGameObject.transform.Find("linkBox/Button/Text (TMP)").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = link.ButtonText;
            linkGameObject.transform.parent = go.transform;
        }
        
        return go;
    }
    
    
}
