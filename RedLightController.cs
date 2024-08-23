using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedLightController : MonoBehaviour
{

    //List of all light animator controller components
    public List<Animator> light_list = new List<Animator>();



    private IEnumerator holdLight;      // keeps the light on for a certain amount of time
    public float redWaitTime;           // max time ther red light will stay on
    public float greenWaitTime;			// max time the green light will stay on
    public bool keepSwitching = true;

    Manager the_man;

    public float redLightSpeed;

    public float randNum;				// holds a random number for the wait times

    public bool is_green = true;

    // Use this for initialization
    void Start()
    {
        if (gameObject.name == "RedLightController")
        {
            Pause_Animation(); //start with the lights paused and red
            holdLight = waitLight(greenWaitTime, redWaitTime);  // assigns the coroutine
            StartCoroutine(holdLight);                         // starts the coroutine
            the_man = GameObject.Find("GameManager").GetComponent<Manager>();
            redLightSpeed = (the_man.trial_list[the_man.trial_num - 1].light_switch_speed * 1.0f) / 50.0f;
        }
    }

    //Pauses all the lights and flips the light status
    public void Pause_Animation()
    {
        if (gameObject.name == "RedLightController")
        {
            //Flip the light status
            is_green = !is_green;
            //Pause all the light animations
            foreach (Animator a in light_list)
            {
                a.speed = 0;
            }
        }
    }

    //Resume all the lights
    void Resume_Animation()
    {
        //Resume all the light animations
        foreach (Animator a in light_list)
        {
            a.speed = redLightSpeed;
        }
    }


    // controls the the switching between red light and green light
    private IEnumerator waitLight(float greenMaxTime, float redMaxTime)
    {
        while (true)
        {

            randNum = Random.Range(2, greenMaxTime);            // finds a random number 2s -> Max
            yield return new WaitForSeconds(randNum);          // waits that random number of s

            //If paused, keep coroutine stalled here
            while (!keepSwitching)
            {
                yield return null;
            }

            Resume_Animation();  //After holding, resume the animation

            randNum = Random.Range(2, redMaxTime);              // finds a random number 2s -> Max
            yield return new WaitForSeconds(randNum);          // waits that random number of s

            //If paused, keep coroutine stalled here
            while (!keepSwitching)
            {
                yield return null;
            }

            Resume_Animation();   //After holding, resume the animation

        }
    }
}

