using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionUIRelativeFromTop : MonoBehaviour
{
    // Start is called before the first frame update

    private int Screeenheight;
    private int Screeenwidth;
    private float UI_height;
    private Vector2 InViewPosition;
    private Vector2 OutOfViewPosition;
    public GameObject error_logging;
    public RectTransform endGameButton;
    public RectTransform thisEndNotice;
    private float endGameButtonHeight;

    void Awake()
    {

        this.error_logging = GameObject.FindGameObjectWithTag("errorlogging");

        StartCoroutine(WaitUntilEndOfFrame());
    }

    IEnumerator WaitUntilEndOfFrame() {

        yield return new WaitForEndOfFrame();

        this.UI_height = this.transform.GetComponent<RectTransform>().rect.height;
        this.Screeenheight = Screen.height;
        this.Screeenwidth = Screen.width;
        this.endGameButtonHeight = this.endGameButton.rect.height;

        //ScaleMyRect(this.transform.GetComponent<RectTransform>());

        //this.InViewPosition = new Vector2(this.transform.GetComponent<RectTransform>().offsetMax.x, this.transform.GetComponent<RectTransform>().offsetMax.y);
        this.InViewPosition = new Vector2(0, Screen.height);
        this.InViewPosition = new Vector2(0, 0);


        //this.OutOfViewPosition = this.transform.GetComponent<RectTransform>().rect.position;
        this.OutOfViewPosition = new Vector2(0, -(this.UI_height - this.endGameButtonHeight));

        Debug.Log("container height" + this.UI_height);
        Debug.Log("screen height" + this.Screeenheight);
        Debug.Log("out of view position" + this.OutOfViewPosition);
        Debug.Log("in view position" + this.InViewPosition);


        this.transform.GetComponent<RectTransform>().offsetMax = this.OutOfViewPosition;
        this.transform.GetComponent<RectTransform>().offsetMin = this.OutOfViewPosition;

        //this.error_logging.GetComponent<UnityEngine.UI.Text>().text += "this screen is:" + this.Screeenheight + "heigh and width is:" + this.Screeenwidth;
        //this.error_logging.GetComponent<UnityEngine.UI.Text>().text += "places end notification @:" + this.OutOfViewPosition + "_will move to:" + this.InViewPosition;



    }

    public void ScrollUp() {

        //this.transform.LeanMoveLocal(new Vector2(0, this.UI_height), 2).setEaseOutQuart();
        //StartCoroutine(Animate());
  
    }


    private void ScaleMyRect(RectTransform myRect) {
        //If you want the middle of the rect be somewhere else then the middle of the screen change it here (0 ... 1, 0 ... 1)
        Vector2 rectMiddle = new Vector2(0.5f, 0.5f);

        float horizontalSize = 1f; //50% of horizontal screen used
        float verticalSize = 1f; //75%  of vertical screen used

        myRect.sizeDelta = Vector2.zero; //Dont want any delta sizes, because that would defeat the point of anchors
        myRect.anchoredPosition = Vector2.zero; //And the position is set by the anchors aswell so we set the offset to 0

        myRect.anchorMin = new Vector2(rectMiddle.x - horizontalSize / 2,
                                    rectMiddle.y - verticalSize / 2);
        myRect.anchorMax = new Vector2(rectMiddle.x + horizontalSize / 2,
                                    rectMiddle.y + verticalSize / 2);
    }


    private IEnumerator Animate() {
        float t = 0;
        while (this.transform.position.y >= this.InViewPosition.y && t < 1f) {
 
            //Debug.Log(this.transform.GetComponent<RectTransform>().rect.width + "y postion" + this.transform.position.y + "should scroll to" + this.InViewPosition.y);
            //this.error_logging.GetComponent<UnityEngine.UI.Text>().text += "moved to:" + this.transform.GetComponent<RectTransform>().offsetMax;


            this.transform.GetComponent<RectTransform>().offsetMax = Vector2.Lerp(this.OutOfViewPosition, this.InViewPosition, t);
            this.transform.GetComponent<RectTransform>().offsetMin = Vector2.Lerp(this.OutOfViewPosition, this.InViewPosition, t);
            t += Time.deltaTime;


            yield return null;
        }


    }
}
