using System;
using System.Collections;
using System.Collections.Generic;
using ARLocation;
using Mapbox.Unity.Location;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FindToolController : MonoBehaviour {
    public GameObject arrow;
    public GameObject text;
    public AudioSource audioSource;
    bool _isInitialized;

    public bool triggerAnimationPart1 = false;
    public bool triggerAnimationPart2 = false;
    public AnimationControllerFundDetektor AnimationControllerFundDetektor_script;
    public FullEllipseAnimation FullEllipseAnimation_script;
    private ARLocationProvider _arLocationProvider;
    CrossGameManager crossGameManager;

    public ItemOnMap itemOnMapCurrentlyClosestToPlayer;

    public bool ShouldDynamicallyFindCLosestItemOnMap;

    public bool editorForceDistanceToBeNull;

    public GameObject gamecontroller;

    public GameObject gamePoiLoader;

    

    void Start() {
        this._arLocationProvider = ARLocationProvider.Instance;
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        if (SceneManager.GetActiveScene().name != "MainScene") {
            crossGameManager.IsVisitingFromGame = true;
        }

    }


    public void GetItemOnMapCurrentlyClosestToPlayer() {
        double previousDistance = -1;
        double distance;

        if (SceneManager.GetActiveScene().name != "MainScene") {

            StartCoroutine(GetPOIGame2());

        } else {
            foreach (ItemOnMap item in crossGameManager.AllItemsOnMap) {
                if (!item.HasBeenVisited) {
                    distance = crossGameManager.CalculateDistanceFromPinToPlayer(item);
                    if (distance < previousDistance || previousDistance == -1) {
                        previousDistance = distance;
                        itemOnMapCurrentlyClosestToPlayer = item;
                    }
                }


            }
        }
    }

     IEnumerator  GetPOIGame2() {

        yield return new WaitForSeconds(1);
        var item = crossGameManager.AllItemsOnMap.Find(mapitem => {
            print(gamePoiLoader.GetComponent<loadcontent_game2>().associatedItemOnMap.Poi.id);
            return gamePoiLoader.GetComponent<loadcontent_game2>().associatedItemOnMap.Poi.id == mapitem.Poi.id;
        });
        itemOnMapCurrentlyClosestToPlayer = item;

    }

    private void Update() {
        // http://www.movable-type.co.uk/scripts/latlong.html?from=47.80423,-120.03866&to=47.830481,-120.00987

        if (itemOnMapCurrentlyClosestToPlayer != null) {

            if (ShouldDynamicallyFindCLosestItemOnMap) {
                GetItemOnMapCurrentlyClosestToPlayer();
            }

            double lat1 = this.crossGameManager._location.x,
                lng1 = this.crossGameManager._location.y,
                lat2 = itemOnMapCurrentlyClosestToPlayer.Poi.attributes.lat,
                lng2 = itemOnMapCurrentlyClosestToPlayer.Poi.attributes.lng;

            double R = 6371000;
            double p1 = lat1 * Math.PI / 180;
            double p2 = lat2 * Math.PI / 180;
            double dp = (lat2 - lat1) * Math.PI / 180;
            double dL = (lng2 - lng1) * Math.PI / 180;
            double a = Math.Sin(dp / 2) * Math.Sin(dp / 2) +
                       Math.Cos(p1) * Math.Cos(p2) *
                       Math.Sin(dL / 2) * Math.Sin(dL / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;


            double y = Math.Sin(lng2 - lng1) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) -
                       Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(lng2 - lng1);
            double p = Math.Atan2(y, x);
            double brng = (p * 180 / Math.PI + 360) % 360; // in degrees
            double rot = brng;


            double volume = 0.10f;


            if (distance < 100 && distance > 25) {
                volume = (100 - distance) / 100;

                if (!triggerAnimationPart1) {
                    FullEllipseAnimation_script.StartAnimationFadeIn();

                    triggerAnimationPart1 = true;
                }
            }

            if (distance < 25 || editorForceDistanceToBeNull) {

                if (!triggerAnimationPart2) {
                    AnimationControllerFundDetektor_script.AlignEllipses();
                    FullEllipseAnimation_script.StartAnimationMove();

                    triggerAnimationPart2 = true;
                }

            }


            this.text.GetComponent<TMPro.TextMeshProUGUI>().SetText("Distanz bis zum Objekt \n" + distance.ToString("0") + "m");
            this.audioSource.volume = (float)volume;
            this.arrow.transform.rotation = Quaternion.Euler(0, 0,
                (float)(rot + this._arLocationProvider.CurrentHeading.heading + 180.0f));
        }
    }

    IEnumerator LoadGame() {
        yield return new WaitForSeconds(5f);
        gamecontroller.GetComponent<startGame2>().startScan();
    }

    public void StartGame() {
        GameObject.FindGameObjectWithTag("detector").gameObject.SetActive(false);
    }

    public double Calculate_Distance(float long_a, float lat_a, float long_b, float lat_b) {
        float a_long_r, a_lat_r, p_long_r, p_lat_r, dist_x, dist_y, total_dist;
        a_long_r = this.DegToRad(long_a);
        a_lat_r = this.DegToRad(lat_a);
        p_long_r = this.DegToRad(long_b);
        p_lat_r = this.DegToRad(lat_b);
        dist_x = this.Distance_x(a_long_r, p_long_r, a_lat_r, p_lat_r);
        dist_y = this.Distance_y(a_lat_r, p_lat_r);
        total_dist = this.Final_distance(dist_x, dist_y);
        return total_dist;
    }

    float DegToRad(float deg) {
        float temp;
        temp = (deg * (float)(Math.PI)) / 180.0f;
        temp = Mathf.Tan(temp);
        return temp;
    }

    float Distance_x(float lon_a, float lon_b, float lat_a, float lat_b) {
        float temp;
        float c;
        temp = (lat_b - lat_a);
        c = Mathf.Abs(temp * Mathf.Cos((lat_a + lat_b)) / 2);
        return c;
    }

    private float Distance_y(float lat_a, float lat_b) {
        float c;
        c = (lat_b - lat_a);
        return c;
    }

    float Final_distance(float x, float y) {
        float c;
        c = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f))) * 6371;
        return c;
    }

    static double DegreeBearing(
        double lat1, double lon1,
        double lat2, double lon2) {
        var dLon = ToRad(lon2 - lon1);
        var dPhi = Math.Log(
            Math.Tan(ToRad(lat2) / 2 + Math.PI / 4) / Math.Tan(ToRad(lat1) / 2 + Math.PI / 4));
        if (Math.Abs(dLon) > Math.PI)
            dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
        return ToBearing(Math.Atan2(dLon, dPhi));
    }

    public static double ToRad(double degrees) {
        return degrees * (Math.PI / 180);
    }

    public static double ToDegrees(double radians) {
        return radians * 180 / Math.PI;
    }

    public static double ToBearing(double radians) {
        // convert radians to degrees (as bearing: 0...360)
        return (ToDegrees(radians) + 360) % 360;
    }

    public void GetPoifromGame2() {
    
    }
}