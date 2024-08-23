using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Scroll_Controller : MonoBehaviour {

    public Scrollbar s_bar;

    public GameObject Cards;

    public Trial_Manager trial_man;

    public GameObject extra_fields;

    public GameObject select_outline;


    float sz = 1.0f;


    EventSystem myeventsystem;


    private void OnEnable()
    {
        StartCoroutine(Scroll_Control());
    }


    //Checks to see if we can scroll
    bool Check_Selected()
    {
        return true;
        //Automatically return true if we are selected
        if(myeventsystem.currentSelectedGameObject == this.gameObject)
        {
            return true;
        }
        if (myeventsystem.currentSelectedGameObject == null)
        {
            return false;
        }
        if (this.gameObject.name == "detailed_scroll")
        {
            if (myeventsystem.currentSelectedGameObject.transform.parent.gameObject == extra_fields)
            {
                return true;
            }
        }
        //return true if a card is selected
        else
        {
            if(myeventsystem.currentSelectedGameObject.transform.parent.parent.gameObject.name == "Trial_Creator")
            {
                return true;
            }
        }
        return false;
    }


    IEnumerator Scroll_Control()
    {
        if (myeventsystem == null)
        {
            myeventsystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
        while (true)
        {
            if(Check_Selected())
            {
                if (s_bar.size < 1.0f)
                {
                    if (Input.mouseScrollDelta.y > 0)
                    {
                        s_bar.value -= 0.06f;
                        if (s_bar.value < 0)
                        {
                            s_bar.value = 0;
                        }
                    }
                    if (Input.mouseScrollDelta.y < 0)
                    {
                        s_bar.value += 0.06f;
                        if (s_bar.value > 1)
                        {
                            s_bar.value = 1;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    public void Scroll_Fields()
    {
        float new_y = (s_bar.value * 800);
        extra_fields.transform.localPosition = new Vector2(extra_fields.transform.localPosition.x, 160.0f + new_y);
    }

    public void Scroll_Cards()
    {
        float new_y = (s_bar.value * 500.0f / s_bar.size);

        if (sz > 1)
        {
            if (new_y > s_bar.value * 500.0f / (1.0f / ((sz - 1) * 1.0f)))
            {
                new_y = (s_bar.value * 500.0f / (1.0f / ((sz - 1) * 1.0f)));
            }
        }

        Cards.transform.localPosition = new Vector3(Cards.transform.localPosition.x, new_y, Cards.transform.localPosition.z);
        trial_man.Update_Card_Positions();
    }

    //Make the size of the bar smaller the more more trial boards there are
    public void Adjust_Size(int size)
    {
        float prev_sz = sz;
        float new_size = 1.0f / (size * 1.0f);
        s_bar.size = new_size;
        sz = size * 1.0f;

        //Adjust the position of the handle
    }

}
