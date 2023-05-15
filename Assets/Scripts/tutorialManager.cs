using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class tutorialManager : MonoBehaviour
{
    public GameObject ignore;

    public VisualElement m_Root;

    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        this.m_Root = GetComponent<UIDocument>().rootVisualElement;
        Button introStartBtn = m_Root.Q<Button>("btnStartGame");

        if (introStartBtn != null) {

            introStartBtn.clicked += delegate {
                this.m_Root.style.display = DisplayStyle.None;

                Debug.Log("tutorial button clicked");

                if (gameManager != null && gameManager.GetComponent<startGame2>()) {
                    gameManager.GetComponent<startGame2>().startScan();
                }
            };

           
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ignore.tag == "ignore") {
            return;
        }
    }
}
