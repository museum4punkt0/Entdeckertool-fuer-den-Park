using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAllScripts : MonoBehaviour
{
     CrossGameManager crossGameManager;

    void Awake()
    {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    private void Start() {
        LoadAllContent();
    }

    private void LoadAllContent() {
        StartCoroutine(crossGameManager.strapiService.getArchaelogieGrabungen(LoadArchaelogieGrabungen));
        StartCoroutine(crossGameManager.strapiService.getArchaelogieMenuContent(LoadArchaelogieMenuContent));
        StartCoroutine(crossGameManager.strapiService.getARFilterPageContent(LoadARFilterPageContent));
        StartCoroutine(crossGameManager.strapiService.getARPageContent(LoadARPageContent));
        StartCoroutine(crossGameManager.strapiService.getBelohnungenPageContent(LoadBelohnungenPageContent));
        StartCoroutine(crossGameManager.strapiService.getConfigurationContent(LoadConfigurationContent));
        StartCoroutine(crossGameManager.strapiService.getEntdeckerfragenMenuContent(LoadEntdeckerfragenMenuContent));
        StartCoroutine(crossGameManager.strapiService.getFilterPageContent(LoadFilterPageContent));
        StartCoroutine(crossGameManager.strapiService.getFunddetektorPageContent(LoadFunddetektorPageContent));
        StartCoroutine(crossGameManager.strapiService.getGameMenuContent(LoadGameMenuContent));
        StartCoroutine(crossGameManager.strapiService.getGrabungen(LoadGrabungen));
        StartCoroutine(crossGameManager.strapiService.getGrabungenPageContent(LoadGrabungenPageContent));
        StartCoroutine(crossGameManager.strapiService.getHighlights(LoadHighlights));
        StartCoroutine(crossGameManager.strapiService.getHilfeMenuContent(LoadHilfeMenuContent));
        StartCoroutine(crossGameManager.strapiService.getIllustrations(LoadIllustrations));
        StartCoroutine(crossGameManager.strapiService.getInfoMenuContent(LoadInfoMenuContent));
        StartCoroutine(crossGameManager.strapiService.getIntroScenePageContent(LoadIntroScenePageContent));
        StartCoroutine(crossGameManager.strapiService.getItems(LoadItems));
        StartCoroutine(crossGameManager.strapiService.getListPageContent(LoadListPageContent));
        StartCoroutine(crossGameManager.strapiService.getMainMenuContent(LoadMainMenuContent));
        StartCoroutine(crossGameManager.strapiService.getNewsEntry(LoadNewsEntry));
        StartCoroutine(crossGameManager.strapiService.getPointOfInterests(LoadPointOfInterests));
        StartCoroutine(crossGameManager.strapiService.getQuestions(LoadQuestions));
        //StartCoroutine(crossGameManager.strapiService.getSingleIllustration(LoadSingleIllustration));
        //StartCoroutine(crossGameManager.strapiService.getSinglePointOfInterests(LoadSinglePointOfInterests));
        StartCoroutine(crossGameManager.strapiService.getSpiel1Content(LoadSpiel1Content));
        StartCoroutine(crossGameManager.strapiService.getSpiel1TutorialContent(LoadSpiel1TutorialContent));
        StartCoroutine(crossGameManager.strapiService.getSpiel2Content(LoadSpiel2Content));
        StartCoroutine(crossGameManager.strapiService.getSpiel3Content(LoadSpiel3Content));
        StartCoroutine(crossGameManager.strapiService.getSpiel4Content(LoadSpiel4Content));
        StartCoroutine(crossGameManager.strapiService.getSpiel4TutorialContent(LoadSpiel4TutorialContent));
        StartCoroutine(crossGameManager.strapiService.getSpiel5Content(LoadSpiel5Content));
        //StartCoroutine(crossGameManager.strapiService.getTour(LoadTour));
        StartCoroutine(crossGameManager.strapiService.getTourenMenuContent(LoadTourenMenuContent));
        StartCoroutine(crossGameManager.strapiService.getTours(LoadTours));
        StartCoroutine(crossGameManager.strapiService.getTutorialPageContent(LoadTutorialPageContent));
    }

    async void LoadArchaelogieGrabungen(StrapiSingleResponse<ArchaeologieGrabungen> res) { }
    async void LoadArchaelogieMenuContent(StrapiSingleResponse<ArchaeologieMenuData> res) { }
    async void LoadARFilterPageContent(StrapiSingleResponse<ARFilterPageData> res) { }
    async void LoadARPageContent(StrapiSingleResponse<ARPageData> res) { }
    async void LoadBelohnungenPageContent(StrapiSingleResponse<BelohnungenPageData> res) { }
    async void LoadConfigurationContent(StrapiSingleResponse<ConfigurationData> res) { }
    async void LoadEntdeckerfragenMenuContent(StrapiSingleResponse<GameMenuData> res) { }
    async void LoadFilterPageContent(StrapiSingleResponse<FilterPageData> res) { }
    async void LoadFunddetektorPageContent(StrapiSingleResponse<FunddetektorPageData> res) { }
    async void LoadGameMenuContent(StrapiSingleResponse<GameMenuData> res) { }
    async void LoadGrabungen(StrapiItemResponse res) { }
    async void LoadGrabungenPageContent(StrapiSingleResponse<GrabungenPageData> res) { }
    async void LoadHighlights(StrapiResponse<HighlightData> res) { }
    async void LoadHilfeMenuContent(StrapiSingleResponse<HilfeMenuData> res) { }
    async void LoadIllustrations(StrapiResponse<illustrationContainer> res) { }
    async void LoadInfoMenuContent(StrapiSingleResponse<HilfeMenuData> res) { }
    async void LoadIntroScenePageContent(StrapiSingleResponse<IntroScenePageData> res) { }
    async void LoadItems(StrapiItemResponse res) { }
    async void LoadListPageContent(StrapiSingleResponse<ListPageData> res) { }
    async void LoadMainMenuContent(StrapiMainMenuResponse res) { }
    async void LoadNewsEntry(StrapiResponse<HighlightData> res) { }
    async void LoadPointOfInterests(StrapiResponse<Poi> res) { }
    async void LoadQuestions(StrapiItemResponse res) { }
    //async void LoadSingleIllustration(StrapiSingleResponse<illustrationData> res) { }
    //async void LoadSinglePointOfInterests(StrapiSingleResponse<Poi> res) { }
    async void LoadSpiel1Content(StrapiSingleResponse<Game1Data> res) { }
    async void LoadSpiel1TutorialContent(StrapiSingleResponse<Game> res) { }
    async void LoadSpiel2Content(StrapiSingleResponse<Game> res) { }
    async void LoadSpiel3Content(StrapiSingleResponse<Game3Data> res) { }
    async void LoadSpiel4Content(StrapiSingleResponse<Game4Data> res) { }
    async void LoadSpiel4TutorialContent(StrapiSingleResponse<Game> res) { }
    async void LoadSpiel5Content(StrapiSingleResponse<Game5Data> res) { }
    //async void LoadTour(StrapiSingleResponse<Tour> res) { }
    async void LoadTourenMenuContent(StrapiSingleResponse<GameMenuData> res) { }
    async void LoadTours(StrapiResponse<Tour> res) { }
    async void LoadTutorialPageContent(StrapiSingleResponse<TutorialPageData> res) { }

}
