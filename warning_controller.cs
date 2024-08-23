using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class warning_controller : MonoBehaviour {

    public MenuScript menuscript;

    public Dropdown dropdown;

    public Trial_Manager trial_man;


    public int warning_type = 0;


    public void Set_Warning(int t)
    {
        warning_type = t;
    }


    //Types:
    //
    // 0 - Leaving Creator Without saving
    // 1 - Switching Decks without saving

    public void Confirm()
    {
        if(warning_type == 0)
        {
            trial_man.Save_Exit(1);
        }
        else if (warning_type == 1)
        {
            trial_man.Revert_Match_Cards(1);
        }
        else if (warning_type == 2)
        {
            trial_man.Create_Result(1);
        }
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        if (warning_type == 0)
        {
            trial_man.Save_Exit(0);
        }
        else if (warning_type == 1)
        {
            trial_man.Revert_Match_Cards(0);
        }
        else if (warning_type == 2)
        {
            trial_man.Create_Result(0);
        }
        gameObject.SetActive(false);
    }

    public void Exit()
    {
        if (warning_type == 0)
        {
            trial_man.Save_Exit(-1);
        }
        else if (warning_type == 1)
        {
            trial_man.Revert_Match_Cards(-1);
        }
        else if (warning_type == 2)
        {
            trial_man.Create_Result(-1);
        }
        gameObject.SetActive(false);
    }

}
