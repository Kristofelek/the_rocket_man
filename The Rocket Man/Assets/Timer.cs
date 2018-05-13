using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text timerLabel;

    private float timer;

    // Use this for initialization
    void Start () {
        timer = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        //TODO MAKE GUI object to display the timer

        //var minutes = timer / 60; //Divide the guiTime by sixty to get the minutes.
        var seconds = timer % 60; //Use the euclidean division for the seconds.
        var fraction = (timer * 100) % 100;
        //timerLabel.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
        timerLabel.text = string.Format("{0:00} : {1:000}", seconds, fraction);
    }
}
