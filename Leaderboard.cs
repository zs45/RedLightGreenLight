using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Leaderboard : MonoBehaviour
{

    Manager man;
    Dropdown ddown;

    Text first, second, third, fourth, fifth;
    Text t1, t2, t3, t4, t5, nope, n1, n2, n3, n4, n5, usr, cross;

    // Use this for initialization
    void Start()
    {
        man = GameObject.Find("GameManager").GetComponent<Manager>();
        ddown = GameObject.Find("Set_Dropdown").GetComponent<Dropdown>();

        first = GameObject.Find("1stName").GetComponent<Text>();
        second = GameObject.Find("2ndName").GetComponent<Text>();
        third = GameObject.Find("3rdName").GetComponent<Text>();
        fourth = GameObject.Find("4thName").GetComponent<Text>();
        fifth = GameObject.Find("5thName").GetComponent<Text>();

        t1 = GameObject.Find("1stTime").GetComponent<Text>();
        t2 = GameObject.Find("2ndTime").GetComponent<Text>();
        t3 = GameObject.Find("3rdTime").GetComponent<Text>();
        t4 = GameObject.Find("4thTime").GetComponent<Text>();
        t5 = GameObject.Find("5thTime").GetComponent<Text>();

        nope = GameObject.Find("none").GetComponent<Text>();

        n1 = GameObject.Find("first").GetComponent<Text>();
        n2 = GameObject.Find("second").GetComponent<Text>();
        n3 = GameObject.Find("third").GetComponent<Text>();
        n4 = GameObject.Find("fourth").GetComponent<Text>();
        n5 = GameObject.Find("fifth").GetComponent<Text>();

        usr = GameObject.Find("Usrnm").GetComponent<Text>();
        cross = GameObject.Find("TimeFinished").GetComponent<Text>();

        ClearBoard();
        n1.enabled = false;
        n2.enabled = false;
        n3.enabled = false;
        n4.enabled = false;
        n5.enabled = false;
        usr.enabled = false;
        cross.enabled = false;
        nope.transform.localPosition = Vector3.zero;

    }

    public void Update_Display()
    {
        if(ddown == null)
        {
            man = GameObject.Find("GameManager").GetComponent<Manager>();
            ddown = GameObject.Find("Set_Dropdown").GetComponent<Dropdown>();

            first = GameObject.Find("1stName").GetComponent<Text>();
            second = GameObject.Find("2ndName").GetComponent<Text>();
            third = GameObject.Find("3rdName").GetComponent<Text>();
            fourth = GameObject.Find("4thName").GetComponent<Text>();
            fifth = GameObject.Find("5thName").GetComponent<Text>();

            t1 = GameObject.Find("1stTime").GetComponent<Text>();
            t2 = GameObject.Find("2ndTime").GetComponent<Text>();
            t3 = GameObject.Find("3rdTime").GetComponent<Text>();
            t4 = GameObject.Find("4thTime").GetComponent<Text>();
            t5 = GameObject.Find("5thTime").GetComponent<Text>();

            nope = GameObject.Find("none").GetComponent<Text>();

            n1 = GameObject.Find("first").GetComponent<Text>();
            n2 = GameObject.Find("second").GetComponent<Text>();
            n3 = GameObject.Find("third").GetComponent<Text>();
            n4 = GameObject.Find("fourth").GetComponent<Text>();
            n5 = GameObject.Find("fifth").GetComponent<Text>();

            usr = GameObject.Find("Usrnm").GetComponent<Text>();
            cross = GameObject.Find("TimeFinished").GetComponent<Text>();
        }
        if (ddown.value < 4)
        {
            ClearBoard();
            n1.enabled = false;
            n2.enabled = false;
            n3.enabled = false;
            n4.enabled = false;
            n5.enabled = false;
            usr.enabled = false;
            cross.enabled = false;
            nope.transform.localPosition = Vector3.zero;
        }
        else if (man.trial_sets[ddown.value].leaderboard.Count >= 5)
        {
            ClearBoard();
            nope.transform.localPosition = new Vector3(1900f, 0f, 0f);
            n1.enabled = true;
            n2.enabled = true;
            n3.enabled = true;
            n4.enabled = true;
            n5.enabled = true;
            usr.enabled = true;
            cross.enabled = true;
            first.text = man.trial_sets[ddown.value].leaderboard[0].username;
            t1.text = man.trial_sets[ddown.value].leaderboard[0].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[0].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[0].record_milliseconds.ToString("00");

            second.text = man.trial_sets[ddown.value].leaderboard[1].username;
            t2.text = man.trial_sets[ddown.value].leaderboard[1].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[1].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[1].record_milliseconds.ToString("00");

            third.text = man.trial_sets[ddown.value].leaderboard[2].username;
            t3.text = man.trial_sets[ddown.value].leaderboard[2].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[2].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[2].record_milliseconds.ToString("00");

            fourth.text = man.trial_sets[ddown.value].leaderboard[3].username;
            t4.text = man.trial_sets[ddown.value].leaderboard[3].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[3].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[3].record_milliseconds.ToString("00");

            fifth.text = man.trial_sets[ddown.value].leaderboard[4].username;
            t5.text = man.trial_sets[ddown.value].leaderboard[4].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[4].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[4].record_milliseconds.ToString("00");

        }
        else if (man.trial_sets[ddown.value].leaderboard.Count == 4)
        {
            ClearBoard();
            nope.transform.localPosition = new Vector3(1900f, 0f, 0f);
            n1.enabled = true;
            n2.enabled = true;
            n3.enabled = true;
            n4.enabled = true;
            n5.enabled = true;
            usr.enabled = true;
            cross.enabled = true;
            first.text = man.trial_sets[ddown.value].leaderboard[0].username;
            t1.text = man.trial_sets[ddown.value].leaderboard[0].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[0].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[0].record_milliseconds.ToString("00");

            second.text = man.trial_sets[ddown.value].leaderboard[1].username;
            t2.text = man.trial_sets[ddown.value].leaderboard[1].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[1].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[1].record_milliseconds.ToString("00");

            third.text = man.trial_sets[ddown.value].leaderboard[2].username;
            t3.text = man.trial_sets[ddown.value].leaderboard[2].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[2].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[2].record_milliseconds.ToString("00");

            fourth.text = man.trial_sets[ddown.value].leaderboard[3].username;
            t4.text = man.trial_sets[ddown.value].leaderboard[3].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[3].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[3].record_milliseconds.ToString("00");
        }
        else if (man.trial_sets[ddown.value].leaderboard.Count == 3)
        {
            ClearBoard();
            nope.transform.localPosition = new Vector3(1900f, 0f, 0f);
            n1.enabled = true;
            n2.enabled = true;
            n3.enabled = true;
            n4.enabled = true;
            n5.enabled = true;
            usr.enabled = true;
            cross.enabled = true;
            first.text = man.trial_sets[ddown.value].leaderboard[0].username;
            t1.text = man.trial_sets[ddown.value].leaderboard[0].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[0].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[0].record_milliseconds.ToString("00");

            second.text = man.trial_sets[ddown.value].leaderboard[1].username;
            t2.text = man.trial_sets[ddown.value].leaderboard[1].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[1].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[1].record_milliseconds.ToString("00");

            third.text = man.trial_sets[ddown.value].leaderboard[2].username;
            t3.text = man.trial_sets[ddown.value].leaderboard[2].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[2].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[2].record_milliseconds.ToString("00");
        }
        else if (man.trial_sets[ddown.value].leaderboard.Count == 2)
        {
            ClearBoard();
            nope.transform.localPosition = new Vector3(1900f, 0f, 0f);
            n1.enabled = true;
            n2.enabled = true;
            n3.enabled = true;
            n4.enabled = true;
            n5.enabled = true;
            usr.enabled = true;
            cross.enabled = true;
            first.text = man.trial_sets[ddown.value].leaderboard[0].username;
            t1.text = man.trial_sets[ddown.value].leaderboard[0].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[0].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[0].record_milliseconds.ToString("00");

            second.text = man.trial_sets[ddown.value].leaderboard[1].username;
            t2.text = man.trial_sets[ddown.value].leaderboard[1].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[1].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[1].record_milliseconds.ToString("00");
        }
        else if (man.trial_sets[ddown.value].leaderboard.Count == 1)
        {
            ClearBoard();
            nope.transform.localPosition = new Vector3(1900f, 0f, 0f);
            n1.enabled = true;
            n2.enabled = true;
            n3.enabled = true;
            n4.enabled = true;
            n5.enabled = true;
            usr.enabled = true;
            cross.enabled = true;
            first.text = man.trial_sets[ddown.value].leaderboard[0].username;
            t1.text = man.trial_sets[ddown.value].leaderboard[0].record_minutes.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[0].record_seconds.ToString("00") + ":" + man.trial_sets[ddown.value].leaderboard[0].record_milliseconds.ToString("00");
        }
        else
        {
            ClearBoard();
            n1.enabled = true;
            n2.enabled = true;
            n3.enabled = true;
            n4.enabled = true;
            n5.enabled = true;
            usr.enabled = true;
            cross.enabled = true;
            nope.transform.localPosition = new Vector3(1900f, 0f, 0f);
        }


    }

    public void ClearBoard()
    {
        first = GameObject.Find("1stName").GetComponent<Text>();
        second = GameObject.Find("2ndName").GetComponent<Text>();
        third = GameObject.Find("3rdName").GetComponent<Text>();
        fourth = GameObject.Find("4thName").GetComponent<Text>();
        fifth = GameObject.Find("5thName").GetComponent<Text>();

        t1 = GameObject.Find("1stTime").GetComponent<Text>();
        t2 = GameObject.Find("2ndTime").GetComponent<Text>();
        t3 = GameObject.Find("3rdTime").GetComponent<Text>();
        t4 = GameObject.Find("4thTime").GetComponent<Text>();
        t5 = GameObject.Find("5thTime").GetComponent<Text>();

        first.text = t1.text = second.text = t2.text = third.text = t3.text = fourth.text = t4.text = fifth.text = t5.text = "";
    }
}
