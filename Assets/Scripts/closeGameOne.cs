using UnityEngine;
using UnityEngine.SceneManagement;

public class closeGameOne : MonoBehaviour
{
    CrossGameManager crossGameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        if (SceneManager.GetActiveScene().name == "Game1") {
            crossGameManager.IsVisitingFromGame = true;
            crossGameManager.IsVisitingFromPOI = false;
            crossGameManager.IsVisitingFromTour = false;
            crossGameManager.IsVisitingFrom360 = false;
            crossGameManager.IsVisitingFromMask = false;
            crossGameManager.IsVisitingFromDetectorToFilter = false;
            crossGameManager.IsVisitingFromDetectorToListe = false;

        }

    }

}
