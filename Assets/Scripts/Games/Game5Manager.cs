using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TourLoader))]
public class Game5Manager : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField]
    //CrossGameAssets
    public CrossGameManager crossGameManager;
    private TourLoader tourLoader;
    public GameObject GameOverlayContainer;

    public GameObject UIGame5Controller;

    public List<Sprite> images = new List<Sprite>(9);
    public List<Sprite> unfilled_images = new List<Sprite>(9);

    public Tour currentTour;
    public int currentNetto_Reward;

    public bool isFinished;
    public bool canFinish;

    public Game5Data game;
    [SerializeField] bool shouldShowItemsOnMap;
    public GameObject part1;
    public GameObject part2;

    private bool crossGameManagerIsDone;

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }
    void OnEnable() {
       SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable() {
       SceneManager.sceneLoaded -= OnSceneLoaded;
     }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        tourLoader = transform.GetComponent<TourLoader>();

        if (crossGameManager.isPlayingGame5) {
            UIGame5Controller.GetComponent< UIGame5Controller>().returnToGameFromItemView();
            print("returns from itemview");
        }
        tourLoader.CanInteractWithMap = false;


    }

    public void LoadGameAndPlacePoints() {
        crossGameManagerIsDone = true;

        StartCoroutine(this.crossGameManager.strapiService.getSpiel5Content((StrapiSingleResponse<Game5Data> res) => {
            game = res.data;

            int index = 0;

            Debug.Log("Has game data" + game.attributes.point_of_interests.data.Count);

            foreach (Poi point in game.attributes.point_of_interests.data) {
                ItemOnMap itemOnMap = crossGameManager.AllItemsOnMap.Find(item => item.ID == point.id);

                if (shouldShowItemsOnMap) {
                    tourLoader.spawnPoint(itemOnMap, index);
                }

                tourLoader.ItemsOnMap.Add(itemOnMap);

                if (itemOnMap.Poi.attributes.title.Contains("Dolch")) {
                    itemOnMap.AssociatedMenuItems.Add(images[0]);
                    itemOnMap.AssociatedMenuItems.Add(unfilled_images[0]);
                }
                if (itemOnMap.Poi.attributes.title.Contains("fibel")) {
                    itemOnMap.AssociatedMenuItems.Add(images[1]);
                    itemOnMap.AssociatedMenuItems.Add(unfilled_images[1]);
                }

                if (itemOnMap.Poi.attributes.title.Contains("schurzriemen")) {
                    itemOnMap.AssociatedMenuItems.Add(images[2]);
                    itemOnMap.AssociatedMenuItems.Add(unfilled_images[2]);
                }
                if (itemOnMap.Poi.attributes.title.Contains("Unterkiefer")) {
                    itemOnMap.AssociatedMenuItems.Add(images[3]);
                    itemOnMap.AssociatedMenuItems.Add(unfilled_images[3]);
                }
                if (itemOnMap.Poi.attributes.title.Contains("Pilum")) {
                    itemOnMap.AssociatedMenuItems.Add(images[4]);
                    itemOnMap.AssociatedMenuItems.Add(unfilled_images[4]);
                }
                if (itemOnMap.Poi.attributes.title.Contains("Schienen")) {
                    itemOnMap.AssociatedMenuItems.Add(images[5]);
                    itemOnMap.AssociatedMenuItems.Add(unfilled_images[5]);
                }
                if (itemOnMap.Poi.attributes.title.Contains("Sandal")) {
                    itemOnMap.AssociatedMenuItems.Add(images[6]);
                    itemOnMap.AssociatedMenuItems.Add(unfilled_images[6]);
                }
                if (itemOnMap.Poi.attributes.title.Contains("Silberblech")) {
                    itemOnMap.AssociatedMenuItems.Add(images[7]);
                    itemOnMap.AssociatedMenuItems.Add(unfilled_images[7]);
                }
                if (itemOnMap.Poi.attributes.title.Contains("Schwert")) {
                    itemOnMap.AssociatedMenuItems.Add(images[8]);
                    itemOnMap.AssociatedMenuItems.Add(unfilled_images[8]);
                }



                index++;
            }

        }));
    }




    private void Update() {
      
        if (!isFinished && canFinish && tourLoader.ItemsOnMap.Count <= tourLoader.itemsVisited) {
            BeginDressingRomanPerson();
        }

        if (crossGameManager != null && crossGameManager.AllItemsOnMap.Count > 10 && !crossGameManagerIsDone) {
            LoadGameAndPlacePoints();
        }


    }



    public IEnumerator EndTour() {
            isFinished = true;
            crossGameManager.AddToScore(crossGameManager.colorToType("tour"), tourLoader.tour.attributes.tourTeaser.reward);

            yield return new WaitForSeconds(1);

            showTourOverlay(currentTour, currentNetto_Reward);

     }

     public void showTourOverlay(Tour tour, int netto_reward) {

            currentTour = tour;

            if (this.GameOverlayContainer != null && this.GameOverlayContainer.GetComponent<ParkTourOverlay>() != null) {
                this.GameOverlayContainer.GetComponent<ParkTourOverlay>().UpdateAndShowGameOverlay(tour.attributes.tourTeaser.headline, tour.attributes.tourTeaser.subheadline, tour.attributes.point_of_interests.data.Count, netto_reward, tour.attributes.tourTeaser.duration);

            } else {
                Debug.Log("cant find overlay component");
            }

        }

    public void BeginDressingRomanPerson() {
        isFinished = true;
        
        part1.SetActive(false);

        tourLoader.DeletePOIs();

        part2.SetActive(true);

 


    }


}
