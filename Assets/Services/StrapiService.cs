using System;
using System.Collections;
using UnityEngine;

namespace Services {
    public class StrapiService : HttpService {
#if DEVELOPMENT_BUILD
            string baseUrl = "https://var-staging.xailabs.com/api/";
#else
        string baseUrl = "https://var-production.xailabs.com/api/";
#endif
        //string baseUrl = "https://var-staging.xailabs.com/api/";
        public StrapiService(string cacheLocation): base(cacheLocation) {
        }

        public IEnumerator getPointOfInterests(Action<StrapiResponse<Poi>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+ "pois?populate=*",
                (string response) => {
                    Debug.Log("POIS____" + response);
                    callback(JsonUtility.FromJson<StrapiResponse<Poi>>(response));
                }
            );
        }


        public IEnumerator getIllustrations(Action<StrapiResponse<illustrationContainer>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"illustrations/",
                (string response) => {
                    Debug.Log("from strapi service ILLUSTRATIONS" + response);
                    callback(JsonUtility.FromJson<StrapiResponse<illustrationContainer>>(response));
                }
            );
        }


        public IEnumerator getSingleIllustration(int ID,
            Action<StrapiSingleResponse<illustrationData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"illustrations/" + ID + "",
                (string response) => {
                    Debug.Log("from strapi service ILLUSTRATION" + response);

                    callback(JsonUtility.FromJson<StrapiSingleResponse<illustrationData>>(response));
                }
            );
        }


        public IEnumerator getSinglePointOfInterests(int ID, Action<StrapiSingleResponse<Poi>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"pois/" + ID + " ?populate=*",
                (string response) => {
                    Debug.Log("from strapi service POI" + response);
                    callback(JsonUtility.FromJson<StrapiSingleResponse<Poi>>(response));
                }
            );
        }

        public IEnumerator getTours(Action<StrapiResponse<Tour>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"tours/", (string response) => {
                    Debug.Log("from strapi service TOUR" + response);
                    callback(JsonUtility.FromJson<StrapiResponse<Tour>>(response));
                }
            );
        }


        public IEnumerator getTour(int ID, Action<StrapiSingleResponse<Tour>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"tours/" + ID + "?populate=*",
                (string response) => {
                    Debug.Log("from strapi service TOUR" + response);
                    callback(JsonUtility.FromJson<StrapiSingleResponse<Tour>>(response));
                }
            );
        }

        public IEnumerator getItems(Action<StrapiItemResponse> callback = null) {
            yield return this.RequestRoutine(
                this.baseUrl+"items?populate=accordionItems.video.video,sliderItems.media,links,buttons",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiItemResponse>(response));
                }
            );
        }

        public IEnumerator getGrabungen(Action<StrapiItemResponse> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"grabungen?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiItemResponse>(response));
                }
            );
        }

        public IEnumerator getMainMenuContent(Action<StrapiMainMenuResponse> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"menu-main-page?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiMainMenuResponse>(response));
                });
        }

        public IEnumerator getQuestions(Action<StrapiItemResponse> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"context-questions?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiItemResponse>(response));
                }
            );
        }

        public IEnumerator getEntdeckerfragenMenuContent(Action<StrapiSingleResponse<GameMenuData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"menu-entdeckerfragen?populate=*",
                (string response) => {
                    Debug.Log(response);
                    callback(JsonUtility.FromJson<StrapiSingleResponse<GameMenuData>>(response));
                }
            );
        }

        public IEnumerator getGameMenuContent(Action<StrapiSingleResponse<GameMenuData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"menu-games?populate=*",
                (string response) => {
                    Debug.Log(response);
                    callback(JsonUtility.FromJson<StrapiSingleResponse<GameMenuData>>(response));
                }
            );
        }

        public IEnumerator getTourenMenuContent(Action<StrapiSingleResponse<GameMenuData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"menu-touren?populate=*",
                (string response) => {
                    Debug.Log("SS: ");
                    Debug.Log(response);
                    callback(JsonUtility.FromJson<StrapiSingleResponse<GameMenuData>>(response));
                }
            );
        }

        public IEnumerator getHighlights(Action<StrapiResponse<HighlightData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"highlights?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiResponse<HighlightData>>(response));
                }
            );
        }

        public IEnumerator getNewsEntry(Action<StrapiResponse<HighlightData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"news-entries?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiResponse<HighlightData>>(response));
                }
            );
        }

        public IEnumerator
            getArchaelogieMenuContent(Action<StrapiSingleResponse<ArchaeologieMenuData>> callback = null) {
            yield return this.RequestRoutine(
                this.baseUrl+"menue-archaeologie?populate=*,video.video,FAQAccordionItems.*,FAQAccordionItems.video.video,cards.*,Highlights.*,HighlightsImage.*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<ArchaeologieMenuData>>(response));
                }
            );
        }

        public IEnumerator
            getArchaelogieGrabungen(Action<StrapiSingleResponse<ArchaeologieGrabungen>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"archaologie-grabungen?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<ArchaeologieGrabungen>>(response));
                }
            );
        }

        public IEnumerator getHilfeMenuContent(Action<StrapiSingleResponse<HilfeMenuData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"menu-hilfe?populate=AccordionItem",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<HilfeMenuData>>(response));
                }
            );
        }

        public IEnumerator getInfoMenuContent(Action<StrapiSingleResponse<HilfeMenuData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"menu-hilfe?populate=AccordionItem",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<HilfeMenuData>>(response));
                }
            );
        }

        public IEnumerator getSpiel1Content(Action<StrapiSingleResponse<Game1Data>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"game1?populate=*,content.image",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<Game1Data>>(response));
                }
            );
        }

        public IEnumerator getSpiel5Content(Action<StrapiSingleResponse<Game5Data>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"Game5?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<Game5Data>>(response));
                }
            );
        }

        public IEnumerator getSpiel4Content(Action<StrapiSingleResponse<Game4Data>> callback = null) {
            yield return this.RequestRoutine(
                this.baseUrl+"Game4?populate=*,content.imageAfter,content.imageBefore,content.tool,content.tool.image,popupStart,popupEnd",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<Game4Data>>(response));
                }
            );
        }


        public IEnumerator getSpiel3Content(Action<StrapiSingleResponse<Game3Data>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"Game3?populate=*",
                (string response) => {
                    Debug.Log("gets spiel 3 content" + response);
                    callback(JsonUtility.FromJson<StrapiSingleResponse<Game3Data>>(response));
                }
            );
        }

        public IEnumerator getSpiel2Content(Action<StrapiSingleResponse<Game>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"Game2?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<Game>>(response));
                }
            );
        }

        public IEnumerator getSpiel1TutorialContent(Action<StrapiSingleResponse<Game>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"Game1?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<Game>>(response));
                }
            );
        }

        public IEnumerator getSpiel4TutorialContent(Action<StrapiSingleResponse<Game>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"Game4?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<Game>>(response));
                }
            );
        }

        public IEnumerator getTutorialPageContent(Action<StrapiSingleResponse<TutorialPageData>> callback = null) {
            yield return this.RequestRoutine(
                this.baseUrl+"tutorial-page?populate=*,images.media", (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<TutorialPageData>>(response));
                }
            );
        }

        public IEnumerator getARFilterPageContent(Action<StrapiSingleResponse<ARFilterPageData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"ar-filter-page?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<ARFilterPageData>>(response));
                }
            );
        }

        public IEnumerator getARPageContent(Action<StrapiSingleResponse<ARPageData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"ar-page?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<ARPageData>>(response));
                }
            );
        }

        public IEnumerator getConfigurationContent(Action<StrapiSingleResponse<ConfigurationData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"configuration?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<ConfigurationData>>(response));
                }
            );
        }

        public IEnumerator getFilterPageContent(Action<StrapiSingleResponse<FilterPageData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"filter-page?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<FilterPageData>>(response));
                }
            );
        }

        public IEnumerator getFunddetektorPageContent(
            Action<StrapiSingleResponse<FunddetektorPageData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"funddetektor-page?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<FunddetektorPageData>>(response));
                }
            );
        }

        public IEnumerator getIntroScenePageContent(Action<StrapiSingleResponse<IntroScenePageData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"intro-scene?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<IntroScenePageData>>(response));
                }
            );
        }


        public IEnumerator getListPageContent(Action<StrapiSingleResponse<ListPageData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"list-page?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<ListPageData>>(response));
                }
            );
        }

        public IEnumerator
            getBelohnungenPageContent(Action<StrapiSingleResponse<BelohnungenPageData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"menu-belohnungen?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<BelohnungenPageData>>(response));
                }
            );
        }

        public IEnumerator getPushNotifications(Action<StrapiResponse<Notification>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"notifications?populate=*",
                (string response) => {
                    Debug.Log("Notifications____" + response);
                    callback(JsonUtility.FromJson<StrapiResponse<Notification>>(response));
                }
            );
        }

        public IEnumerator getGrabungenPageContent(Action<StrapiSingleResponse<GrabungenPageData>> callback = null) {
            yield return this.RequestRoutine(this.baseUrl+"archaologie-grabungen?populate=*",
                (string response) => {
                    callback(JsonUtility.FromJson<StrapiSingleResponse<GrabungenPageData>>(response));
                }
            );
        }
    }
}