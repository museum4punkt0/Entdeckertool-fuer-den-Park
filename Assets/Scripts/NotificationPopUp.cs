using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NotificationPopUp : MonoBehaviour
{
    CrossGameManager crossGameManager;

    public GameObject popup;
    public Button closePopUpBTN;
    public Button closeSceneBTN;
    public TextMeshProUGUI ctaButton;
    public TextMeshProUGUI headLine;
    public TextMeshProUGUI subHeadline;
    public string Target;
    void Start()
    {

        //crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
        //closePopUpBTN = this.m_Root.Q<Button>("closePopUp");
        //closeSceneBTN = this.m_Root.Q<Button>("closeSceneBtn");
        //ctaButton = this.m_Root.Q<Button>("ctaButton");
        //headLine = this.m_Root.Q<Label>("headline");
        //subHeadline = this.m_Root.Q<Label>("subheadline");


        //this.m_Root.Q<VisualElement>("PopUp").style.display = DisplayStyle.None;

        //closePopUpBTN.clicked += delegate {
        //    this.m_Root.Q<VisualElement>("PopUp").style.display = DisplayStyle.None;
        //    Debug.Log("pushes other button");
        //};

        //ctaButton.clicked += delegate {
        //    GoToNotificationTarget();
        //};
    }

    public void GoToNotificationTarget() {
        crossGameManager.LastSceneVisited = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(Target);
        popup.SetActive(false);
    }

    // Update is called once per frameine
    public void Show(string headline, string subheadline, string buttonText, string buttonTarget)
    {
        this.Target = buttonTarget;
        ctaButton.text = buttonText;
        headLine.text = headline;
        subHeadline.text = subheadline;


        popup.SetActive(true);


        crossGameManager.loadsWithNotification = false;
    }
}
