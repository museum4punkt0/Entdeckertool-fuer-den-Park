using UnityEngine;
using UnityEngine.UIElements;

public class UITutorialController : MonoBehaviour
{
    public GameObject tuturialScene;
    public GameObject ubersichtScene;
    public VisualElement m_Root_Tutorial;
    public VisualElement m_Root_Ubersicht;
    public GameObject game5UI;
    public GameObject gameManager;

    private bool hasAssignedDescriptionToField;

    // Start is called before the first frame update
    void Start()
    {
        m_Root_Tutorial = tuturialScene.GetComponent<UIDocument>().rootVisualElement;

        if (ubersichtScene != null) {
            m_Root_Ubersicht = ubersichtScene.GetComponent<UIDocument>().rootVisualElement;

        }
        // m_Root_Ubersicht = ubersichtScene.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Game5");
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAssignedDescriptionToField && gameManager.GetComponent<Game5Manager>() && gameManager.GetComponent<Game5Manager>().game.attributes.description != "" && gameManager.GetComponent<Game5Manager>().game.attributes.description != null) {
            m_Root_Tutorial.Q<TextElement>("tutorial-text").text = gameManager.GetComponent<Game5Manager>().game.attributes.description;
            hasAssignedDescriptionToField = true;
        }

        if (m_Root_Tutorial.Q<Button>("btnStartGame") != null) {
            m_Root_Tutorial.Q<Button>("btnStartGame").clicked += delegate {

                m_Root_Tutorial.style.display = DisplayStyle.None;
                tuturialScene.active = false;
            };
        }

        if (m_Root_Ubersicht != null) {

            m_Root_Ubersicht.Q<Button>("popup-close").clicked += delegate {
                m_Root_Ubersicht.Q<VisualElement>("PopUp").style.display = DisplayStyle.None;
          
            };

            m_Root_Ubersicht.Q<Button>("popup-button").clicked += delegate {
                m_Root_Ubersicht.Q<VisualElement>("PopUp").style.display = DisplayStyle.None;
            };


        }


    }

}
