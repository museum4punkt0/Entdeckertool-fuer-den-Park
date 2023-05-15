using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider timerSlider;
    public float gameTime;

    private bool hasStartedTimer = false;
    private bool stopTimer;
    void Start()
    {
    }
    public void StartTimer() {
        Debug.Log("STARTS TIMER");
        this.timerSlider.gameObject.SetActive(true);
        this.stopTimer = false;
        this.hasStartedTimer = true;
        this.timerSlider.maxValue = this.gameTime;
        this.timerSlider.value = this.gameTime;
    }

    // Update is called once per frame
    void Update()
    {

        float time = this.gameTime - Time.time;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60f);
        if (this.hasStartedTimer) {
            if (time <= 0) {
                this.stopTimer = true;
                GetComponent<startGame2>().finishGameError();
            }
            if (this.stopTimer == false) {


                this.timerSlider.value = time;
            }

        }
   
    }
}
