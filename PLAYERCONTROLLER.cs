using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PLAYERCONTROLLER : MonoBehaviour
{

    //Variables that control the speed of the player's movement options
    public float step_speed;    //Speed of the step
    public float jump_speed;   //Speed of the jump
    public int jump_length = 11;    //How long the jump lasts
    public int jump_delay;      // The delay of the jump

    public int jump_count = 0;
    public int step_count = 0;
    public int setback_count = 0;

    public KeyCode step_key = KeyCode.Space;        //The key that controls the step
    public KeyCode jump_key = KeyCode.LeftShift;    //The key that controls the jump

    bool is_ai = false;  //Flag that tells the PC if it belongs to an AI or not
    public AIController ai; //Reference to the AI controller if it needs one

    public RedLightController red_light;
    public turbine_controller turbine;
    public Vector3 initial_position;

    public GameObject sweat_obj;

    AudioManager aman;

    Manager the_man;


    public int step_presses = 0;
    public int jump_presses = 0;

    bool stepflag = false;
    bool hopflag = false;


    public bool paused = false;

    // Use this for initialization
    void Start()
    {
        the_man = GameObject.Find("GameManager").GetComponent<Manager>();
        aman = gameObject.GetComponent<AudioManager>();
        step_speed = the_man.trial_list[the_man.trial_num - 1].step_distance/50.0f * 5;
        jump_speed = the_man.trial_list[the_man.trial_num - 1].hop_distance/50.0f * 10;
        jump_delay = the_man.trial_list[the_man.trial_num - 1].hop_delay * 2;

        if (!is_ai)
        {
            StartCoroutine(Input_Loop());   //Simply Start the Input loop
        }
        //initial_position = gameObject.transform.position;

    }


    //Sets this PC to behave as an AI
    public void Set_AI(AIController ia)
    {
        is_ai = true;
        ai = ia;
        StopAllCoroutines();
    }

    public IEnumerator Punish()
    {
        if (SceneManager.GetActiveScene().name == "3D_test_room")
        {
            while (gameObject.transform.position.z != 0)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(gameObject.transform.position.x, 0, 0), 0.1f);
                yield return null;
            }
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, 0);
            for (int r = 0; r < 10; r++)
            {
                yield return null;
            }
        }
        else
        {
            turbine.Start_Rising();
            yield return null;
            while (turbine.ani.speed != 0)
            {
                //Move the player back to the starting line
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(gameObject.transform.position.x, -4.23f, 0), 0.1f);
                yield return null;
            }
            //Make sure the player is behind the starting line
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, -4.23f, 0);
        }
        if (is_ai)
        {
            ai.is_paused = false;
        }
        else
        {
            StartCoroutine(Input_Loop());
        }
        yield break;
    }


    public void Call_Step()
    {
        stepflag = true;
    }
    public void Call_Hop()
    {
        if (the_man.hop_mode)
        {
            hopflag = true;
        }
    }


    //This coroutine plays while the player has control, and allows input for stepping and jumping
    IEnumerator Input_Loop()
    {
        stepflag = false;
        hopflag = false;
        while (true)
        {
            //Check if the game is paused 
            if (paused)
            {
                while (paused)
                {
                    yield return null;
                }
            }

            //Check for stepping
            if (stepflag || Input.GetKeyDown(step_key))
            {
                stepflag = false;
                step_presses += 1;
                //If the light is red, we need to reset the players position 
                if (!red_light.is_green)
                {
                    StartCoroutine(Punish());
                    setback_count++;
                    yield break;
                }
                else
                {
                    Step();
                }
            }
            //Check for jumping
            else if (hopflag || (Input.GetKeyDown(jump_key) && the_man.hop_mode))
            {
                hopflag = false;
                jump_presses += 1;
                //If the light is red, we need to reset the players position 
                if (!red_light.is_green)
                {
                    StartCoroutine(Punish());
                    setback_count++;
                    yield break;
                }
                else
                {
                    StartCoroutine(Jump());
                    yield break;
                }

            }
            yield return null;
        }
    }

    //Moves the player by one step
    public void Step()
    {
        //aman.playMusic(0);
        step_count++;
        if (SceneManager.GetActiveScene().name == "3D_test_room")
        {
            Debug.Log("3D Step");
            gameObject.transform.Translate(Vector3.forward * step_speed * Time.deltaTime);
        }
        else
        {
            aman.playMusic(0);
            gameObject.transform.Translate(Vector2.up * step_speed * Time.deltaTime);
        }
    }


    //This coroutine makes the player jump for a set time, at a set speed, during which there is no player input
    public IEnumerator Jump()
    {
        //aman.playMusic(1);
        jump_count++;
        //Set the length and speed
        int timer = jump_length;
        float j_s = jump_speed;

        while (timer > 0)
        {

            //Check if the game is paused 
            if (paused)
            {
                while (paused)
                {
                    yield return null;
                }
            }

            //Move the player and slowly decrease the speed
            if (SceneManager.GetActiveScene().name == "3D_test_room")
            {
                gameObject.transform.Translate(Vector3.forward * j_s * Time.deltaTime);
            }
            else
            {
                gameObject.transform.Translate(Vector2.up * j_s * Time.deltaTime);
            }
            j_s -= 1.0f;
            //Don't let the speed turn negative
            if (j_s < 0) { j_s = 0; }
            timer -= 1;
            yield return null;
        }
        //Once the jump is done, return control to the player, or ai, after a 1 seconds delay
        sweat_obj.SetActive(true);
        for (int dl = jump_delay; dl > 0; dl -= 1)
        {
            yield return null;
        }
        sweat_obj.SetActive(false);
        if (is_ai)
        {
            ai.is_paused = false;
        }
        else
        {

            StartCoroutine(Input_Loop());
        }
        yield break;
    }

}