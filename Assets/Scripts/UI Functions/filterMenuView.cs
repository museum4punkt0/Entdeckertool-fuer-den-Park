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

public class filterMenuView: MonoBehaviour {

    #region Variables
    CrossGameManager crossGameManager;
    public VisualElement m_Root;
    public VisualElement m_Panel;
    public VisualElement m_scrollContent;
    public VisualElement m_fixedBar;
	public VisualElement m_fixedContent;
    public VisualElement m_BGFilterListe;
    public VisualElement m_PanelListe;
    public VisualElement m_PanelItem;
    public Button filter;
    public string menuType;
    public Button m_Filter;
 
    public bool isFilterPanelOpen;
    public bool isFirstTimeUse;
    public bool isOpen;
    public bool resetFilter;

    public GameObject FilterClickBlocker;
    public Button funde;
    public Button auswahlAnzeigen;
    public Button allesAnzeigen;
    public VisualElement filterTagsWrapper;

    public List<ARFilterDataInput> tags = new List<ARFilterDataInput>();

    public List<ARFilterDataInput> tagsInView;

    public List<Button> buttons = new List<Button>();

    #endregion

    public filterMenuView() {
    }

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    async void Start() {
        this.m_Root = GetComponent<UIDocument>().rootVisualElement;

        //Search the root for the SlotContainer Visual Element
        this.m_scrollContent = this.m_Root.Q<VisualElement>("ScrollContent");
        this.m_BGFilterListe = this.m_Root.Q<VisualElement>("PanelFilter");
        this.m_PanelListe = this.m_Root.Q<VisualElement>("PanelListe");
        this.m_PanelItem = this.m_Root.Q<VisualElement>("PanelItem");
        this.m_fixedBar = this.m_Root.Q<VisualElement>("FixedBar");
		this.m_fixedContent = this.m_Root.Q<VisualElement>("FixedContent");

        menuType = "menuOnClosedPosition";

  

        this.m_scrollContent.Q<VisualElement>("unity-slider").style.opacity = 0;
        this.m_scrollContent.Q<VisualElement>("unity-low-button").style.opacity = 0;
        this.m_scrollContent.Q<VisualElement>("unity-high-button").style.opacity = 0;
        this.m_scrollContent.Q<VisualElement>("unity-slider").style.opacity = 0;
        this.m_scrollContent.style.flexDirection = FlexDirection.Column;

        menuClick();

        m_Panel = this.m_Root.Q("Panel");

        isFilterPanelOpen = false;

        Filter();
        //UpdateMenu(menuType);

        if (isFirstTimeUse) {
            isOpen = true;
            //StartCoroutine(crossGameManager.strapiService.getEntdeckerfragenMenuContent(loadEntdeckerfragenMenuContent));
            //this.gameObject.GetComponent<Swiper>().FirstTimeUse = true;
            isFirstTimeUse = false;

            this.m_Root.Q<VisualElement>("Menu").style.display = DisplayStyle.Flex;
            this.m_Root.Q<VisualElement>("menu-filter").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("menu-liste").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("detectorButton").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("menu-360").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("menu-info").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("menu-return").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("empty").style.display = DisplayStyle.None;
            this.m_Root.Q<VisualElement>("menu-questions").style.display = DisplayStyle.None;
            //SetIconState("info", "innactive");
            //SetIconState("questions", "active");
            m_Panel.style.translate = new Translate(0, Length.Percent(40), 0);
            menuType = "";
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

   
 
    public void menuClick() {
        Sprite active_filter = Resources.Load<Sprite>("menu/icons/active/filter");
        Sprite innactive_filter = Resources.Load<Sprite>("menu/icons/innactive/filter");
        Sprite active_liste = Resources.Load<Sprite>("menu/icons/active/liste");
        Sprite innactive_liste = Resources.Load<Sprite>("menu/icons/innactive/liste");





            //filter
            m_Filter = this.m_Root.Q<Button>("menu-filter");
            m_Filter.clicked += delegate {
                if (!isFilterPanelOpen) {
                    FilterClickBlocker.gameObject.SetActive(true);

              
                    this.m_BGFilterListe.style.display = DisplayStyle.Flex;
                    isFilterPanelOpen = true;
                    isOpen = false;
                    resetFilter = false;

                    m_Filter.style.backgroundImage = new StyleBackground(active_filter);


                } else if (isFilterPanelOpen) {
                    FilterClickBlocker.gameObject.SetActive(false);

                    this.m_BGFilterListe.style.display = DisplayStyle.None;
                    isFilterPanelOpen = false;

                    m_Filter.style.backgroundImage = new StyleBackground(innactive_filter);

                    resetFilter = true;
                    //this.gameObject.GetComponent<filterController>().ActivateAllPins();

                }
            };

          

        



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
        filter.text = "filter".ToUpper();
        auswahlAnzeigen = new Button();
        auswahlAnzeigen.AddToClassList("cms-filter-header-auswahlAnzeigen-disabled");
        auswahlAnzeigen.text = "Auswahl anzeigen";
        headerWrapper.Add(filter);
        headerWrapper.Add(auswahlAnzeigen);
        BGscroller.Add(headerWrapper);

        auswahlAnzeigen.clicked += delegate {
            if (filter.text != "FILTER") {

                if (!isFilterPanelOpen) {
                    FilterClickBlocker.gameObject.SetActive(true);

             
                    this.m_BGFilterListe.style.display = DisplayStyle.Flex;
                    isFilterPanelOpen = true;
                    isOpen = false;

                    

                } else if (isFilterPanelOpen) {
                    FilterClickBlocker.gameObject.SetActive(false);

                    this.m_BGFilterListe.style.display = DisplayStyle.None;
                    isFilterPanelOpen = false;

                    //toggleParticlesWithTags(tagsInView, true);
                    ShowParticlesWithTags(tagsInView);
                }
            }
        };

        //filter.clicked += delegate {
        //    if (filter.text != "FILTER") {
        //        this.gameObject.GetComponent<ARTagFilterController>().ActivateAllPins();
        //    }
        //};

        VisualElement angewendeteFilterWrapper = new VisualElement();
        angewendeteFilterWrapper.AddToClassList("cms-filter-angewendete-wrapper");
        Label angewendeteFilter = new Label();
        angewendeteFilter.text = "Angewendete filter".ToUpper();
        angewendeteFilterWrapper.Add(angewendeteFilter);
        filterTagsWrapper = new VisualElement();
        filterTagsWrapper.AddToClassList("cms-filter-tags-wrapper");
        angewendeteFilterWrapper.Add(filterTagsWrapper);
        BGscroller.Add(angewendeteFilterWrapper);

        allesAnzeigen = new Button();
        allesAnzeigen.AddToClassList("cms-filter-box-allesAnzeigen-enabled");
        allesAnzeigen.text = "Alles anzeigen";
        BGscroller.Add(allesAnzeigen);

        allesAnzeigen.clicked += delegate {
       
                if (!isFilterPanelOpen) {
                    FilterClickBlocker.gameObject.SetActive(true);

              
                    this.m_BGFilterListe.style.display = DisplayStyle.Flex;
                    isFilterPanelOpen = true;
                    isOpen = false;

                } else if (isFilterPanelOpen) {
                    FilterClickBlocker.gameObject.SetActive(false);

                    this.m_BGFilterListe.style.display = DisplayStyle.None;
                    isFilterPanelOpen = false;

                    ShowParticlesWithTags(tags);

                }
        };



        foreach (ARFilterDataInput tag in tags) {

            Button button = new Button();
            buttons.Add(button);
            button.name = tag.name;
            button.AddToClassList("cms-filter-box");
            button.text = tag.name;
            BGscroller.Add(button);
            Color t_color = tag.color;


            button.style.backgroundColor = new Color(255,255,255,0);
            button.style.borderTopColor = t_color;
            button.style.borderRightColor = t_color;
            button.style.borderLeftColor = t_color;
            button.style.borderBottomColor = t_color;


            button.clicked += delegate {
                //change box color 
                button.RemoveFromClassList("cms-filter-box");
                button.AddToClassList("cms-filter-box-selected");

                button.style.backgroundColor = t_color;

                //add tag in the filter section
                ARFilterTag _tag = new ARFilterTag(button, null, null, null, filter, auswahlAnzeigen, allesAnzeigen, filterTagsWrapper, this);

                tagsInView.Add(tag);

                //disable alles anzeigen and button
                button.SetEnabled(false);

            };
        }


    
    }

    #endregion

    public void toggleParticlesWithTags(List<ARFilterDataInput> tags, bool shouldTurnOn) {

        foreach (ARFilterDataInput tag in tags) {
            if (GameObject.FindGameObjectsWithTag(tag.targetTag).Length > 0) {

                GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag.targetTag);

                Debug.Log("has objects with tag" + objectsWithTag.Length);

                for (int i = 0; i < objectsWithTag.Length; i++) {

                    if (objectsWithTag[i].GetComponent<ParticleSystem>()) {
                        if (shouldTurnOn) {
                            objectsWithTag[i].GetComponent<ParticleSystem>().Play();

                            Debug.Log("plays particle system" + objectsWithTag[i].GetComponent<ParticleSystem>());
                        } else {
                            objectsWithTag[i].GetComponent<ParticleSystem>().Stop();
                            Debug.Log("stops particle system" + objectsWithTag[i].GetComponent<ParticleSystem>());

                        }

                    }

                }
            }
        }





    }


    public void ShowParticlesWithTags(List<ARFilterDataInput> tagsInView ) {

        foreach (ARFilterDataInput tag in tags) {

            ARFilterDataInput tagInViewList = tagsInView.Find(item => item.name == tag.name);

            if (tagInViewList != null) {
                if (GameObject.FindGameObjectsWithTag(tag.targetTag).Length > 0) {

                    GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag.targetTag);

                    Debug.Log("has objects with tag" + objectsWithTag.Length);

                    for (int i = 0; i < objectsWithTag.Length; i++) {

                        if (objectsWithTag[i].GetComponent<ParticleSystem>()) {

                            if (!objectsWithTag[i].GetComponent<ParticleSystem>().isPlaying) {
                                objectsWithTag[i].transform.position = new Vector3(objectsWithTag[i].transform.position.x, -100f, objectsWithTag[i].transform.position.z);
                            }

                            objectsWithTag[i].GetComponent<ParticleSystem>().Play();

                            Debug.Log("plays particle system" + objectsWithTag[i].GetComponent<ParticleSystem>());

                        } else if (objectsWithTag[i].transform.childCount > 0) {
                            for (int x = 0; i < objectsWithTag[i].transform.childCount; x++) {
                                GameObject GO = objectsWithTag[i].transform.GetChild(x).gameObject;
                                if (GO.GetComponent<ParticleSystem>()) {

                                    if (!GO.GetComponent<ParticleSystem>().isPlaying) {
                                        GO.transform.position = new Vector3(objectsWithTag[i].transform.position.x, -100f, objectsWithTag[i].transform.position.z);
                                    }

                                    GO.GetComponent<ParticleSystem>().Play();

                                    Debug.Log("plays particle system" + objectsWithTag[i].GetComponent<ParticleSystem>());

                                }
                            }
                        }

                    }
                }
            } else {
                if (GameObject.FindGameObjectsWithTag(tag.targetTag) != null && GameObject.FindGameObjectsWithTag(tag.targetTag).Length > 0) {

                    GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag.targetTag);

                    Debug.Log("has objects with tag" + objectsWithTag.Length);
                
                    for (int i = 0; i < objectsWithTag.Length; i++) {

                        if (objectsWithTag[i].GetComponent<ParticleSystem>()) {
                            objectsWithTag[i].GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                            //objectsWithTag[i].GetComponent<ParticleSystem>().Play();

                            Debug.Log("stops particle system" + objectsWithTag[i].GetComponent<ParticleSystem>());

                        } else if (objectsWithTag[i].transform.childCount > 0) {
                            for (int x = 0; i < objectsWithTag[i].transform.childCount; x++) {
                                GameObject GO = objectsWithTag[i].transform.GetChild(x).gameObject;
                                if (GO.GetComponent<ParticleSystem>()) {

                                    objectsWithTag[i].GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);


                                    Debug.Log("stops particle system" + objectsWithTag[i].GetComponent<ParticleSystem>());

                                }
                            }
                        }

                    }
                }
            }


           
        }





    }





}