using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Services;
using UIBuilder;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.Video;


public class UIItemViewController : MonoBehaviour {
    #region Variables

    CachingService cachingService;
    public VisualElement m_Root;
    public VisualElement m_Panel;
    public VisualElement m_scrollContent;
    public VisualElement m_fixedBar;
    public VisualElement m_fixedContent;
    public VisualElement m_BGFilterListe;
    public VisualElement m_PanelListe;
    public VisualElement m_PanelItem;
    public Button filter;
    public Button funde;
    public Button auswahlAnzeigen;
    public Button allesAnzeigen;
    Label angewendeteFilter;
    public VisualElement filterTagsWrapper;
    public Button fundeDropdownIcon;
    public VisualElement fundeWrapper;
    public GroupBox fundeDropdown;
    public ScrollView BGscrollerListe;
    public Button m_InfoButton;
    public Button m_QuestionsButton;
    public Button m_Filter;
    public Button m_Liste;
    public Button m_MainButton;
    public Button m_Detektor;
    public Button m_360;
    public Button m_Return;
    Item item;
    public string highlights_configuration = "";
    public string readmore_configuration = "";
    public string readless_configuration = "";
    bool OpenMenuInHalf;
    public bool isOpen = true;
    private string nextView;
    private List<HighlightData> highlights;
    public int itemId = 2;
    public string menuType;
    private float height = Screen.height * 0.65f;
    public string entedeckerFrageTitle;
    public string entedeckerFrageType;
    public string grabungenPageContentTitle;
    public string grabungenPageContentHeadline;
    public bool fundeDropdownValue;
    public bool fundeBoxValue;
    public bool grabungenDropdownValue;
    public bool grabungenBoxValue;
    public bool isFilterPanelOpen;
    public bool isListePanelOpen = false;
    static bool isFirstTimeUse = true;
    public bool mainButtonClick = false;
    public bool showHighlightsMenu = false;
    public bool resetFilter = false;
    public bool is360ViewOpen = false;
    bool menuSwitcherInfo = false;
    bool menuSwitcherQuestions = false;
    bool allowMainMenu = false;
    public GameObject FilterClickBlocker;
    public GameObject ListeClickBlocker;
    GameObject[] POIs;
    public string currentContent = "";
    public int valueSliderHelper = 0;
    public ScrollView scrollViewSliderHelper;
    public bool sliderHelper = false;
    public GameObject ARSession;
    public GameObject detectorPrefab;
    public GameObject detector;
    public bool isDetectorOpen = false;
    public string filterConfig_title = "";
    public string filterConfig_delete = "";
    public string filterConfig_display = "";
    public string filterConfig_inUse = "";
    public string filterConfig_showAll = "";

    public TourLoader tourloader;
    private AppPermissions appPermissions;

    public bool isARLocationReady = true;
    public PanoramaSceneManager panoramaSceneManager;
    public GameObject VideoCanvas;
    public GameObject VideoPanel;
    public VideoPlayer videoPlayer;
    public GameObject yearPanel;
    public GameObject tutorial;
    public int currentImageonSlider;
    public GameObject Zoom;
    public zoomController zoomController;

    public CrossGameManager crossGameManager;

    #endregion

    async void Start() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        this.m_Root = GetComponent<UIDocument>().rootVisualElement;

        //Search the root for the SlotContainer Visual Element
        this.m_scrollContent = this.m_Root.Q<VisualElement>("ScrollContent");
        this.m_BGFilterListe = this.m_Root.Q<VisualElement>("PanelFilter");
        this.m_PanelListe = this.m_Root.Q<VisualElement>("PanelListe");
        this.m_PanelItem = this.m_Root.Q<VisualElement>("PanelItem");
        this.m_fixedBar = this.m_Root.Q<VisualElement>("FixedBar");
        this.m_fixedContent = this.m_Root.Q<VisualElement>("FixedContent");

        
        StartCoroutine(crossGameManager.strapiService.getConfigurationContent(SetConfiguration));
        Filter();
        StartCoroutine(crossGameManager.strapiService.getListPageContent(Liste));

        this.m_scrollContent.Q<VisualElement>("unity-slider").style.opacity = 0;
        this.m_scrollContent.Q<VisualElement>("unity-low-button").style.opacity = 0;
        this.m_scrollContent.Q<VisualElement>("unity-high-button").style.opacity = 0;
        this.m_scrollContent.Q<VisualElement>("unity-slider").style.opacity = 0;
        this.m_scrollContent.style.flexDirection = FlexDirection.Column;

        //video
        if (videoPlayer != null && videoPlayer.source != VideoSource.Url)
            videoPlayer.source = VideoSource.Url;

        menuClick();

        m_Panel = this.m_Root.Q("Panel");

        isFilterPanelOpen = false;


        if (crossGameManager.FundObjektIDToView != 0 && crossGameManager.FundObjektIDToView != null) {
            Debug.Log("has fundobjectIDTOVIEW in UI itemController" + crossGameManager.FundObjektIDToView);

            m_Panel.style.translate = new Translate(0, Length.Percent(78), 0);
            isOpen = false;
            menuType = "menuOnClosedPosition";

            navigate("item", crossGameManager.FundObjektIDToView.ToString());
        } else {
            navigate("entdeckerFragen", "");
        }

        if (GameObject.FindGameObjectWithTag("TourMap")) {
            ARSession = GameObject.FindGameObjectWithTag("TourMap").gameObject;
        }

        if (GameObject.FindGameObjectWithTag("tourloader")) {
            tourloader = GameObject.FindGameObjectWithTag("tourloader").GetComponent<TourLoader>();
        }


        if (crossGameManager.IsVisitingFromGame) {
            StartCoroutine(crossGameManager.strapiService.getGameMenuContent(loadGameMenuContent));
            crossGameManager.IsVisitingFromGame = false;
        }

        if (crossGameManager.IsVisitingFromGame4AfterCompleted) {
            StartCoroutine(crossGameManager.strapiService.getArchaelogieMenuContent(loadArchaelogieMenuContent));
            crossGameManager.IsVisitingFromGame4AfterCompleted = false;
        }

        if (crossGameManager.IsVisitingFromTour) {
            StartCoroutine(crossGameManager.strapiService.getTourenMenuContent(loadTourenMenuContent));
            crossGameManager.IsVisitingFromTour = false;
        }

        if (crossGameManager.IsVisitingFromMask) {
            //StartCoroutine(DisablePois());
            StartCoroutine(crossGameManager.strapiService.getGameMenuContent(loadGameMenuContent));
            crossGameManager.IsVisitingFromMask = false;
        }

        

        if (crossGameManager.IsVisitingFrom360) {
            isOpen = false;
            m_Panel.style.translate = new Translate(0, Length.Percent(78), 0);
            crossGameManager.IsVisitingFrom360 = false;
            menuType = "menuOnClosedPosition";
        }


        isOpen = true;
        allowMainMenu = true;
        menuType = "menuOnOpenPosition";
        m_Panel.style.translate = new Translate(0, Length.Percent(0), 0);

        if (crossGameManager.currentContent != "") {
            string[] targets = crossGameManager.currentContent.Split(":");
            if (targets[1] == "0") {
                navigate(targets[0], null);
            } else if (targets[1] != "0") {
                navigate(targets[0], targets[1]);
            }

            crossGameManager.currentContent = "";
            menuType = "game";
            UpdateMenu(menuType);
        }

        if (crossGameManager.IsVisitingFromDetector) {
            crossGameManager.IsVisitingFromDetector = false;
            GameObject.FindGameObjectWithTag("ClickBlocker").gameObject.SetActive(false);
            isOpen = false;
            m_Panel.style.translate = new Translate(0, Length.Percent(78), 0);
            menuType = "menuOnClosedPosition";
        }

        ///360 mode access
        m_360 = this.m_Root.Q<Button>("menu-360");
        //m_360.style.opacity = 0.5f;
        m_360.style.opacity = 0;
        m_360.SetEnabled(false);
        m_360.clickable = null;

        if (isFirstTimeUse) {
            this.m_Root.Q<Button>("menu-return").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("empty").style.display = DisplayStyle.None;
            crossGameManager.currentContent = "";
            menuType = "menuOnOpenPosition";
            isFirstTimeUse = false;
        }


        UpdateMenu(menuType);
    }

    public void SetARButtonToInactive(bool isAble) {
        crossGameManager.ErrorLog("360BTN" + this.m_Root.Q<Button>("menu-360").name);

        m_360 = this.m_Root.Q<Button>("menu-360");

        if (isAble) {
            m_360.style.opacity = 1f;
            m_360.SetEnabled(true);

            m_360.clicked += delegate {
                if (panoramaSceneManager.displaysAR && is360ViewOpen) {
                    View360("close");
                } else {
                    View360("open");

                    if (isDetectorOpen) {
                        Detector("close");
                    }

                    if (isListePanelOpen) {
                        Liste("close");
                    }

                    if (isFilterPanelOpen) {
                        Filter("close");
                    }
                }
            };
        }
    }

    public void RestyleMainMenuCompassForAR() {
        this.m_Root.Q("menu-filter").style.visibility = Visibility.Hidden;
        this.m_Root.Q("menu-liste").style.visibility = Visibility.Hidden;
        this.m_Root.Q("detectorButton").style.visibility = Visibility.Hidden;
        this.m_Root.Q("bar").style.backgroundColor = new Color(1f, 1f, 1f, 1f);
        this.m_Root.Q("background").style.backgroundColor = new Color(1f, 1f, 1f, 0f);
        this.m_Root.Q("FixedBar").style.backgroundColor = new Color(1f, 1f, 1f, 0f);
        this.m_Root.Q("ScrollContent").style.backgroundColor = new Color(1f, 1f, 1f, 0f);


        this.m_Root.Q<Button>("menu-360").AddToClassList("Ar360Btn");
        this.m_Root.Q<Button>("menu-360").RemoveFromClassList("b360Btn");

        this.m_Root.Q<Button>("mainButton").style.display = DisplayStyle.None;
        this.m_Root.Q<Button>("mainButton360").style.display = DisplayStyle.Flex;
    }

    public void RestyleMainMenuCompassForMap() {
        this.m_Root.Q("menu-filter").style.visibility = Visibility.Visible;
        this.m_Root.Q("menu-liste").style.visibility = Visibility.Visible;
        this.m_Root.Q("detectorButton").style.visibility = Visibility.Visible;

        this.m_Root.Q("bar").style.backgroundColor = new Color(0.3607843f, 0.2156863f, 0.007843138f, 1f);
        this.m_Root.Q("background").style.backgroundColor = new Color(1f, 1f, 1f, 1f);
        this.m_Root.Q("FixedBar").style.backgroundColor = new Color(1f, 1f, 1f, 1f);
        this.m_Root.Q("ScrollContent").style.backgroundColor = new Color(1f, 1f, 1f, 1f);

        this.m_Root.Q<Button>("menu-360").RemoveFromClassList("Ar360Btn");
        this.m_Root.Q<Button>("menu-360").AddToClassList("b360Btn");

        this.m_Root.Q<Button>("mainButton").style.display = DisplayStyle.Flex;
        this.m_Root.Q<Button>("mainButton360").style.display = DisplayStyle.None;
    }

    #region Navigation

    public void navigate(string targetCategory, string targetId) {

        ///reset temp ID to view 
        crossGameManager.FundObjektIDToView = 0;

        switch (targetCategory) {
            case "menu":
                switch (targetId) {
                    case "main":
                        this.loadMainMenu();
                        break;
                    case "touren":
                        StartCoroutine(crossGameManager.strapiService.getTourenMenuContent(loadTourenMenuContent));
                        break;
                    case "archaeologie":
                        StartCoroutine(crossGameManager.strapiService.getArchaelogieMenuContent(loadArchaelogieMenuContent));
                        break;
                    case "belohnungen":
                        StartCoroutine(crossGameManager.strapiService.getBelohnungenPageContent(belohnungen));
                        break;
                    case "games":
                        StartCoroutine(crossGameManager.strapiService.getGameMenuContent(loadGameMenuContent));
                        break;
                }

                break;
            case "game":
                this.loadGame(targetId);

                break;
            case "item":
                this.itemId = Convert.ToInt32(targetId);

                Debug.Log("NAVIGATE TO ITEM:" + this.itemId);
                StartCoroutine(crossGameManager.strapiService.getItems(loadItemContent));
                this.m_scrollContent.Q<ScrollView>("ScrollContent").mode = ScrollViewMode.Vertical;

                break;
            case "question":
                this.itemId = Convert.ToInt32(targetId);
                StartCoroutine(crossGameManager.strapiService.getQuestions(loadContextQuestion));

                break;
            case "info":
                StartCoroutine(crossGameManager.strapiService.getInfoMenuContent(loadInfoMenuContent));

                break;
            case "entdeckerFragen":
                StartCoroutine(crossGameManager.strapiService.getEntdeckerfragenMenuContent(loadEntdeckerfragenMenuContent));
                //StartCoroutine(crossGameManager.strapiService.getEntdeckerfragenMenuContent(loadEntdeckerfragenMenuContent));

                break;
            case "highlights":
                StartCoroutine(crossGameManager.strapiService.getHighlights(loadHighlights));
                this.m_scrollContent.Q<ScrollView>("ScrollContent").mode = ScrollViewMode.Vertical;

                break;
            case "touren":
                crossGameManager.tourIDtoStart = Int16.Parse(targetId);
                SceneManager.LoadScene("parktour");

                break;

            case "archaologie":
                StartCoroutine(crossGameManager.strapiService.getGrabungen(loadArchaelogieGrabungen));
                break;

            case "error":
                this.m_Root.Q("ErrorLog").style.display = DisplayStyle.Flex;
                this.m_Root.Q("ErrorLog").Q<Button>("closebtn").clicked += delegate {
                    this.m_Root.Q("ErrorLog").style.display = DisplayStyle.None;
                };
                break;
        }
    }

    private void loadGame(string targetId) {
        string sceneName = null;

        switch (targetId) {
            case "71":
                sceneName = "Scenes/games/Game7";
                break;

            case "1":
                sceneName = "Scenes/games/Game1";
                break;
            case "2":
                sceneName = "Scenes/games/Game2";
                break;
            case "3":
                sceneName = "Scenes/games/Game3";
                break;
            case "4":
                sceneName = "Scenes/games/Game4";
                break;
            case "5":
                sceneName = "Scenes/games/Game5";
                break;
            case "6":
                sceneName = "Scenes/ARFilterScene";
                break;
            case "7":
                sceneName = "Scenes/360_Illustrations";
                break;
            case "8":
                sceneName = "Scenes/Trials/DragNDrop_PlanePlacement";
                break;
            case "9":
                sceneName = "Scenes/360_Illustrations";
                break;
            case "21":
                sceneName = "Scenes/games/Game2_AR";
                break;
            case "31":
                sceneName = "Scenes/games/Game3_AR";
                break;

            case "51":
                sceneName = "Scenes/games/Game5_AR";
                break;
            case "61":
                sceneName = "Scenes/games/game6/Game6_1";
                break;
            case "62":
                sceneName = "Scenes/games/game6/Game6_2";
                break;
            case "63":
                sceneName = "Scenes/games/game6/Game6_3";
                break;
        }


        if (sceneName != null) {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void menuClick() {
        Sprite active_filter = Resources.Load<Sprite>("menu/icons/active/filter");
        Sprite innactive_filter = Resources.Load<Sprite>("menu/icons/innactive/filter");
        Sprite active_liste = Resources.Load<Sprite>("menu/icons/active/liste");
        Sprite innactive_liste = Resources.Load<Sprite>("menu/icons/innactive/liste");


        //info
        m_InfoButton = this.m_Root.Q<Button>("menu-info");
        m_InfoButton.clicked += delegate {
            if (menuSwitcherInfo) {
                //this.loadMainMenu();
                navigate("entdeckerFragen", "");
                menuSwitcherInfo = false;
            } else if (!menuSwitcherInfo || menuSwitcherQuestions) {
                navigate("info", "");
                menuSwitcherInfo = true;
                menuSwitcherQuestions = false;
            }
        };

        //fragen
        m_QuestionsButton = this.m_Root.Q<Button>("menu-questions");
        m_QuestionsButton.clicked += delegate {
            if (menuSwitcherQuestions) {
                navigate("entdeckerFragen", "");
                menuSwitcherQuestions = false;
            } else if (!menuSwitcherQuestions || menuSwitcherInfo) {
                //navigate("entdeckerFragen", "");
                this.loadMainMenu();
                menuSwitcherQuestions = true;
                menuSwitcherInfo = false;
            }
        };

        m_MainButton = this.m_Root.Q<Button>("mainButton");


        //filter
        m_Filter = this.m_Root.Q<Button>("menu-filter");
        m_Filter.clicked += delegate {
            if (!isFilterPanelOpen) {
                Filter("open");

                if (isDetectorOpen) {
                    Detector("close");
                }

                if (is360ViewOpen) {
                    View360("close");
                }
            } else if (isFilterPanelOpen) {
                Filter("close");
            }
        };

        //liste
        m_Liste = this.m_Root.Q<Button>("menu-liste");
        m_Liste.clicked += delegate {
            if (!isListePanelOpen) {
                Liste("open");

                if (isDetectorOpen) {
                    Detector("close");
                }

                if (is360ViewOpen) {
                    View360("close");
                }
            } else if (isListePanelOpen) {
                Liste("close");
            }
        };

        //detektor
        m_Detektor = this.m_Root.Q<Button>("detectorButton");
        m_Detektor.clicked += delegate {
            if (!isDetectorOpen) {
                Detector("open");

                if (isFilterPanelOpen) {
                    Filter("close");
                }

                if (isListePanelOpen) {
                    Liste("close");
                }

                if (is360ViewOpen) {
                    View360("close");
                }
            } else if (isDetectorOpen) {
                Detector("close");
            }
        };


        m_Return = this.m_Root.Q<Button>("menu-return");
        m_Return.clicked += delegate {
            if (menuType == "archaeologieGrabungen") {
                StartCoroutine(crossGameManager.strapiService.getArchaelogieMenuContent(loadArchaelogieMenuContent));
            } else if (menuType == "contextQuestions") {
                StartCoroutine(crossGameManager.strapiService.getEntdeckerfragenMenuContent(loadEntdeckerfragenMenuContent));
            } else {
                this.loadMainMenu();
                //navigate("entdeckerFragen", "");
                this.m_Root.Q<VisualElement>("menu-return").style.display = DisplayStyle.None;
                this.m_Root.Q<VisualElement>("empty").style.display = DisplayStyle.None;
            }
        };
    }

    public void ManagePanelsFilterListe(string panel) {
        if (panel == "liste") {
            ListeClickBlocker.gameObject.SetActive(true);
            this.m_PanelListe.style.display = DisplayStyle.Flex;
            isListePanelOpen = true;
            SetIconState("liste", "active");
            this.gameObject.GetComponent<listeController>().EnableAllPOIs();
        } else if (panel == "filter") {
            FilterClickBlocker.gameObject.SetActive(true);
            this.m_BGFilterListe.style.display = DisplayStyle.Flex;
            isFilterPanelOpen = true;
            SetIconState("filter", "active");
        }
    }

    public void Filter(string action) {
        Sprite active_filter = Resources.Load<Sprite>("menu/icons/active/filter");
        Sprite innactive_filter = Resources.Load<Sprite>("menu/icons/innactive/filter");
        Sprite active_liste = Resources.Load<Sprite>("menu/icons/active/liste");
        Sprite innactive_liste = Resources.Load<Sprite>("menu/icons/innactive/liste");

        if (action == "open") {
            FilterClickBlocker.gameObject.SetActive(true);

            if (isListePanelOpen) {
                this.m_PanelListe.style.display = DisplayStyle.None;
                isListePanelOpen = false;
                m_Liste.style.backgroundImage = new StyleBackground(innactive_liste);
            }

            this.m_BGFilterListe.style.display = DisplayStyle.Flex;
            isFilterPanelOpen = true;
            isOpen = false;
            resetFilter = false;

            m_Liste.style.backgroundImage = new StyleBackground(innactive_liste);
            m_Filter.style.backgroundImage = new StyleBackground(active_filter);
            StartCoroutine(crossGameManager.strapiService.getFilterPageContent(SetFilterConfiguration));

        } else if (action == "close") {
            FilterClickBlocker.gameObject.SetActive(false);
            ListeClickBlocker.gameObject.SetActive(false);

            this.m_BGFilterListe.style.display = DisplayStyle.None;
            isFilterPanelOpen = false;

            m_Filter.style.backgroundImage = new StyleBackground(innactive_filter);

            resetFilter = true;
            this.gameObject.GetComponent<filterController>().ActivateAllPins();
        }
    }

    public void Liste(string action) {
        Sprite active_filter = Resources.Load<Sprite>("menu/icons/active/filter");
        Sprite innactive_filter = Resources.Load<Sprite>("menu/icons/innactive/filter");
        Sprite active_liste = Resources.Load<Sprite>("menu/icons/active/liste");
        Sprite innactive_liste = Resources.Load<Sprite>("menu/icons/innactive/liste");

        if (action == "open") {
            ListeClickBlocker.gameObject.SetActive(true);

            if (isFilterPanelOpen) {
                ListeClickBlocker.gameObject.SetActive(false);
                this.m_BGFilterListe.style.display = DisplayStyle.None;
                isFilterPanelOpen = false;
                m_Filter.style.backgroundImage = new StyleBackground(innactive_filter);
            }

            this.m_PanelListe.style.display = DisplayStyle.Flex;
            isListePanelOpen = true;

            m_Filter.style.backgroundImage = new StyleBackground(innactive_filter);
            m_Liste.style.backgroundImage = new StyleBackground(active_liste);

            this.gameObject.GetComponent<listeController>().EnableAllPOIs();
        } else if (action == "close") {
            ListeClickBlocker.gameObject.SetActive(false);
            FilterClickBlocker.gameObject.SetActive(false);

            this.m_PanelListe.style.display = DisplayStyle.None;
            isListePanelOpen = false;

            m_Liste.style.backgroundImage = new StyleBackground(innactive_liste);
        }
    }

    public void Detector(string action) {
        if (action == "open") {
            detectorPrefab.SetActive(true);
            GameObject.FindGameObjectWithTag("FindTool").GetComponent<FindToolController>().enabled = true;
            isDetectorOpen = true;
            SetIconState("detector", "active");
            DisablePinsClick(false);
        } else if (action == "close") {
            crossGameManager.IsVisitingFromDetector = true;
            SceneManager.LoadScene("MainScene");
            GameObject.FindGameObjectWithTag("FindTool").GetComponent<DetectorFundPopUp>().popUpPanel.style.display = DisplayStyle.None;
            GameObject.FindGameObjectWithTag("FindTool").GetComponent<AnimationControllerFundDetektor>()
                .RestartFunddetektor();
            GameObject.FindGameObjectWithTag("FindTool").GetComponent<FindToolController>().triggerAnimationPart1 =
                false;
            GameObject.FindGameObjectWithTag("FindTool").GetComponent<FindToolController>().triggerAnimationPart2 =
                false;
            GameObject.FindGameObjectWithTag("FindTool").GetComponent<FindToolController>().enabled = false;

            detectorPrefab.SetActive(false);
            isDetectorOpen = false;
            SetIconState("detector", "innactive");
            DisablePinsClick(true);
        }
    }

    public void View360(string action) {
        if (action == "open") {
            //RestyleMainMenuCompassForAR();
            tourloader.blockMapInteractionBelowMenu = false;

            this.m_Root.style.display = DisplayStyle.None;
            panoramaSceneManager.StartAR();
            is360ViewOpen = true;
        } else if (action == "close") {
            //RestyleMainMenuCompassForMap();

            this.m_Root.style.display = DisplayStyle.Flex;

            is360ViewOpen = false;
        }
    }

    #endregion

    #region UIScreens

    public async void loadItemContent(StrapiItemResponse res) {
        this.gameObject.GetComponent<Swiper>().allowSwiper = false;
        //crossGameManager.currentContent = "item:0";

        this.m_PanelItem.style.display = DisplayStyle.Flex;
        ScrollView scrollContent = this.m_PanelItem.Q<ScrollView>();

        scrollContent.Q<VisualElement>("unity-slider").style.opacity = 0;
        scrollContent.Q<VisualElement>("unity-low-button").style.opacity = 0;
        scrollContent.Q<VisualElement>("unity-high-button").style.opacity = 0;
        scrollContent.Clear();
        scrollContent.style.flexDirection = FlexDirection.Column;

        VisualElement header = new VisualElement();
        Button closeBtn = new Button();
        Button shareBtn = new Button();

        //this.item = res.data[this.itemId];
        ///NOTICE: looking for the correct Item instead of simply the impicit index
        this.item = res.data.Find(item => item.id == this.itemId);
        crossGameManager.fundObjektIDtoImageZoom = this.item.id;

        //header
        header.AddToClassList("cms-item-header");
        closeBtn.AddToClassList("cms-item-header-button-close");
        shareBtn.AddToClassList("cms-item-header-button-share");
        header.Add(closeBtn);
        header.Add(shareBtn);
        scrollContent.Add(header);

        shareBtn.clicked += delegate {
            gameObject.GetComponent<ShareContent>().ShareContent_static();
        };

        closeBtn.clicked += delegate {
            this.m_PanelItem.style.display = DisplayStyle.None;
            this.gameObject.GetComponent<Swiper>().allowSwiper = true;

            if (crossGameManager.IsVisitingFromDetector) {
                //SetIconState("detector", "innactive");
                UpdateMenu("menuOnClosedPosition");
                crossGameManager.IsVisitingFromDetector = false;
                GameObject.FindGameObjectWithTag("FindTool").GetComponent<FindToolController>().enabled = true;
            }

            if (crossGameManager.LastSceneVisited == "Game5") {
                SceneManager.LoadScene(crossGameManager.LastSceneVisited);
            }

            if (crossGameManager.LastSceneVisited == "ParkTour") {
                SceneManager.LoadScene("ParkTour");
            } else if (crossGameManager.IsVisitingFromTour) {
                SceneManager.LoadScene("ParkTour");
            } else {
                navigate("entdeckerFragen", "");
            }

            if (detectorPrefab.active) {
                DisablePinsClick(false);
            }
        };

        //slider
        CMSSlider slider = new CMSSlider(await this.item.attributes.GetSliderItemsWithMedia(), this.m_scrollContent,
            this);
        scrollContent.Add(slider);

        //division bar
        scrollContent.Add(new CMSDivisionBar("thickBar"));

        //headline
        CMSHeadline headline = new CMSHeadline(this.item.attributes.fundTitle, this.item.attributes.fundTitle);
        scrollContent.Add(headline);

        //MASK BUTTON
        if (this.item.id == 14) {
            Button arButton = new Button();
            arButton.text = "Du mit Maske";
            arButton.AddToClassList("cms-buttonAR");
            VisualElement container = new VisualElement();
            container.style.flexDirection = FlexDirection.Column;
            container.style.alignItems = Align.Center;
            container.Add(arButton);

            arButton.clicked += delegate {
                SceneManager.LoadScene("Scenes/ARFilterScene");
            };

            scrollContent.Add(container);
        } else {
            VisualElement spacer = new VisualElement();
            spacer.style.height = 100;
            spacer.style.width = Length.Percent(90);
            scrollContent.Add(spacer);
        }

        VisualElement scrollIcon = new VisualElement();
        scrollIcon.AddToClassList("cms-buttonScroll");
        scrollContent.Add(scrollIcon);

        //text
        scrollContent.Add(new CMSCollapsibleText(this.item.attributes.shortText, this.item.attributes.longText,
            readmore_configuration, readless_configuration));

        /*
        if (this.item.attributes.buttons.Length != 0) {
            foreach (OverlayButton btn in this.item.attributes.buttons) {
                CMSButton createBtn = new CMSButton(btn, this);
                scrollContent.Add(createBtn);
            }
        }
        */

        //accordions
        foreach (AccordionItem accordionItem in this.item.attributes.accordionItems) {
            scrollContent.Add(new CMSAccordionItem(accordionItem, this));
        }

        //links
        foreach (Link attributesLink in this.item.attributes.links) {
            scrollContent.Add(new CMSLinkBox(attributesLink, this));
        }

        VisualElement footer = new VisualElement();
        footer.style.height = 200;
        footer.style.width = Length.Percent(80);
        scrollContent.Add(footer);
    }

    private void loadMainMenu() {
        StartCoroutine(this.crossGameManager.strapiService.getHighlights(loadHighlights));
    }

    async void loadMainMenuContent(StrapiMainMenuResponse res) {
        this.m_scrollContent.Clear();
        this.m_scrollContent.RemoveFromClassList("cms-fragen-background-waves");
        this.m_scrollContent.style.display = DisplayStyle.None;
        this.m_fixedBar.style.display = DisplayStyle.None;
        this.m_fixedContent.style.display = DisplayStyle.Flex;
        this.m_fixedContent.Clear();

        this.m_fixedContent.Add(new CMSHeadline(highlights_configuration, "highlights"));
        this.m_fixedContent.Add(new CMSMenuHighlightsSlider(this.highlights, menuType, this));
        this.m_fixedContent.Add(new CMSMenuEntries(res.data.attributes.links, crossGameManager.score.coinsAmount, this));
    }

    async void loadHighlights(StrapiResponse<HighlightData> res) {
        crossGameManager.currentContent = "highlights:0";
        this.m_fixedContent.RemoveFromClassList("cms-fragen-background-waves");
        this.highlights = res.data;
        SetIconState("info", "innactive");
        SetIconState("questions", "active");
        menuSwitcherInfo = false;
        menuSwitcherQuestions = true;
        StartCoroutine(this.crossGameManager.strapiService.getMainMenuContent(loadMainMenuContent));
    }

    async void loadNewEntries(StrapiResponse<HighlightData> res) {
        this.m_scrollContent.Clear();
        this.m_scrollContent.RemoveFromClassList("cms-fragen-background-waves");
        this.highlights = res.data;
        StartCoroutine(this.crossGameManager.strapiService.getArchaelogieMenuContent(loadArchaelogieMenuContent));
    }

    async void loadGameMenuContent(StrapiSingleResponse<GameMenuData> res) {
        crossGameManager.currentContent = "menu:games";
        this.m_fixedContent.RemoveFromClassList("cms-fragen-background-waves");
        this.m_fixedBar.style.display = DisplayStyle.None;
        this.m_scrollContent.style.display = DisplayStyle.None;
        this.m_fixedContent.style.display = DisplayStyle.Flex;
        this.m_fixedContent.Clear();

        menuType = "game";
        UpdateMenu(menuType);

        GameMenuData _data = res.data;

        VisualElement wrapper = new VisualElement();
        wrapper.style.flexDirection = FlexDirection.Column;

        wrapper.Add(new CMSMenuGameSlider(_data.attributes.cards, _data.attributes.pageTitle, menuType, this));
        this.m_fixedContent.Add(wrapper);
    }

    async void loadTourenMenuContent(StrapiSingleResponse<GameMenuData> res) {
        crossGameManager.currentContent = "menu:touren";
        this.m_fixedContent.RemoveFromClassList("cms-fragen-background-waves");
        this.m_fixedBar.style.display = DisplayStyle.None;
        this.m_scrollContent.style.display = DisplayStyle.None;
        this.m_fixedContent.style.display = DisplayStyle.Flex;
        this.m_fixedContent.Clear();

        TextElement category = new TextElement();
        this.m_scrollContent.Q<ScrollView>("ScrollContent").mode = ScrollViewMode.Horizontal;

        menuType = "touren";
        UpdateMenu(menuType);

        GameMenuData _data = res.data;

        VisualElement wrapper = new VisualElement();
        wrapper.style.flexDirection = FlexDirection.Column;

        wrapper.Add(new CMSMenuGameSlider(_data.attributes.cards, _data.attributes.pageTitle, menuType, this));

        this.m_fixedContent.Add(wrapper);
    }

    async void belohnungen(StrapiSingleResponse<BelohnungenPageData> res) {
        crossGameManager.currentContent = "menu:belohnungen";

        this.m_fixedBar.style.display = DisplayStyle.None;
        this.m_scrollContent.style.display = DisplayStyle.None;
        this.m_fixedContent.style.display = DisplayStyle.Flex;
        this.m_fixedContent.Clear();

        menuType = "belohnungen";
        UpdateMenu(menuType);

        BelohnungenPageData _data = res.data;

        Label category = new Label();
        TextElement title = new TextElement();
        VisualElement coinBox = new VisualElement();
        TextElement titleBox = new TextElement();
        VisualElement coinWrapper = new VisualElement();
        VisualElement coinIcon = new VisualElement();
        TextElement coinText = new TextElement();
        VisualElement accordionWrapper = new VisualElement();
        VisualElement accordionIcon = new VisualElement();
        TextElement accordionText = new TextElement();
        Button buttonMaske = new Button();
        VisualElement buttonMaskeIcon = new VisualElement();
        TextElement buttonMaskeText = new TextElement();
        Button buttonLegionar = new Button();
        VisualElement buttonLegionarIcon = new VisualElement();
        TextElement buttonLegionarText = new TextElement();
        VisualElement buttonsWrapper = new VisualElement();

        category.text = _data.attributes.pageTitle.ToUpper();
        category.AddToClassList("cms-belohnungen-category");
        this.m_fixedContent.Add(category);

        title.text = _data.attributes.headline.ToUpper();
        title.AddToClassList("headline-belohnungen");
        this.m_fixedContent.Add(title);

        titleBox.text = _data.attributes.totalCoinsHeadline;
        coinBox.Add(titleBox);
        this.m_fixedContent.Add(coinBox);

        coinWrapper.AddToClassList("cms-belohnungen-icon-wrapper");

        if (crossGameManager.score.coinsAmount == 0) {
            coinBox.AddToClassList("cms-belohnungen-coins");
            coinIcon.AddToClassList("cms-belohnungen-icon");

            accordionWrapper.AddToClassList("cms-belohnungen-accordion-wrapper-active");
            accordionIcon.AddToClassList("cms-belohnungen-accordion-icon-disabled");
            accordionWrapper.style.opacity = 0.75f;

            buttonMaske.AddToClassList("cms-belohnungen-button");
            buttonMaskeIcon.AddToClassList("cms-belohnungen-button-icon");
            buttonMaskeText.text = _data.attributes.maskButtonTextDisabled;
            buttonMaskeText.AddToClassList("cms-belohnungen-button-text");

            buttonLegionar.AddToClassList("cms-belohnungen-button");
            buttonLegionarIcon.AddToClassList("cms-belohnungen-button-icon");
            buttonLegionarText.text = _data.attributes.helmetButtonTextDisabled;
            buttonLegionarText.AddToClassList("cms-belohnungen-button-text");
        }

        if (crossGameManager.score.coinsAmount > 0) {
            coinBox.AddToClassList("cms-belohnungen-coins-active");
            coinIcon.AddToClassList("cms-belohnungen-icon-active");

            accordionWrapper.AddToClassList("cms-belohnungen-accordion-wrapper-active");
            accordionIcon.AddToClassList("cms-belohnungen-accordion-icon-active");

            buttonMaske.AddToClassList("cms-belohnungen-button-active");
            buttonMaskeIcon.AddToClassList("cms-belohnungen-button-icon-active");
            buttonMaskeText.text = _data.attributes.maskButtonTextEnabled;
            buttonMaskeText.AddToClassList("cms-belohnungen-button-text");
            buttonMaske.clicked += delegate {
                SceneManager.LoadScene("Scenes/ARFilterScene");
            };

            buttonLegionar.AddToClassList("cms-belohnungen-button-active");
            buttonLegionarIcon.AddToClassList("cms-belohnungen-button-icon-active");
            buttonLegionarText.text = _data.attributes.helmetButtonTextEnabled;
            buttonLegionarText.AddToClassList("cms-belohnungen-button-text");
            buttonLegionar.clicked += delegate {
                SceneManager.LoadScene("Scenes/ARFilterSceneHelmet");
            };
        }

        coinText.text = crossGameManager.score.coinsAmount.ToString();

        coinWrapper.Add(coinIcon);
        coinWrapper.Add(coinText);
        coinBox.Add(coinWrapper);

        accordionText.text = _data.attributes.dropdownHeadline;
        accordionWrapper.Add(accordionText);
        accordionWrapper.Add(accordionIcon);
        this.m_fixedContent.Add(accordionWrapper);

        buttonMaske.Add(buttonMaskeText);
        buttonMaske.Add(buttonMaskeIcon);

        buttonLegionar.Add(buttonLegionarText);
        buttonLegionar.Add(buttonLegionarIcon);

        buttonsWrapper.AddToClassList("cms-belohnungen-buttons-wrapper");
        buttonsWrapper.Add(buttonMaske);
        buttonsWrapper.Add(buttonLegionar);

        this.m_fixedContent.Add(buttonsWrapper);
    }

    async void Filter() {
        ScrollView BGscroller = new ScrollView();
        BGscroller.mode = ScrollViewMode.Vertical;
        BGscroller.style.width = Length.Percent(100);
        BGscroller.Q<VisualElement>("unity-slider").style.opacity = 0;
        BGscroller.Q<VisualElement>("unity-low-button").style.opacity = 0;
        BGscroller.Q<VisualElement>("unity-high-button").style.opacity = 0;
        BGscroller.Q<VisualElement>("unity-slider").style.opacity = 0;
        this.m_BGFilterListe.Add(BGscroller);


        VisualElement headerWrapper = new VisualElement();
        headerWrapper.AddToClassList("cms-filter-header-wrapper");
        filter = new Button();
        filter.AddToClassList("cms-filter-header-filter-button");
        filter.text = filterConfig_title.ToUpper();
        auswahlAnzeigen = new Button();
        auswahlAnzeigen.AddToClassList("cms-filter-header-auswahlAnzeigen-disabled");
        auswahlAnzeigen.text = filterConfig_display;
        headerWrapper.Add(filter);
        headerWrapper.Add(auswahlAnzeigen);
        BGscroller.Add(headerWrapper);

        auswahlAnzeigen.clicked += delegate {
            if (filter.text != "FILTER") {
                if (!isFilterPanelOpen) {
                    FilterClickBlocker.gameObject.SetActive(true);

                    if (isListePanelOpen) {
                        this.m_PanelListe.style.display = DisplayStyle.None;
                        isListePanelOpen = false;
                    }

                    this.m_BGFilterListe.style.display = DisplayStyle.Flex;
                    isFilterPanelOpen = true;
                    isOpen = false;
                } else if (isFilterPanelOpen) {
                    FilterClickBlocker.gameObject.SetActive(false);
                    ListeClickBlocker.gameObject.SetActive(false);

                    this.m_BGFilterListe.style.display = DisplayStyle.None;
                    isFilterPanelOpen = false;
                }
            }
        };

        filter.clicked += delegate {
            if (filter.text != "FILTER") {
                this.gameObject.GetComponent<filterController>().ActivateAllPins();
            }
        };

        VisualElement angewendeteFilterWrapper = new VisualElement();
        angewendeteFilterWrapper.AddToClassList("cms-filter-angewendete-wrapper");
        angewendeteFilter = new Label();
        angewendeteFilter.text = filterConfig_inUse.ToUpper();
        angewendeteFilterWrapper.Add(angewendeteFilter);
        filterTagsWrapper = new VisualElement();
        filterTagsWrapper.AddToClassList("cms-filter-tags-wrapper");
        angewendeteFilterWrapper.Add(filterTagsWrapper);
        BGscroller.Add(angewendeteFilterWrapper);

        allesAnzeigen = new Button();
        allesAnzeigen.AddToClassList("cms-filter-box-allesAnzeigen-enabled");
        allesAnzeigen.text = filterConfig_showAll;
        BGscroller.Add(allesAnzeigen);

        allesAnzeigen.clicked += delegate {
            if (filter.text == "FILTER") {
                if (!isFilterPanelOpen) {
                    FilterClickBlocker.gameObject.SetActive(true);

                    if (isListePanelOpen) {
                        this.m_PanelListe.style.display = DisplayStyle.None;
                        isListePanelOpen = false;
                    }

                    this.m_BGFilterListe.style.display = DisplayStyle.Flex;
                    isFilterPanelOpen = true;
                    isOpen = false;
                } else if (isFilterPanelOpen) {
                    FilterClickBlocker.gameObject.SetActive(false);
                    ListeClickBlocker.gameObject.SetActive(false);

                    this.m_BGFilterListe.style.display = DisplayStyle.None;
                    isFilterPanelOpen = false;
                }
            }
        };

        //FUNDE
        funde = new Button();
        funde.AddToClassList("cms-filter-box-funde-button");
        funde.text = "Funde";
        BGscroller.Add(funde);

        fundeDropdownIcon = new Button();
        fundeDropdownIcon.name = "fundeDropdownIcon";
        fundeDropdownIcon.AddToClassList("cms-filter-box-funde-dropdown-icon-closed");

        fundeWrapper = new VisualElement();
        fundeWrapper.AddToClassList("cms-filter-box-funde");
        fundeWrapper.AddToClassList("cms-filter-box-funde-dropdown-icon-wrapper");
        fundeWrapper.Add(funde);
        fundeWrapper.Add(fundeDropdownIcon);
        BGscroller.Add(fundeWrapper);

        fundeDropdown = new GroupBox();
        fundeDropdown.name = "fundeDropdown";
        fundeDropdown.AddToClassList("cms-filter-box-funde-dropdown");
        BGscroller.Add(fundeDropdown);

        fundeDropdownValue = false;
        fundeDropdownIcon.AddToClassList("cms-filter-box-funde-dropdown-icon-closed");
        fundeBoxValue = false;

        fundeDropdownIcon.clicked += delegate {
            if (!fundeDropdownValue) {
                fundeDropdown.style.display = DisplayStyle.Flex;

                if (fundeBoxValue) {
                    fundeDropdownIcon.RemoveFromClassList("cms-filter-box-funde-dropdown-icon-closed-selected");
                    fundeDropdownIcon.AddToClassList("cms-filter-box-funde-dropdown-icon-opened-selected");
                } else if (!fundeBoxValue) {
                    fundeDropdownIcon.RemoveFromClassList("cms-filter-box-funde-dropdown-icon-closed");
                    fundeDropdownIcon.AddToClassList("cms-filter-box-funde-dropdown-icon-opened");
                }

                fundeDropdownValue = true;
            } else if (fundeDropdownValue) {
                fundeDropdown.style.display = DisplayStyle.None;
                if (fundeBoxValue) {
                    fundeDropdownIcon.RemoveFromClassList("cms-filter-box-funde-dropdown-icon-closed-selected");
                    fundeDropdownIcon.AddToClassList("cms-filter-box-funde-dropdown-icon-closed-selected");
                } else if (!fundeBoxValue) {
                    fundeDropdownIcon.RemoveFromClassList("cms-filter-box-funde-dropdown-icon-opened");
                    fundeDropdownIcon.AddToClassList("cms-filter-box-funde-dropdown-icon-closed");
                }

                fundeDropdownValue = false;
            }
        };

        funde.clicked += delegate {
            //add tag in the filter section
            CMSFilterTag tag = new CMSFilterTag(funde, null, fundeDropdownIcon, fundeWrapper, filter, auswahlAnzeigen,
                allesAnzeigen, filterTagsWrapper, this);

            this.gameObject.GetComponent<filterController>().UpdateButtonDropdownUI();

            this.gameObject.GetComponent<filterController>().loadPoi("fundObjekt");

            fundeBoxValue = true;
        };


        Button panorama = new Button();
        panorama.AddToClassList("cms-filter-box-panorama");
        panorama.text = "Panorama-Ansichten";
        BGscroller.Add(panorama);
        panorama.clicked += delegate {
            //change box color 
            panorama.RemoveFromClassList("cms-filter-box-panorama");
            panorama.AddToClassList("cms-filter-box-panorama-selected");

            //add tag in the filter section
            CMSFilterTag tag = new CMSFilterTag(panorama, null, null, null, filter, auswahlAnzeigen, allesAnzeigen,
                filterTagsWrapper, this);

            //disable alles anzeigen and button
            panorama.SetEnabled(false);

            this.gameObject.GetComponent<filterController>().loadPoi("panorama");
        };

        Button spiele = new Button();
        spiele.AddToClassList("cms-filter-box-spiele");
        spiele.text = "Spiele";
        BGscroller.Add(spiele);
        spiele.clicked += delegate {
            //change box color 
            spiele.RemoveFromClassList("cms-filter-box-spiele");
            spiele.AddToClassList("cms-filter-box-spiele-selected");

            //add tag in the filter section
            CMSFilterTag tag = new CMSFilterTag(spiele, null, null, null, filter, auswahlAnzeigen, allesAnzeigen,
                filterTagsWrapper, this);

            //disable alles anzeigen and button
            spiele.SetEnabled(false);

            this.gameObject.GetComponent<filterController>().loadPoi("spiel");
        };

        Button touren = new Button();
        touren.AddToClassList("cms-filter-box-touren");
        touren.text = "Park-Touren";
        BGscroller.Add(touren);
        touren.clicked += delegate {
            //change box color 
            touren.RemoveFromClassList("cms-filter-box-touren");
            touren.AddToClassList("cms-filter-box-touren-selected");

            //add tag in the filter section
            CMSFilterTag tag = new CMSFilterTag(touren, null, null, null, filter, auswahlAnzeigen, allesAnzeigen,
                filterTagsWrapper, this);

            //disable alles anzeigen and button
            touren.SetEnabled(false);

            this.gameObject.GetComponent<filterController>().loadPoi("tour");
        };

        Button grabungen = new Button();
        grabungen.AddToClassList("cms-filter-box-grabungen-button");
        grabungen.text = "Grabungen";
        BGscroller.Add(grabungen);

        Button grabungenDropdownIcon = new Button();
        grabungenDropdownIcon.name = "grabungenDropdownIcon";
        grabungenDropdownIcon.AddToClassList("cms-filter-box-grabungen-dropdown-icon-closed");

        VisualElement grabungenWrapper = new VisualElement();
        grabungenWrapper.AddToClassList("cms-filter-box-grabungen");
        grabungenWrapper.AddToClassList("cms-filter-box-grabungen-dropdown-icon-wrapper");
        grabungenWrapper.Add(grabungen);
        //grabungenWrapper.Add(grabungenDropdownIcon);
        BGscroller.Add(grabungenWrapper);

        VisualElement grabungenDropdown = new VisualElement();
        grabungenDropdown.AddToClassList("cms-filter-box-grabungen-dropdown");
        BGscroller.Add(grabungenDropdown);

        //add radio button to dropdown box
        //CMSDropdownBox optGrabungen = new CMSDropdownBox("Grabungen", null);
        //grabungenDropdown.Add(optGrabungen);

        grabungenDropdownValue = false;
        grabungenBoxValue = false;

        grabungenDropdownIcon.clicked += delegate {
            if (!grabungenDropdownValue) {
                grabungenDropdown.style.display = DisplayStyle.Flex;

                if (grabungenBoxValue) {
                    grabungenDropdownIcon.RemoveFromClassList("cms-filter-box-grabungen-dropdown-icon-closed-selected");
                    grabungenDropdownIcon.AddToClassList("cms-filter-box-grabungen-dropdown-icon-opened-selected");
                } else {
                    grabungenDropdownIcon.AddToClassList("cms-filter-box-grabungen-dropdown-icon-opened");
                }

                grabungenDropdownValue = true;
            } else if (grabungenDropdownValue) {
                grabungenDropdown.style.display = DisplayStyle.None;
                if (grabungenBoxValue) {
                    grabungenDropdownIcon.RemoveFromClassList("cms-filter-box-grabungen-dropdown-icon-closed-selected");
                    grabungenDropdownIcon.AddToClassList("cms-filter-box-grabungen-dropdown-icon-closed-selected");
                } else {
                    grabungenDropdownIcon.RemoveFromClassList("cms-filter-box-grabungen-dropdown-icon-opened");
                    grabungenDropdownIcon.AddToClassList("cms-filter-box-grabungen-dropdown-icon-closed");
                }

                grabungenDropdownValue = false;
            }
        };

        grabungen.clicked += delegate {
            grabungen.RemoveFromClassList("cms-filter-box-grabungen-button");
            grabungen.AddToClassList("cms-filter-box-grabungen-button-selected");
            grabungenWrapper.RemoveFromClassList("cms-filter-box-grabungen");
            grabungenWrapper.AddToClassList("cms-filter-box-grabungen-selected");

            grabungenDropdownIcon.RemoveFromClassList("cms-filter-box-grabungen-dropdown-icon-closed");
            grabungenDropdownIcon.AddToClassList("cms-filter-box-grabungen-dropdown-icon-closed-selected");

            grabungenBoxValue = true;

            //add tag in the filter section
            CMSFilterTag tag = new CMSFilterTag(grabungen, null, grabungenDropdownIcon, grabungenWrapper, filter,
                auswahlAnzeigen, allesAnzeigen, filterTagsWrapper, this);

            //disable alles anzeigen and button
            grabungen.SetEnabled(false);

            this.gameObject.GetComponent<filterController>().loadPoi("grabung");
        };

        Button landschaft = new Button();
        landschaft.AddToClassList("cms-filter-box-landschaft");
        landschaft.text = "Landschaft";
        BGscroller.Add(landschaft);
        landschaft.clicked += delegate {
            //change box color 
            landschaft.RemoveFromClassList("cms-filter-box-landschaft");
            landschaft.AddToClassList("cms-filter-box-landschaft-selected");

            //add tag in the filter section
            CMSFilterTag tag = new CMSFilterTag(landschaft, null, null, null, filter, auswahlAnzeigen, allesAnzeigen,
                filterTagsWrapper, this);

            //disable alles anzeigen and button
            landschaft.SetEnabled(false);

            this.gameObject.GetComponent<filterController>().loadPoi("landschaft");
        };
    }

    async void Liste(StrapiSingleResponse<ListPageData> res) {
        this.m_PanelListe.Clear();

        BGscrollerListe = new ScrollView();
        BGscrollerListe.mode = ScrollViewMode.Vertical;
        BGscrollerListe.style.width = Length.Percent(100);
        BGscrollerListe.Q<VisualElement>("unity-slider").style.opacity = 0;
        BGscrollerListe.Q<VisualElement>("unity-low-button").style.opacity = 0;
        BGscrollerListe.Q<VisualElement>("unity-high-button").style.opacity = 0;
        BGscrollerListe.Q<VisualElement>("unity-slider").style.opacity = 0;

        ListPageData _data = res.data;

        Label title = new Label();
        title.AddToClassList("cms-filter-header-filter-button");
        title.style.marginTop = 100;
        title.style.marginBottom = 50;
        title.text = _data.attributes.pageTitle.ToUpper();
        this.m_PanelListe.Add(title);

        this.m_PanelListe.Add(BGscrollerListe);
    }

    async void loadArchaelogieMenuContent(StrapiSingleResponse<ArchaeologieMenuData> res) {
        crossGameManager.currentContent = "menu:archaeologie";

        this.m_fixedContent.style.display = DisplayStyle.None;
        this.m_scrollContent.style.display = DisplayStyle.Flex;

        this.m_scrollContent.RemoveFromClassList("cms-fragen-background-waves");
        // this.m_scrollContent.Q<ScrollView>().mode = ScrollViewMode.Vertical;
        // this.m_scrollContent.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
        // this.m_scrollContent.Q<ScrollView>().horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        this.m_fixedBar.style.display = DisplayStyle.Flex;
        this.m_fixedBar.Clear();
        this.m_scrollContent.Clear();
        this.gameObject.GetComponent<Swiper>().allowSwiper = false;

        TextElement category = new TextElement();
        TextElement aktuelles = new TextElement();
        TextElement highlights = new TextElement();
        TextElement grabungen = new TextElement();
        TextElement videoTitle = new TextElement();
        TextElement faq = new TextElement();
        VisualElement footer = new VisualElement();
        menuType = "archaologie";
        UpdateMenu(menuType);

        ArchaeologieMenuData _data = res.data;

        category.text = _data.attributes.pageTitle.ToUpper();
        category.AddToClassList("cms-arch-category");

        this.m_fixedBar.Add(category);

        aktuelles.text = _data.attributes.NewsHeadline.ToUpper();
        aktuelles.AddToClassList("cms-arch-headline");
        this.m_scrollContent.Add(aktuelles);

        this.m_scrollContent.Add(new CMSArchaologieHighlightsSlider(_data.attributes.Highlights, menuType, this));

        highlights.text = _data.attributes.HighlightsHeadline.ToUpper();
        highlights.AddToClassList("cms-arch-headline");
        this.m_scrollContent.Add(highlights);

        VisualElement wrapper = new VisualElement();
        wrapper.style.flexDirection = FlexDirection.Column;
        wrapper.Add(new CMSMenuGameSlider(_data.attributes.cards, _data.attributes.cardsHeadline, menuType, this));
        this.m_scrollContent.Add(wrapper);


        //video
        if (videoPlayer != null && videoPlayer.source != VideoSource.Url)
            videoPlayer.source = VideoSource.Url;

        foreach (VideoItem videoElement in _data.attributes.video) {
            this.m_scrollContent.Add(new CMSVideo(videoElement, this));
        }

        //Häufigen Fragen
        faq.text = _data.attributes.FAQHeadline.ToUpper();
        faq.AddToClassList("cms-arch-headline");
        this.m_scrollContent.Add(faq);

        //accordions
        foreach (AccordionItem accordionItem in _data.attributes.FAQAccordionItems) {
            this.m_scrollContent.Add(new CMSAccordionItem(accordionItem, this));
        }

        footer.AddToClassList("cms-arch-footer");
        this.m_scrollContent.Add(footer);

        StartCoroutine(crossGameManager.strapiService.getGrabungenPageContent(GrabungenPageContent));
    }

    async void GrabungenPageContent(StrapiSingleResponse<GrabungenPageData> res) {
        GrabungenPageData _data = res.data;

        grabungenPageContentTitle = _data.attributes.pageTitle;
        grabungenPageContentHeadline = _data.attributes.headline;
    }

    async void loadArchaelogieGrabungen(StrapiItemResponse res) {
        crossGameManager.currentContent = "menu:archaeologieGrabungen";
        menuType = "archaeologieGrabungen";
        this.m_scrollContent.Clear();
        this.m_scrollContent.RemoveFromClassList("cms-fragen-background-waves");
        //this.m_scrollContent.Q<ScrollView>().mode = ScrollViewMode.Vertical;
        //this.m_scrollContent.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
        //this.m_scrollContent.Q<ScrollView>().horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        this.m_fixedBar.style.display = DisplayStyle.Flex;
        this.m_fixedBar.Clear();
        this.gameObject.GetComponent<Swiper>().allowSwiper = false;

        TextElement category = new TextElement();
        TextElement headline = new TextElement();

        category.text = grabungenPageContentTitle.ToUpper();
        category.AddToClassList("cms-arch-category");

        this.m_fixedBar.Add(category);

        headline.text = grabungenPageContentHeadline.ToUpper();
        headline.AddToClassList("cms-arch-headline");
        this.m_scrollContent.Add(headline);

        this.m_scrollContent.Add(new CMSDecadeArchaologieGrabungen("1989-1999", res, this));
        this.m_scrollContent.Add(new CMSDecadeArchaologieGrabungen("2000-2009", res, this));
        this.m_scrollContent.Add(new CMSDecadeArchaologieGrabungen("2010-2019", res, this));
        this.m_scrollContent.Add(new CMSDecadeArchaologieGrabungen("2020-2029", res, this));
    }

    async void loadEntdeckerfragenMenuContent(StrapiSingleResponse<GameMenuData> res) {
        crossGameManager.currentContent = "entdeckerFragen:0";
        /*
        this.m_scrollContent.style.display = DisplayStyle.Flex;
        this.m_scrollContent.Clear();
        this.m_scrollContent.Q<ScrollView>("ScrollContent").mode = ScrollViewMode.Horizontal;
        this.m_scrollContent.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
        this.m_scrollContent.Q<ScrollView>().horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        this.m_scrollContent.AddToClassList("cms-fragen-background-waves");
        this.m_fixedBar.style.display = DisplayStyle.None;
        this.m_fixedContent.style.display = DisplayStyle.None;
        */

        this.m_fixedBar.style.display = DisplayStyle.None;
        this.m_scrollContent.style.display = DisplayStyle.None;
        this.m_fixedContent.style.display = DisplayStyle.Flex;
        this.m_fixedContent.Clear();
        this.m_fixedContent.AddToClassList("cms-fragen-background-waves");

        GameMenuData _data = res.data;

        menuType = "fragen";
        SetIconState("questions", "innactive");
        SetIconState("info", "innactive");
        this.m_Root.Q<VisualElement>("menu-return").style.display = DisplayStyle.None;
        this.m_Root.Q<VisualElement>("empty").style.display = DisplayStyle.None;

        VisualElement wrapper = new VisualElement();
        wrapper.style.flexDirection = FlexDirection.Column;

        wrapper.Add(new CMSMenuGameSlider(_data.attributes.cards, _data.attributes.pageTitle, menuType, this));
        this.m_fixedContent.Add(wrapper);
        //this.m_scrollContent.Add(wrapper);
    }

    public void setEntedeckerFrageTitle(string title, string type) {
        entedeckerFrageTitle = title;
        entedeckerFrageType = type;
    }

    async void loadContextQuestion(StrapiItemResponse res) {
        menuType = "contextQuestions";
        UpdateMenu(menuType);
        SetIconState("questions", "innactive");
        SetIconState("info", "innactive");
        this.m_scrollContent.style.display = DisplayStyle.Flex;
        this.m_scrollContent.Clear();
        this.m_scrollContent.Q<ScrollView>("ScrollContent").mode = ScrollViewMode.Vertical;
        this.m_scrollContent.RemoveFromClassList("cms-fragen-background-waves");
        this.m_fixedBar.style.display = DisplayStyle.None;
        this.m_fixedContent.style.display = DisplayStyle.None;

        TextElement headline = new TextElement();
        TextElement title = new TextElement();
        TextElement bodyText = new TextElement();
        this.item = res.data.Find(i => {
            return i.id == this.itemId;
        });

        foreach (Item item in res.data) {
            if (entedeckerFrageTitle == item.attributes.headline) {
                this.item = item;
            }
        }

        headline.text = entedeckerFrageTitle.ToUpper();
        headline.style.fontSize = 80;
        headline.style.marginBottom = 80;

        //button
        VisualElement btnWrapper = new VisualElement();
        btnWrapper.AddToClassList("cms-center-justifier-wrapper");
        Button btnLink = new Button();
        btnLink.text = this.item.attributes.button.text;
        btnLink.clicked += delegate {
            string[] targets = this.item.attributes.button.target.Split(":");
            navigate(targets[0], targets[1]);
        };
        VisualElement btnLinkIcon = new VisualElement();
        btnLink.Add(btnLinkIcon);
        btnWrapper.Add(btnLink);


        headline.AddToClassList("cms-menu-fragen-context-headline");
        headline.style.fontSize = 120;

        switch (entedeckerFrageType) {
            case "funde":
                headline.style.color = new Color(0.1098039f, 0.7019608f, 1f);
                btnLink.AddToClassList("funde-text");
                btnLink.AddToClassList("cms-menu-fragen-button-funde");
                btnLinkIcon.AddToClassList("cms-menu-fragen-button-icon-funde");
                break;

            case "welt":
                headline.style.color = new Color(0.3019608f, 0.1921569f, 0.6039216f);
                btnLink.AddToClassList("welt-text");
                btnLink.AddToClassList("cms-menu-fragen-button-welt");
                btnLinkIcon.AddToClassList("cms-menu-fragen-button-icon-welt");
                break;

            case "touren":
                headline.style.color = new Color(0.3137255f, 0.7568628f, 0.1647059f);
                btnLink.AddToClassList("touren-text");
                btnLink.AddToClassList("cms-menu-fragen-button-touren");
                btnLinkIcon.AddToClassList("cms-menu-fragen-button-icon-touren");
                break;

            case "ausgrabungen":
                headline.style.color = new Color(0.1529412f, 0.1882353f, 1f);
                btnLink.AddToClassList("ausgrabungen-text");
                btnLink.AddToClassList("cms-menu-fragen-button-ausgrabungen");
                btnLinkIcon.AddToClassList("cms-menu-fragen-button-icon-ausgrabungen");
                break;

            case "spiele":
                headline.style.color = new Color(0.9921569f, 0.5137255f, 0f);
                btnLink.AddToClassList("spiele-text");
                btnLink.AddToClassList("cms-menu-fragen-button-spiele");
                btnLinkIcon.AddToClassList("cms-menu-fragen-button-icon-spiele");
                break;
        }


        title.text = this.item.attributes.headline;
        title.AddToClassList("cms-item-fragen-title");
        bodyText.text = this.item.attributes.bodyText;


        Button btn2 = new Button();
        btn2.clicked += delegate {
            navigate("error", null);
        };
        VisualElement btn2Icon = new VisualElement();

        btn2.text = this.item.attributes.buttonText;
        btn2.AddToClassList("cms-item-fragen-btn2");
        btn2Icon.AddToClassList("cms-item-fragen-btn2-icon");
        btn2.Add(btn2Icon);

        this.m_scrollContent.Add(headline);
        if (this.item.attributes.button.text != null) {
            this.m_scrollContent.Add(btnWrapper);
        }

        this.m_scrollContent.Add(title);
        this.m_scrollContent.Add(bodyText);
        //this.m_scrollContent.Add(btn2);

        //links
        foreach (Link attributesLink in this.item.attributes.links) {
            this.m_scrollContent.Add(new CMSLinkBox(attributesLink, this));
        }
    }

    async void loadInfoMenuContent(StrapiSingleResponse<HilfeMenuData> res) {
        crossGameManager.currentContent = "info:0";
        this.m_fixedContent.style.display = DisplayStyle.None;
        this.m_scrollContent.style.display = DisplayStyle.Flex;
        this.m_scrollContent.Clear();
        this.m_scrollContent.RemoveFromClassList("cms-fragen-background-waves");
        this.m_scrollContent.Q<ScrollView>().mode = ScrollViewMode.Vertical;
        this.m_scrollContent.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
        this.m_scrollContent.Q<ScrollView>().horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        this.m_fixedBar.style.display = DisplayStyle.Flex;
        this.m_fixedBar.Clear();

        menuType = "menuOnOpenPosition";
        UpdateMenu(menuType);
        SetIconState("info", "active");
        SetIconState("questions", "innactive");

        appPermissions = this.gameObject.GetComponent<AppPermissions>();

        HilfeMenuData _data = res.data;
      
        TextElement category = new TextElement();
        TextElement title = new TextElement();
        TextElement title2 = new TextElement();
        TextElement bodyText = new TextElement();
        TextElement appTitlePt2 = new TextElement();
        TextElement appTitlePt3 = new TextElement();
        VisualElement logoWrapper = new VisualElement();
        VisualElement appLogo = new VisualElement();
        VisualElement varusLogo = new VisualElement();
        TextElement impressum = new TextElement();
        VisualElement varus = new VisualElement();
        Button appNeuStartBTN = new Button();
        Button clearCacheBtn = new Button();
        Button standortBTN = new Button();
        Button kameraBTN = new Button();
        Button einfuehrungBTN = new Button();
        VisualElement btnIcon1 = new VisualElement();
        VisualElement btnIcon2 = new VisualElement();
        VisualElement btnIcon3 = new VisualElement();
        VisualElement btnIcon4 = new VisualElement();
        VisualElement btnIcon5 = new VisualElement();
        Slider slider = new Slider();
        VisualElement sliderWrapper = new VisualElement();
        Slider sliderDaten = new Slider();
        VisualElement sliderDatenWrapper = new VisualElement();
        Slider sliderDiagnose = new Slider();
        VisualElement sliderDiagnoseWrapper = new VisualElement();

        category.text = _data.attributes.pageTitle.ToUpper();
        title.text = _data.attributes.headline.ToUpper();
        title2.text = _data.attributes.FAQHeadline.ToUpper();

        

        //buttons

        clearCacheBtn.AddToClassList("cms-hilfe-btn");
        clearCacheBtn.text = _data.attributes.restartButton;
        btnIcon5.AddToClassList("cms-hilfe-btn-icon");
        clearCacheBtn.Add(btnIcon5);
        clearCacheBtn.clicked += delegate {
            crossGameManager.ErrorLog("tries to delete cache");
            crossGameManager.strapiService.cachingService.clearCache();
            crossGameManager.ReStart();
            crossGameManager.ErrorLog("runs clearcache no error");
            crossGameManager.score.coinsAmount = 0;
        };

        /*
        appNeuStartBTN.AddToClassList("cms-hilfe-btn");
        appNeuStartBTN.text = _data.attributes.restartButton;
        btnIcon1.AddToClassList("cms-hilfe-btn-icon");
        appNeuStartBTN.Add(btnIcon1);
        appNeuStartBTN.clicked += delegate {
            //ReStartGame();
        };
        */

        standortBTN.AddToClassList("cms-hilfe-btn");
        standortBTN.text = _data.attributes.locationAdmisionButton;
        btnIcon2.AddToClassList("cms-hilfe-btn-icon");
        standortBTN.Add(btnIcon2);

        kameraBTN.AddToClassList("cms-hilfe-btn");
        kameraBTN.text = _data.attributes.cameraAdmissionButton;
        btnIcon3.AddToClassList("cms-hilfe-btn-icon");
        kameraBTN.Add(btnIcon3);
        kameraBTN.clicked += delegate {
            appPermissions.RquestCameraAccess();
        };

        einfuehrungBTN.AddToClassList("cms-hilfe-btn");
        einfuehrungBTN.text = _data.attributes.tutorialButton;
        btnIcon4.AddToClassList("cms-hilfe-btn-icon");
        einfuehrungBTN.Add(btnIcon4);
        einfuehrungBTN.clicked += delegate {
            tutorial.SetActive(true);
            tutorial.GetComponent<TutorialController>().StartTutorial();
        };

        //slider
        slider.Q("unity-tracker").AddToClassList("cms-hilfe-slider-tracker");
        slider.Q("unity-dragger").AddToClassList("cms-hilfe-slider-dragger");
        slider.AddToClassList("cms-hilfe-slider");
        sliderWrapper.AddToClassList("cms-hilfe-slider-wrapper");
        TextElement sliderText2 = new TextElement();
        sliderText2.text = "Push-Nachrichten inaktiv";
        sliderWrapper.Add(slider);
        sliderWrapper.Add(sliderText2);

        sliderDaten.Q("unity-tracker").AddToClassList("cms-hilfe-slider-tracker");
        sliderDaten.Q("unity-dragger").AddToClassList("cms-hilfe-slider-dragger");
        sliderDaten.AddToClassList("cms-hilfe-slider");
        sliderDatenWrapper.AddToClassList("cms-hilfe-slider-wrapper");
        TextElement sliderDatenText1 = new TextElement();
        sliderDatenText1.text = "Analysedaten mit Entwicklern der App teilen";
        sliderDatenText1.style.width = 500;
        sliderDatenWrapper.Add(sliderDaten);
        sliderDatenWrapper.Add(sliderDatenText1);

        sliderDiagnose.Q("unity-tracker").AddToClassList("cms-hilfe-slider-tracker");
        sliderDiagnose.Q("unity-dragger").AddToClassList("cms-hilfe-slider-dragger");
        sliderDiagnose.AddToClassList("cms-hilfe-slider");
        sliderDiagnoseWrapper.AddToClassList("cms-hilfe-slider-wrapper");
        TextElement sliderDiagnoseText1 = new TextElement();
        sliderDiagnoseText1.text = "Diagnose- und Nutzungsdaten mit Entwicklern der App teilen";
        sliderDiagnoseText1.style.width = 500;
        sliderDiagnoseWrapper.Add(sliderDiagnose);
        sliderDiagnoseWrapper.Add(sliderDiagnoseText1);

        category.AddToClassList("cms-info-category");
        title.AddToClassList("cms-info-headline");
        title2.AddToClassList("cms-info-headline");

        this.m_fixedBar.Add(category);
        this.m_scrollContent.Add(title);

        //this.m_scrollContent.Add(appNeuStartBTN);
        this.m_scrollContent.Add(clearCacheBtn);
        //this.m_scrollContent.Add(standortBTN);
        //this.m_scrollContent.Add(kameraBTN);
        this.m_scrollContent.Add(einfuehrungBTN);

        VisualElement spacer = new VisualElement();
        spacer.style.width = Length.Percent(100);
        spacer.style.height = 100;
        this.m_scrollContent.Add(spacer);

        //this.m_scrollContent.Add(sliderWrapper);
        this.m_scrollContent.Add(title2);

        appLogo.AddToClassList("cms-app-logo");

        logoWrapper.AddToClassList("cms-app-logo-wrapper");
        logoWrapper.style.marginBottom = 100;
        logoWrapper.style.marginTop = 100;
        logoWrapper.Add(appLogo);

        appTitlePt2.text = "varus archäologie park".ToUpper();
        appTitlePt2.AddToClassList("cms-app-title-part2");
        appTitlePt3.text = "Unseren Park mit \nder VAR - APP \nentdecken";
        appTitlePt3.AddToClassList("cms-app-title-part3");

        foreach (AccordionItem accordionItem in _data.attributes.AccordionItem) {
            this.m_scrollContent.Add(new CMSAccordionItem(accordionItem, this));
        }

        //this.m_scrollContent.Add(sliderDatenWrapper);
        //this.m_scrollContent.Add(sliderDiagnoseWrapper);

        this.m_scrollContent.Add(logoWrapper);
        this.m_scrollContent.Add(appTitlePt2);
        this.m_scrollContent.Add(appTitlePt3);

        Label imprint = new Label();
        imprint.text = "Impressum".ToUpper();
        imprint.AddToClassList("cms-menu-info-label");
        this.m_scrollContent.Add(imprint);

        impressum.text = _data.attributes.impressum;
        this.m_scrollContent.Add(impressum);

        varus.AddToClassList("cms-logo-varus");
        this.m_scrollContent.Add(varus);
    }

    #endregion

    #region Menu

    public void mainButtonOnClick(string direction) {
        //disable icons
        Sprite innactive_filter = Resources.Load<Sprite>("menu/icons/innactive/filter");
        Sprite innactive_liste = Resources.Load<Sprite>("menu/icons/innactive/liste");
        m_Liste.style.backgroundImage = new StyleBackground(innactive_liste);
        m_Filter.style.backgroundImage = new StyleBackground(innactive_filter);

        if (GameObject.FindGameObjectWithTag("MapMarker")) {
            POIs = GameObject.FindGameObjectsWithTag("MapMarker");
        }

        if (GameObject.FindGameObjectWithTag("ClickBlocker")) {
            Destroy(GameObject.FindGameObjectWithTag("ClickBlocker"));
        }


        if (direction == "Down") {
            DisablePinsClick(true);

            m_Panel.style.translate = new Translate(0, Length.Percent(78), 0);

            isOpen = false;
            menuType = "menuOnClosedPosition";


        } else if (direction == "Up") {
            DisablePinsClick(false);

            m_Panel.style.translate = new Translate(0, Length.Percent(0), 0);

            isOpen = true;
            menuType = "menuOnOpenPosition";
            this.gameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Menu").style.display = DisplayStyle.Flex;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("menu-filter").style.display = DisplayStyle.None;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("menu-liste").style.display = DisplayStyle.None;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("detectorButton").style.display = DisplayStyle.None;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("menu-360").style.display = DisplayStyle.None;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("menu-info").style.display = DisplayStyle.Flex;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("menu-return").style.display = DisplayStyle.None;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("empty").style.display = DisplayStyle.None;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("menu-questions").style.display = DisplayStyle.Flex;

            m_BGFilterListe.style.display = DisplayStyle.None;
            isFilterPanelOpen = false;
            m_PanelListe.style.display = DisplayStyle.None;
            isListePanelOpen = false;

            if (ListeClickBlocker.gameObject.active) {
                ListeClickBlocker.gameObject.SetActive(false);
            }

            if (FilterClickBlocker.gameObject.active) {
                FilterClickBlocker.gameObject.SetActive(false);
            }

            if (isDetectorOpen) {
                Detector("close");
            }

            if (allowMainMenu) {
                //this.loadMainMenu();
                navigate("entdeckerFragen", "");
            }

            if (SceneManager.GetActiveScene().name == "FindTool" || showHighlightsMenu) {
                navigate("highlights", null);
            }

            allowMainMenu = true;
        }

        if (crossGameManager.currentContent != "") {
            string[] targets = crossGameManager.currentContent.Split(":");
            if (targets[0] == "info") {
                SetIconState("info", "active");
            } else if (targets[0] == "entdeckerFragen") {
                SetIconState("questions", "innactive");
            }
        }

        UpdateMenu(menuType);
    }

    public void MenuOnDownPosition() {
        m_Panel.style.translate = new Translate(0, Length.Percent(78), 0);

        this.m_Root.Q("menu-filter").style.display = DisplayStyle.Flex;
        this.m_Root.Q("menu-liste").style.display = DisplayStyle.Flex;
        this.m_Root.Q("detectorButton").style.display = DisplayStyle.Flex;
        this.m_Root.Q("menu-360").style.display = DisplayStyle.Flex;

        this.m_Root.Q("empty").style.display = DisplayStyle.None;
        this.m_Root.Q("menu-info").style.display = DisplayStyle.None;
        this.m_Root.Q("menu-questions").style.display = DisplayStyle.None;
        this.m_Root.Q("mainButton-AR").style.display = DisplayStyle.None;
        this.m_Root.Q("closeAR").style.display = DisplayStyle.None;
        this.m_Root.Q("menu-info-enabled").style.display = DisplayStyle.None;
    }

    async void UpdateMenu(string menuType) {

        if (menuType == "menuOnOpenPosition") {

            m_Panel.style.translate = new Translate(0, Length.Percent(0), 0);

            this.gameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Menu").style.display = DisplayStyle.Flex;

            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("menu-filter").style.display = DisplayStyle.None;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("menu-liste").style.display = DisplayStyle.None;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("detectorButton").style.display = DisplayStyle.None;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("menu-360").style.display = DisplayStyle.None;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("menu-info").style.display = DisplayStyle.Flex;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("menu-return").style.display = DisplayStyle.None;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("empty").style.display = DisplayStyle.None;
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("menu-questions").style.display = DisplayStyle.Flex;

            SetIconState("info", "innactive");
            SetIconState("questions", "innactive");

            DisablePinsClick(false);
        } else if (menuType == "menuOnClosedPosition") {
            m_Panel.style.translate = new Translate(0, Length.Percent(78), 0);

            this.m_Root.Q<VisualElement>("menu-filter").style.display = DisplayStyle.Flex;
            this.m_Root.Q<VisualElement>("menu-liste").style.display = DisplayStyle.Flex;
            this.m_Root.Q<VisualElement>("detectorButton").style.display = DisplayStyle.Flex;
            this.m_Root.Q<VisualElement>("menu-360").style.display = DisplayStyle.Flex;
            this.m_Root.Q<VisualElement>("menu-info").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("menu-return").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("empty").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("menu-questions").style.display = DisplayStyle.None;

            SetIconState("info", "innactive");
            SetIconState("questions", "innactive");

            DisablePinsClick(true);
            /*
            if (SceneManager.GetActiveScene().name == "MainScene") {
                foreach (Transform child in ARSession.transform) {
                    if (child.gameObject.tag == "MapMarker") {
                        child.gameObject.SetActive(true);
                    }
                }
            }
            */
        } else if (menuType == "game" || menuType == "touren" || menuType == "archaologie" ||
                   menuType == "belohnungen" || menuType == "contextQuestions") {
            m_Panel.style.translate = new Translate(0, Length.Percent(0), 0);

            this.m_Root.Q<VisualElement>("Menu").style.display = DisplayStyle.Flex;
            this.m_Root.Q<VisualElement>("menu-filter").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("menu-liste").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("detectorButton").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("menu-360").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("menu-info").style.display = DisplayStyle.Flex;
            this.m_Root.Q<VisualElement>("menu-return").style.display = DisplayStyle.Flex;
            this.m_Root.Q<VisualElement>("empty").style.display = DisplayStyle.Flex;
            this.m_Root.Q<VisualElement>("menu-questions").style.display = DisplayStyle.Flex;

            SetIconState("info", "innactive");
            SetIconState("questions", "innactive");

            DisablePinsClick(false);
            /*
            if (SceneManager.GetActiveScene().name == "MainScene") {
                foreach (Transform child in ARSession.transform) {
                    if (child.gameObject.tag == "MapMarker") {
                        child.gameObject.SetActive(false);
                    }
                }
            }
            */
        }

        this.m_scrollContent.Q<ScrollView>("ScrollContent").mode = ScrollViewMode.Vertical;
    }

    public void DisablePinsClick(bool status) {
        if (SceneManager.GetActiveScene().name == "MainScene") {
            foreach (Transform child in ARSession.transform) {
                if (child.gameObject.tag == "MapMarker") {
                    child.gameObject.SetActive(status);
                }
            }
        }
    }

    IEnumerator DisablePois() {
        yield return new WaitForEndOfFrame();
        mainButtonOnClick("Up");
    }


    /*
    IEnumerator StopSlider(ScrollView slider, int value) {
        Debug.Log("hallo entrei");
        yield return new WaitForSeconds(1f);
        slider.horizontalScroller.valueChanged += (v) => {
            slider.horizontalScroller.value = value;
        };
    }
    */

    #endregion

    #region MenuIcons

    public void SetIconState(string icon, string state) {
        if (icon == "filter") {
            Sprite active_filter = Resources.Load<Sprite>("menu/icons/active/filter");
            Sprite innactive_filter = Resources.Load<Sprite>("menu/icons/innactive/filter");

            if (state == "active") {
                m_Filter.style.backgroundImage = new StyleBackground(active_filter);
            } else if (state == "innactive") {
                m_Filter.style.backgroundImage = new StyleBackground(innactive_filter);
            }
        }

        if (icon == "liste") {
            Sprite active_liste = Resources.Load<Sprite>("menu/icons/active/liste");
            Sprite innactive_liste = Resources.Load<Sprite>("menu/icons/innactive/liste");

            if (state == "active") {
                m_Liste.style.backgroundImage = new StyleBackground(active_liste);
            } else if (state != "innacive") {
                m_Liste.style.backgroundImage = new StyleBackground(innactive_liste);
            }
        }

        if (icon == "detector") {
            Sprite active_detector = Resources.Load<Sprite>("menu/icons/active/detector");
            Sprite innactive_detector = Resources.Load<Sprite>("menu/icons/innactive/detector");

            if (state == "active") {
                m_Detektor.style.backgroundImage = new StyleBackground(active_detector);
            } else if (state == "innactive") {
                m_Detektor.style.backgroundImage = new StyleBackground(innactive_detector);
            }
        }

        if (icon == "info") {
            Sprite active_info = Resources.Load<Sprite>("menu/icons/active/info");
            Sprite innactive_info = Resources.Load<Sprite>("menu/icons/innactive/info");

            if (state == "active") {
                m_InfoButton.style.backgroundImage = new StyleBackground(active_info);
            } else if (state == "innactive") {
                m_InfoButton.style.backgroundImage = new StyleBackground(innactive_info);
            }
        }

        if (icon == "questions") {
            Sprite active_questions = Resources.Load<Sprite>("menu/icons/active/questions");
            Sprite innactive_questions = Resources.Load<Sprite>("menu/icons/innactive/questions");

            if (state == "active") {
                m_QuestionsButton.style.backgroundImage = new StyleBackground(active_questions);
            } else if (state == "innactive") {
                m_QuestionsButton.style.backgroundImage = new StyleBackground(innactive_questions);
            }
        }
    }

    #endregion
    async void SetFilterConfiguration(StrapiSingleResponse<FilterPageData> res) {
        FilterPageData _data = res.data;

        filterConfig_title = _data.attributes.header.filter;
        filterConfig_delete = _data.attributes.header.filterDelete;
        filterConfig_display = _data.attributes.header.display;
        filterConfig_inUse = _data.attributes.header.inUse;
        filterConfig_showAll = _data.attributes.showAllOption;

        LoadFilterContent();
    }

    void LoadFilterContent() {
        filter.text = filterConfig_title.ToUpper();
        auswahlAnzeigen.text = filterConfig_display;
        allesAnzeigen.text = filterConfig_showAll;
        angewendeteFilter.text = filterConfig_inUse.ToUpper();
    }

    async void SetConfiguration(StrapiSingleResponse<ConfigurationData> res) {
        ConfigurationData _data = res.data;
        crossGameManager.configurationData = _data;

        highlights_configuration = _data.attributes.highlights;
        readless_configuration = _data.attributes.read_less;
        readmore_configuration = _data.attributes.read_more;
    }

    private void ReStartGame() {
        crossGameManager.PinsFromPointOfInterest.Clear();
        crossGameManager.AllItemsOnMap.Clear();
        crossGameManager.Tours.Clear();
        crossGameManager.Game4PhaseStorage.Clear();
        crossGameManager.IsVisitingFromPOI = false;
        crossGameManager.IsVisitingFromTour = false;
        crossGameManager.IsVisitingFromGame = false;
        crossGameManager.IsVisitingFromGame4AfterCompleted = false;
        crossGameManager.IsVisitingFromDetectorToFilter = false;
        crossGameManager.IsVisitingFromDetectorToListe = false;
        crossGameManager.IsVisitingFromDetector = false;
        crossGameManager.IsVisitingFrom360 = false;
        crossGameManager.IsVisitingFromMask = false;
        crossGameManager.currentContent = "";
        crossGameManager.LastSceneVisited = "";
        crossGameManager.ScoreToAdd = 0;
        crossGameManager.hasCompletedGame1 = false;
        crossGameManager.hasCompletedGame2 = false;
        crossGameManager.hasCompletedGame3 = false;
        crossGameManager.hasCompletedGame4 = false;
        crossGameManager.hasCompletedGame5 = false;

        //not working
        crossGameManager.ReStart();
    }
}