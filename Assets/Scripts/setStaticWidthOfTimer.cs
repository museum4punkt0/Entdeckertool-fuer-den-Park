using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setStaticWidthOfTimer : MonoBehaviour
{
    // Start is called before the first frame update

    private float width;

    private float height;

    public RectTransform slider;
    void Start()
    {

        this.width = this.slider.rect.width;
        this.height = this.slider.rect.height;

        Debug.Log("SLIDER:_W"+this.width + "_H:"+this.height);
    
        StartCoroutine(this.SetWidth());
    }

    public IEnumerator SetWidth() {
     
        this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(this.width/2, this.height/2);
        Debug.Log("CHILD SLIDER:_W" + this.transform.GetComponent<RectTransform>().rect.height + "_H:" + this.transform.GetComponent<RectTransform>().rect.width);
        yield return new WaitForEndOfFrame();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
