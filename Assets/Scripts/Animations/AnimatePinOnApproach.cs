using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimatePinOnApproach : MonoBehaviour
{
    public PlayableDirector director;
    public CrossGameManager crossGameManager;

    public Transform player;
    public bool hasAnimated;
    public bool shouldWaitToAnimate = true;
    public bool isSiedlung;

    public bool play;
   
    private void Start() {
        GameObject crossGameManagerObject = GameObject.FindGameObjectWithTag("CrossGameManager");
        if (crossGameManagerObject != null) {
            crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        }



        if (shouldWaitToAnimate) {
            StartCoroutine(wait());
        } else {
            StartCoroutine(waitToAnimateMaterials());
        }

        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        
        if (_player != null) {
            player = _player.transform;
        }
    
    }

    private void OnTriggerEnter(Collider other) {

        if (crossGameManager) {
            crossGameManager.ErrorLog("players enters trigger");
        }

        if (other.CompareTag("Player")) {
            StartCoroutine(Play());

        }
    }

    IEnumerator wait() {

        
        yield return new WaitForSeconds(0.5f);
        print("Pauses");
        director.Pause();
    }
    IEnumerator waitToAnimateMaterials() {


        yield return new WaitForSeconds(2f);
        GetComponent<PinCostumiser>().animateMaterials();
    }
    

    IEnumerator Play() {
        hasAnimated = true;

      
        director.Play();

        yield return new WaitForSeconds(2f);
        
       
        
        if (crossGameManager) {
            crossGameManager.ErrorLog("pin plays" + director.isActiveAndEnabled +"__" );
        }

        director.enabled = true;
        director.Resume();


        if (GetComponent<PinCostumiser>().siedlung != null) {

            print("has access to siedlung");

            GetComponent<PinCostumiser>().siedlung.GetComponent<FadeInChildren>().appearFade();
        }

        GetComponent<PinCostumiser>().animateMaterials();



    }

    private void Update() {
        if (player != null && this.GetComponent<Collider>().bounds.Contains(player.position) && !hasAnimated) {

            if (crossGameManager) {
                crossGameManager.ErrorLog("players is in bounds");
            }
            StartCoroutine(Play());
        }
        if (play && !hasAnimated) {

   
            StartCoroutine(Play());
        }
    }

}
