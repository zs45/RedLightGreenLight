using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHold_Script : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TrialCard_Controller card_master;


    public bool ispressed = false;

    public Image im;

    public int type;

    public void OnPointerDown(PointerEventData eventData)
    {
        ispressed = true;
        if (type == 1)
        {
            im.color = new Color(1, 1, 1, 1);
            card_master.StartCoroutine(card_master.Mouse_Drag_Card(this));
        }
        else if (type == 2)
        {
            card_master.StartCoroutine(card_master.Mouse_Drag_AI_Skill(this));
        }
        else if (type == 3)
        {
            card_master.StartCoroutine(card_master.Mouse_Drag_AI_Cheat(this));
        }
        else if (type == 4)
        {
            card_master.StartCoroutine(card_master.Mouse_Drag_Step(this));
        }
        else if (type == 5)
        {
            card_master.StartCoroutine(card_master.Mouse_Drag_Hop(this));
        }
        else if (type == 6)
        {
            card_master.StartCoroutine(card_master.Mouse_Drag_Delay(this));
        }
        else if (type == 7)
        {
            card_master.StartCoroutine(card_master.Mouse_Drag_Switch(this));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ispressed = false;
        if (type == 1)
        {
            im.color = new Color(1, 1, 1, 0);
        }
    }





}
