using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinCostumiser : MonoBehaviour
{
    public GameObject frame;
    public GameObject scanContainer;
    public List<GameObject> scans = new List<GameObject>();
    public GameObject IllustrationContainerPrefab;
    public Illustration illustration;

    public GameObject portrait;
    public GameObject square;
    public GameObject content;

    public float FundObjectAppearAnimationDuration;
    public GameObject ImageObjectPrefab;
    CrossGameManager crossGameManager;

    [Header("Context Animation")]
    public float endOpacity = 0;
    public float duration = 10f;

    public bool hasIllustrationParts;
    public bool shouldCreateIllustration = false;

    [Header("If it contains 3D element it shouldnt contain a background")]
    public bool Has3D;
    public GameObject background;

    public List<GameObject> TransperencyLayersNeededFor3D = new List<GameObject>();
    public List<GameObject> objectsThatHasMaterialsThatChangeOnPlay = new List<GameObject>();

    public GameObject siedlung;


    private void Start() {

        GameObject crossGameManagerObject = GameObject.FindGameObjectWithTag("CrossGameManager");

        if (crossGameManagerObject != null) {
            crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        }

    }
    private void OnDisable() {

        print("disables illustration controller and stops coroutines");
        StopAllCoroutines();
    }

    private void OnDestroy() {

        print("destroy illustration controller and stops coroutines");
        StopAllCoroutines();

    }


    public IEnumerator CreatePinContent(ItemOnMap selectedItemOnMap){

        if (crossGameManager == null) {
            GameObject crossGameManagerObject = GameObject.FindGameObjectWithTag("CrossGameManager");

            //if (crossGameManagerObject != null) {
            //    crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
            //}

        }


        if (selectedItemOnMap.Poi.attributes.illustration != null && selectedItemOnMap.Poi.attributes.illustration.data.id != 0) {

            crossGameManager.ErrorLog("illustration:" + selectedItemOnMap.Poi.attributes.illustration.data.id);
            ////DELETE ANY ILLUSTRATION SCAN CONTENT
            if (scanContainer.transform.childCount > 0) {
                for (int i = 0; i < scanContainer.transform.childCount; i++) {
                    Destroy(scanContainer.transform.GetChild(i));
                }
            }

            ////SET FRAME FOR DIF CATEGORY CONTENT
            if (selectedItemOnMap.Poi.attributes.fundobjekt.data.id != 0) {

                string category = selectedItemOnMap.Poi.attributes.fundobjekt.data.attributes.category;

                crossGameManager.ErrorLog("CATEGORY:" + selectedItemOnMap.Name + category);

                if (category == "Germanen" || category == "UbersForschen" || category == "AlltagUndUnterwegsSein" || category == "Abschalten") {

                    square.SetActive(true);
                    content = square;
                    portrait.SetActive(false);

                    ChangeColorOfChildren(content.transform, crossGameManager.colorToType(category), true);



                } else {

                    crossGameManager.ErrorLog("isnt a selected category");

                    portrait.SetActive(true);
                    content = portrait;
                    square.SetActive(false);

                    ChangeColorOfChildren(content.transform, crossGameManager.colorToType(category), true);

                }

            } else {
                content = portrait;
                portrait.SetActive(true);
                square.SetActive(false);

                ChangeColorOfChildren(content.transform, crossGameManager.colorToType(""), true);

            }


            if (!selectedItemOnMap.Poi.attributes.illustration.data.attributes.isFramed) {
                content.SetActive(false);
                portrait.transform.parent.transform.parent.gameObject.GetComponent<Collider>().enabled = false;
            }


            ////SINGLE SCAN CONTAINER
            if (selectedItemOnMap.Poi.attributes.illustration.data.attributes.IllustrationParts.Count == 1 && selectedItemOnMap.Poi.attributes.illustration.data.attributes.IllustrationParts[0].scan != null && selectedItemOnMap.Poi.attributes.illustration.data.attributes.IllustrationParts[0].scan != "" && selectedItemOnMap.Poi.attributes.illustration.data.attributes.IllustrationParts[0].scan != "Siedlung") {
                
                /////create normal Pin with 3D scan
                IllustrationPart illustrationPart = selectedItemOnMap.Poi.attributes.illustration.data.attributes.IllustrationParts[0];

                GameObject scanObject = Instantiate(scans.Find(item => item.name == illustrationPart.scan), this.scanContainer.transform);
                scanObject.transform.localScale = new Vector3(illustrationPart.Scale, illustrationPart.Scale, illustrationPart.Scale);

                scanObject.GetComponent<MeshRenderer>().material.SetFloat("_Effect", 3);
                objectsThatHasMaterialsThatChangeOnPlay.Add(scanObject);


                illustration = selectedItemOnMap.Poi.attributes.illustration.data.attributes;
                

                //REGULAR ILLUSTRATION CONATINER
            } else if (selectedItemOnMap.Poi.attributes.illustration.data.attributes.IllustrationParts.Count > 0) {


                print("CREATES ILLUSTRATION CONTAINER");

                illustration = selectedItemOnMap.Poi.attributes.illustration.data.attributes;

                CreateIllustrationContainer(illustration);

            } else {
                /// delete frame
                frame.gameObject.SetActive(false);
                print("deletes frame");

            }
        } else {
            /// delete frame
            frame.gameObject.SetActive(false);
            print("deletes frame 2");

        }

        yield return null;

    }

    public void animateMaterials() {
        print("animates materials" + objectsThatHasMaterialsThatChangeOnPlay.Count);
        foreach (GameObject scanObject in objectsThatHasMaterialsThatChangeOnPlay) {
            print("animates material in" + scanObject.name);
            StartCoroutine(ChangeValueInMaterial("_Effect", scanObject.GetComponent<MeshRenderer>().material, scanObject.GetComponent<MeshRenderer>().material.GetFloat("_Effect"), -4f, FundObjectAppearAnimationDuration));

        }
    }

    IEnumerator ChangeValueInMaterial(string floatName, Material mat, float v_start, float v_end, float duration) {


        yield return new WaitForSeconds(duration-3);
        Debug.Log("change mat" + mat.name);
        float value = v_start;
        float elapsed = 0.0f;
        while (elapsed < duration) {
            value = Mathf.Lerp(v_start, v_end, elapsed / duration);
            mat.SetFloat(floatName, value);

            elapsed += Time.deltaTime;
            yield return null;
        }
        value = v_end;
        mat.SetFloat(floatName, value);
    }

    public void CreateIllustrationContainer(Illustration Illustration) {

        hasIllustrationParts = true;

        ConvertToSprite(Illustration);

        Has3D = Illustration.contains3D;

        duration = Illustration.animationSpeed;

    }



    public async void ConvertToSprite(Illustration illustration) {

        foreach (IllustrationPart illustrationPart in illustration.IllustrationParts) {

            Debug.Log("CHECK Illustration part" + illustrationPart.image);
            if (illustrationPart.image != null && illustrationPart.image.ConvertedSprite == null) {
                Sprite image = await illustrationPart.image.ToSprite();
                Debug.Log("Image" + image);

            }
        }

        StartCoroutine(LoadIllustration(illustration));
    }



    public IEnumerator LoadIllustration(Illustration illustration) {

        print("LOADS ILLUSTRATION");

        foreach (IllustrationPart illustrationPart in illustration.IllustrationParts) {

            GameObject ImageObject;

            if (illustrationPart.scan == null || illustrationPart.scan == "") {

                ImageObject = Instantiate(ImageObjectPrefab, new Vector3(illustrationPart.xOffset, illustrationPart.yOffSet, illustrationPart.zOffSet), Quaternion.identity, this.frame.transform);
                if (!ImageObjectPrefab.GetComponent<SpriteRenderer>()) {
                    ImageObject.AddComponent<SpriteRenderer>();
                }
                ImageObject.GetComponent<SpriteRenderer>().sprite = illustrationPart.image.ConvertedSprite;
                ImageObject.transform.localPosition = new Vector3(illustrationPart.xOffset, illustrationPart.yOffSet, illustrationPart.zOffSet);

                if (illustration.isFramed) {
                    ImageObject.layer = 6;
                } else {
                    ImageObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                }
               
                ImageObject.transform.localScale = new Vector3(illustrationPart.Scale, illustrationPart.Scale, illustrationPart.Scale);


                if (illustrationPart.shouldAnimate) {
                    StartCoroutine(FadeOut(ImageObject.GetComponent<SpriteRenderer>(), this.duration));
                }

            } else if (illustrationPart.scan != null) {

                ImageObject = Instantiate(scans.Find(item => item.name == illustrationPart.scan), new Vector3(illustrationPart.xOffset, illustrationPart.yOffSet, illustrationPart.zOffSet), Quaternion.identity);

                if (illustrationPart.scan != "Siedlung") {
                    ImageObject.transform.parent = this.scanContainer.transform;
                } else {
                    ImageObject.transform.parent = this.transform;
                    siedlung = ImageObject;
                    GetComponent<AnimatePinOnApproach>().isSiedlung = true;
                }

                ImageObject.transform.localPosition = new Vector3(illustrationPart.xOffset, illustrationPart.yOffSet, illustrationPart.zOffSet);
                ImageObject.transform.eulerAngles = new Vector3(-90f, 0, 0);
                if (illustration.isFramed) {
                    ImageObject.layer = 6;
                }
                ImageObject.transform.localScale = new Vector3(illustrationPart.Scale, illustrationPart.Scale, illustrationPart.Scale);
                if (ImageObject.GetComponent<MeshRenderer>()) {
                    ImageObject.GetComponent<MeshRenderer>().material.SetFloat("_Effect", 2);
                    objectsThatHasMaterialsThatChangeOnPlay.Add(ImageObject);
                }



            }

            if (!this.Has3D) {
                this.background.gameObject.SetActive(true);
                foreach (GameObject TransperencyLayerNeededFor3D in this.TransperencyLayersNeededFor3D) {
                    TransperencyLayerNeededFor3D.SetActive(false);
                }
            } else {
                this.background.gameObject.SetActive(false);
                foreach (GameObject TransperencyLayerNeededFor3D in this.TransperencyLayersNeededFor3D) {
                    TransperencyLayerNeededFor3D.SetActive(true);
                }

            }

            yield return null;
        }


    }


    private IEnumerator FadeOut(SpriteRenderer sr, float duration) {
        float alphaVal = sr.color.a;
        Color tmp = sr.color;

        while (sr.color.a > 0) {
            alphaVal -= 0.01f;
            tmp.a = alphaVal;
            sr.color = tmp;

            yield return new WaitForSeconds(duration); // update interval
        }

        StartCoroutine(FadeIn(sr, duration));


    }


    private IEnumerator FadeIn(SpriteRenderer sr, float duration) {
        float alphaVal = sr.color.a;
        Color tmp = sr.color;

        while (sr.color.a < 1) {
            alphaVal += 0.01f;
            tmp.a = alphaVal;
            sr.color = tmp;

            yield return new WaitForSeconds(duration); // update interval
        }

        StartCoroutine(FadeOut(sr, duration));

    }

    public IEnumerator AppearIn(SpriteRenderer sr, float duration) {
        float alphaVal = sr.color.a;
        Color tmp = sr.color;

        while (sr.color.a < 1) {
            alphaVal += 0.01f;
            tmp.a = alphaVal;
            sr.color = tmp;

            yield return new WaitForSeconds(duration); // update interval
        }
    }



    // Update is called once per frame
    public void ChangeColorOfChildren(Transform transform, Color CustomColor, bool shouldChangePin) {

        
        if (transform.gameObject.CompareTag("custom_color")) {

            if (transform.GetComponent<SkinnedMeshRenderer>()) {
                transform.GetComponent<SkinnedMeshRenderer>().materials[0].color = CustomColor;
                transform.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_Color", CustomColor);

            } else if (transform.GetComponent<MeshRenderer>()) {
                transform.GetComponent<MeshRenderer>().material.color = CustomColor;
                transform.GetComponent<MeshRenderer>().material.SetColor("_Color", CustomColor);

            } else if (transform.GetComponent<SpriteRenderer>()) {
                transform.GetComponent<SpriteRenderer>().color = CustomColor;

            }

        }


        foreach (Transform child in transform) {
            if (child.gameObject.CompareTag("custom_color")) {



                if (shouldChangePin) {
                    if (child.GetComponent<SkinnedMeshRenderer>()) {
                        child.GetComponent<SkinnedMeshRenderer>().materials[0].color = CustomColor;
                        child.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_Color", CustomColor);
                    } else if (child.GetComponent<MeshRenderer>()) {
                        child.GetComponent<MeshRenderer>().material.color = CustomColor;
                        child.GetComponent<MeshRenderer>().material.SetColor("_Color", CustomColor);

                    } else if (child.GetComponent<SpriteRenderer>()) {
                        child.GetComponent<SpriteRenderer>().color = CustomColor;


                    
                    }
                } else {
                    if (child.gameObject.name != "Pin_New" && child.GetComponent<SkinnedMeshRenderer>()) {
                        child.GetComponent<SkinnedMeshRenderer>().materials[0].color = CustomColor;
                        child.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_Color", CustomColor);
                    } else if (child.gameObject.name != "Pin_New" && child.GetComponent<MeshRenderer>()) {
                        child.GetComponent<MeshRenderer>().material.color = CustomColor;
                        child.GetComponent<MeshRenderer>().material.SetColor("_Color", CustomColor);

                    } else if (child.GetComponent<SpriteRenderer>()) {
                        child.GetComponent<SpriteRenderer>().color = CustomColor;
                

                    }
                }

       




            }



            if (child.childCount > 0) {


                ChangeColorOfChildren(child, CustomColor, true);
            }
        }
    }

}
