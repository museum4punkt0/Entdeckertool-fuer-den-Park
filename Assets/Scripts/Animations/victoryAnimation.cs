using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class victoryAnimation : MonoBehaviour
{
    private float h_value = 0;
    public GameObject error_logging;
    
    [Header("Game Overlay")]
    public GameObject GameOverlay;

    public string Headline = "Congratulation!";
    public string Detail = "Alle Silber Munzen Gefunden";
    public Sprite CoverImage;

    [SerializeField]
    public Color UIBackgroundColor;

    [Header("Which Scene Should come Next?")]
    public string NextScene = "MainScene";

    [Header("Buttons")]
    public string GameButtonText = "Naechste Spiel: Mitt Schaufel & Kelle";




    void Start() {
        this.transform.GetComponent<Image>().color = new Color(0,0,0, this.h_value);
        StartCoroutine(Timer());

        //this.error_logging = GameObject.FindGameObjectWithTag("errorlogging");
        //this.error_logging.GetComponent<UnityEngine.UI.Text>().text += " activates end notification";

    }

    // Update is called once per frame
    void Update()
    {
        if (this.h_value < 0.4) {
            this.h_value += 0.05f;
            this.transform.GetComponent<Image>().color = new Color(0,0,0, this.h_value);
        }
    }

    IEnumerator Timer() {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5f);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        //this.error_logging.GetComponent<UnityEngine.UI.Text>().text += " SHOULD ROLL UP ENDGAME NOTIFICATION";



        this.GameOverlay.GetComponent<GameOverlayWithResults>().UpdateAndShowGameOverlay(this.Headline, this.Detail, this.GameButtonText, true, false, this.NextScene, this.CoverImage, this.UIBackgroundColor, 6);
    }

}
