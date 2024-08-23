using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class input_field_controller : MonoBehaviour {

    public InputField input_field;

    public Trial_Manager trial_man;

    public int type;

    EventSystem myeventsystem;


    private void OnEnable()
    {
        myeventsystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        input_field = gameObject.GetComponent<InputField>();
        StartCoroutine(Match_Text());
    }

    IEnumerator Match_Text()
    {
        while (true)
        {
            //While there is a selected card, make sure it's variables are reflected in the input field, and vice versa
            if(trial_man.selected_card != null)
            {
                if (myeventsystem.currentSelectedGameObject == this.gameObject)
                {
                    Check_Input();
                    trial_man.selected_card.Convert_To_Card();
                    trial_man.Convert_Big(trial_man.selected_card, true);
                    trial_man.has_changes = true;
                }

                else
                {
                    switch (type)
                    {
                        case 0:
                            input_field.text = trial_man.selected_card.win_msg;
                            break;
                        case 1:
                            input_field.text = trial_man.selected_card.loss_msg;
                            break;
                        case 2:
                            input_field.text = trial_man.selected_card.ais.ToString();
                            break;
                        case 3:
                            input_field.text = trial_man.selected_card.aic.ToString();
                            break;
                        case 4:
                            input_field.text = trial_man.selected_card.step.ToString();
                            break;
                        case 5:
                            input_field.text = trial_man.selected_card.hop.ToString();
                            break;
                        case 6:
                            input_field.text = trial_man.selected_card.delay.ToString();
                            break;            
                        case 7:
                            input_field.text = trial_man.selected_card.swtch.ToString();
                            break;
                    }
                }
            }
            yield return null;
        }
    }

    public void Check_Input()
    {
        //Make sure the input field doesn't go beyond 0-100
        int val = 0;
        bool attempt;
        bool empty = false;
        if (type > 1)
        {

            if (input_field.text == "")
            {
                empty = true;
            }

            else
            {
                attempt = int.TryParse(input_field.text, out val);

                if (!attempt)
                {
                    val = 1;
                }


                if (val < 0)
                {
                    val = 0;
                }
                if (val > 100)
                {
                    val = 100;
                }
                input_field.text = val.ToString();
            }
        }

        //Have the cards reflect the text
        if (!empty)
        {
            switch (type)
            {
                case 0:
                    trial_man.selected_card.win_msg = input_field.text;
                    break;
                case 1:
                    trial_man.selected_card.loss_msg = input_field.text;
                    break;
                case 2:
                    trial_man.selected_card.ais = val;
                    break;
                case 3:
                    trial_man.selected_card.aic = val;
                    break;
                case 4:
                    trial_man.selected_card.step = val;
                    break;
                case 5:
                    trial_man.selected_card.hop = val;
                    break;
                case 6:
                    trial_man.selected_card.delay = val;
                    break;
                case 7:
                    trial_man.selected_card.swtch = val;
                    break;
            }
        }
    }
}
