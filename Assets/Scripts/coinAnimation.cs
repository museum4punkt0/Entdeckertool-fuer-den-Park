using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class coinAnimation : MonoBehaviour
{
    // Start is called before the first frame updat
    public GameObject[] colored_ui_elements;
    public List<GameObject> coins = new List<GameObject>();
    public GameObject CoinAmountText;
    public GameObject animationContainer;
    public float duration;


    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        colored_ui_elements = GameObject.FindGameObjectsWithTag("ui_element_to_change_color");
        foreach (GameObject coin in coins) {
            coin.SetActive(true);
        }

    }


    public void UpdateAndShowCoinAnimation(Color uiColor, int CoinAmount) {

        Debug.Log("PLAYS COIN ANIM");
        this.animationContainer.SetActive(true);
        colored_ui_elements = GameObject.FindGameObjectsWithTag("ui_element_to_change_color");

        if (CoinAmountText.GetComponent<TextMeshProUGUI>()) {
            CoinAmountText.GetComponent<TextMeshProUGUI>().text = CoinAmount.ToString();
        }

        /*
        if (CoinAmount < 6) {
            int amountToRemove = 6 - CoinAmount;
            for (int i = 0; i < amountToRemove; i++) {
                int randomIndex = Random.Range(0, coins.Count-1);
                coins[randomIndex].SetActive(false);
                coins.RemoveAt(randomIndex);
            }


        }

        */
        for (int i = 0; i < colored_ui_elements.Length; i++) {
            if (colored_ui_elements[i].GetComponent<Image>()) {
                colored_ui_elements[i].GetComponent<Image>().color = uiColor;
            } else if (colored_ui_elements[i].GetComponent<TextMeshProUGUI>()) {
                colored_ui_elements[i].GetComponent<TextMeshProUGUI>().color = uiColor;
            }
        }

        StartCoroutine(RemoveCoinAnimation());

    }

    public IEnumerator RemoveCoinAnimation() {
        yield return new WaitForSecondsRealtime(duration);
        this.animationContainer.SetActive(false);

    }
}
