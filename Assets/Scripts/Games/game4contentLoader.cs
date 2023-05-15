using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class game4contentLoader : MonoBehaviour
{

    CrossGameManager crossGameManager;
    public saveCurrentStateGame4 saveGame;

    public VisualElement m_Root;
    public VisualElement material_eisen;
    public VisualElement material_silber;
    public VisualElement material_gold;
    public VisualElement material_bronze;
    Label subText;
    public VisualElement popUpEnd;
    public VisualElement btnContainer;
    public Label title;
    public Label coinCount;

    string currentItem = "";
    Texture2D current_imgAfter;
    int totalNumberofbjects = 0;
    string imgAfter_url = "";

    public Game4Data _data;
    public List<Game4DataContent> storage = new List<Game4DataContent>();

    public List<Game4ToolContent> defaulttools = new List<Game4ToolContent>();

    public RadialWheel radialWheel;

    public bool isFirstTimeVisiting = true;

    Color win = new Color(0.0627451f, 0.6235294f, 0.5803922f);

    public game4contentLoader() {
    }

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    void Start()
    {
        this.m_Root = GetComponent<UIDocument>().rootVisualElement;
        material_eisen = this.m_Root.Q<VisualElement>("materialWrapper01");
        material_silber = this.m_Root.Q<VisualElement>("materialWrapper03");
        material_gold = this.m_Root.Q<VisualElement>("materialWrapper04");
        material_bronze = this.m_Root.Q<VisualElement>("materialWrapper02");
        subText = this.m_Root.Q<Label>("subText");
        popUpEnd = this.m_Root.Q<VisualElement>("PopUpEnd");
        btnContainer = this.m_Root.Q<VisualElement>("BtnContainer");
        title = this.m_Root.Q<Label>("title");
        coinCount = this.m_Root.Q<Label>("coins-label");

        coinCount.text = crossGameManager.Game4CurrentScore.ToString();

        if (crossGameManager.Game4PhaseStorage.Count == 0) {
            StartCoroutine(crossGameManager.strapiService.getSpiel4Content(StartloadContent));
        } else {
            isFirstTimeVisiting = false;
            ReLoadContentFromStorage();
            StartCoroutine(crossGameManager.strapiService.getSpiel4Content(ContinueLoadContent));
            btnContainer.style.display = DisplayStyle.Flex;
        }

    }

    async void LoadContent(Game4DataContent item) {
        VisualElement rowWrapper = new VisualElement();
        rowWrapper.AddToClassList("cms-game4-firstRow");

        Button vorher = new Button();
        vorher.name = "vorher_btn";
        vorher.AddToClassList("cms-game4-card-vorher");
        VisualElement img_vorher = new VisualElement();
        img_vorher.name = "img_vorher";
        //img_vorher.style.backgroundImage = new StyleBackground(await item.imageBefore.GetTexture2D());
        Davinci.get().load(item.imageBefore.data.attributes.GetFullImageUrl()).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(img_vorher).start();
        Texture2D imgBefore = (Texture2D)(await item.imageBefore.GetTexture2D());
        img_vorher.AddToClassList("cms-game4-materialEisen-card01-vorher-image");

        Label title_vorhher = new Label();
        title_vorhher.text = "vorher".ToUpper();
        title_vorhher.AddToClassList("cms-game4-card-vorher-title");

        vorher.Add(img_vorher);
        vorher.Add(title_vorhher);

        VisualElement nachher = new VisualElement();
        nachher.AddToClassList("cms-game4-card-vorher");
        VisualElement img_nachher = new VisualElement();
        img_nachher.AddToClassList("cms-game4-materialEisen-card01-nachher-image");
        img_nachher.style.backgroundImage = null;
        img_nachher.name = "img_nachher";
        Texture2D imgAfter = (Texture2D)(await item.imageAfter.GetTexture2D());
        imgAfter_url = item.imageAfter.data.attributes.GetFullImageUrl();
        Label title_nachher = new Label();
        title_nachher.text = "nachher".ToUpper();
        title_nachher.AddToClassList("cms-game4-card-vorher-title");

        nachher.Add(img_nachher);
        nachher.Add(title_nachher);

        rowWrapper.Add(vorher);
        rowWrapper.Add(nachher);
        rowWrapper.name = item.headline;

        if (item.tool.Count > 0) {
            foreach (Game4ToolContent tool in item.tool) {
                if (tool.image != null) {
                    print("TOOL: " + tool.headline);
                    Sprite imageTool = await tool.image.ToSprite();
                    tool.image.ConvertedSprite = imageTool;
                }
            }
        }

        if (item.material == "eisen") {
            material_eisen.Add(rowWrapper);
        } else if (item.material == "silber") {
            material_silber.Add(rowWrapper);
        } else if (item.material == "bronze") {
            material_bronze.Add(rowWrapper);
        } else if (item.material == "gold") {
            material_gold.Add(rowWrapper);
        }

        vorher.clicked += delegate {
            item.completed = true;
            currentItem = item.headline;
            current_imgAfter = imgAfter;

            vorher.SetEnabled(false);
            vorher.style.display = DisplayStyle.None;

            CloneButton(imgBefore, rowWrapper);

            Navigate();
            this.gameObject.transform.GetComponent<Game4UIController>().StartGameFunc(imgBefore, imgAfter);
        };


        if (item.completed) {
            currentItem = item.headline;
            current_imgAfter = imgAfter;
            vorher.SetEnabled(false);
            vorher.style.display = DisplayStyle.None;
            CloneButton(imgBefore, rowWrapper);

            material_eisen.Query<VisualElement>().ForEach((child) => {
            
            if (child.name == item.headline) {
                    child.Q<VisualElement>("img_nachher").style.borderBottomColor = win;
                    child.Q<VisualElement>("img_nachher").style.borderLeftColor = win;
                    child.Q<VisualElement>("img_nachher").style.borderRightColor = win;
                    child.Q<VisualElement>("img_nachher").style.borderTopColor = win;
                    //child.Q<VisualElement>("img_nachher").style.backgroundImage = new StyleBackground(current_imgAfter);
                    Davinci.get().load(imgAfter_url).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(child.Q<VisualElement>("img_nachher")).start();

                }
            });

            material_silber.Query<VisualElement>().ForEach((child) => {

            if (child.name == item.headline) {
                    child.Q<VisualElement>("img_nachher").style.borderBottomColor = win;
                    child.Q<VisualElement>("img_nachher").style.borderLeftColor = win;
                    child.Q<VisualElement>("img_nachher").style.borderRightColor = win;
                    child.Q<VisualElement>("img_nachher").style.borderTopColor = win;
                    //child.Q<VisualElement>("img_nachher").style.backgroundImage = new StyleBackground(current_imgAfter);
                    Davinci.get().load(imgAfter_url).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(child.Q<VisualElement>("img_nachher")).start();
                }
            });

            material_gold.Query<VisualElement>().ForEach((child) => {

            if (child.name == item.headline) {
                    child.Q<VisualElement>("img_nachher").style.borderBottomColor = win;
                    child.Q<VisualElement>("img_nachher").style.borderLeftColor = win;
                    child.Q<VisualElement>("img_nachher").style.borderRightColor = win;
                    child.Q<VisualElement>("img_nachher").style.borderTopColor = win;
                    //child.Q<VisualElement>("img_nachher").style.backgroundImage = new StyleBackground(current_imgAfter);
                    Davinci.get().load(imgAfter_url).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(child.Q<VisualElement>("img_nachher")).start();
                }
            });

            material_bronze.Query<VisualElement>().ForEach((child) => {

            if (child.name == item.headline) {
                    child.Q<VisualElement>("img_nachher").style.borderBottomColor = win;
                    child.Q<VisualElement>("img_nachher").style.borderLeftColor = win;
                    child.Q<VisualElement>("img_nachher").style.borderRightColor = win;
                    child.Q<VisualElement>("img_nachher").style.borderTopColor = win;
                    child.Q<VisualElement>("img_nachher").style.backgroundImage = new StyleBackground(current_imgAfter);
                    Davinci.get().load(imgAfter_url).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(child.Q<VisualElement>("img_nachher")).start();
                }
            });
        }

    }

    async void ContinueLoadContent(StrapiSingleResponse<Game4Data> res) {
        _data = res.data;

        LoadPopUps(_data);
        totalNumberofbjects = _data.attributes.content.Count;

        title.text = _data.attributes.title;
        subText.text = _data.attributes.subTitle;
        btnContainer.Q<Button>().text = _data.attributes.winButtonHeadline;
    }

    async void StartloadContent(StrapiSingleResponse<Game4Data> res) {
        _data = res.data;

        LoadPopUps(_data);
        
        totalNumberofbjects = _data.attributes.content.Count;

        title.text = _data.attributes.title;
        subText.text = _data.attributes.subTitle;
        //subText.text = "Restauriere alle " + totalNumberofbjects + " Funde.";
        btnContainer.Q<Button>().text = _data.attributes.winButtonHeadline;

        foreach (Game4DataContent item in _data.attributes.content) {

            LoadContent(item); 

            crossGameManager.Game4PhaseStorage.Add(item);
 
        }
       
    }

    async void GetContentFromStrapi(StrapiSingleResponse<Game4Data> res) {
        _data = res.data;
    }

    public void ReLoadContentFromStorage() {

        foreach (Game4DataContent item in crossGameManager.Game4PhaseStorage) {
            print(item.headline);

            LoadContent(item);
        }

        totalNumberofbjects = crossGameManager.Game4PhaseStorage.Count;
    }

    public void CloneButton(Texture2D img, VisualElement rowWrapper) {

        Button clone = new Button();
        clone.AddToClassList("cms-game4-card-vorher");
        VisualElement img_vorher = new VisualElement();
        img_vorher.name = "img_vorher";
        img_vorher.style.backgroundImage = new StyleBackground(img);
        img_vorher.AddToClassList("cms-game4-materialEisen-card01-vorher-image");
        Label title_vorhher = new Label();
        title_vorhher.text = "vorher".ToUpper();
        title_vorhher.AddToClassList("cms-game4-card-vorher-title");

        img_vorher.style.borderBottomColor = win;
        img_vorher.style.borderLeftColor = win;
        img_vorher.style.borderRightColor = win;
        img_vorher.style.borderTopColor = win;

        clone.Add(img_vorher);
        clone.Add(title_vorhher);

        rowWrapper.Add(clone);
        rowWrapper.style.flexDirection = FlexDirection.RowReverse;

        VisualElement info_icon = new VisualElement();
        info_icon.AddToClassList("cms-game4-info-icon");

        VisualElement bg = new VisualElement();
        bg.AddToClassList("cms-game4-bg");
        VisualElement close_icon = new VisualElement();
        close_icon.AddToClassList("cms-game4-close-icon");
        Label bg_title = new Label();
        bg_title.AddToClassList("cms-game4-bg-title");
        bg_title.text = currentItem;

        img_vorher.Add(info_icon);
        img_vorher.Add(bg);
        bg.Add(close_icon);
        bg.Add(bg_title);

        bool clickDetector = false;

        clone.clicked += delegate {
            if (!clickDetector) {
                bg.style.display = DisplayStyle.Flex;
                info_icon.style.display = DisplayStyle.None;
                clickDetector = true;
            } else if (clickDetector) {
                bg.style.display = DisplayStyle.None;
                info_icon.style.display = DisplayStyle.Flex;
                clickDetector = false;
            }
            
        };

    }

    public void CheckObjs(int coinAmount) {    

        if (crossGameManager.Game4CurrentScore == totalNumberofbjects) {

            title.text = "Gratulation!";
            subText.text = "Alle " + totalNumberofbjects + " Funde sind restauriert";
            crossGameManager.ScoreToAdd = coinAmount;
            crossGameManager.hasCompletedGame4 = true;

            StartCoroutine(ShowPopUpEnd());
            
            btnContainer.style.display = DisplayStyle.Flex;

            btnContainer.Q<Button>().clicked += delegate {
                SceneManager.LoadScene("Game5");
            };

            
        }

        material_eisen.Query<VisualElement>().ForEach((item) => {
            
            if (item.name == currentItem) {
                item.Q<VisualElement>("img_nachher").style.borderBottomColor = win;
                item.Q<VisualElement>("img_nachher").style.borderLeftColor = win;
                item.Q<VisualElement>("img_nachher").style.borderRightColor = win;
                item.Q<VisualElement>("img_nachher").style.borderTopColor = win;
                //item.Q<VisualElement>("img_nachher").style.backgroundImage = new StyleBackground(current_imgAfter);
                Davinci.get().load(imgAfter_url).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(item.Q<VisualElement>("img_nachher")).start();

            }
        });

        material_silber.Query<VisualElement>().ForEach((item) => {

            if (item.name == currentItem) {
                item.Q<VisualElement>("img_nachher").style.borderBottomColor = win;
                item.Q<VisualElement>("img_nachher").style.borderLeftColor = win;
                item.Q<VisualElement>("img_nachher").style.borderRightColor = win;
                item.Q<VisualElement>("img_nachher").style.borderTopColor = win;
                //item.Q<VisualElement>("img_nachher").style.backgroundImage = new StyleBackground(current_imgAfter);
                Davinci.get().load(imgAfter_url).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(item.Q<VisualElement>("img_nachher")).start();
            }
        });

        material_gold.Query<VisualElement>().ForEach((item) => {

            if (item.name == currentItem) {
                item.Q<VisualElement>("img_nachher").style.borderBottomColor = win;
                item.Q<VisualElement>("img_nachher").style.borderLeftColor = win;
                item.Q<VisualElement>("img_nachher").style.borderRightColor = win;
                item.Q<VisualElement>("img_nachher").style.borderTopColor = win;
                //item.Q<VisualElement>("img_nachher").style.backgroundImage = new StyleBackground(current_imgAfter);
                Davinci.get().load(imgAfter_url).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(item.Q<VisualElement>("img_nachher")).start();

            }
        });

        material_bronze.Query<VisualElement>().ForEach((item) => {

            if (item.name == currentItem) {
                item.Q<VisualElement>("img_nachher").style.borderBottomColor = win;
                item.Q<VisualElement>("img_nachher").style.borderLeftColor = win;
                item.Q<VisualElement>("img_nachher").style.borderRightColor = win;
                item.Q<VisualElement>("img_nachher").style.borderTopColor = win;
                //item.Q<VisualElement>("img_nachher").style.backgroundImage = new StyleBackground(current_imgAfter);
                Davinci.get().load(imgAfter_url).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(item.Q<VisualElement>("img_nachher")).start();

            }
        });

    }

    void LoadPopUps(Game4Data _data) {
        VisualElement PopUpStart = this.m_Root.Q<VisualElement>("PopUpStart");
        PopUpStart.Q<Label>("popup-headline").text = _data.attributes.popupStart.headline;
        PopUpStart.Q<Label>("popup-subHeadline").text = _data.attributes.popupStart.subHeadline;
        PopUpStart.Q<Button>("popup-button").text = _data.attributes.popupStart.buttonText;

        popUpEnd.Q<Label>("popup-headline").text = _data.attributes.popupEnd.headline;
        popUpEnd.Q<Label>("popup-subHeadline").text = _data.attributes.popupEnd.subHeadline;
        popUpEnd.Q<Button>("popup-button").text = _data.attributes.popupEnd.buttonText;
    }



    public void Navigate() {
        Game4DataContent content = _data.attributes.content.Find(item => item.headline == currentItem);


        print("has spec content" + content.headline + "with tools: " + content.tool.Count);
        print("******CURRENT ITEM: " + currentItem);
        List<Game4ToolContent> tools = content.tool;

        if (tools.Count > 0) {

            if (isFirstTimeVisiting) {
                radialWheel.ChangeCustomMenu(tools);
            } else {
                foreach (Game4DataContent item in crossGameManager.Game4PhaseStorage) {
                    if (currentItem == item.headline) {
                        radialWheel.ChangeCustomMenu(item.tool);
                    }
                }
            }

                
        } else {
            radialWheel.ChangeCustomMenu(this.defaulttools);
        }


    }

    IEnumerator ShowPopUpEnd() {
        yield return new WaitForSeconds(5);
        popUpEnd.style.display = DisplayStyle.Flex;

        popUpEnd.Q<Button>("popup-close").clicked += delegate {
            popUpEnd.style.display = DisplayStyle.None;
        };

        popUpEnd.Q<Button>("popup-button").clicked += delegate {
            
            crossGameManager.IsVisitingFromGame4AfterCompleted = true;
            crossGameManager.IsVisitingFromGame = false;
            SceneManager.LoadScene("MainScene");
        };

    }

}
