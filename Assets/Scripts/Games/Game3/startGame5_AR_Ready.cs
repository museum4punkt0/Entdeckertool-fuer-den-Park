using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Playables;
using Services;

public class startGame5_AR_Ready : MonoBehaviour
{

    [Header("Game stats (goal should be set to be equal the amount of placable materials")]
    public int scores = 0;
    public int goal;

    [Header("Which Scene Should come Next?")]
    public string NextScene = "MainScene";

    [Header("Buttons")]
    public string StartGameButtonText = "Spiel beginnen";
    public string EndGameButtonText = "Neues Spiel Finden";
    [SerializeField]
    public GameObject nextBnt;

    [Header("Finishing Materials")]
    public List<Material> MaterialToApplyToMesh;
    public List<GameObject> MeshToApplyMaterialTo;


    public CrossGameManager crossGameManager;
    public TourLoader tourLoader;
    public RadialWheel radialWheel;
    public GameObject RomanSoldier;

    public FundInfo fundInfo;

    public PlayableDirector director;

    public Game5Data _data;

    private string winMessage_Headline;
    private string winMessage_subheadline;
    private string winMessage_ButtonText;
    private int reward;
    public startGame5_AR_Ready() {
    }

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    private IEnumerator Start() {
        yield return new WaitForEndOfFrame();
        
        if (crossGameManager.hasCompletedGame5) {
            Debug.Log("should load other roman guy");
            EnterSceneWithFinishedCharacter();
        }

        StartCoroutine(crossGameManager.strapiService.getSpiel5Content(LoadPopUpContent));
    }

    async void LoadPopUpContent(StrapiSingleResponse<Game5Data> res) {
        _data = res.data;

        winMessage_Headline = _data.attributes.winMessage.headline;
         winMessage_subheadline = _data.attributes.winMessage.subHeadline;
        winMessage_ButtonText = _data.attributes.winMessage.buttonText;

        reward = _data.attributes.reward;
    }

    public void SetScore1() { ////keeps score and toggles New Part (teil2) when score 2 is met. 
        this.scores += 1;

        if (this.scores >= goal) {
            finishGame();
        }
    }
    public void ApplyFinalMaterials() {

   

        int index = 0;

        foreach (GameObject mesh in this.MeshToApplyMaterialTo) {


            if (mesh.GetComponent<Renderer>().materials.Length > 1) {

                for (int i = 0; i < mesh.GetComponent<Renderer>().materials.Length; i++) {
                    mesh.GetComponent<Renderer>().materials[i] = this.MaterialToApplyToMesh[index];


                    if (mesh.GetComponent<SkinnedMeshRenderer>()) {

                        SkinnedMeshRenderer renderer = mesh.GetComponentInChildren<SkinnedMeshRenderer>();
                        Material[] mats = renderer.materials;

                        mats[i] = this.MaterialToApplyToMesh[i];

                        renderer.materials = mats;


                    }

                    index++;
                }

            } else {

                mesh.GetComponent<Renderer>().material = this.MaterialToApplyToMesh[index];
                index++;

            }



        }

    }

    public void EnterSceneWithFinishedCharacter() {

        foreach (RadialMenuItem menuItem in radialWheel.menuItems) {
            if (menuItem.Item.GetChild(0) != null && menuItem.Item.GetChild(0).GetComponent<DragDrop>()) {
                DragDrop dragdrop = menuItem.Item.GetChild(0).GetComponent<DragDrop>();
                Debug.Log("has menu item" + menuItem.Item.GetChild(0).name);

                dragdrop.enabled = true;
                dragdrop.ChangeMaterialsOnMeshWithTag(new GameObject());
                dragdrop.enabled = false;

            }
        }
    
        ApplyFinalMaterials();
        director.Play();

        radialWheel.EndRadialMenu("Der Legionär ist ausgestattet.");


    }

    public void finishGame() {
        crossGameManager.isPlayingGame5 = false;
        ApplyFinalMaterials();

        crossGameManager.AddToScore(crossGameManager.colorToType("spiel"), tourLoader.netto_reward);
        crossGameManager.hasCompletedGame5 = true;

        GameObject.FindGameObjectWithTag("DragContainer").transform.GetChild(0).Destroy();

        radialWheel.EndRadialMenu("Der Legionär ist ausgestattet.");


        crossGameManager.ErrorLog("plays anim" + director.name);
        director.Play();

        StartCoroutine(DelayFundInfo());

    }
    IEnumerator DelayFundInfo() {

        float animTime = (float)director.playableAsset.duration;
        yield return new WaitForSecondsRealtime(animTime);
        ItemOnMap emptyItemOnMap = new ItemOnMap();
        fundInfo.Show(emptyItemOnMap, false, winMessage_Headline, reward.ToString(), winMessage_subheadline, winMessage_ButtonText, false);

    }



}
