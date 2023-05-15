using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Services;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startGame2 : MonoBehaviour
{
    public Camera ARCamera;
    public Camera InGameCamera;
    public ScreenShot screenshot;
    ARCameraManager cameraManager;
    Texture2D texture;
    public RenderTexture targetRenderTexture;

    public GameObject ARSession;
    public GameObject gameElementsContainer;
    public UnityEngine.UI.Image snapshotImageContainer;

    public Material snapShotmaterial;
    public GameObject tutorialOverlay;

    public GameObject tuturialScene;
    public VisualElement m_Root_Tutorial;
 
    public startGame2() {
    }

    public string popupStart_Headline;
    public string popupStart_subHeadline;
    public string popupStart_ButtonText;

    public string winMessage_Headline;
    public string winMessage_subheadline;
    public string winMessage_ButtonText;
   
    public string failMessage_Headline;
    public string failMessage_subheadline;
    public string failMessage_ButtonText;

    public CrossGameManager crossGameManager;
    public GamePopUp gamePopUp;
    public FundInfo fundInfo;
    public Game game2;
    public ItemOnMap associatedItemOnMap;
    public string state = "state1";

    public RawImage rawImage;

    public List<GameObject> CollectedCoins = new List<GameObject>(); /// list of coins to check for success
    public List<RectTransform> CollectedCoinsOverlays = new List<RectTransform>(); /// list of coins to check for success
    public RectTransform Overlay;

    private bool gameHasStarted = false;
    public UnityEngine.UI.Slider timerSlider;
    public float gameTime;
    private float tempGameTime;

    public GameObject error_logging;

    public FindToolController findToolController;

    public GameObject detector;

    private bool shouldGetGameInfo = true;

    public bool shouldScan;

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

    }
    private IEnumerator Start() {

        if (SceneManager.GetActiveScene().name == "game2") {
            shouldScan = false;
        }

        this.InGameCamera.enabled = false;
        this.InGameCamera.gameObject.SetActive(false);

        this.gameElementsContainer.SetActive(false);

        m_Root_Tutorial = tuturialScene.GetComponent<UIDocument>().rootVisualElement;

        gamePopUp.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("PopUp").style.display = DisplayStyle.None;

        yield return new WaitForSeconds(1);
        
        this.tempGameTime = this.gameTime;
        this.timerSlider.gameObject.transform.parent.gameObject.SetActive(false);
        cameraManager = ARCamera.GetComponent<ARCameraManager>();

       //cameraManager.frameReceived += GetTextureFromCam;
        crossGameManager.ErrorLog("starts scene");
        

    }

    public IEnumerator LerpAlphaValueInShader(Material material, float _alpha) {
    

        
        while (_alpha > 0.7f) {

            _alpha -= 0.01f;
            material.SetFloat("_alpha", _alpha);

            yield return null; //wait for next frame
        }

        yield return null;

        //material.SetFloat("_alpha", 0.5f);
    }



    public void startScan() {
        shouldScan = true;
        if (crossGameManager != null) {

            crossGameManager.ErrorLog("starts scan");
        }

        if (detector != null && detector.activeSelf) {
            detector.SetActive(false);
        }


        StartCoroutine(FinishScan());
    }

    public IEnumerator FinishScan() {

        crossGameManager.ErrorLog("starts to finish scan");


        yield return new WaitForSeconds(10);
        
        crossGameManager.ErrorLog("finish scan");

        //snapshotImageContainer.material = snapShotmaterial;
        //snapshotImageContainer.material.SetTexture("_MainTex", targetRenderTexture);

        screenshot.TakeScreenshot();
        gamePopUp.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("PopUp").style.display = DisplayStyle.Flex;
        gamePopUp.ShowAndUpdatePopUp(popupStart_Headline, "", popupStart_subHeadline, popupStart_ButtonText, "Info");
    }



    public void StartNextState() {
        if (state == "unEndedGame") {
            RestartGame();
        } else if (state == "state1") {
            startGame();
        }
    }

    void Update() {

        


        if (gameHasStarted && Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            if (Input.touchCount > 0 && Input.touchCount < 2) {
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    checkTouch(Input.GetTouch(0).position);
                }
            }
        } else if (gameHasStarted && Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
            if (Input.GetMouseButtonDown(0)) {
                checkTouch(Input.mousePosition);
                Debug.Log("mouse click");
            }
        }

        if (this.CollectedCoins.Count == GameObject.FindGameObjectsWithTag("coin").Length && this.gameHasStarted == true) {

            finishGameSuccess();
        }

        if (shouldGetGameInfo && crossGameManager != null && crossGameManager.AllItemsOnMap.Count > 1) {

            StartCoroutine(GetRelevantPois());

        }

        if (gameHasStarted) {
        
        }
    }


    private IEnumerator GetRelevantPois() {
        shouldGetGameInfo = false;

        StartCoroutine(this.crossGameManager.strapiService.getSpiel2Content((StrapiSingleResponse<Game> res) => {
            game2 = res.data;

            int poiID = res.data.attributes.point_of_interest.data.id;

            popupStart_Headline = game2.attributes.popupStart.headline;
            popupStart_subHeadline = game2.attributes.popupStart.subHeadline;
            popupStart_ButtonText = game2.attributes.popupStart.buttonText;

            winMessage_Headline = game2.attributes.winMessageP1.headline;
            winMessage_subheadline = game2.attributes.winMessageP1.subHeadline;
            winMessage_ButtonText = game2.attributes.winMessageP1.buttonText;

            failMessage_Headline = game2.attributes.failMessage.headline;
            failMessage_subheadline = game2.attributes.failMessage.subHeadline;
            failMessage_ButtonText = game2.attributes.failMessage.buttonText;

            associatedItemOnMap = crossGameManager.AllItemsOnMap.Find(item => item.ID == poiID);

            if (findToolController != null) {
                findToolController.itemOnMapCurrentlyClosestToPlayer = associatedItemOnMap;
            }


//            m_Root_Tutorial.Q<TextElement>("tutorial-text").text = game2.attributes.description;

        }));

        yield return null;
    }

    public void startGame() {

        //StartCoroutine(LerpAlphaValueInShader(snapshotImageContainer.material, 1f));
        //snapshotImageContainer.material.SetFloat("_alpha", 0.8f);

        this.gameElementsContainer.SetActive(true);

    
        rawImage.CrossFadeAlpha(0.4f, 2f, false);

        this.InGameCamera.gameObject.SetActive(true);
        this.InGameCamera.enabled = true;

        ARCamera.enabled = false;
        this.ARSession.SetActive(false);


        this.StartTimer();
        this.gameHasStarted = true;

    }

    public void StartTimer() {

        this.timerSlider.gameObject.transform.parent.gameObject.SetActive(true);
        this.timerSlider.maxValue = this.gameTime;
        this.timerSlider.value = this.gameTime;
        StartCoroutine(this.Timer());
    }

    public IEnumerator Timer() {

        //if (this.gameHasStarted) {

        //    float counter = this.gameTime;
        //    while (counter > 0) {


        //        yield return new WaitForSeconds(0.1f);
        //        counter--;
        //        this.timerSlider.value = counter;
        //    }


        //    finishGameError();
        //}


        //yield return null;

        float duration = gameTime; 
   
        while (this.timerSlider.value > 0) {

            this.timerSlider.value -= Time.deltaTime / duration;
            yield return null; 
        }

        finishGameError();
    }

    public void RestartGame() {
        foreach (RectTransform Overlay in this.CollectedCoinsOverlays) {
            Destroy(Overlay.gameObject);
        }

        //this.gameElementsContainer.SetActive(true);
        this.gameTime = this.tempGameTime;
     
        this.CollectedCoins.Clear();
        this.CollectedCoinsOverlays.Clear();
        this.startGame();
    }
    public void finishGameSuccess() {
 
        this.gameHasStarted = false;
        StopCoroutine(this.Timer());
   

        crossGameManager.AddToScore(crossGameManager.colorToType("spiel"), game2.attributes.reward);
        
        StartCoroutine(ShowFundObjectOverlay());


    }

    IEnumerator ShowFundObjectOverlay() {
        fundInfo.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);



        //Sprite sprite = (Sprite)(await game2.attributes.winimage1.media.ToSprite());
        //Texture2D texture = game2.attributes.winimage1.media.GetTexture2D();
        //fundInfo.setImage(sprite);
        SetImage();

        fundInfo.Show(associatedItemOnMap, true, winMessage_Headline, game2.attributes.reward.ToString(), winMessage_subheadline, winMessage_ButtonText, true);

        foreach (RectTransform Overlay in this.CollectedCoinsOverlays) {
            Destroy(Overlay.gameObject);
        }
    }

    async void SetImage() {
        Sprite sprite = (Sprite)(await game2.attributes.winimage1.media.ToSprite());
        fundInfo.setImage(game2.attributes.winimage1.media.data.attributes.GetFullImageUrl());
    }

    public void finishGameError() {


        if (this.gameHasStarted) {
            gamePopUp.ShowAndUpdatePopUp(failMessage_Headline, "", failMessage_subheadline, failMessage_ButtonText, "Error");
            state = "unEndedGame";
        }


    }


    // Start is called before the first frame update
    private void checkTouch(Vector3 pos) {

        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        Debug.Log("is it touch?");


        if (Physics.Raycast(ray, out hit, 100)) {

            Debug.Log("checks touch" + hit.transform.name);

            if (gameHasStarted && hit.transform.gameObject.CompareTag("coin")) {
                GameObject coin = hit.transform.gameObject;


                ///check if player has already collected Coin
                if (!this.CollectedCoins.Contains(coin)) {

                    Vector3 screenPos = Camera.main.WorldToScreenPoint(coin.transform.position);
                    float overlaywidth = FindWidthInScreen(coin);


                    this.CollectedCoins.Add(hit.transform.gameObject);
           
                    RectTransform CoinOverlay = (RectTransform)Instantiate(this.Overlay, screenPos, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
                    
                    
                    this.CollectedCoinsOverlays.Add(CoinOverlay);



                    CoinOverlay.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
                    StartCoroutine(Animate(overlaywidth/1f, CoinOverlay.GetComponent<RectTransform>()));
                    
                }

            }
            var planeAreaBehaviour = hit.collider.gameObject.GetComponent<PlaneAreaBehaviour>();
            if (planeAreaBehaviour != null) {
                planeAreaBehaviour.ToggleAreaView();
            }
        }

    }

    private IEnumerator Animate(float DesiredWidth, RectTransform Overlay) {
        float t = 0;
        //var target = transform.position - targetPos;

        while (Overlay.rect.width <= DesiredWidth) {
           
            Overlay.sizeDelta = Vector2.Lerp(new Vector2(0,0), new Vector2(DesiredWidth, DesiredWidth), t*10);
     
            t += Time.deltaTime;
            yield return null;
        }
    }



    /// <summary>
    /// /HELPER FUNCTIONS
    /// </summary>
    /// <param name="_TargetGameObject"></param>
    /// <returns></returns>
    private float FindWidthInScreen(GameObject _TargetGameObject) {
        Vector2 _ObjectScreenCord, _Xmin = new Vector2(Screen.width, 0), _Xmax = Vector2.zero, _Ymin = new Vector2(Screen.height, 0), _Ymax = Vector2.zero;
        float _Height, _Width;


        for (int i = 0; i != 8; i++) {
            //FindBoundCord() locates the eight coordinates that forms the boundries, followed by converting the coordinates to screen space.
            // The entire script starts in FindBoundCord
            _ObjectScreenCord = Camera.main.WorldToScreenPoint(FindBoundCord(i, _TargetGameObject));

            /* After gathering the coordinates and converting them to screen space
             we try to locate which of these have the highest/minimum x and y values.
            The difference between highest/minimum x and y must correspond to
             width and height.*/

            if (_ObjectScreenCord.x > _Xmax.x) {
                _Xmax.x = _ObjectScreenCord.x;
            } else if (_ObjectScreenCord.x < _Xmin.x) {
                _Xmin.x = _ObjectScreenCord.x;
            }
            if (_ObjectScreenCord.y > _Ymax.x) {
                _Ymax.x = _ObjectScreenCord.y;
            } else if (_ObjectScreenCord.y < _Ymin.x) {
                _Ymin.x = _ObjectScreenCord.y;
            }
        }



        //Too measure the distance between them, I use the Vector2 method Distance.
        _Height = Vector2.Distance(_Ymax, _Ymin);
        if (_Height > Screen.height || _Height < 0) {
            _Height = 0;
        }

        _Width = Vector2.Distance(_Xmax, _Xmin);
        if (_Width > Screen.width || _Width < 0) {
            _Width = 0;
        }

        // Here we simply make a check on which of the height or the 
        // width is the biggest. It was a necessary part for my project.
        if (_Height > _Width) {
            return _Height;
        } else {
            return _Width;
        }
        /*
        The method appears to work, though there are som serious issues when 
        Screen coordinates gets into negative values.
        When screencoordinates get negative the width/height 
        explodes, and I think it is because of the 
        Trapez formed viewport. */


    }

    private Vector3 FindBoundCord(int i, GameObject _GameObject) {
        /*This is basically where the code starts. It starts out by creating a 
         * bounding box around the target GameObject. 
        It calculates the 8 coordinates forming the bounding box, and 
        returns them all to the for loop.
        Because there are no real method which returns the coordinates 
        from the bounding box I had to create a switch/case which utillized 
        Bounds.center and Bounds.extents.*/

        Bounds _TargetBounds = _GameObject.GetComponent<Renderer>().bounds;
        Vector3 _TargetCenter = _TargetBounds.center;
        Vector3 _TargetExtents = _TargetBounds.extents;


        switch (i) {
            case 0:
                return _TargetCenter + new Vector3(_TargetExtents.x, _TargetExtents.y, _TargetExtents.z);
            case 1:
                return _TargetCenter + new Vector3(_TargetExtents.x, _TargetExtents.y, _TargetExtents.z * -1);
            case 2:
                return _TargetCenter + new Vector3(_TargetExtents.x, _TargetExtents.y * -1, _TargetExtents.z);
            case 3:
                return _TargetCenter + new Vector3(_TargetExtents.x, _TargetExtents.y * -1, _TargetExtents.z * -1);
            case 4:
                return _TargetCenter + new Vector3(_TargetExtents.x * -1, _TargetExtents.y, _TargetExtents.z);
            case 5:
                return _TargetCenter + new Vector3(_TargetExtents.x * -1, _TargetExtents.y, _TargetExtents.z * -1);
            case 6:
                return _TargetCenter + new Vector3(_TargetExtents.x * -1, _TargetExtents.y * -1, _TargetExtents.z);
            case 7:
                return _TargetCenter + new Vector3(_TargetExtents.x * -1, _TargetExtents.y * -1, _TargetExtents.z * -1);
            default:
                return Vector3.zero;
        }

    }
}
