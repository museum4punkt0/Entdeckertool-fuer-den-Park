using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CustomGameOneCloser : MonoBehaviour
{
    public Game1Manager tutorial;
    VisualElement closePopUp;
    Button gameSave;
    Button gameContinue;
    Button close;
    Button exit;

    // Start is called before the first frame update
    void Start()
    {
        close = this.gameObject.GetComponent<UIDocument>().rootVisualElement.Q<Button>("close");
        closePopUp = this.gameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("CloseGamePopUp");
        gameSave = this.gameObject.GetComponent<UIDocument>().rootVisualElement.Q<Button>("save");
        gameContinue = this.gameObject.GetComponent<UIDocument>().rootVisualElement.Q<Button>("continue");
        exit = this.gameObject.GetComponent<UIDocument>().rootVisualElement.Q<Button>("exit");

        close.clicked += delegate {
            closePopUp.style.display = DisplayStyle.Flex;
        };

        gameSave.clicked += delegate {
            SceneManager.LoadScene("MainScene");
        };

        gameContinue.clicked += delegate {
            closePopUp.style.display = DisplayStyle.None;
        };

        exit.clicked += delegate {
            closePopUp.style.display = DisplayStyle.None;
        };
    }

}
