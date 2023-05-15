using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class game2Controller : MonoBehaviour
{
    public GameObject detector;
    public Button introStartBtn;
    CrossGameManager crossGameManager;

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Button introStartBtn = GetComponent<UIDocument>().rootVisualElement.Q<Button>("btnStartGame");

        introStartBtn.clicked += delegate {
            detector.SetActive(true);
        
        };

        StartCoroutine(crossGameManager.strapiService.getSpiel2Content(UploadTutorialContent));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void UploadTutorialContent(StrapiSingleResponse<Game> res) {
        Game _data = res.data;

//        this.gameObject.GetComponent<UIDocument>().rootVisualElement.Q<TextElement>("tutorial-text").text = _data.attributes.description;
    }

}
