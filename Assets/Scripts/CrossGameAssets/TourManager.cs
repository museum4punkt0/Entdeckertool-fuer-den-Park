using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TourLoader))]
public class TourManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private CrossGameManager crossGameManager;
    private TourLoader tourLoader;
    public GameObject GameOverlayContainer;

    public Tour currentTour;
    public int currentNetto_Reward;
    public int PoisInTour;

    public GamePopUp popup;

    public bool isFinished;
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        crossGameManager.IsVisitingFromTour = false;
        tourLoader = transform.GetComponent<TourLoader>();
        crossGameManager.ShouldUpdateScore = false;

        if (popup != null) {
            popup.hidePopUp();
        }


    }



    private void Update() {
        if (tourLoader.ItemsOnMap.Count > 0 && !isFinished && tourLoader.ItemsOnMap.Count == tourLoader.itemsVisited) {

            print("should finish ");
            StartCoroutine(EndTour());
        }
    }

    public IEnumerator EndTour() {

        print("ends tour ");
        isFinished = true;
        crossGameManager.AddToScore(crossGameManager.colorToType("tour"), tourLoader.tour.attributes.tourTeaser.reward);

        yield return new WaitForSeconds(2);

        popup.ShowAndUpdatePopUp("Gratulation! Alles ist gefunden.", "", "", "Schliessen", "tour");
        //showTourOverlay(currentTour, currentNetto_Reward);
    }


    public void OpenEndTourPopUp() {
        popup.ShowAndUpdatePopUp("Park-Tour schliessen?", "", "", "Ja, Park-Tour schliessen", "tour");
    }


    public void LeaveTour() {
        crossGameManager.IsVisitingFromTour = false;
        if (crossGameManager.LastSceneVisited == "ParkTour") {
            crossGameManager.LastSceneVisited = "MainScene";
        }
        crossGameManager.GoBackToPreviousScene();
    }


    public void updateTourHeader(Tour tour, int itemsVisited) {


        if (this.GameOverlayContainer.GetComponent<ParkTourOverlay>() != null) {
            this.GameOverlayContainer.GetComponent<ParkTourOverlay>().UpdateHeader(tour.attributes.tourTeaser.headline, itemsVisited, tour.attributes.point_of_interests.data.Count);

        } else {
            Debug.Log("cant find overlay component");
        }

    }

}
