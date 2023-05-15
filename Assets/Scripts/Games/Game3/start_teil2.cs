using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class start_teil2 : MonoBehaviour
{

    //AR properties
    private Vector2 touchPosition;
    static List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    private Camera _camera;
    private bool testingOnLapTop = true;

    public GameObject error_logging;

    //Tool Selection States
    private bool gameHasStarted = false;
    private string[] Tools = new string[4] {"tool1", "tool2", "tool3", "tool4" };
    private string CurrentTool;
    
    private int index;


    //to keep chekc on how many items are selected
    private int scores = 0;

    //Game Overlay Properties
    [Header("Overlay Properties")]

    public GameObject GameOverlay;

    [Header("Game Overlay After Selected Glocke")]
    public string GlockeHeadline = "Herzlichen Glückwunsch, Sie haben eine Glocke gefunden";
    public string GlockeDetail = "Helfen Sie mit, ein Zelt aufzubauen, bereiten Sie die Ausgrabung vor und fangen Sie an zu graben";  
    public Sprite GlockeImage;

    [SerializeField]
    public Color GlockeUIBackgroundColor;

    [Header("Game Overlay After Selected Maultier")]
    public string MaultierHeadline = "Herzlichen Glückwunsch, Sie haben eine Maultier gefunden";
    public string MaultierDetail = "Helfen Sie mit, ein Zelt aufzubauen, bereiten Sie die Ausgrabung vor und fangen Sie an zu graben";
    public Sprite MaultierImage;

    [SerializeField]
    public Color MaultierUIBackgroundColor;

    [Header("Which Scene Should come Next?")]
    public string NextScene = "MainScene";

    [Header("Buttons")]
    public string ContinueGameButtonText = "Es gibt noch mehr Schätze. Suchen Sie weiter";
    public string EndGameButtonText = "Neue Spiel beginnen";



    private void Awake() {

       this.CurrentTool = this.Tools[Random.Range(0, 3)];

        //this.error_logging = GameObject.FindGameObjectWithTag("errorlogging");

        if (!Camera.main) {
            this._camera = Camera.current;
        } else {
            this._camera = Camera.main;
        }

       if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            this.testingOnLapTop = false;
         
       } else {
            this.testingOnLapTop = true;        
       }
    }

    bool TryGetTouchPosition(out Vector2 touchPosition) {
      if (Input.touchCount > 0) {
          touchPosition = Input.GetTouch(0).position;
          return true;
      }
      touchPosition = default;
      return false;
    }

    void Update() {
        if (this.testingOnLapTop == true) {
            if (Input.GetMouseButtonDown(0)) {
                HandleTouch(Input.mousePosition);
            }
        } else {
            if (!TryGetTouchPosition(out Vector2 touchPosition)) {
                return;
            }

            if (Input.touchCount > 0 && Input.touchCount < 2) {
                Touch touch = Input.GetTouch(0);
                touchPosition = touch.position;

                 if (touch.phase == TouchPhase.Began) {
                    HandleTouch(touchPosition);
                 }
            }
        }
    }

    public void SetCurrentTool(string tool_name) {
        this.CurrentTool = tool_name;
    }

    public void ShowDropDownUIWithItem(GameObject Object) {


        if (!this.GameOverlay.activeInHierarchy) {
            this.GameOverlay.SetActive(true);
            this.GameOverlay.gameObject.SetActive(true);
        }

        bool ShouldChangeScene = false;
        string ButtonText = this.ContinueGameButtonText;

        if (this.scores > 1) {
            ShouldChangeScene = true;
            ButtonText = this.EndGameButtonText;
        }


        if (Object.gameObject.name == "Glocke") {
            this.GameOverlay.GetComponent<GameOverlay>().UpdateAndShowGameOverlay(this.GlockeHeadline, this.GlockeDetail, ButtonText, ShouldChangeScene, false, this.NextScene, this.GlockeImage, this.GlockeUIBackgroundColor);

        } else if (Object.gameObject.name == "Maultier") {
            this.GameOverlay.GetComponent<GameOverlay>().UpdateAndShowGameOverlay(this.MaultierHeadline, this.MaultierDetail, ButtonText, ShouldChangeScene, false, this.NextScene, this.MaultierImage, this.MaultierUIBackgroundColor);

        }


   
    }
    void HandleTouch(Vector3 pos) {
        Ray ray = this._camera.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {

            if (hit.transform.name == "Maultier") {
                GameObject Object = hit.transform.gameObject;
                //this.error_logging.GetComponent<UnityEngine.UI.Text>().text += " _touch disc object " + Object.name;

                this.scores += 1;

                Object.layer = 0;
                Object.GetComponent<Animator>().Play("pickingObject");
                //this.error_logging.GetComponent<UnityEngine.UI.Text>().text += " anuimator?:" + Object.GetComponent<Animator>();
            }
            
            if (hit.transform.name == "Glocke") {
                GameObject Object = hit.transform.gameObject;
                this.scores += 1;
                Object.layer = 0;
                Object.GetComponent<Animator>().Play("pickingObject");

                //this.error_logging.GetComponent<UnityEngine.UI.Text>().text += " __moves" + Object.name;

            }
        }
    }
   
}
