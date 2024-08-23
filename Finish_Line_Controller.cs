using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finish_Line_Controller : MonoBehaviour {

    //Reference to various objects and components on the screen
    public GameObject P_Menu;
    public PLAYERCONTROLLER pc;
    public Timer timer;
    public RedLightController Light;
    public Text p_text;

    public string win_msg;
    public string loss_msg;

    //public GameObject r_button_1;
    //public GameObject r_button_2;
    //public GameObject r_button_3;


    public CameraController3D cam3d;

    //Called when a character touches the finish line
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //If it's the player, trigger win
        if (collision.gameObject == pc.gameObject)
        {
            Finish_Game(true);
        }
        //Else it's the AI, so trigger loss
        else
        {
            Finish_Game(false);
        }
    }

    //Called when a character touches the finish line
    private void OnTriggerEnter(Collider collision)
    {

        //If it's the player, trigger win
        if (collision.gameObject == pc.gameObject)
        {
            Finish_Game(true);
        }
        //Else it's the AI, so trigger loss
        else
        {
            Finish_Game(false);
        }
    }

    //Coroutine that controls the ending screen
    IEnumerator Ending(bool player_win)
    {

        if(cam3d != null)
        {
            cam3d.enabled = false;
        }

        GameObject pauseB = GameObject.Find("pause_button");
        GameObject menuB = GameObject.Find("menu_button");

        pauseB.SetActive(false);
        menuB.SetActive(false);

        AudioManager aman = GameObject.Find("GameManager").GetComponent<AudioManager>();

        aman.playMusic(2);

        //Simulate A camera flash
        float ap = 0.0f;
        Image white_flash = GameObject.Find("White_Flash").GetComponent<Image>();
        for(int f = 0;f < 30; f++)
        {
            white_flash.color = new Color(1, 1, 1,ap);
            ap += 0.3f;
            if(ap > 1.0)
            {
                ap = 1.0f;
            }
            yield return null;
        }
        //Stay on White Flash
        for(int f = 0;f < 10; f++)
        {
            yield return null;
        }

        //Turn on sepia
        GameObject.Find("Main Camera").GetComponent<CAMEFFECT>().enabled = true;
        //Clear away flash
        for (int f = 0; f < 40; f++)
        {
            white_flash.color = new Color(1, 1, 1, ap);
            ap -= 0.02f;
            if(ap < 0)
            {
                ap = 0.0f;
            }
            yield return null;
        }

        white_flash.color = new Color(1, 1, 1, 0);

        //Hold on screen for a bit
        for (int sp = 0;sp < 200; sp++)
        {
            yield return null;
        }

        //Set screen and music to reflect win/loss
        if (player_win)
        {
            aman.playMusic(3);
            p_text.text = win_msg;
        }
        else
        {
            aman.playMusic(4);
            p_text.text = loss_msg;
        }

        float move_speed = 5000f;
        //Move the pause menu into center view
        bool done_moving = false;
        while (!done_moving)
        {
            P_Menu.transform.Translate(Vector3.down * move_speed * Time.deltaTime);
            if (P_Menu.transform.localPosition.y <= 0)
            {
                P_Menu.transform.localPosition = Vector3.zero;
                done_moving = true;
            }
            else
            {
                yield return null;
            }
        }
        //Stall for a couple of seconds
        for (int p = 0;p < 200;p++)
        {
            yield return null;
        }
        
        //Tell the manager to load the next trial
        GameObject.Find("GameManager").GetComponent<Manager>().Trial_Finished(player_win);
        yield break;
    }


    //Runs whenever a player or ai touches the finish line, plays ending
    public void Finish_Game(bool player_win)
    {
        //r_button_1.SetActive(false);
        //r_button_2.SetActive(false);
        //r_button_3.SetActive(false);
        //Pause everything
        pc.paused = true;
        GameObject.Find("AI").GetComponent<AIController>().PAUSED = true;
        GameObject.Find("AI").GetComponent<Animator>().speed = 0;
        pc.gameObject.GetComponent<Animator>().speed = 0;
        timer.stop_updating = true;
        Light.keepSwitching = false;
        //Start ending script
        StartCoroutine(Ending(player_win));
    }

}
