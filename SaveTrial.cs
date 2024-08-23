using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;


public class SaveTrial : MonoBehaviour {

    public Manager manager;					// Get the Manager to find the gametype

    public ExportData Exporter;             // Get the ExportData to ????

    public string filename = "trialdeck.txt"; // trial deck file name

    public ErrorPopUp pop_up;               // Get the pop up script

    // Use this for initialization
    void Start () {
		//readTrialFromFile();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void saveTrialToFile()
    {
        manager = GameObject.Find("GameManager").GetComponent<Manager>();

        try{
            var sr = File.CreateText (filename);

            int cnt = 4;  // set cnt initially to 4 to skip basic trials

            if (manager.trial_sets.Count > 4)
            {
                while(cnt < manager.trial_sets.Count)
                {
                    sr.WriteLine(manager.trial_sets[cnt].name);
                    //Debug.Log(manager.trial_sets[cnt].name);

                    int trial_size = manager.trial_sets[cnt].trial_list.Count;
                    int leader_count = manager.trial_sets[cnt].leaderboard.Count;
                    sr.WriteLine(trial_size.ToString());
                    sr.WriteLine(leader_count.ToString());

                    for(int i = 0; i < trial_size; i++)
                    {
                        sr.WriteLine(manager.trial_sets[cnt].trial_list[i].num_players.ToString());
                        sr.WriteLine(manager.trial_sets[cnt].trial_list[i].hop_mode.ToString());
                        sr.WriteLine(manager.trial_sets[cnt].trial_list[i].max_time.ToString());
                        sr.WriteLine(manager.trial_sets[cnt].trial_list[i].ai_level.ToString());
                        sr.WriteLine(manager.trial_sets[cnt].trial_list[i].ai_cheat.ToString());
                        sr.WriteLine(manager.trial_sets[cnt].trial_list[i].step_distance.ToString());
                        sr.WriteLine(manager.trial_sets[cnt].trial_list[i].hop_distance.ToString());
                        sr.WriteLine(manager.trial_sets[cnt].trial_list[i].hop_delay.ToString());
                        sr.WriteLine(manager.trial_sets[cnt].trial_list[i].light_switch_speed.ToString());
                    }
                    for (int j = 0; j < leader_count; ++j) {

                        sr.WriteLine(manager.trial_sets[cnt].leaderboard[j].username);
                        sr.WriteLine(manager.trial_sets[cnt].leaderboard[j].record_minutes);
                        sr.WriteLine(manager.trial_sets[cnt].leaderboard[j].record_seconds);
                        sr.WriteLine(manager.trial_sets[cnt].leaderboard[j].record_milliseconds);
                    }

                    cnt ++;
                }
            }
            sr.Close();
        }
        catch (Exception e){
            Debug.Log("Exception Caught");
            pop_up.PopUp("Error occured while saving to trialdeck. Make sure that this file is closed prior to saving");
        }
    }

    public void readTrialFromFile()
    {
        manager = GameObject.Find("GameManager").GetComponent<Manager>();

        StreamReader sr;

        try
        {
            sr = new StreamReader(filename);
        }
        catch
        {
            File.CreateText(filename);
            sr = new StreamReader(filename);
        }

        int trial_cnt = 0;

        while(!sr.EndOfStream)
        {
            Trial_Set tmp = new Trial_Set();
            tmp.name = sr.ReadLine();
            int size = int.Parse(sr.ReadLine());
            int ldrs = int.Parse(sr.ReadLine());

            for(int i = 0; i < size; i++)
            {
                tmp.trial_list.Add(new Trial_Data());
                tmp.trial_list[i].num_players = int.Parse(sr.ReadLine());
                tmp.trial_list[i].hop_mode = bool.Parse(sr.ReadLine());
                tmp.trial_list[i].max_time = int.Parse(sr.ReadLine());
                tmp.trial_list[i].ai_level = int.Parse(sr.ReadLine());
                tmp.trial_list[i].ai_cheat = int.Parse(sr.ReadLine());
                tmp.trial_list[i].step_distance = int.Parse(sr.ReadLine());
                tmp.trial_list[i].hop_distance = int.Parse(sr.ReadLine());
                tmp.trial_list[i].hop_delay = int.Parse(sr.ReadLine());
                tmp.trial_list[i].light_switch_speed = int.Parse(sr.ReadLine());
                
            }
            for (int j = 0; j < ldrs; ++j)
            {
                tmp.leaderboard.Add(new Leader());
                tmp.leaderboard[j].username = sr.ReadLine();
                tmp.leaderboard[j].record_minutes = int.Parse(sr.ReadLine());
                tmp.leaderboard[j].record_seconds = int.Parse(sr.ReadLine());
                tmp.leaderboard[j].record_milliseconds = int.Parse(sr.ReadLine());
            }

            manager.trial_sets.Add(tmp);
            trial_cnt++;
        }

        
        //Debug.Log("End of file");
        sr.Close();

        //Read in the options file
        Read_Options();

        manager.Update_Dropdown();
    }


    public void Save_Options()
    {
        manager = GameObject.Find("GameManager").GetComponent<Manager>();
        try
        {
            var sw = File.CreateText("config_file.txt");
            sw.WriteLine(manager.admin_password);
            sw.WriteLine(((int)manager.step_key).ToString());
            sw.WriteLine(((int)manager.hop_key).ToString());
            sw.Close();
        }
        catch
        {
            Debug.Log("EXCEPTION");
        }
    }


    //Reads in the options from the settings file
    void Read_Options()
    {
        manager = GameObject.Find("GameManager").GetComponent<Manager>();

        StreamReader sr;

        try
        {
            sr = new StreamReader("config_file.txt");
            manager.admin_password = sr.ReadLine();
            manager.step_key = (KeyCode)int.Parse(sr.ReadLine());
            manager.hop_key = (KeyCode)int.Parse(sr.ReadLine());
            sr.Close();
        }
        //If there is no options file, create one with default settings
        catch
        {
            var sw = File.CreateText("config_file.txt");
            sw.WriteLine("1adminpassword");
            sw.WriteLine(((int)KeyCode.Space).ToString());
            sw.WriteLine(((int)KeyCode.Z).ToString());
            manager.admin_password = "1adminpassword";
            manager.step_key = KeyCode.Space;
            manager.hop_key = KeyCode.Z;
            sw.Close();
        }

    }

}
