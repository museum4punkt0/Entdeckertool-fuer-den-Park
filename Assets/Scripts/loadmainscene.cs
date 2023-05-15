using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class loadmainscene : MonoBehaviour
{

    Button start;
    Label loadingLabel;

    public void LoadGame_static() {
        start = this.gameObject.GetComponent<UIDocument>().rootVisualElement.Q<Button>("startBtn");
        start.clicked += delegate {
            SceneManager.LoadScene("MainScene");
    	};
        loadingLabel = this.gameObject.GetComponent<UIDocument>().rootVisualElement.Q<Label>("loading");

        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame() {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("MainScene");
        loadOperation.allowSceneActivation = false;

        while (!loadOperation.isDone) {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingLabel.text = "Loading . . . " + progressValue.ToString();
            print("progressValue: " + progressValue);

            if (loadOperation.progress >= 0.9f) {
                loadingLabel.style.display = DisplayStyle.None;
                start.style.display = DisplayStyle.Flex;
            }

            yield return null;
        }
    }
}



