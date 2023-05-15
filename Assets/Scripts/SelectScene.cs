using UnityEngine;
using UnityEngine.SceneManagement;


    public class SelectScene : MonoBehaviour
    {

        // Use this for initialization

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
