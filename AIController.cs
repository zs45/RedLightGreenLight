using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

    //Reference the AI's PLayercontroller script
    public PLAYERCONTROLLER ai_pc;

    public RedLightController red_light;
    public turbine_controller turbine;

    public bool is_paused = false;
    public bool PAUSED = false;

    public int AI_SPEED = 0;   //Determines how often the AI does something


    public int AI_INT = 100;  //The rate at which the AI checks the red light

    public int AI_CHEAT = 1;  //The rate at which the AI completely ignores the red light

    public Manager the_man;


	//Initialize
	void Start () {
        the_man = GameObject.Find("GameManager").GetComponent<Manager>();
        //If the playercontroller has not been set yet, do it now
        if (ai_pc == null)
        {
            ai_pc = gameObject.GetComponent<PLAYERCONTROLLER>();
        }
        //Tell the pc that it is an AI
        ai_pc.Set_AI(this);
        StartCoroutine(AI_Loop());

        AI_INT = the_man.trial_list[the_man.trial_num - 1].ai_level;
        AI_CHEAT = the_man.trial_list[the_man.trial_num - 1].ai_cheat;

    }

    //This coroutine controls the AI, which tells the opponent when to move
    IEnumerator AI_Loop()
    { 
        int ai_timer = AI_SPEED;
        Manager the_man = GameObject.Find("GameManager").GetComponent<Manager>();
        while (true)
        {
            //Keep the AI from doing anything while paused
            while (is_paused || PAUSED) { yield return null; }

            //After a preset time, tell the AI to act
            ai_timer -= 1;
            if(ai_timer <= 0)
            {
                //Reset AI timer
                ai_timer = AI_SPEED;

                int ai_cheat_result = Random.Range(0, 100);
                int ai_check_result = Random.Range(0, 100);


                bool go_ahead = true;
                //First we need to check the Red light (most of the time)
                if(ai_check_result <= AI_INT)
                {
                    //If the light is red, stop the AI from acting
                    if (!red_light.is_green)
                    {
                        go_ahead = false;
                    }
                }
                //If the AI can cheat, go ahead regardless
                if(ai_cheat_result <= AI_CHEAT)
                {
                    go_ahead = true;
                }


                if (go_ahead)
                {

                    //If the light is still red, send the AI back, unless it cheats
                    if (!red_light.is_green && ai_cheat_result > AI_CHEAT)
                    {
                        StartCoroutine(ai_pc.Punish());
                        is_paused = true;
                    }

                    else
                    {
                        //Either step or jump
                        int ra = Random.Range(0, 10);
                        if (ra != 0 || !the_man.hop_mode)
                        {
                            ai_pc.Step();
                        }
                        else
                        {
                            ai_pc.StartCoroutine(ai_pc.Jump());
                            is_paused = true;
                        }
                    }
                }
            }

            yield return null;
        }
    }

	
	
}
