using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sprite_selector : MonoBehaviour {

    public bool for_player;

    public int value;

    public Manager the_man;
    public Sprite_Database database;

    public Image sprite_image;

    public Toggle tog;

    void Start()
    {
        the_man = GameObject.Find("GameManager").GetComponent<Manager>();
        database = GameObject.Find("GameManager").GetComponent<Sprite_Database>();

        if (for_player)
        {
            value = the_man._PLAYER_SPRITE_INDEX;  
        }
        else
        {
            value = the_man._AI_SPRITE_INDEX;
        }
        sprite_image.sprite = database.sprite_list[value];

    }


    public void Shift_Red()
    {
        if (tog.isOn)
        {
            the_man.make_red = true;
            sprite_image.color = Color.red;
        }
        else
        {
            the_man.make_red = false;
            sprite_image.color = Color.white;
        }
    }


    public void Adjust_Value(int dir)
    {
        value += dir;
        if(value < 0)
        {
            value = database.sprite_list.Count - 1;
        }
        if(value >= database.sprite_list.Count)
        {
            value = 0;
        }

        sprite_image.sprite = database.sprite_list[value];
        if (for_player)
        {
            the_man._PLAYER_SPRITE_INDEX = value;
        }
        else
        {
            the_man._AI_SPRITE_INDEX = value;
        }
    }
}
