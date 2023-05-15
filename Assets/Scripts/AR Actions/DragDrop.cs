using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;

    public List<Material> MaterialToApplyToMesh;
    public List<GameObject> MeshToApplyMaterialTo;

    public List<Material> SecondaryMaterialToApplyToMesh;
    public List<GameObject> SecondaryMeshToApplyMaterialTo;

    private Vector2 position;
    public GameObject GameController;
    public GameObject RadialMenu;

    private GameObject highlightedMesh;
    public Material hightLightMaterial;
    public Material notHighLightMaterial;

    public GameObject HighLightRing;

    public GameObject ParticleCloudDiggingEffect;

    private bool canCheckSwiping;
    private Vector2 fingerDownPos;
    private Vector2 fingerUpPos;
    public bool detectSwipeAfterRelease = false;
    public float SWIPE_THRESHOLD = 20f;
    public float swipeQouta = 70;
    private float swipeAmount = 0;
    CrossGameManager crossGameManager;


    public float currentAlpha = 1;
    bool hasParticleEffect = false;

    private void Awake() {
        this.rectTransform = GetComponent<RectTransform>();
        this.position = this.rectTransform.transform.position;
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

    }

    public void OnDrag(PointerEventData eventData) {
        this.rectTransform.anchoredPosition += eventData.delta /this.canvas.scaleFactor;
        this.HighLightRing.SetActive(false);
        this.GetComponent<Outline>().enabled = true;

        ///handle draggin over gameobjects as well
        if (this.GameController.GetComponent<startGame5_AR_Ready>() || this.GameController.GetComponent<startGame3_AR_Ready>() && this.GameController.GetComponent<startGame3_AR_Ready>().part1.activeSelf && this.hightLightMaterial != null) {

            HandleDrag(eventData.position);
        }

        if (this.GameController.GetComponent<Game4Controller>()) {
            canCheckSwiping = true;

        }

    }

    void HandleDrag(Vector3 pos) {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)) {

            ///if dragable UI rect transform hits specified tag (either hardcoded or because it is tagged with the same as the object name
            if (this.transform.gameObject.CompareTag(hit.transform.gameObject.tag) || this.transform.gameObject.tag == hit.transform.gameObject.name) {

                this.highlightedMesh = hit.transform.gameObject;

                HighLightOnHover(this.highlightedMesh, true);


            } else {
                HighLightOnHover(this.highlightedMesh, false);

            }

        } else {

            HighLightOnHover(this.highlightedMesh, false);
        }

    }

    public void OnEndDrag(PointerEventData eventData) {
        
        this.GetComponent<Outline>().enabled = false;

        if (this.GameController.GetComponent<Game4Controller>()) {
            canCheckSwiping = false;
            HandleDrop(eventData.position);

        } else {
            HandleDrop(eventData.position);
        }


    }

    void Update() {

        if (canCheckSwiping) {
            foreach (Touch touch in Input.touches) {
                if (touch.phase == TouchPhase.Began) {
                    fingerUpPos = touch.position;
                    fingerDownPos = touch.position;
                }

                //Detects Swipe while finger is still moving on screen
                if (touch.phase == TouchPhase.Moved) {
                    if (!detectSwipeAfterRelease) {
                        fingerDownPos = touch.position;
                        DetectSwipe();
                    }
                }

                ////Detects swipe after finger is released from screen
                //if (touch.phase == TouchPhase.Ended) {
                //    fingerDownPos = touch.position;
                //    DetectSwipe();
                //}
            }

        }

    }


    void HandleDrop(Vector3 pos) {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)) {

            Debug.Log(hit.transform.gameObject.name);


            ///if dragable UI rect transform hits specified tag (either hardcoded or because it is tagged with the same as the object name
            if (this.transform.gameObject.CompareTag(hit.transform.gameObject.tag) || this.transform.gameObject.tag == hit.transform.gameObject.name) {

                ///checks if the drag and drop is connected to teil2 - then toggle custom animation
                if (hit.transform.gameObject.GetComponent<Animator>()) {

                    if (this.transform.gameObject.tag == "tool1" && hit.transform.gameObject.name == "tool1") {
                        hit.transform.gameObject.GetComponent<Animator>().Play("digging1");

                        //StartCoroutine(MoveObject(hit.transform.gameObject, hit.transform.position, new Vector3(hit.transform.position.x, hit.transform.position.y - 10f, hit.transform.position.z), 10));
                        
                        StartCoroutine(DiggingAnimation(hit));
                        this.GameController.GetComponent<startGame3_AR_Ready>().SetScore1();

               
                        StartCoroutine(FadeEarthMaterial("Dig_loweringPlane1", hit));

                    }
                    if (this.transform.gameObject.tag == "tool2" && hit.transform.gameObject.name == "tool2") {
                        hit.transform.gameObject.GetComponent<Animator>().Play("digging2");
                        //StartCoroutine(MoveObject(hit.transform.gameObject, hit.transform.position, new Vector3(hit.transform.position.x, hit.transform.position.y - 10f, hit.transform.position.z), 20));
                        StartCoroutine(DiggingAnimation(hit));
                        this.GameController.GetComponent<startGame3_AR_Ready>().SetScore1();

                        StartCoroutine(FadeEarthMaterial("Dig_loweringPlane2", hit));

                    }
                    if (this.transform.gameObject.tag == "tool3" && hit.transform.gameObject.name == "tool3") {
                        hit.transform.gameObject.GetComponent<Animator>().Play("digging3");
                        //StartCoroutine(MoveObject(hit.transform.gameObject, hit.transform.position, new Vector3(hit.transform.position.x, hit.transform.position.y - 10f, hit.transform.position.z), 30));

                        StartCoroutine(DiggingAnimation(hit));
                        this.GameController.GetComponent<startGame3_AR_Ready>().SetScore1();

                       StartCoroutine(FadeEarthMaterial("Dig_loweringPlane3", hit));

                    }
                } 
                else if (this.GameController.GetComponent<startGame5_AR_Ready>()) {

                    ChangeMaterialsOnMeshWithTag(hit.transform.gameObject);
                    this.GameController.GetComponent<startGame5_AR_Ready>().SetScore1();

                }
                ////checks if the drag and drop is connected to teil1 - then add material to mesh
                else if (this.GameController.GetComponent<startGame3_AR_Ready>()) {

                    if(!this.GameController.GetComponent<startGame3_AR_Ready>().part2.activeSelf){
                        ChangeMaterialsOnMeshWithTag(hit.transform.gameObject);
                    }

                    this.GameController.GetComponent<startGame3_AR_Ready>().SetScore1();

                }
                
                
                this.Destroy();

                Destroy(this.gameObject);

                this.RadialMenu.GetComponent<RadialWheel>().RemoveFromMenu();
                this.HighLightRing.SetActive(true);

            } else {

                this.rectTransform.position = this.position;
            }

        } else {
            this.rectTransform.position = this.position;
            
            //this.HighLightRing.SetActive(true);

        }

    }


    IEnumerator MoveObject(GameObject player, Vector3 source, Vector3 target, float duration) {



        Debug.Log("starts to move object");
        float elapsedTime = 0;

        while (elapsedTime < duration)
         {

            Debug.Log("time passed" + elapsedTime + "__pos:" + player.transform.position.y);
            player.transform.position = Vector3.Lerp(source, target, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;

            Debug.Log("time passed again" + elapsedTime + "__pos:" + player.transform.position.y);
            yield return null;
        }
        player.transform.position = target;



    }

    IEnumerator DiggingAnimation(RaycastHit hit) {
    
        hit.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
    }

    public IEnumerator FadeEarthMaterial(string animationName, RaycastHit hit) {


      
        //float elapsedTime = 0;

        //while (hit.transform.gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("Effect_Start") < 0.6) {
        //    float DirtErosionValue = hit.transform.gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("Effect_Start") + 0.001f;

        //    Debug.Log("erodes" + hit.transform.gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("Effect_Start"));

        //    hit.transform.gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("Effect_Start", DirtErosionValue);

        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}


        //hit.transform.gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("Effect_Start", 0.59f);
        //hit.transform.gameObject.Destroy();



        float dissolveValue = hit.transform.gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("Effect_Start");
        float goalvalue = 0.59f;
        Debug.Log("Dissolve coroutine started");
        while(dissolveValue < goalvalue)
        {
            dissolveValue += Time.deltaTime / 100f;
         
            hit.transform.gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("Effect_Start", dissolveValue);

            yield return null;
        }
    }

    void set_skinned_mat(SkinnedMeshRenderer renderer, int Mat_Nr, Material Mat) {
        Material[] mats = renderer.materials;

        mats[Mat_Nr] = Mat;

        renderer.materials = mats;
    }

    void set_mat(MeshRenderer renderer, int Mat_Nr, Material Mat) {
        Material[] mats = renderer.materials;

        mats[Mat_Nr] = Mat;

        renderer.materials = mats;
    }

    public void ChangeMaterialsOnMeshWithTag(GameObject item)
    {
  
        int index = 0;
        foreach (GameObject mesh in this.MeshToApplyMaterialTo) {




            for (int i = 0; i < mesh.GetComponent<Renderer>().materials.Length; i++) {
                Material materialInstance = mesh.GetComponent<Renderer>().materials[i];
                //Material materialInstance = mesh.GetComponent<Renderer>().sharedMaterials[i];


                materialInstance = this.MaterialToApplyToMesh[i];



                if (mesh.GetComponent<SkinnedMeshRenderer>()) {

                    SkinnedMeshRenderer renderer = mesh.GetComponent<SkinnedMeshRenderer>();
                    //Material[] mats = renderer.materials;

                    //mats[i] = this.MaterialToApplyToMesh[i];

                    set_skinned_mat(renderer, i, this.MaterialToApplyToMesh[i]);

                    crossGameManager.ErrorLog("MAT SMR" + i + renderer.materials[i].name);


                    //renderer.materials = mats;

                    //crossGameManager.ErrorLog("apply" + mats[i].name + "to SMR");


                } else if (mesh.GetComponent<MeshRenderer>()) {
                    MeshRenderer renderer = mesh.GetComponent<MeshRenderer>();
                    //Material[] mats = renderer.materials;

                    //mats[i] = this.MaterialToApplyToMesh[i];


                    //renderer.materials = mats;

                    //crossGameManager.ErrorLog("apply" + mats[i].name + "to MR");

                    set_mat(renderer, i, this.MaterialToApplyToMesh[i]);

                    crossGameManager.ErrorLog("MAT MR" + i + renderer.materials[i].name);




                }
            }

            index++;
        
        }



        if (item.GetComponent<MeshCollider>()) {
            item.GetComponent<MeshCollider>().enabled = false;
        }
        if (item.GetComponent<BoxCollider>()) {
            item.GetComponent<BoxCollider>().enabled = false;
        }
        if (item.GetComponent<SphereCollider>()) {
            item.GetComponent<SphereCollider>().enabled = false;
        }

        crossGameManager.ErrorLog("runs entire mat update");

    }

    void HighLightOnHover(GameObject highlightedMesh, bool shouldHighLight) {


        if (this.GameController.GetComponent<startGame3_AR_Ready>() || this.GameController.GetComponent<startGame5_AR_Ready>()) {
            if (shouldHighLight) {
                
                highlightedMesh.GetComponent<Renderer>().material = this.hightLightMaterial;

                //crossGameManager.ErrorLog("highlights mesh" + highlightedMesh.GetComponent<Renderer>().material);

            } else if (this.highlightedMesh != null) {
                //Debug.Log("Tries to deselect" + highlightedMesh.name);

                //crossGameManager.ErrorLog("deselects mesh");

                //Debug.Log("Tries to remove highlight" + highlightedMesh.GetComponent<Renderer>().sharedMaterial);
                highlightedMesh.GetComponent<Renderer>().material = this.notHighLightMaterial;
                this.highlightedMesh = null;
            }
        }


    }

    void DetectSwipe() {

        if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue()) {
            if (fingerDownPos.y - fingerUpPos.y > 0) {
                OnSwipeUp();
            } else if (fingerDownPos.y - fingerUpPos.y < 0) {
                OnSwipeDown();
            }
            fingerUpPos = fingerDownPos;

        } else if (HorizontalMoveValue() > SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue()) {
            if (fingerDownPos.x - fingerUpPos.x > 0) {
                OnSwipeRight();
            } else if (fingerDownPos.x - fingerUpPos.x < 0) {
                OnSwipeLeft();
            }
            fingerUpPos = fingerDownPos;

        } else {
            Debug.Log("No Swipe Detected!");
        }
    }

    float VerticalMoveValue() {
        return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
    }

    float HorizontalMoveValue() {
        return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
    }

    void OnSwipeUp() {
        //Do something when swiped up
    }

    void OnSwipeDown() {
        //Do something when swiped down
    }

    void OnSwipeLeft() {
        //Do something when swiped left
        SwipeEffect();
    }

    void OnSwipeRight() {
        //Do something when swiped right
        SwipeEffect();

    }

    void SwipeEffect() {
        swipeAmount += 1f;


        print("swipe quota: " + swipeQouta);
        GameObject cleanEffect;


        float objectCleanliness = this.GameController.GetComponent<Game4Controller>().CleanObject();
        
        if (RadialMenu.GetComponent<RadialWheel>().particleEffect != null) {

            hasParticleEffect = true;

            cleanEffect = Instantiate(RadialMenu.GetComponent<RadialWheel>().particleEffect.gameObject, this.transform);

            ParticleSystem ps = cleanEffect.GetComponent<ParticleSystem>();
            var main = ps.main;

            if (true) {
                
                Vector3[] corners = new Vector3[4];

                Image image = this.GameController.GetComponent<Game4Controller>().objectDirty_Image;

                image.rectTransform.GetWorldCorners(corners);

                Rect newRect = new Rect(corners[0], corners[2] - corners[0]);
                
                
                if (newRect.Contains(Input.mousePosition)) {
                    Vector2 size = new Vector2(822, 512);
                    Vector2 pixelCoord = Input.mousePosition - corners[0];
                    pixelCoord /= image.rectTransform.rect.size;
                    pixelCoord *= size;
                    Color colorPixel = image.sprite.texture.GetPixel((int)pixelCoord.x, (int)pixelCoord.y);

                    print("COLOR under cursor" + colorPixel);

                    Gradient gradient = new Gradient();
                    GradientColorKey[] colorKey = colorKey = new GradientColorKey[2];
                    colorKey[0].color = colorPixel;
                  
                    colorKey[0].time = 0.0f;
                    colorKey[1].color = colorPixel;

                    colorKey[1].time = 1.0f;

                    GradientAlphaKey[] alphaKey = new GradientAlphaKey[3];
                    alphaKey[0].alpha = 0f;
                    alphaKey[0].time = 0.0f;
                    alphaKey[1].alpha = 1f;
                    alphaKey[1].time = 0.5f;
                    alphaKey[2].alpha = 0f;
                    alphaKey[2].time = 1f;

                    gradient.SetKeys(colorKey, alphaKey);

                    main.startColor = gradient;


                }
            }

            //Texture2D texture2D = new Texture2D(this.GameController.GetComponent<Game4Controller>().objectDirty_Image.mainTexture.width, this.GameController.GetComponent<Game4Controller>().objectDirty_Image.mainTexture.height, TextureFormat.RGBA32, false);

        




            cleanEffect.SetActive(true);
        }


     

    

        print("goal:" + (this.GameController.GetComponent<Game4Controller>().currentAlpha - (swipeQouta / 100)) + "__currentOpacity: " + objectCleanliness + "__current alpha" + this.GameController.GetComponent<Game4Controller>().currentAlpha);

        if (objectCleanliness <= this.GameController.GetComponent<Game4Controller>().currentAlpha -(swipeQouta / 100)) {

            this.GameController.GetComponent<Game4Controller>().currentAlpha = objectCleanliness;

            this.GameController.GetComponent<Game4Controller>().CheckIfObjectIsClean();

            if (this.GameController.GetComponent<DetachAndKillParticles>()) {
                this.GameController.GetComponent<DetachAndKillParticles>().DetachAndKillParticle(this.gameObject);
            }


            this.Destroy();
            Destroy(this.gameObject);

            this.RadialMenu.GetComponent<RadialWheel>().RemoveFromMenu();



        }
    }


   



}
