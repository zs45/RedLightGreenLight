using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trial_Manager : MonoBehaviour {

    Manager The_Man;

    //List pointing to all the cards lined up in the new potential list
    public List<TrialCard_Controller> trial_cards = new List<TrialCard_Controller>();


    public List<Card_Holder_Controller> card_holders = new List<Card_Holder_Controller>();

    public GameObject total_card_board;

    //Initial Trial Card Prefab
    public GameObject trial_card_prefab;

    public GameObject card_board_prefab;

    public Text save_message;

    public Scroll_Controller scroll_bar;

    public Dropdown set_dropdown;

    public GameObject card_outline;

    public TrialCard_Controller big_card;

    public TrialCard_Controller selected_card;

    public InputField set_name_input_field;

    public GameObject extra_fields;

    public Text big_number;

    //public GameObject extra_scroll;

    public GameObject select_outline;


    public MenuScript menuscript;

    public warning_controller warning;

    public bool has_changes = false;

    public int drop_value = -1;

    public int board_size = 2;

    public GameObject canvas_shield;

    public GameObject cover_prefab;

    public RectTransform big_cover;

    public GameObject save_button;
    public GameObject create_card_button;
    public GameObject delete_deck_button;

    bool already_matching = false;

    IEnumerator Display_MSG()
    {
        save_message.enabled = true;
        int timerr = 120;
        while (timerr > 0)
        {
            timerr -= 1;
            yield return null;
        }
        save_message.enabled = false;
    }


    //Creates a card and places it in an empty slot
    public void Create_Card()
    {

        if (set_dropdown.value >= 0)
        {

            has_changes = true;

            GameObject new_card = Instantiate(trial_card_prefab);
            TrialCard_Controller card_controller = new_card.GetComponent<TrialCard_Controller>();
            card_controller.trial_man = this;

            card_controller.ais = 80;
            card_controller.aic = 1;
            card_controller.step = 50;
            card_controller.hop = 50;
            card_controller.delay = 50;
            card_controller.swtch = 50;

            card_controller.win_msg = "YOU WIN!";
            card_controller.loss_msg = "YOU LOSE....";


            new_card.transform.SetParent(gameObject.transform);

            //Attach this card to a board
            //If this card number exceeds the number of boards, add a new one
            int t = trial_cards.Count;
            if (t >= card_holders.Count)
            {
                Add_Board();
            }
            card_holders[t].Set_Active(card_controller);

            selected_card = card_controller;


            //Match the card's visuals to the new parameters
            card_controller.Convert_To_Card();

            Convert_Big(selected_card, true);

            //Save this card in the list
            trial_cards.Add(card_controller);

            Update_Card_Positions();

        }

    }

    private void Update()
    {
        if(selected_card != null)
        {
            SetOutline(selected_card.current_holder, true);
            if(set_dropdown.options.Count == 0)
            {
                SetOutline(null, true);
            }
        }
        else
        {
            SetOutline(null, true);
        }
    }

    public void Convert_Big(TrialCard_Controller smol, bool to_big)
    {
        if(smol == null)
        {
            big_card.gameObject.SetActive(false);
            extra_fields.gameObject.SetActive(false);
            //extra_scroll.gameObject.SetActive(false);
        }


        else
        {
            if (!big_card.gameObject.activeInHierarchy)
            {
                big_card.gameObject.SetActive(true);
                extra_fields.gameObject.SetActive(true);
                //extra_scroll.gameObject.SetActive(true);
            }
            //Convert the small card to the big card
            if (to_big)
            {
                big_card.button_mode = smol.button_mode;
                big_card.aic = smol.aic;
                big_card.ais = smol.ais;
                big_card.hop = smol.hop;
                big_card.step = smol.step;
                big_card.delay = smol.delay;
                big_card.swtch = smol.swtch;
                big_card.max_time = smol.max_time;
                big_card.Convert_To_Card();
                big_number.text = smol.current_holder.number.ToString();
                if(smol.current_holder.number < 10)
                {
                    big_number.text = "0" + big_number.text;
                }
            }
            //Convert the big card to the smol card
            else
            {
                smol.button_mode = big_card.button_mode;
                smol.aic = big_card.aic;
                smol.ais = big_card.ais;
                smol.hop = big_card.hop;
                smol.step = big_card.step;
                smol.delay = big_card.delay;
                smol.swtch = big_card.swtch;
                smol.max_time = big_card.max_time;
                smol.Convert_To_Card();
            }
        }
    }

    public void Attempt_Exit()
    {
        if (has_changes)
        {
            warning.gameObject.SetActive(true);
            warning.Set_Warning(0);
        }
        else
        {
            set_dropdown.value = 0;

            //set_dropdown.captionText.text = "Select Trial Deck";

            Match_Cards_To_Trial();
            foreach (Card_Holder_Controller ch in card_holders)
            {
                ch.Set_Active(null);
            }

            menuscript.Swap_Menus();
        }
    }

    public void Save_Exit(int confirm_save)
    {
        warning.gameObject.SetActive(false);
        if (confirm_save >= 0)
        {
            if (confirm_save == 1)
            {
                Save_Trial_List();
            }

            Convert_Big(null, true);
            set_dropdown.value = 0;
            //set_dropdown.captionText.text = "Select Trial Deck";

            Match_Cards_To_Trial();
            has_changes = false;
            SetOutline(null, true);
            menuscript.Swap_Menus();
        }
    }

    //Places the outline on the hovered card slot
    public void SetOutline(Card_Holder_Controller ch, bool for_select)
    {
        //If theres no holder, place it out of sight
        if(ch == null)
        {
            if (!for_select)
            {
                card_outline.SetActive(false);
            }
            else
            {
                select_outline.SetActive(false);
            }
        }
        else
        {
            if (!for_select)
            {
                card_outline.SetActive(true);
                card_outline.transform.SetParent(ch.gameObject.transform);
                card_outline.transform.localPosition = new Vector3(50, -50, 0);
                card_outline.transform.SetParent(gameObject.transform.parent);
                card_outline.transform.SetSiblingIndex(2);
            }
            else
            {
                select_outline.SetActive(true);
                select_outline.transform.SetParent(ch.gameObject.transform);
                select_outline.transform.localPosition = new Vector3(50, -50, 0);
                select_outline.transform.SetParent(gameObject.transform.parent);
                select_outline.transform.SetSiblingIndex(2);
            }

        }
    }

    public void Attempt_Create()
    {
        if (has_changes)
        {
            warning.gameObject.SetActive(true);
            warning.Set_Warning(2);

        }
        else
        {
            Create_Set();
        }
    }

    public void Create_Result(int result)
    {
        warning.gameObject.SetActive(false);
        if (result == 1)
        {
            Save_Trial_List();
            Create_Set();
            selected_card = null;
            SetOutline(null, true);
        }
        else if(result == 0)
        {
            Create_Set();
        }
    }


    public void Create_Set()
    {
        set_name_input_field.gameObject.SetActive(true);
        save_button.SetActive(true);
        delete_deck_button.SetActive(true);
        create_card_button.SetActive(true);

        The_Man.trial_sets.Add(new Trial_Set());

        The_Man.trial_sets[The_Man.trial_sets.Count - 1].name = "Trial # " + (The_Man.trial_sets.Count-4).ToString();

        The_Man.Update_Dropdown();

        set_dropdown.value = set_dropdown.options.Count - 1;
        set_dropdown.captionText.text = set_dropdown.options[set_dropdown.value].text;
        drop_value = set_dropdown.value;
        Match_Cards_To_Trial();
        Convert_Big(null, false);
    }

    public void Rename_Set()
    {
        has_changes = true;
        The_Man.trial_sets[set_dropdown.value + 4].name = set_name_input_field.text;
        set_dropdown.options[set_dropdown.value].text = set_name_input_field.text;
        set_dropdown.captionText.text = set_name_input_field.text;
        The_Man.set_dropdown.options[set_dropdown.value + 4].text = set_name_input_field.text;
        if(The_Man.set_dropdown.value == set_dropdown.value + 4)
        {
            The_Man.set_dropdown.captionText.text = set_name_input_field.text;
        }
    }

    public void Attempt_Match_Cards()
    {
        selected_card = null;
        SetOutline(null, true);
        create_card_button.SetActive(true);
        delete_deck_button.SetActive(true);
        if (has_changes)
        {
            warning.gameObject.SetActive(true);
            warning.Set_Warning(1);

        }
        else
        {
            Match_Cards_To_Trial();
        }
    }

    public void Revert_Match_Cards(int result)
    {
        warning.gameObject.SetActive(false);
        selected_card = null;
        if (result == 1)
        {
            int new_drop = set_dropdown.value;
            set_dropdown.value = drop_value;
            Save_Trial_List();
            set_dropdown.value = new_drop;
            SetOutline(null, true);
            selected_card = null;
        }
        else if(result == 0)
        {
            int new_drop = set_dropdown.value;
            set_dropdown.value = new_drop;
            SetOutline(null, true);
            selected_card = null;
        }
        else
        {
            set_dropdown.value = drop_value;
        }
   
    }


    IEnumerator Animate_Deck_Swap()
    {
        
        //Bring it down
        for(int s = 50;s <= 1050; s+=100)
        {
            big_cover.sizeDelta = new Vector2(900, s);
            yield return null;
        }


        //Hold for a bit
        for(int s = 0;s < 20; s++)
        {
            yield return null;
        }

        Actually_Match();
        selected_card = null;

        //Bring it up
        for (int s = 1050; s >= 50; s -= 100)
        {
            big_cover.sizeDelta = new Vector2(900, s);
            yield return null;
        }

        canvas_shield.SetActive(false);
        already_matching = false;
        yield break;
    }


    //The old match cards to trial function, which now calls the coroutine that calls this
    void Actually_Match()
    {
        set_name_input_field.gameObject.SetActive(true);

        Convert_Big(null, false);

        //First, delete every card
        foreach (TrialCard_Controller card in trial_cards)
        {
            Destroy(card.gameObject);
        }
        trial_cards.Clear();

        //Delete every board except the last one
        while (card_holders.Count > board_size)
        {
            Delete_Board();
        }

        //Set the last board inactive
        foreach (Card_Holder_Controller ch in card_holders)
        {
            ch.Set_Active(null);
        }


        if (set_dropdown.options.Count == 0)
        {
            set_name_input_field.gameObject.SetActive(false);
            set_dropdown.captionText.text = "Create Trial Deck";

            total_card_board.transform.localPosition = new Vector3(2000, 0);
        }

        else
        {
            total_card_board.transform.localPosition = new Vector3(200, 0);
            //Find the set we're loading 
            Trial_Set current_set = The_Man.trial_sets[set_dropdown.value + 4];

            drop_value = set_dropdown.value;

            set_name_input_field.text = current_set.name;

            //Create a card for every trial in the set
            for (int nt = 0; nt < current_set.trial_list.Count; nt++)
            {
                GameObject new_card = Instantiate(trial_card_prefab);
                TrialCard_Controller card_controller = new_card.GetComponent<TrialCard_Controller>();
                card_controller.trial_man = this;

                //Place this new card
                new_card.transform.SetParent(gameObject.transform);


                //Set the cards parameters
                if (current_set.trial_list[nt].num_players == 1 && !current_set.trial_list[nt].hop_mode)
                {
                    card_controller.button_mode = 1;
                }
                if (current_set.trial_list[nt].num_players == 2 && !current_set.trial_list[nt].hop_mode)
                {
                    card_controller.button_mode = 2;
                }
                if (current_set.trial_list[nt].num_players == 1 && current_set.trial_list[nt].hop_mode)
                {
                    card_controller.button_mode = 3;
                }
                if (current_set.trial_list[nt].num_players == 2 && current_set.trial_list[nt].hop_mode)
                {
                    card_controller.button_mode = 4;
                }

                card_controller.win_msg = current_set.trial_list[nt].win_msg;
                card_controller.loss_msg = current_set.trial_list[nt].loss_msg;


                card_controller.max_time = current_set.trial_list[nt].max_time;
                card_controller.ais = current_set.trial_list[nt].ai_level;
                card_controller.aic = current_set.trial_list[nt].ai_cheat;

                card_controller.step = current_set.trial_list[nt].step_distance;
                card_controller.hop = current_set.trial_list[nt].hop_distance;
                card_controller.delay = current_set.trial_list[nt].hop_delay;
                card_controller.swtch = current_set.trial_list[nt].light_switch_speed;

                //Attach this card to a board
                //If this card number exceeds the number of boards, add a new one
                if (nt == card_holders.Count)
                {
                    Add_Board();
                }
                card_holders[nt].Set_Active(card_controller);


                //Match the card's visuals to the new parameters
                card_controller.Convert_To_Card();

                //Save this card in the list
                trial_cards.Add(card_controller);
            }
            Update_Card_Positions();
        }
    }



    //Looks at the value of the dropdown, and matches the cards to the new trial set
    public void Match_Cards_To_Trial()
    {
        if (!already_matching)
        {
            already_matching = true;
            canvas_shield.SetActive(true);
            StartCoroutine(Animate_Deck_Swap());
        }
    }


    private void Start()
    {
        GameObject tm = GameObject.Find("GameManager");

        //Initialize the first board to match the board size

        if(board_size < 3)
        {
            GameObject first_holder = card_holders[0].gameObject;
            card_holders.RemoveAt(0);
            Destroy(first_holder);
            if(board_size < 2)
            {
                first_holder = card_holders[0].gameObject;
                card_holders.RemoveAt(0);
                Destroy(first_holder);
            }
            for(int d = 0; d < card_holders.Count; d++)
            {
                card_holders[d].number = d + 1;
                card_holders[d].num_text.text = "0" + (d + 1).ToString();
            }
        }

        if (tm != null)
        {
            The_Man = tm.GetComponent<Manager>();
            //Looks at the manager's list to initialize the cards
            for (int t = 0; t < The_Man.trial_list.Count; t++)
            {
                GameObject new_card = Instantiate(trial_card_prefab);
                TrialCard_Controller card_controller = new_card.GetComponent<TrialCard_Controller>();
                card_controller.trial_man = this;

                //Place this new card
                new_card.transform.SetParent(gameObject.transform);


                //Set the cards parameters
                if(The_Man.trial_list[t].num_players == 1 && !The_Man.trial_list[t].hop_mode)
                {
                    card_controller.button_mode = 1;
                }
                if(The_Man.trial_list[t].num_players == 2 && !The_Man.trial_list[t].hop_mode)
                {
                    card_controller.button_mode = 2;
                }
                if(The_Man.trial_list[t].num_players == 1 && The_Man.trial_list[t].hop_mode)
                {
                    card_controller.button_mode = 3;
                }
                if(The_Man.trial_list[t].num_players == 2 && The_Man.trial_list[t].hop_mode)
                {
                    card_controller.button_mode = 4;
                }

                card_controller.max_time = The_Man.trial_list[t].max_time;
                card_controller.ais = The_Man.trial_list[t].ai_level;
                card_controller.aic = The_Man.trial_list[t].ai_cheat;

                card_controller.step = The_Man.trial_list[t].step_distance;
                card_controller.hop = The_Man.trial_list[t].hop_distance;
                card_controller.delay = The_Man.trial_list[t].hop_delay;
                card_controller.swtch = The_Man.trial_list[t].light_switch_speed;

                //Attach this card to a board
                //If this card number exceeds the number of boards, add a new one
                if (t == card_holders.Count)
                {
                    Add_Board();
                }
                card_holders[t].Set_Active(card_controller);


                //Match the card's visuals to the new parameters
                card_controller.Convert_To_Card();

                //Save this card in the list
                trial_cards.Add(card_controller);

            }
            Update_Card_Positions();
        }
    }

    //Make sure every card is in the right place
    public void Update_Card_Positions()
    {
        //First, go through every holder and make sure there are no gaps
        for(int h = 0; h < card_holders.Count; h++)
        {
            //If we find a gap of cards, shift every card right of the gap 1 space to the left
            if(card_holders[h].current_card == null)
            {
                for(int sh = h; sh < card_holders.Count; sh++)
                {
                    if(sh == card_holders.Count - 1)
                    {
                        card_holders[sh].Set_Active(null);
                    }
                    else
                    {
                        card_holders[sh].Set_Active(card_holders[sh + 1].current_card);
                        if (card_holders[sh].current_card != null)
                        {
                            card_holders[sh].current_card.StartCoroutine(card_holders[sh].current_card.Move_Over(card_holders[sh + 1], card_holders[sh]));
                            card_holders[sh].current_card.moving = true;
                        }
                    }
                }
            }
        }

        //Make sure every card is in the correct position 65, -115

        for(int c = 0; c < trial_cards.Count; c++)
        {
            if (!trial_cards[c].moving)
            {
                trial_cards[c].gameObject.transform.position = new Vector2(trial_cards[c].current_holder.gameObject.transform.position.x + 114.5f, trial_cards[c].current_holder.gameObject.transform.position.y - 165);
            }
        }
    }

    public void Delete_Deck()
    {
        if (set_dropdown.options.Count > 0 && set_dropdown.value > -1)
        {
            has_changes = false;
            Debug.Log("SETTING INACTIVE");
            select_outline.SetActive(false);
            int set_index = set_dropdown.value + 4;
            The_Man.trial_sets.RemoveAt(set_index);
            The_Man.Update_Dropdown();
            int new_index = set_index - 4;
            if(new_index >= set_dropdown.options.Count)
            {
                new_index = set_dropdown.options.Count - 1;
            }
            set_dropdown.value = new_index;
            //Debug.Log("VALUE " + new_index.ToString());
            if (new_index < 0)
            {
                set_dropdown.captionText.text = "Create Trial Deck";
                set_name_input_field.gameObject.SetActive(false);
                save_button.SetActive(false);
                delete_deck_button.SetActive(false);
                create_card_button.SetActive(false);

            }
            else
            {
                Debug.Log("LOOKING AT " + set_dropdown.value.ToString());
                set_dropdown.captionText.text = set_dropdown.options[new_index].text;
            }
            Match_Cards_To_Trial();
            The_Man.Load_Set();
        }
    }


    //Creates the manager's trial list from the created card sorting
    public void Save_Trial_List()
    {

        has_changes = false;

        if (trial_cards.Count > 0)
        {

            int set_index = set_dropdown.value + 4;

            //If the set index is larger than the max size of sets, create a new one
            if (set_index >= The_Man.trial_sets.Count)
            {
                The_Man.trial_sets.Add(new Trial_Set());
                The_Man.trial_sets[The_Man.trial_sets.Count - 1].name = "TRIAL # " + (The_Man.trial_sets.Count).ToString();
                set_index = The_Man.trial_sets.Count - 1;
            }


            //Make sure the manager's list is empty first

            The_Man.trial_sets[set_index].trial_list.Clear();

            //Go through each card in the potential list, and extract their trial data for a new trial
            foreach (TrialCard_Controller tc in trial_cards)
            {
                Trial_Data new_td = new Trial_Data();

                //Use the card's button mode to get a player number and control mode
                if (tc.button_mode == 1)
                {
                    new_td.num_players = 1;
                    new_td.hop_mode = false;
                }
                else if (tc.button_mode == 2)
                {
                    new_td.num_players = 2;
                    new_td.hop_mode = false;
                }
                else if (tc.button_mode == 3)
                {
                    new_td.num_players = 1;
                    new_td.hop_mode = true;
                }
                else
                {
                    new_td.num_players = 2;
                    new_td.hop_mode = true;
                }

                //Find the time limit for the trial
                new_td.max_time = tc.max_time;

                //Get the ai parameters
                new_td.ai_level = tc.ais;
                new_td.ai_cheat = tc.aic;
                new_td.step_distance = tc.step;
                new_td.hop_distance = tc.hop;
                new_td.hop_delay = tc.delay;
                new_td.light_switch_speed = tc.swtch;
                new_td.win_msg = tc.win_msg;
                new_td.loss_msg = tc.loss_msg;

                //Insert this new trial into the managers list
                The_Man.trial_sets[set_index].trial_list.Add(new_td);
            }

            The_Man.Load_Set();
        }

    }

    //Called when all the current card holders are filled, and creates a new one at the bottom
    public void Add_Board()
    {
        GameObject new_board = Instantiate(card_board_prefab);
        //Set the new board to be local to the board system
        new_board.transform.SetParent(total_card_board.transform);
        new_board.transform.SetAsLastSibling();

        //Place the board at the bottom of the system
        int board_number = card_holders.Count / board_size;
        new_board.transform.localPosition = new Vector2(-537, 531 - board_number * 500.0f);
       
    
        //Create references to the new card holders
        for (int d = 0; d < 3; d++)
        {
            card_holders.Add(new_board.transform.GetChild(d+1).gameObject.GetComponent<Card_Holder_Controller>());
        }

        //Shrink the board to match the board size
        if (board_size < 3)
        {
            GameObject first_holder = card_holders[card_holders.Count-3].gameObject;
            card_holders.RemoveAt(card_holders.Count - 3);
            Destroy(first_holder);
            if (board_size < 2)
            {
                first_holder = card_holders[card_holders.Count - 2].gameObject;
                card_holders.RemoveAt(card_holders.Count - 2);
                Destroy(first_holder);
            }
        }

        //Set the numbers 
        for (int d = 0; d < board_size; d++)
        {  
            card_holders[card_holders.Count - (d+1)].number = card_holders.Count - (d);
            card_holders[card_holders.Count - (d+1)].num_text.text = (card_holders.Count-d).ToString();
            if (card_holders[card_holders.Count - (d+1)].num_text.text.Length == 1)
            {
                card_holders[card_holders.Count - (d+1)].num_text.text = "0" + (card_holders.Count - (d)).ToString();
            }
        }
      
        if (card_holders.Count > board_size)
        {
            scroll_bar.gameObject.SetActive(true);
        }

        scroll_bar.Adjust_Size((card_holders.Count / board_size));

    }

    //Called when the bottom card board is not needed, erasing it
    public void Delete_Board()
    {

        has_changes = false;
        //Get the board reference
        GameObject last_board = card_holders[card_holders.Count - 1].gameObject.transform.parent.gameObject;
        //Remove the last row of references in the card board list
        for (int d = 0; d < board_size; d++)
        {
            card_holders.RemoveAt(card_holders.Count - 1);
        }


        //If there are no more than a row cards, hide the scrollbar
        if(card_holders.Count <= board_size)
        {
            scroll_bar.gameObject.SetActive(false);
        }

        //Board go bye bye
        Destroy(last_board);
        scroll_bar.Adjust_Size((card_holders.Count / board_size));
    }

}
