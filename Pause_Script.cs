using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause_Script : MonoBehaviour {

    public PLAYERCONTROLLER pc;

    public Timer timer;

    bool paused;

    public RedLightController Light;

    public Sprite pause_s;
    public Sprite play_s;

    public Image image;

    IEnumerator Pausing()
    {
        paused = true;
        //While the game is paused, stall here
        while (paused)
        {
            yield return null;
        }
        //Once pause flag is unflagged, unpause the game

        //Unpause all relevant objects
        pc.paused = false;
        GameObject.Find("AI").GetComponent<AIController>().PAUSED = false;
        timer.keepTiming = true;
        Light.keepSwitching = true;
        yield break;
    }


    public void Pause_Game()
    {
        if (paused)
        {
            image.sprite = pause_s;
            paused = false;
        }
        else
        {
            image.sprite = play_s;
            pc.paused = true;
            paused = true;
            GameObject.Find("AI").GetComponent<AIController>().PAUSED = true;
            timer.keepTiming = false;
            Light.keepSwitching = false;
            StartCoroutine(Pausing());
        }
    }
}
