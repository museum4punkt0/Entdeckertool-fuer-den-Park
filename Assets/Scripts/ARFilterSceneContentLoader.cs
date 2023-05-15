using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Services;
//using UnityEngine.UIElements;
using TMPro;


public class ARFilterSceneContentLoader : MonoBehaviour
{
    public ARFilterController controller;

    public GameObject popUp;
    public TextMeshProUGUI popUpHeadline;
    public TextMeshProUGUI popUpConfirmButton;

    public TextMeshProUGUI maskButtonText;
    public TextMeshProUGUI helmetButtonText;

    public GameObject SelectFilterContainer;
    public GameObject AfterScreenshotContainer;
    public GameObject ScreenshotButton;

    public GameObject ImgPreview;


    public CrossGameManager crossGameManager;





    void Awake()
    {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

     //   if (SceneManager.GetActiveScene().name == "ARFilterScene") {
     //       mask.style.color = new Color(1f, 1f, 1f);
     //       mask.style.backgroundColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
     //   } else {
     //       helmet.style.color = new Color(1f, 1f, 1f);
     //       helmet.style.backgroundColor = new Color(0.3019608f, 0.1921569f, 0.6039216f);
     //   }

     //   screenshotBtn.clicked += delegate {
     //       controller.TakeScreenShot();
     //       crossGameManager.ErrorLog("takes screenshot");
    	//};

     //   mask.clicked += delegate {
     //       SceneManager.LoadScene("ARFilterScene");
     //   };

     //   helmet.clicked += delegate {
     //       SceneManager.LoadScene("ARFilterSceneHelmet");
     //   };

     //   closeScene.clicked += delegate {
     //       SceneManager.LoadScene("MainScene");
     //   };

     //   confirmPopUp.clicked += delegate {
     //       popUp.style.display = DisplayStyle.None;
     //   };

     //   closePopup.clicked += delegate {
     //       popUp.style.display = DisplayStyle.None;
     //   };

     //   trash.clicked += delegate {
     //       controller.TrashSchreenshot();
     //       BackToCameraView();
     //   };

     //   save.clicked += delegate {
     //       controller.SaveSchreenshot();
     //       popUp.style.display = DisplayStyle.Flex;
     //       BackToCameraView();
     //   };

     //   share.clicked += delegate {
     //       controller.ShareSchreenshot();
     //       BackToCameraView();
     //   };

        StartCoroutine(crossGameManager.strapiService.getARFilterPageContent(LoadContent));

    }

    async void LoadContent(StrapiSingleResponse<ARFilterPageData> res) {
        ARFilterPageData _data = res.data;
        maskButtonText.text = _data.attributes.maskButtonText;
        helmetButtonText.text = _data.attributes.helmetButtonText;
        popUpHeadline.text = _data.attributes.popUpText;
        popUpConfirmButton.text = _data.attributes.popUpButton;
    }

    public void CloseScene() {
        SceneManager.LoadScene("MainScene");
    }
    public void GoToHelmetScene() {
        SceneManager.LoadScene("ARFilterSceneHelmet");
    }
    public void GoToMaskeScene() {
        SceneManager.LoadScene("ARFilterScene");

    }


    public void PostScreenshot() {
        ScreenshotButton.SetActive(false);
        AfterScreenshotContainer.SetActive(true);
        SelectFilterContainer.SetActive(false);


    }

    public void BackToCameraView() {
        ScreenshotButton.SetActive(true);

        SelectFilterContainer.SetActive(true);
        AfterScreenshotContainer.SetActive(false);
        ImgPreview.SetActive(false);

    }

}
