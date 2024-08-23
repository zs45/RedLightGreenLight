using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;			// lets the script find the text box to change

	public float seconds;			// holds the seconds
	public float minutes;			// holds the minutes
	public float milliseconds;		// holds the milliseconds

    public float time_left = 0f;


    public bool keepTiming = true;

    public Finish_Line_Controller controller; 
    public Manager manager;

    public bool stop_updating = true;

	// Use this for initialization
	void Start () {
        manager = GameObject.Find("GameManager").GetComponent<Manager>();
        controller = GameObject.Find("finish_line").GetComponent<Finish_Line_Controller>();
        time_left = manager.max_time;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(!stop_updating)
        {
            if (keepTiming)
            {
                time_left -= Time.deltaTime;
            }
            if (time_left > 0)
            {
                minutes = (int)((time_left) / 60f);      // Time is in seconds so it finds the minutes
                seconds = (int)((time_left) % 60f);      // Finds the seconds
                milliseconds = ((time_left) * 1000f);
                milliseconds = (milliseconds % 1000f);  // Finds the milliseconds
                timerText.text = minutes.ToString("00") + "::" + seconds.ToString("00") + "::" + milliseconds.ToString("000");
            }
            else
            {
                stop_updating = true;
                timerText.text = "00:00:000";
                controller.Finish_Game(false);
            }
        }
	}
}
