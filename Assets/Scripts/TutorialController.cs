using System.Collections;
using Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour {

    CrossGameManager crossGameManager;
    public VisualElement m_Root;
    private VisualElement wrapper;
    //private VisualElement imgContainer;
    private Button imgContainer;
    private VisualElement panel;
    private VisualElement circle;
    private Button closeBtn;
    TutorialPageData _data;
    List<VisualElement> images;
    Label paginationCounter;
    int totalNTutorialImages;
    int i;
    [SerializeField] float speed;
    bool clickClose = false;
    bool finished = false;
    StrapiService strapiService;

    private void Awake() {
        this.strapiService = new StrapiService(Application.persistentDataPath + "/cachedRequests");
        //crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        StartTutorial();
    }

    public void StartTutorial() {
        this.m_Root = this.gameObject.GetComponent<UIDocument>().rootVisualElement;
        this.wrapper = this.m_Root.Q<VisualElement>("Wrapper");
        this.imgContainer = this.m_Root.Q<Button>("imgContainer");
        this.paginationCounter = this.m_Root.Q<Label>("paginationCounter");
        this.closeBtn = this.m_Root.Q<Button>("closeBtn");
        CloseScene();
        this.panel = this.m_Root.Q<VisualElement>("Panel");
        this.circle = this.m_Root.Q<VisualElement>("Circle");
        images = new List<VisualElement>();

        //StartCoroutine(crossGameManager.strapiService.getTutorialPageContent(LoadContent));
        StartCoroutine(strapiService.getTutorialPageContent(LoadContent));
    }

    private void CloseScene() {
        this.closeBtn.clicked += delegate {
            if (SceneManager.GetActiveScene().name == "IntroScene") {
                clickClose = true;
                StartCoroutine(StartAnimationCircle());
            } else {
                gameObject.SetActive(false);
            }
        };
    }

    async void LoadContent(StrapiSingleResponse<TutorialPageData> res) {
        _data = res.data;

        this.panel.Q<Label>("subheadline").text = _data.attributes.introText;
        this.panel.Q<Button>("startBtn").text = _data.attributes.buttonText;
        this.panel.Q<Button>("startBtn").clicked += delegate {

            SceneManager.LoadScene("MainScene");
        };
        this.panel.Q<Button>("reStartTutorial").text = _data.attributes.restartButton.ToUpper();
        this.panel.Q<Button>("reStartTutorial").clicked += delegate {
            
            SceneManager.LoadScene("IntroScene");

        };

        foreach (SliderItem img in _data.attributes.images) {
            VisualElement imgBox = new VisualElement();
            //imgBox.style.backgroundImage = new StyleBackground(await img.media.GetTexture2D());
            Davinci.get().load(img.media.data.attributes.GetFullImageUrl()).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(imgBox).start();
            imgBox.AddToClassList("img-in-container");
            imgBox.style.display = DisplayStyle.None;
            imgContainer.Add(imgBox);
            images.Add(imgBox);
        }

        if (SceneManager.GetActiveScene().name == "MainScene"){
            ShowImage_static();
        }


    }

    public void ShowImage_static() {
        this.wrapper.style.display = DisplayStyle.Flex;
        totalNTutorialImages = _data.attributes.images.Length;
        this.paginationCounter.text = "1 / " + totalNTutorialImages.ToString();
        images[0].style.display = DisplayStyle.Flex;
        i = 0;
        StartCoroutine(ShowImage(i));
    }

    IEnumerator ShowImage(int i) {

        /*
        imgContainer.clicked += delegate {
            this.paginationCounter.text = (i + 2).ToString() + " / " + totalNTutorialImages.ToString();
            images[i].style.display = DisplayStyle.None;
            images[i + 1].style.display = DisplayStyle.Flex;
            i = i + 1;

            if (i < totalNTutorialImages - 1) {
                StartCoroutine(ShowImage(i));
            }

            if (i >= totalNTutorialImages - 2) {
                finished = true;
                StartCoroutine(StartAnimationCircle());
            }
        };

        if (!finished) {
        */
          
            yield return new WaitForSeconds(speed);
            this.paginationCounter.text = (i + 2).ToString() + " / " + totalNTutorialImages.ToString();
            images[i].style.display = DisplayStyle.None;
            images[i + 1].style.display = DisplayStyle.Flex;
            i = i + 1;

            if (i < totalNTutorialImages - 1) {
                StartCoroutine(ShowImage(i));
            }

            if (i == totalNTutorialImages - 1) {
                StartCoroutine(StartAnimationCircle());
            }

        
        
    }

    IEnumerator StartAnimationCircle() {
        if (clickClose) {
            speed = 0f;
        } 

        yield return new WaitForSeconds(speed);
        this.wrapper.style.opacity = 1;
        this.circle.style.display = DisplayStyle.Flex;
        this.circle.style.opacity = 1;
        //this.wrapper.style.display = DisplayStyle.None;
        //this.circle.style.display = DisplayStyle.Flex;
        if ((SceneManager.GetActiveScene().name == "Tutorial") || (SceneManager.GetActiveScene().name == "IntroScene"))  {
            StartCoroutine(FadeInPanel());
        } else {
            StartCoroutine(CloseTutorial());
        }
    }

    IEnumerator CloseTutorial() {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    IEnumerator FadeInPanel() {
        yield return new WaitForSeconds(1);
        this.wrapper.style.display = DisplayStyle.None;
        this.panel.style.display = DisplayStyle.Flex;
        this.panel.Q<VisualElement>("content").style.display = DisplayStyle.Flex;
        this.panel.style.opacity = 1;
        this.gameObject.GetComponent<loadmainscene>().LoadGame_static();

    }
}
