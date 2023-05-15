using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UIBuilder;
using Services;

public class loadcontent_game2 : MonoBehaviour
{
    public Game game2;
    public ItemOnMap associatedItemOnMap;
    public CrossGameManager crossGameManager;
    public FindToolController findToolController;
    bool loadedPois = false;
    public GameObject tutorial;
    public loadcontent_game2() {
    }

    // Start is called before the first frame update
    void Awake()
    {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    private void Update() {
        if (crossGameManager != null) {
        
            if (crossGameManager.AllItemsOnMap.Count != 0) {

                if (!loadedPois) {
                    loadedPois = true;
                    StartCoroutine(GetRelevantPois());
                }
            }
            
        }
    }

    private IEnumerator GetRelevantPois() {

        StartCoroutine(this.crossGameManager.strapiService.getSpiel2Content((StrapiSingleResponse<Game> res) => {
            game2 = res.data;

            tutorial.GetComponent<UIDocument>().rootVisualElement.Q<Label>("title").text = game2.attributes.gameTitle.ToUpper();
            tutorial.GetComponent<UIDocument>().rootVisualElement.Q<Label>("subheadline").text = game2.attributes.description;

            int poiID = game2.attributes.point_of_interest.data.id;

            associatedItemOnMap = crossGameManager.AllItemsOnMap.Find(item => item.ID == poiID);

            if (findToolController != null) {
                findToolController.itemOnMapCurrentlyClosestToPlayer = associatedItemOnMap;
            }

        }));

        yield return null;
    }
}
