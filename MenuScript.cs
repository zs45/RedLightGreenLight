using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public GameObject Menu_Obj;
    public GameObject Options_Obj;
    public GameObject Load_Obj;
    public GameObject Help_Obj;
    public GameObject General_Obj;
    public GameObject Leaderboard_Obj;
    Manager the_man;
    public Slider vol_slider;
    public GameObject password_Obj;

    public Text pass_text;

    public message_controller msg;

    public InputField password_changer;

    public GameObject error_txt;

    public Vector3 in_pos;

    public Vector3 out_pos;

    public Button button_2d;
    public Button button_3d;
    public Button button_1st;

    public Button Leader_Button;

    public Text step_key_text;
    public Text hop_key_text;

    public Image one_button;
    public Image two_button;
    public Image step_button;
    public Image hop_button;

    int quick_num_players = 1;
    bool quick_hop_mode = false;

    public int main_mode = 2;


    public void Set_Version(int m)
    {
        main_mode = m;
        the_man.vision_mode = m;
        if(main_mode == 1)
        {
            button_2d.image.color = Color.black;
            button_3d.image.color = Color.white;
            button_1st.image.color = Color.white;
        }
        else if(main_mode == 2)
        {        
            button_2d.image.color = Color.white;
            button_3d.image.color = Color.black;
            button_1st.image.color = Color.white;
        }
        else
        {
            button_3d.image.color = Color.white;
            button_2d.image.color = Color.white;
            button_1st.image.color = Color.black;
        }
    }

    //Called by one of the buttons for custom controls, allows controls to be set
    public void Look_For_Key_Input(int k)
    {
        StopAllCoroutines();
        if(k == 1)
        {
            step_key_text.color = Color.red;
            hop_key_text.color = Color.black;
        }
        else
        {
            step_key_text.color = Color.black;
            hop_key_text.color = Color.red;
        }
        StartCoroutine(Looking_Input(k));
    }


    void Set_Key(KeyCode i, int k)
    {
        if(k == 1)
        {
            the_man.step_key = i;
            step_key_text.text = i.ToString();
        }
        else
        {
            the_man.hop_key = i;
            hop_key_text.text = i.ToString();
        }
        step_key_text.color = Color.black;
        hop_key_text.color = Color.black;
        the_man.gameObject.GetComponent<SaveTrial>().Save_Options();
    }


    public void Change_Password()
    {
        string new_pass = password_changer.text;
        the_man.admin_password = new_pass;
        the_man.gameObject.GetComponent<SaveTrial>().Save_Options();
    }

    public void Show_Message()
    {
        msg.gameObject.SetActive(true);
        msg.StartCoroutine(msg.Stay_On());
    }


    //Checks to see what key is being pressed for custom input
    int Check_Input()
    {
        for(int kc=0;kc < 321; kc++)
        {
            KeyCode key = (KeyCode)kc;
            if (Input.GetKeyDown(key))
            {
                return kc;
            }
        }
        return -1;
    }

    //Wait for player input to change controls
    IEnumerator Looking_Input(int k)
    {
        bool done = false;
        while (!done)
        {
            int res = Check_Input();
            if(res != -1)
            {
                done = true;
                Set_Key((KeyCode)res, k);
            }
            yield return null;
        }

        yield break;
    }


    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Menu_Obj = GameObject.Find("Menu");
            Options_Obj = GameObject.Find("Options");
            Load_Obj = GameObject.Find("Loading_Screen");
            Help_Obj = GameObject.Find("Help_Menu");
            General_Obj = GameObject.Find("Options_General");
            Leaderboard_Obj = GameObject.Find("Leaderboard");
            Help_Obj.transform.position = out_pos;
            Menu_Obj.transform.position = in_pos;
            Options_Obj.transform.position = out_pos;
            Load_Obj.transform.position = out_pos;
            General_Obj.transform.position = out_pos;
            Leaderboard_Obj.transform.position = out_pos;
            the_man = GameObject.Find("GameManager").GetComponent<Manager>();

            Set_Version(the_man.vision_mode);

            step_key_text.text = the_man.step_key.ToString();
            hop_key_text.text = the_man.hop_key.ToString();

            password_changer.text = the_man.admin_password;

        }

    }

    public void Change_Volume()
    {
        Change_volume(vol_slider.value);
    }


    void Change_volume(float val)
    {
        the_man.gameObject.GetComponent<AudioManager>().asrc.volume = val;
    }


    public void Ask_Password(int on)
    {
        if(on == 1)
        {
            password_Obj.SetActive(true);
        }
        else
        {
            password_Obj.SetActive(false);
        }
    }

    public void Try_To_Check()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Check_Password();
        }
    }

    public void Check_Password()
    {
        if(pass_text.text == the_man.admin_password)
        {
            pass_text.gameObject.transform.parent.gameObject.GetComponent<InputField>().text = "";
            Swap_General();
            Ask_Password(0);
            StopAllCoroutines();
        }
        else
        {
            pass_text.gameObject.transform.parent.gameObject.GetComponent<InputField>().text = "";
            StartCoroutine(Show_Error());
        }
    }

    IEnumerator Show_Error()
    {
        error_txt.SetActive(true);
        for(int t = 0;t < 120; t++)
        {
            yield return null;
        }
        error_txt.SetActive(false);
        yield break;
    }

    //Called by the dropdown menu, runs the manager update function
    public void Update_Set()
    {
        GameObject.Find("GameManager").GetComponent<Manager>().Load_Set();
        GameObject.Find("Leaderboard").GetComponent<Leaderboard>().Update_Display();
    }


    public void changeScene(string sceneName)
    {


        SceneManager.LoadScene(sceneName);


    }

    public void Start_Game()
    {
        if (GameObject.Find("Set_Dropdown").GetComponent<Dropdown>().value > -1)
        {
            if (main_mode == 1)
            {
                SceneManager.LoadScene("test_room");
            }
            else
            {
                SceneManager.LoadScene("3D_test_room");
            }
        }
    }


    public void Swap_Help()
    {
        if(Help_Obj.transform.position == out_pos)
        {
            Help_Obj.transform.position = in_pos;
            Options_Obj.transform.position = out_pos;
        }
        else
        {
            Help_Obj.transform.position = out_pos;
            Options_Obj.transform.position = in_pos;
        }
    }

    public void Swap_Leaderboard()
    {
        if (Leaderboard_Obj.transform.position == out_pos)
        {
            Leaderboard_Obj.transform.position = in_pos;
            Menu_Obj.transform.position = out_pos;
        }
        else
        {
            Leaderboard_Obj.transform.position = out_pos;
            Menu_Obj.transform.position = in_pos;
        }
    }

    public void Swap_General()
    {
        StopAllCoroutines();
        step_key_text.color = Color.black;
        hop_key_text.color = Color.black;
        if (General_Obj.transform.position == out_pos)
        {
            General_Obj.transform.position = in_pos;
            Menu_Obj.transform.position = out_pos;
        }
        else
        {
            General_Obj.transform.position = out_pos;
            Menu_Obj.transform.position = in_pos;
        }
    }

    public void Quit_Game()
    {
        Application.Quit();
    }


    public void Swap_Menus()
    {
        StopAllCoroutines();
        step_key_text.color = Color.black;
        hop_key_text.color = Color.black;
        if (General_Obj.transform.position == in_pos)
        {
            General_Obj.transform.position = out_pos;
            Options_Obj.transform.position = in_pos;
        }
        else
        {
            General_Obj.transform.position = in_pos;
            Options_Obj.transform.position = out_pos;
        }
    }
}
