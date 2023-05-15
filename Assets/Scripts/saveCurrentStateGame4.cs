using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class saveCurrentStateGame4 : MonoBehaviour
{
    public GameObject header;
    public GameObject gameUI;
    VisualElement gameUIRoot;
    VisualElement materialWrapper01;
    VisualElement materialWrapper02;
    VisualElement materialWrapper03;
    VisualElement materialWrapper04;
    VisualElement root;
    Button closeBtn;
    CrossGameManager crossGameManagerScript;
    public List<VisualElement> storage = new List<VisualElement>();

    // Start is called before the first frame update
    void Start()
    {
        crossGameManagerScript = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();

        root = header.gameObject.GetComponent<UIDocument>().rootVisualElement;
        gameUIRoot = gameUI.gameObject.GetComponent<UIDocument>().rootVisualElement;
        materialWrapper01 = gameUIRoot.Q<VisualElement>("materialWrapper01");
        materialWrapper02 = gameUIRoot.Q<VisualElement>("materialWrapper02");
        materialWrapper03 = gameUIRoot.Q<VisualElement>("materialWrapper03");
        materialWrapper04 = gameUIRoot.Q<VisualElement>("materialWrapper04");

        closeBtn = root.Q<Button>("close");

        closeBtn.clicked += delegate {

            Save();
        };
    }

    public void Save() {
        // materialWrapper01.Children();
        materialWrapper01 = gameUIRoot.Q<VisualElement>("materialWrapper01");


        //gameUIRoot.Q<VisualElement>("materialWrappers").visualTreeAssetSource.CloneTree(gameUIRoot.Q<VisualElement>("temp"));
        //gameUIRoot.Q<VisualElement>("temp").visualTreeAssetSource.CloneTree(gameUIRoot.Q<VisualElement>("materialWrappers"));
        //LoadSavedContent();
        SceneManager.LoadScene("MainScene");
    }

    public void LoadSavedContent() {
        
    

    }

}
