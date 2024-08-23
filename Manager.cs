using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class Trial_Data
{
    //Variables that decide the main type of the game
    public int num_players = 1;
    public bool hop_mode = true;

    //Time limit for the trial, 60 = 1 minute
    public int max_time = 120;

    //AI parameters, goes from 0 - 100
    public int ai_level = 95;
    public int ai_cheat = 0;

    public int step_distance = 50;
    public int hop_distance = 50;
    public int hop_delay = 5;
    public int light_switch_speed = 50;

    public string win_msg = "YOU WIN!";
    public string loss_msg = "YOU LOSE...";

}

[System.Serializable]
public class Leader
{
    // A leadeer will contain a username and a record time
    public string username;
    public int record_minutes;
    public int record_seconds;
    public int record_milliseconds;

    //public int record_total;
}

[System.Serializable]
public class Trial_Set
{
    public string name;
    public List<Trial_Data> trial_list = new List<Trial_Data>();
    public List<Leader> leaderboard = new List<Leader>();

}




public class Manager : MonoBehaviour
{

    public string admin_password;

    public int vision_mode = 1;

    public KeyCode step_key = KeyCode.Z;
    public KeyCode hop_key = KeyCode.Space;

    //Reference to the different background objects
    GameObject BG1, BG2;

    public string usrname = "";

    public Dropdown set_dropdown;
    public Dropdown set_dropdown2;

    //Reference to the player and AI objects
    PLAYERCONTROLLER user, AI;

    ExportData exporter;

    SaveTrial saver;

    //Reference to timer and red light controller
    RedLightController light;
    Timer timer;

    float mins;
    float scs;
    float mscs;

    AudioManager aman;

    Button step_button;
    Button hop_button;

    //Variables that control the type of game to be played
    public int num_players;
    public bool hop_mode;
    public int max_time;

    public bool make_red = true;

    //The current trial number
    public int trial_num = 1;


    //Variables that control the various options set by the proctor
    public int _NUM_OF_TRIALS = 0;
    public int _PLAYER_SPRITE_INDEX = 0;
    public int _AI_SPRITE_INDEX = 0;

    //Variables that record trial results
    public List<bool> _TRIAL_WON = new List<bool>();


    //List of Sprites for the players
    List<Sprite> character_sprite_list;


    public List<Trial_Data> trial_list = new List<Trial_Data>();

    public List<Trial_Set> trial_sets = new List<Trial_Set>();

    public int set_index = 0;

    private Vector3 two_step_loc;
    private Vector3 two_hop_loc;

    SaveTrial thisIsTrialSaver = new SaveTrial();

    public GameObject ErrorPop;
    public ErrorPopUp Pops;


    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Menu");
        aman = gameObject.GetComponent<AudioManager>();
        exporter = gameObject.GetComponent<ExportData>();
        Init_Trial_Sets();
        saver = gameObject.GetComponent<SaveTrial>();
        saver.readTrialFromFile();
        character_sprite_list = gameObject.GetComponent<Sprite_Database>().sprite_list;
    }

    //Sets the current trial set to the dropdown menu
    public void Load_Set()
    {
        //Debug.Log("LOADING SET");

        //Read the dropdown menu to get the set index

        int dropdown_index = set_dropdown.value;
        set_index = dropdown_index;

        //If this is a default set, deactivate the leaderboard
        if(set_index <= 3)
        {
            GameObject.Find("Main Camera").GetComponent<MenuScript>().Leader_Button.gameObject.SetActive(false);
        }
        else
        {
            GameObject.Find("Main Camera").GetComponent<MenuScript>().Leader_Button.gameObject.SetActive(true);
        }

        //Empty the currently active list
        trial_list.Clear();

        if (dropdown_index > -1)
        {

            //Debug.Log("DROPDOWN INDEX : " + dropdown_index.ToString());

            for (int cur_t = 0; cur_t < trial_sets[dropdown_index].trial_list.Count; cur_t++)
            {
                trial_list.Add(trial_sets[dropdown_index].trial_list[cur_t]);
            }


            _NUM_OF_TRIALS = trial_list.Count;

            //If trial list size is zero, move to default
            if (trial_list.Count < 1)
            {
                set_dropdown.value = 0;
                set_dropdown2.value = 0;
                Load_Set();
            }
        }

    }

    


    //Updates the dropdown menu to match the trial sets
    public void Update_Dropdown()
    {
        if (set_dropdown != null)
        {
            //Find the custom dropdown menu, too
            Dropdown custom_drop = GameObject.Find("Custom_Dropdown").GetComponent<Dropdown>();


            //Init the options for the menu
            set_dropdown.options.Clear();
            set_dropdown2.options.Clear();

            custom_drop.options.Clear();

            //Add the initial option
            //custom_drop.options.Add(new Dropdown.OptionData());
            //custom_drop.options[0].text = "Create Trial Deck";

            //For each trial set in the sets list, add a matching option
            for (int ts = 0; ts < trial_sets.Count; ts++)
            {
                set_dropdown.options.Add(new Dropdown.OptionData());
                set_dropdown.options[ts].text = trial_sets[ts].name;

                set_dropdown2.options.Add(new Dropdown.OptionData());
                set_dropdown2.options[ts].text = trial_sets[ts].name;

                //If this is not the one of the default sets, add it to the custom menu
                if (ts >= 4)
                {
                    custom_drop.options.Add(new Dropdown.OptionData());
                    custom_drop.options[ts - 4].text = trial_sets[ts].name;
                }
            }
        }
    }




    //Initializes the trial sets
    void Init_Trial_Sets()
    {
        //Create the default sets, one for each game mode
        Trial_Set dt1s = new Trial_Set();
        Trial_Set dt1h = new Trial_Set();
        Trial_Set dt2s = new Trial_Set();
        Trial_Set dt2h = new Trial_Set();

        //Set the default set for a single player, step only game
        dt1s.name = "Default 1-player, Step only";
        dt1s.trial_list = new List<Trial_Data>();
        dt1s.trial_list.Add(new Trial_Data());
        dt1s.trial_list[0].num_players = 1;
        dt1s.trial_list[0].hop_mode = false;

        dt1s.trial_list[0].ai_level = 80;
        dt1s.trial_list[0].ai_cheat = 1;
        dt1s.trial_list[0].step_distance = 50;
        dt1s.trial_list[0].hop_distance = 50;
        dt1s.trial_list[0].hop_delay = 50;
        dt1s.trial_list[0].light_switch_speed = 50;

        dt1s.trial_list[0].win_msg = "FINISHED!";
        dt1s.trial_list[0].loss_msg = "TIME OUT...";



        //Set the default set for a single player, step+hop game
        dt1h.name = "Default 1-player, Step+Hop";
        dt1h.trial_list = new List<Trial_Data>();
        dt1h.trial_list.Add(new Trial_Data());
        dt1h.trial_list[0].num_players = 1;
        dt1h.trial_list[0].hop_mode = true;

        dt1h.trial_list[0].ai_level = 80;
        dt1h.trial_list[0].ai_cheat = 1;
        dt1h.trial_list[0].step_distance = 50;
        dt1h.trial_list[0].hop_distance = 50;
        dt1h.trial_list[0].hop_delay = 10;
        dt1h.trial_list[0].light_switch_speed = 50;

        dt1h.trial_list[0].win_msg = "FINISHED!";
        dt1h.trial_list[0].loss_msg = "TIME OUT...";


        //Set the default set for a two player, step only game
        dt2s.name = "Default 2-player, Step only";
        dt2s.trial_list = new List<Trial_Data>();
        dt2s.trial_list.Add(new Trial_Data());
        dt2s.trial_list[0].num_players = 2;
        dt2s.trial_list[0].hop_mode = false;

        dt2s.trial_list[0].ai_level = 80;
        dt2s.trial_list[0].ai_cheat = 1;
        dt2s.trial_list[0].step_distance = 50;
        dt2s.trial_list[0].hop_distance = 50;
        dt2s.trial_list[0].hop_delay = 50;
        dt2s.trial_list[0].light_switch_speed = 50;

        dt2s.trial_list[0].win_msg = "YOU WIN!";
        dt2s.trial_list[0].loss_msg = "YOU LOSE...";

        //Set the default set for a two player, step+hop game
        dt2h.name = "Default 2-player, Step+Hop";
        dt2h.trial_list = new List<Trial_Data>();
        dt2h.trial_list.Add(new Trial_Data());
        dt2h.trial_list[0].num_players = 2;
        dt2h.trial_list[0].hop_mode = true;

        dt2h.trial_list[0].win_msg = "FINISHED!";
        dt2h.trial_list[0].loss_msg = "TIME OUT...";

        dt2h.trial_list[0].ai_level = 80;
        dt2h.trial_list[0].ai_cheat = 1;
        dt2h.trial_list[0].step_distance = 50;
        dt2h.trial_list[0].hop_distance = 50;
        dt2h.trial_list[0].hop_delay = 50;
        dt2h.trial_list[0].light_switch_speed = 50;

        //Add the sets to the list of sets
        trial_sets.Add(dt1s);
        trial_sets.Add(dt1h);
        trial_sets.Add(dt2s);
        trial_sets.Add(dt2h);


        //Read the saved sets from the input file


        //Update the dropdown list
        Update_Dropdown();
    }




    //Set performance levels
    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    //Subscribe functions to the scene loaded event
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //When this object is gone, unsubscribe it from the scene loaded event
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //When a new scene is loaded, run this function
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset time data for leaderboard purposes
        mins = scs = mscs = 0;

        //If we're in the menu, reset the trial number
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            aman.playMusic(0);
            trial_num = 1;

            //Find the dropdown menu
            set_dropdown = GameObject.Find("Set_Dropdown").GetComponent<Dropdown>();
            set_dropdown2 = GameObject.Find("Set_Dropdown2").GetComponent<Dropdown>();

            Update_Dropdown();
            set_dropdown.value = set_index;
            Load_Set();

        }
        //If the scene we're in is the game test room, intiialize a new race
        if (SceneManager.GetActiveScene().name == "test_room" || SceneManager.GetActiveScene().name == "3D_test_room")
        {
            aman.asrc.Stop();
            ErrorPop = GameObject.Find("ErrorPopUp");
            Pops = ErrorPop.GetComponent<ErrorPopUp>();
            ErrorPop.SetActive(false);
            exporter.Start_Recording();
            //Find the background and player objects
            BG1 = GameObject.Find("1p");
            BG2 = GameObject.Find("2p");
            if (SceneManager.GetActiveScene().name == "test_room")
            {
                // Finds step and hop button objects
                hop_button = GameObject.Find("hopbutton").GetComponent<Button>();
                step_button = GameObject.Find("stepbutton").GetComponent<Button>();
                // Checks to see if platform is not Android, makes step and hop buttons disappear if so
                if (Application.platform != RuntimePlatform.Android)
                {
                    // Debug.Log("Recognized that it was not android");
                    hop_button.transform.localScale = new Vector3(0f, 0f, 0f);
                    step_button.transform.localScale = new Vector3(0f, 0f, 0f);
                }
            }
            else
            {
                if(vision_mode == 2)
                {
                    GameObject.Find("Main Camera").GetComponent<CameraController3D>().first_person = false;
                }
                else
                {
                    GameObject.Find("Main Camera").GetComponent<CameraController3D>().first_person = true;
                }
            }
            Finish_Line_Controller finishl = GameObject.Find("finish_line").GetComponent<Finish_Line_Controller>();

            finishl.win_msg = trial_list[trial_num - 1].win_msg;
            finishl.loss_msg = trial_list[trial_num - 1].loss_msg;

            

            timer = GameObject.Find("TimerController").GetComponent<Timer>();
            light = GameObject.Find("RedLightController").GetComponent<RedLightController>();

            user = GameObject.Find("Player").GetComponent<PLAYERCONTROLLER>();
            AI = GameObject.Find("AI").GetComponent<PLAYERCONTROLLER>();

        


            if (SceneManager.GetActiveScene().name == "test_room")
            {
                //Change the player sprites
                user.gameObject.GetComponent<SpriteRenderer>().sprite = character_sprite_list[_PLAYER_SPRITE_INDEX];
                AI.gameObject.GetComponent<SpriteRenderer>().sprite = character_sprite_list[_AI_SPRITE_INDEX];
            }
            //Set the player controls
            user.step_key = step_key;
            user.jump_key = hop_key;

            if (make_red && SceneManager.GetActiveScene().name == "test_room")
            {
                AI.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }

            

            //Set the trial parameters
            num_players = trial_list[trial_num - 1].num_players;
            hop_mode = trial_list[trial_num - 1].hop_mode;
            max_time = trial_list[trial_num - 1].max_time;

            if (SceneManager.GetActiveScene().name == "test_room")
            {
                // Disables hop button if it is only a step game
                if (hop_mode == false)
                {
                    hop_button.interactable = false;
                }
            }


            AI.gameObject.GetComponent<AIController>().AI_CHEAT = trial_list[trial_num - 1].ai_cheat;
            AI.gameObject.GetComponent<AIController>().AI_INT = trial_list[trial_num - 1].ai_level;

            //If this is a two player game, move the backgrounds and players into position
            if (num_players == 2)
            {

                if (SceneManager.GetActiveScene().name == "test_room")
                {
                    //Move the timer to the center
                    GameObject.Find("TimerController").transform.position = new Vector3(200, 0, 0);

                    //Move the player turbine to the left
                    Vector3 old_local = user.turbine.gameObject.transform.localPosition;
                    user.turbine.gameObject.transform.parent = BG2.transform;
                    user.turbine.gameObject.transform.localPosition = old_local;
                    user.turbine.gameObject.transform.Translate(Vector3.left * 4);

                    //Place the two player background in center view
                    Vector3 temp;
                    temp = BG1.transform.position;
                    BG1.transform.position = BG2.transform.position;
                    BG2.transform.position = temp;

                    //Move the player to the left side of the screen
                    user.transform.position = new Vector3(-4f, -4.23f, 0f);
                    user.initial_position = user.transform.position;

                    //Move the AI player to the right side of the screen
                    AI.transform.position = new Vector3(4f, -4.23f, 0f);
                    AI.initial_position = AI.transform.position;

                    //Move virtual buttons
                    two_step_loc = new Vector3(250f, 570f, 0f);
                    two_hop_loc = new Vector3(250f, 300f, 0f);
                    step_button.transform.position = two_step_loc;
                    hop_button.transform.position = two_hop_loc;
                }
                else
                {
                    //Move the player to the left side of the screen
                    user.transform.position = new Vector3(.8f, 0, 0);
                    user.initial_position = user.transform.position;

                    //Move the AI player to the right side of the screen
                    AI.transform.position = new Vector3(-.8f, 0, 0);
                    AI.initial_position = AI.transform.position;
                }

            }
            //Pause the game first to allow for a race start screen
            StartCoroutine(Race_Start(num_players));

        }
    }


    //This coroutine plays at the beginning of the race, sits for a few seconds, then allows the race to start
    IEnumerator Race_Start(int num_p)
    {
        //Find and set the race screen
        LoadingScreen search_screen = GameObject.Find("Loading_Screen").GetComponent<LoadingScreen>();
        GameObject race_screen = GameObject.Find("Race_Start");
        Text race_text = GameObject.Find("race_start_text").GetComponent<Text>();

        //Pause the players, timer, and lights if not paused already
        user.paused = true;
        AI.GetComponent<AIController>().PAUSED = true;
        timer.keepTiming = false;
        light.keepSwitching = false;

        Image cdown = search_screen.countdown;
        Sprite twob = search_screen.cdown_sprites[1];
        Sprite oneb = search_screen.cdown_sprites[2];
        Sprite gob = search_screen.cdown_sprites[3];

        //If this is a two player race, wait for the search screen to play out
        if (num_p == 2)
        {
            int rt = Random.Range(200, 500);
            search_screen.startLoading(rt);
            search_screen.working = true;
            while(search_screen.working)
            {
                yield return null;
            }
            search_screen.gameObject.SetActive(false);
        }

        race_screen.transform.localPosition = Vector3.zero;
        race_text.text = "STARTING\nRACE #" + trial_num.ToString();

        string c_text = "Step - " + step_key.ToString();
        if (hop_mode)
        {
            c_text += "\n\nHop - " + hop_key.ToString();
        }

        GameObject.Find("controlstext").GetComponent<Text>().text = c_text; 

        

        //Keep the game stalled for a few seconds
        for (int t = 0; t < 200; t++) { yield return null; }

        //Unpause everything, and start the race
        race_screen.transform.localPosition = new Vector3(3000, 3000, 0);

        //Wait for a countdown
        for(int cd = 0; cd < 350; cd++)
        {
            if(cd == 100)
            {
                cdown.sprite = twob;
            }
            else if(cd == 200)
            {
                cdown.sprite = oneb;
            }
            else if(cd == 300)
            {
                cdown.sprite = gob;
            }
            yield return null;
        }
        cdown.enabled = false;
        user.paused = false;
        AI.GetComponent<AIController>().PAUSED = false;
        timer.stop_updating = false;
        timer.keepTiming = true;
        light.keepSwitching = true;
        aman.playMusic(1);
        yield break;
    }


    //Called by another script, tells the manager to end the current trial and advance to the next
    public void Trial_Finished(bool player_win)
    {

        //First record the result
        _TRIAL_WON.Add(player_win);
        exporter.WriteTrialData();

        mins += exporter.timer.minutes;
        scs += exporter.timer.seconds;
        mscs += exporter.timer.milliseconds;

        //If this is the last trial, export the results of all the trials and return to the menu
        if (trial_num >= _NUM_OF_TRIALS)
        {
            exporter.WriteToFile();
            Update_Leaderboard();
        }
        //Otherwise, advance to the next trial
        else
        {
            trial_num += 1;
            //Simply reload the current scene to advance
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    public void Update_Leaderboard()
    {

        if (trial_sets[set_index].leaderboard.Count == 0)
        {
            int i = 0;

            Leader new_leader = new Leader();
            new_leader.record_minutes = (int)mins;
            new_leader.record_seconds = (int)scs;
            new_leader.record_milliseconds = (int)mscs;

            trial_sets[set_index].leaderboard.Add(new_leader);

            //i = trial_sets[set_index].leaderboard.Count;

            StartCoroutine(Input_Leader(i));
        }

        else
        {
            for (int i = 0; i < trial_sets[set_index].leaderboard.Count; ++i)
            {
                float total_time = (mins * 60.0f) + scs + (mscs / 1000.0f);
                float record_time = (trial_sets[set_index].leaderboard[i].record_minutes * 60.0f) + (trial_sets[set_index].leaderboard[i].record_seconds) + (trial_sets[set_index].leaderboard[i].record_milliseconds / 1000.0f);
                if (total_time > record_time)
                {
                    Leader new_leader = new Leader();
                    new_leader.record_minutes = (int)mins;
                    new_leader.record_seconds = (int)scs;
                    new_leader.record_milliseconds = (int)mscs;

                    trial_sets[set_index].leaderboard.Insert(i, new_leader);

                    StartCoroutine(Input_Leader(i));

                    i = trial_sets[set_index].leaderboard.Count;

                }else if ((i == trial_sets[set_index].leaderboard.Count - 1) && (trial_sets[set_index].leaderboard.Count < 5))
                {
                    Leader new_leader = new Leader();
                    new_leader.record_minutes = (int)mins;
                    new_leader.record_seconds = (int)scs;
                    new_leader.record_milliseconds = (int)mscs;

                    trial_sets[set_index].leaderboard.Insert(i+1, new_leader);

                    StartCoroutine(Input_Leader(i+1));

                    i = trial_sets[set_index].leaderboard.Count;
                }
            }
        }

        return;

    }

    IEnumerator Input_Leader(int ind)
    {
        GameObject.Find("username_box").transform.localPosition = Vector3.zero;
        GameObject.Find("Congrats").transform.localPosition = new Vector3(0f, 175f, 0f);

        usrname = "";

        while (usrname == "")
        {

            yield return null;

        }

        trial_sets[set_index].leaderboard[ind].username = usrname;

        thisIsTrialSaver.saveTrialToFile();

        SceneManager.LoadScene("Menu");


        yield break;

    }


}


