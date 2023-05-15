using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    private string _nextScene= "ARScene";

    public void switchToAR() {
        SceneManager.LoadScene("Scenes/ARScene");
        this._nextScene = "MainScene";

    }
    public void switchToMainScene() {
        SceneManager.LoadScene("Scenes/MainScene");
        this._nextScene = "ARScene";

    }

    public void toggleScene() {
        Debug.Log(SceneManager.GetActiveScene().name);
        switch (SceneManager.GetActiveScene().name) {
            case "ARScene":
                this.switchToMainScene();
                break;
            case "MainScene":
                this.switchToAR();
                break;
        }
    }
}
