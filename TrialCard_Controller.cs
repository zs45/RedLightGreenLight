using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrialCard_Controller : MonoBehaviour {

    //References
    public Image mode_button_image;

    //References to the timer's input field and text
    public InputField minutes_field;
    public InputField seconds_field;

    EventSystem myeventsystem;

    //References to ai bars and texts
    public Image ai_skill_bar;
    public Image ai_cheat_bar;

    public Image step_bar;
    public Image hop_bar;

    public Image delay_bar;
    public Image switch_bar;

    public bool is_big;

    //public Text ai_skill_text;
    //public Text ai_cheat_text;

    public int ais = 1;
    public int aic = 1;

    public int step = 1;
    public int hop = 1;
    public int delay = 1;
    public int swtch = 1;

    public string win_msg = "";
    public string loss_msg = "";

    public Trial_Manager trial_man;

    public BoxCollider2D box2d;
    public Rigidbody2D rb;
    public Card_Holder_Controller touched_holder;
    public Card_Holder_Controller current_holder;

    public bool moving = false;

    public List<Sprite> mode_sprites = new List<Sprite>();

    //The mode for this trial
    //
    // 1 - 1 player, step only
    // 2 - 2 player, step only
    // 3 - 1 player, step and hop
    // 4 - 2 player, step and hop
    public int button_mode = 1;

    public int max_time = 120;


    List<char> char_list = new List<char>();


    void Start()
    {
        char_list.Add('0');
        char_list.Add('1');
        char_list.Add('2');
        char_list.Add('3');
        char_list.Add('4');
        char_list.Add('5');
        char_list.Add('6');
        char_list.Add('7');
        char_list.Add('8');
        char_list.Add('9');

        
        myeventsystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.simulated = false;
        StartCoroutine(Timer_Input());
    }

    private void Update()
    {
        ai_skill_bar.rectTransform.sizeDelta = (new Vector2(ais * 2.0f, 20.0f));
        
        ai_cheat_bar.rectTransform.sizeDelta = (new Vector2(aic * 2.0f, 20.0f));

        step_bar.rectTransform.sizeDelta = (new Vector2(step * 2.0f, 20.0f));
        hop_bar.rectTransform.sizeDelta = (new Vector2(hop * 2.0f, 20.0f));
        delay_bar.rectTransform.sizeDelta = (new Vector2(delay * 2.0f, 20.0f));
        switch_bar.rectTransform.sizeDelta = (new Vector2(swtch * 2.0f, 20.0f));
    }


    //Called by the manager when the variables are preset, and changes the cards visuals to match
    public void Convert_To_Card()
    {
        //Convert the button
        button_mode -= 1;
        Cycle_Button(0);

        //Convert the timer

        int sec_val = max_time % 60;
        int min_val = max_time / 60;

        if(min_val < 10)
        {
            minutes_field.text = "0" + min_val.ToString();
        }
        else
        {
            minutes_field.text = min_val.ToString();
        }

        if(sec_val < 10)
        {
            seconds_field.text = "0" + sec_val.ToString();
        }
        else
        {
            seconds_field.text = sec_val.ToString();
        }

       

        //Convert the AI bars and text
        //ai_skill_text.text = ais.ToString();
        //ai_cheat_text.text = aic.ToString();
        ai_skill_bar.rectTransform.sizeDelta = (new Vector2(ais * 2.0f, 20.0f));
        ai_cheat_bar.rectTransform.sizeDelta = (new Vector2(aic * 2.0f, 20.0f));
        step_bar.rectTransform.sizeDelta = (new Vector2(step * 2.0f, 20.0f));
        hop_bar.rectTransform.sizeDelta = (new Vector2(hop * 2.0f, 20.0f));
        delay_bar.rectTransform.sizeDelta = (new Vector2(delay * 2.0f, 20.0f));
        switch_bar.rectTransform.sizeDelta = (new Vector2(swtch * 2.0f, 20.0f));
    }


    //Called when the mode button is pressed, cycles the trial's game mode
    public void Cycle_Button(int button_press)
    {
        if(button_press == 1)
        {
            trial_man.has_changes = true;

            if (!is_big)
            {
                trial_man.SetOutline(current_holder, true);
            }
        }


        if (button_mode == 1)
        {
            button_mode = 2;

        }
        else if (button_mode == 2)
        {
            button_mode = 3;


        }
        else if (button_mode == 3)
        {
            button_mode = 4;

        }
        else
        {
            button_mode = 1;

        }
        mode_button_image.sprite = mode_sprites[button_mode - 1];
        if (!is_big)
        {
            trial_man.selected_card = this;
            if (button_press == 1)
            {
                trial_man.Convert_Big(this, true);
            }
        }
        if (is_big && button_press == 1)
        {
            trial_man.Convert_Big(trial_man.selected_card, false);
        }
    }


    //Plays a move animation for the cards
    public IEnumerator Move_Over(Card_Holder_Controller home, Card_Holder_Controller target)
    {
        Vector2 home_vec = new Vector2(home.gameObject.transform.position.x + 114.5f, home.gameObject.transform.position.y - 165);
        Vector2 targ_vec = new Vector2(target.gameObject.transform.position.x + 114.5f, target.gameObject.transform.position.y - 165);
        //Start the card at it's home holder
        gameObject.transform.position = home_vec;

        while (gameObject.transform.position.x != targ_vec.x || gameObject.transform.position.y != targ_vec.y)
        {
            moving = true;
            //Keep the canvas from being clicked
            trial_man.canvas_shield.SetActive(true);

            //Move the card towards it's target
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, targ_vec, 50.0f);

            yield return null;
        }

            //Make sure the card is placed firmly and end the coroutine
        gameObject.transform.position = targ_vec;
        moving = false;
        trial_man.canvas_shield.SetActive(false);
        yield break;
    }



    //Runs while the card is being dragged by the mouse
    public IEnumerator Mouse_Drag_Card(ButtonHold_Script bhs)
    {
        if (!is_big)
        {
            trial_man.selected_card = this;
            trial_man.Convert_Big(this, true);
        }
        gameObject.transform.SetAsLastSibling();
        touched_holder = null;
        box2d.enabled = true;
        rb.simulated = true;

        Vector3 nexus = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z);
        Vector3 og = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,gameObject.transform.position.z);

        while (true)
        {
            if (!is_big)
            {
                trial_man.SetOutline(current_holder, true);
            }
            //Calculate distance from original mouse position
            float x_distance = Input.mousePosition.x - nexus.x;
            float y_distance = Input.mousePosition.y - nexus.y;

            gameObject.transform.position = new Vector3(og.x + x_distance, og.y + y_distance, og.z);


            //Check if card is let go
            if (bhs.ispressed == false)
            {

                TrialCard_Controller other_card = null;
                Card_Holder_Controller other_holder = null;

                trial_man.SetOutline(null, false);
                if (touched_holder != null && touched_holder != current_holder)
                {
                    other_holder = current_holder;
                    if(touched_holder.current_card != null)
                    {
                        other_card = touched_holder.current_card;
                    }
                    current_holder.Set_Active(touched_holder.current_card);
                    trial_man.has_changes = true;
                    touched_holder.Set_Active(this);
                    trial_man.SetOutline(touched_holder, true);
 
                }
                //Physically swap the cards
                trial_man.Update_Card_Positions();
                if(other_card != null)
                {
                    other_card.StartCoroutine(other_card.Move_Over(current_holder, other_holder));
                }
                touched_holder = null;
                box2d.enabled = false;
                rb.simulated = false;
                trial_man.big_number.text = current_holder.number.ToString();
                if(current_holder.number < 10)
                {
                    trial_man.big_number.text = "0" + trial_man.big_number.text;
                }
                yield break;
            }

            if (is_big)
            {
                trial_man.Convert_Big(trial_man.selected_card, false);
            }

            yield return null;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        touched_holder = collision.gameObject.GetComponent<Card_Holder_Controller>();
        trial_man.SetOutline(touched_holder,false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (touched_holder != null && collision.gameObject == touched_holder.gameObject)
        {
            touched_holder = null;
            trial_man.SetOutline(null,false);
        }
    }

    public IEnumerator Mouse_Drag_AI_Skill(ButtonHold_Script bhs)
    {
        if (!is_big)
        {
            trial_man.selected_card = this;
            trial_man.Convert_Big(this, true);
        }
        while (true)
        {
            if (!is_big)
            {
                trial_man.SetOutline(current_holder, true);
            }
            trial_man.has_changes = true;
            if (!bhs.ispressed)
            {
                yield break;
            }
            //Calculate the distance from the start of the bar to the mouse
            float mouse_distance = Input.mousePosition.x - ai_skill_bar.gameObject.transform.position.x - 50.0f;
            int md = Mathf.RoundToInt(mouse_distance);
            //Keep the distance in the range of 1-100
            if(md < 1)
            {
                md = 1;
            }
            if(md > 100)
            {
                md = 100;
            }
            //Change the text and bar to reflect the distance
            //ai_skill_text.text = md.ToString();
            ais = md;
            ai_skill_bar.rectTransform.sizeDelta = (new Vector2(md * 2.0f, 20.0f));

            if (is_big)
            {
                trial_man.Convert_Big(trial_man.selected_card, false);
            }
            else
            {
                trial_man.Convert_Big(this, true);
            }

            yield return true;
        }
    }

    public IEnumerator Mouse_Drag_AI_Cheat(ButtonHold_Script bhs)
    {
        if (!is_big)
        {
            trial_man.selected_card = this;
            trial_man.Convert_Big(this, true);
        }
        while (true)
        {
            if (!is_big)
            {
                trial_man.SetOutline(current_holder, true);
            }
            trial_man.has_changes = true;
            if (!bhs.ispressed)
            {
                yield break;
            }
            //Calculate the distance from the start of the bar to the mouse
            float mouse_distance = Input.mousePosition.x - ai_cheat_bar.gameObject.transform.position.x -50.0f;
            int md = Mathf.RoundToInt(mouse_distance);
            //Keep the distance in the range of 1-100
            if (md < 1)
            {
                md = 1;
            }
            if (md > 100)
            {
                md = 100;
            }
            //Change the text and bar to reflect the distance
           // ai_cheat_text.text = md.ToString();
            aic = md;
            ai_cheat_bar.rectTransform.sizeDelta = (new Vector2(md * 2.0f, 20.0f));

            if (is_big)
            {
                trial_man.Convert_Big(trial_man.selected_card, false);
            }
            else
            {
                trial_man.Convert_Big(this, true);
            }

            yield return true;
        }
    }


    public IEnumerator Mouse_Drag_Step(ButtonHold_Script bhs)
    {
        if (!is_big)
        {
            trial_man.selected_card = this;
            trial_man.Convert_Big(this, true);
        }
        while (true)
        {
            if (!is_big)
            {
                trial_man.SetOutline(current_holder, true);
            }
            trial_man.has_changes = true;
            if (!bhs.ispressed)
            {
                yield break;
            }
            //Calculate the distance from the start of the bar to the mouse
            float mouse_distance = Input.mousePosition.x - step_bar.gameObject.transform.position.x - 50.0f;
            int md = Mathf.RoundToInt(mouse_distance);
            //Keep the distance in the range of 1-100
            if (md < 1)
            {
                md = 1;
            }
            if (md > 100)
            {
                md = 100;
            }
            //Change the text and bar to reflect the distance
            // ai_cheat_text.text = md.ToString();
            step = md;
            step_bar.rectTransform.sizeDelta = (new Vector2(md * 2.0f, 20.0f));

            if (is_big)
            {
                trial_man.Convert_Big(trial_man.selected_card, false);
            }
            else
            {
                trial_man.Convert_Big(this, true);
            }

            yield return true;
        }
    }

    public IEnumerator Mouse_Drag_Hop(ButtonHold_Script bhs)
    {
        if (!is_big)
        {
            trial_man.selected_card = this;
            trial_man.Convert_Big(this, true);
        }
        while (true)
        {
            if (!is_big)
            {
                trial_man.SetOutline(current_holder, true);
            }
            trial_man.has_changes = true;
            if (!bhs.ispressed)
            {
                yield break;
            }
            //Calculate the distance from the start of the bar to the mouse
            float mouse_distance = Input.mousePosition.x - hop_bar.gameObject.transform.position.x - 50.0f;
            int md = Mathf.RoundToInt(mouse_distance);
            //Keep the distance in the range of 1-100
            if (md < 1)
            {
                md = 1;
            }
            if (md > 100)
            {
                md = 100;
            }
            //Change the text and bar to reflect the distance
            // ai_cheat_text.text = md.ToString();
            hop = md;
            hop_bar.rectTransform.sizeDelta = (new Vector2(md * 2.0f, 20.0f));

            if (is_big)
            {
                trial_man.Convert_Big(trial_man.selected_card, false);
            }
            else
            {
                trial_man.Convert_Big(this, true);
            }

            yield return true;
        }
    }


    public IEnumerator Mouse_Drag_Delay(ButtonHold_Script bhs)
    {
        if (!is_big)
        {
            trial_man.selected_card = this;
            trial_man.Convert_Big(this, true);
        }
        while (true)
        {
            if (!is_big)
            {
                trial_man.SetOutline(current_holder, true);
            }
            trial_man.has_changes = true;
            if (!bhs.ispressed)
            {
                yield break;
            }
            //Calculate the distance from the start of the bar to the mouse
            float mouse_distance = Input.mousePosition.x - delay_bar.gameObject.transform.position.x - 50.0f;
            int md = Mathf.RoundToInt(mouse_distance);
            //Keep the distance in the range of 1-100
            if (md < 1)
            {
                md = 1;
            }
            if (md > 100)
            {
                md = 100;
            }
            //Change the text and bar to reflect the distance
            // ai_cheat_text.text = md.ToString();
            delay = md;
            delay_bar.rectTransform.sizeDelta = (new Vector2(md * 2.0f, 20.0f));

            if (is_big)
            {
                trial_man.Convert_Big(trial_man.selected_card, false);
            }
            else
            {
                trial_man.Convert_Big(this, true);
            }

            yield return true;
        }
    }


    public IEnumerator Mouse_Drag_Switch(ButtonHold_Script bhs)
    {
        if (!is_big)
        {
            trial_man.selected_card = this;
            trial_man.Convert_Big(this, true);
        }
        while (true)
        {
            if (!is_big)
            {
                trial_man.SetOutline(current_holder, true);
            }
            trial_man.has_changes = true;
            if (!bhs.ispressed)
            {
                yield break;
            }
            //Calculate the distance from the start of the bar to the mouse
            float mouse_distance = Input.mousePosition.x - switch_bar.gameObject.transform.position.x - 50.0f;
            int md = Mathf.RoundToInt(mouse_distance);
            //Keep the distance in the range of 1-100
            if (md < 1)
            {
                md = 1;
            }
            if (md > 100)
            {
                md = 100;
            }
            //Change the text and bar to reflect the distance
            // ai_cheat_text.text = md.ToString();
            swtch = md;
            switch_bar.rectTransform.sizeDelta = (new Vector2(md * 2.0f, 20.0f));

            if (is_big)
            {
                trial_man.Convert_Big(trial_man.selected_card, false);
            }
            else
            {
                trial_man.Convert_Big(this, true);
            }

            yield return true;
        }
    }





    public IEnumerator Timer_Input()
    {
 
        int prev_frame_pos = 0;

        if(myeventsystem == null)
        {
            myeventsystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }

        while (true)
        {
            if (myeventsystem.currentSelectedGameObject == seconds_field.gameObject || myeventsystem.currentSelectedGameObject == minutes_field.gameObject)
            {
                if (!is_big)
                {
                    trial_man.SetOutline(current_holder, true);
                }
                trial_man.has_changes = true;
                if (!is_big)
                {
                    trial_man.selected_card = this;
                    trial_man.big_card.minutes_field.text = this.minutes_field.text;
                    trial_man.big_card.seconds_field.text = this.seconds_field.text;
                }
                else
                {
                    trial_man.selected_card.minutes_field.text = this.minutes_field.text;
                    trial_man.selected_card.seconds_field.text = this.seconds_field.text;
                }
                if (myeventsystem.currentSelectedGameObject == seconds_field.gameObject)
                {
                    //Debug.Log("IN SECONDS at : "+seconds_field.caretPosition);
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        //Debug.Log("Left pressed");
                        if (seconds_field.caretPosition == 0 && prev_frame_pos == 0)
                        {
                            //Debug.Log("MOVING TO MINUTES");
                            myeventsystem.SetSelectedGameObject(minutes_field.gameObject);
                            minutes_field.caretPosition = 1;
                            minutes_field.selectionFocusPosition = 1;
                        }
                    }
                    if (myeventsystem.currentSelectedGameObject == seconds_field.gameObject)
                    {
                        prev_frame_pos = seconds_field.caretPosition;
                    }


                }
                else if (myeventsystem.currentSelectedGameObject == minutes_field.gameObject)
                {
                    //Debug.Log("IN MINUTES at : "+minutes_field.caretPosition);
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                       
                        //Debug.Log("Right pressed");
                        if (minutes_field.caretPosition == minutes_field.text.Length && prev_frame_pos == minutes_field.text.Length)
                        {
                            //Debug.Log("MOVING TO SECONDS");
                            myeventsystem.SetSelectedGameObject(seconds_field.gameObject);
                            seconds_field.caretPosition = 1;
                            seconds_field.selectionFocusPosition = 1;
                        }
                    }
                    if (myeventsystem.currentSelectedGameObject == minutes_field.gameObject)
                    {
                        prev_frame_pos = minutes_field.caretPosition;
                    }
                }

            }

            yield return null;
        }
    }

    //Plays a copy animation
    public IEnumerator Copy_Animate(Card_Holder_Controller home,Card_Holder_Controller target)
    {
        //Keep the canvas from being clicked
        trial_man.canvas_shield.SetActive(true);

        //Create the copy effect and get references
        RectTransform cover_transform = Instantiate(trial_man.cover_prefab).GetComponent<RectTransform>();
        Image cover_image = cover_transform.gameObject.GetComponent<Image>();

        //Initialize the copy effect
        cover_transform.sizeDelta = new Vector2(270, 370);
        cover_image.color = new Color(0, 1, 0, 0);
        cover_image.gameObject.transform.SetParent(this.gameObject.transform);
        cover_image.gameObject.transform.SetAsLastSibling();
        cover_image.gameObject.transform.localPosition = new Vector2(135, 0);

        //Increase the opacity of the cover over time
        for(float o = 0;o <= 1.0f; o += 0.05f)
        {
            cover_image.color = new Color(0, 1, 0, o);
            yield return null;
        }

        //Hold the cover for a bit
        for(int b = 0;b < 20; b++)
        {
            yield return null;
        }

        //Move this card to the target
        StartCoroutine(Move_Over(home, target));

        //Hide the cover over time
        for (float o = 1.0f; o >= 0.0f; o -= 0.05f)
        {
            cover_image.color = new Color(0, 1, 0, o);
            yield return null;
        }


        //Deactivate everything
        trial_man.canvas_shield.SetActive(false);
        Destroy(cover_transform.gameObject);
        yield break;
    }

    //Creates a copy of this trial card
    public void Create_Copy()
    {
        trial_man.has_changes = true;
        GameObject card_copy = Instantiate(this.gameObject);
        card_copy.transform.position = this.gameObject.transform.position;
        TrialCard_Controller new_card = card_copy.GetComponent<TrialCard_Controller>();
        card_copy.transform.SetParent(this.gameObject.transform.parent);
        card_copy.transform.localScale = gameObject.transform.localScale;
        new_card.trial_man = trial_man;

        //Place the card in the first empty holder
        for (int h = 0; h < trial_man.card_holders.Count; h++)
        {
            if(trial_man.card_holders[h].current_card == null)
            {
                trial_man.card_holders[h].Set_Active(new_card);
                new_card.StartCoroutine(new_card.Copy_Animate(current_holder, trial_man.card_holders[h]));
                h = trial_man.card_holders.Count;
            }
            //If there are no holders left, create a new board
            else if(h == trial_man.card_holders.Count - 1)
            {
                trial_man.Add_Board();
                trial_man.card_holders[h + 1].Set_Active(new_card);
                new_card.StartCoroutine(new_card.Copy_Animate(current_holder, trial_man.card_holders[h+1]));
                h = trial_man.card_holders.Count;
            }
        }
 
        trial_man.trial_cards.Add(new_card);
    }

     
    //Destroys this trial
    public void Delete_Card()
    {
        if(this == trial_man.selected_card)
        {
            trial_man.SetOutline(null, true);
            trial_man.Convert_Big(null, true);
            trial_man.selected_card = null;
        }
        trial_man.has_changes = true;
        //Don't destroy this card if it's the only one
        // Addendum: Can delete last card now
        if (trial_man.trial_cards.Count != 1 || true)
        {
            current_holder.Set_Active(null);
            trial_man.trial_cards.Remove(this);
            trial_man.Update_Card_Positions();
            //If the last three row of cards are empty, destroy the board
            bool has_card = false;
            for(int d = 0;d < trial_man.board_size; d++)
            {
                if(trial_man.card_holders[trial_man.card_holders.Count-(d+1)].current_card != null)
                {
                    has_card = true;
                    d = trial_man.board_size;
                }
            }
            if (!has_card)
            {
                trial_man.Delete_Board();
            }

            Destroy(this.gameObject);
        }

    }

    public void Check_Field(int is_minute)
    {
        //Check minute field
        if(is_minute == 1)
        {
            if (minutes_field.text.Length == 0)
            {
                minutes_field.text = "00";
            }
            if (minutes_field.text.Length == 1)
            {
                minutes_field.text = "0" + minutes_field.text;
            }
        }
        //Check seconds field
        else
        {
            if (seconds_field.text.Length == 0)
            {
                seconds_field.text = "00";
            }
            if (seconds_field.text.Length == 1)
            {
                seconds_field.text = "0" + seconds_field.text;
            }
            if(char_list.IndexOf(seconds_field.text[0]) > 5)
            {
                seconds_field.text = "59";
            }
        }

        //Calculate max time from fields
        int minute_val = 0;
        minute_val = (char_list.IndexOf(minutes_field.text[0]) * 10) + char_list.IndexOf(minutes_field.text[1]);

        int seconds_val = 0;
        seconds_val = (char_list.IndexOf(seconds_field.text[0]) * 10) + char_list.IndexOf(seconds_field.text[1]);

        max_time = (minute_val * 60) + seconds_val;


        if (!is_big)
        {
            trial_man.big_card.minutes_field.text = this.minutes_field.text;
            trial_man.big_card.seconds_field.text = this.seconds_field.text;
            trial_man.big_card.max_time = this.max_time;
        }
        else
        {
            trial_man.selected_card.minutes_field.text = this.minutes_field.text;
            trial_man.selected_card.seconds_field.text = this.seconds_field.text;
            trial_man.selected_card.max_time = this.max_time;
        }


    }


}
