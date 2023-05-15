
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using DG.Tweening;
using ARLocation;
using UnityEngine.UI;
using UIBuilder;
using Services;

public class listeController : MonoBehaviour {
    public TourLoader _tourLoader;
    public TourPopUp _tourPopup;
    CrossGameManager crossGameManager;
    private bool isFirstTimeUse = true;
    bool starter = true;
    private float nextActionTime = 0.0f;
    public float period = 20f;
    string buttonSubHeadline;

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }
    // Start is called before the first frame update
    void Start() {
        GameObject crossGameManagerObject = GameObject.FindGameObjectWithTag("CrossGameManager");


        StartCoroutine(crossGameManager.strapiService.getListPageContent(LoadConfigurator));
        InvokeRepeating("ReStart", 2.0f, 5.0f);
    }

    async void LoadConfigurator(StrapiSingleResponse<ListPageData> res) {
        ListPageData _data = res.data;

        buttonSubHeadline = _data.attributes.buttonSubHeadline;

    }

    private void Update() {

        if (starter) {
            if (GameObject.FindGameObjectWithTag("MapMarker") != null) {
                LoadPOIs();
                starter = false;
            } 
        }

    }

    public void ReStart() {
        this.gameObject.GetComponent<UIItemViewController>().BGscrollerListe.Clear();
        GetClosestPois();
    }

    public void LoadPOIs() {
        StartCoroutine(loadPOIS());
    }

    public void GetClosestPois() {
        double distance;
        double distanceErrorLog;
        List<int> group = new List<int>();
      
       
        foreach (ItemOnMap item in _tourLoader.ItemsOnMap) {

            distance = crossGameManager.CalculateDistanceFromPinToPlayer(item);
            string target = "";
            target = item.Poi.attributes.tourPoint.TourPointOverlayActive.Target;
            string type = "";

            if (((int)distance) < 50) {

                group.Add((int)distance);
                group.Sort();

                switch (item.Poi.attributes.type)
                {
                    case "spiel":
                        switch (target) {
                            case "Game1":
                                target = "game:1";
                                break;
                            case "Game2":
                                target = "game:2";
                                break;
                            case "Game3":
                                target = "game:3";
                                break;
                            case "Game4":
                                target = "game:4";
                                break;
                            case "Game5":
                                target = "game:5";
                                break;
                        }

                        CMSListeButton spiel = new CMSListeButton("spiel", item.Poi.attributes.type, target,this.gameObject.GetComponent<UIItemViewController>());
                        spiel.Q<Label>("headline").text = item.Poi.attributes.title.ToString();
                        spiel.Q<Label>("subheadline").text = buttonSubHeadline + " " + ((int)distance).ToString() + "m";
                        this.gameObject.GetComponent<UIItemViewController>().BGscrollerListe.Add(spiel);

                        break;
                    case "fundObjekt":
                        CMSListeButton fund = new CMSListeButton("fund",item.Poi.attributes.type, item.Poi.attributes.title,this.gameObject.GetComponent<UIItemViewController>());
                        fund.Q<Label>("headline").text = item.Poi.attributes.title.ToString();
                        fund.Q<Label>("subheadline").text = buttonSubHeadline + " " + ((int)distance).ToString() + "m";
                        this.gameObject.GetComponent<UIItemViewController>().BGscrollerListe.Add(fund);

                        break;
                    case "grabung":
                        CMSListeButton ausgrabung = new CMSListeButton("ausgrabung", item.Poi.attributes.type, item.Poi.attributes.title, this.gameObject.GetComponent<UIItemViewController>());
                        ausgrabung.Q<Label>("headline").text = item.Poi.attributes.title.ToString();
                        ausgrabung.Q<Label>("subheadline").text = buttonSubHeadline + " " + ((int)distance).ToString() + "m";
                        this.gameObject.GetComponent<UIItemViewController>().BGscrollerListe.Add(ausgrabung);

                        break;
                    case "landschaft":

                        break;
                    case "panorama":
                        CMSListeButton panorama = new CMSListeButton("panorama", item.Poi.attributes.type, item.Poi.attributes.title, this.gameObject.GetComponent<UIItemViewController>());
                        panorama.Q<Label>("headline").text = item.Poi.attributes.title.ToString();
                        panorama.Q<Label>("subheadline").text = buttonSubHeadline + " " + ((int)distance).ToString() + "m";
                        this.gameObject.GetComponent<UIItemViewController>().BGscrollerListe.Add(panorama);

                        break;
                    case "tour":
                        CMSListeButton touren = new CMSListeButton("touren", item.Poi.attributes.type, item.Poi.attributes.title, this.gameObject.GetComponent<UIItemViewController>());
                        touren.Q<Label>("headline").text = item.Poi.attributes.title.ToString();
                        touren.Q<Label>("subheadline").text = buttonSubHeadline + " " + ((int)distance).ToString() + "m";
                        this.gameObject.GetComponent<UIItemViewController>().BGscrollerListe.Add(touren);
                        break;
                }
            }
            
        }

    }



    public void EnableAllPOIs() {
        for (int i = 0; i < _tourLoader.ItemsOnMap.Count; i++) {
            _tourLoader.ItemsOnMap[i].Pin.SetActive(true);
        }

    }

    public void ShowPOI(string type) {
        for (int i = 0; i < _tourLoader.ItemsOnMap.Count; i++) {
            _tourLoader.ItemsOnMap[i].Pin.SetActive(false);

            if (_tourLoader.ItemsOnMap[i].Poi.attributes.title == type) {
                _tourLoader.ItemsOnMap[i].Pin.SetActive(true);
            }
        }

    }

    public IEnumerator loadPOIS() {
        yield return new WaitForEndOfFrame();
        if (GameObject.FindGameObjectWithTag("tourloader")) {
            _tourLoader = GameObject.FindGameObjectWithTag("tourloader").GetComponent<TourLoader>();
            crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
            GetClosestPois();
        }
    }

    public IEnumerator loadPOISFirstTime() {
        yield return new WaitForSeconds(5f);
        if (GameObject.FindGameObjectWithTag("tourloader")) {
            _tourLoader = GameObject.FindGameObjectWithTag("tourloader").GetComponent<TourLoader>();
            crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
            GetClosestPois();
        }
    }


}
