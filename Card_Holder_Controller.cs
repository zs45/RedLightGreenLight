using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_Holder_Controller : MonoBehaviour {

    public TrialCard_Controller current_card;

    Trial_Manager trial_man;

    public Sprite active_sprite;
    public Sprite inactive_sprite;

    public Button copy_button;
    public Button delete_button;

    public Image image;

    public int number;

    public Text num_text;
    public void Set_Active(TrialCard_Controller new_card)
    {
        //If there is a null argument, deactivate holder
        if (new_card == null)
        {
            image.sprite = inactive_sprite;
            current_card = null;
            copy_button.interactable = false;
            delete_button.interactable = false;
        }
        else
        {
            image.sprite = active_sprite;
            current_card = new_card;
            copy_button.interactable = true;
            delete_button.interactable = true;
            new_card.current_holder = this;
        }
    }


    public void Copy()
    {
        current_card.Create_Copy();
    }

    public void Delete()
    {
        StartCoroutine(Start_Delete());
    }

    //Play's the animation for card deletion
    IEnumerator Start_Delete()
    {
        current_card.trial_man.canvas_shield.gameObject.SetActive(true);
        RectTransform cover = Instantiate(current_card.trial_man.cover_prefab).GetComponent<RectTransform>();
        cover.SetParent(this.gameObject.transform);
        cover.localPosition = new Vector2(250, -165);
        cover.SetParent(current_card.gameObject.transform.parent);
        cover.SetAsLastSibling();

        int height = 1;

        //Expand the cover
        while(height < 370)
        {
            height += 37;
            if(height > 370) { height = 370; }
            cover.sizeDelta = new Vector2(270, height);
            yield return null;
        }

        //Hold the cover
        for(int t = 0;t < 20; t++)
        {
            yield return null;
        }

        //Hide the card
        current_card.gameObject.transform.Translate(Vector2.up * 2000);

        //Shrink the cover
        while (height > 1)
        {
            height -= 37;
            if (height < 1) { height = 1; }
            cover.sizeDelta = new Vector2(270, height);
            yield return null;
        }

        //Hold the cover
        for (int t = 0; t < 20; t++)
        {
            yield return null;
        }

        //End the coroutine and delete the card
        Destroy(cover.gameObject);
        current_card.trial_man.canvas_shield.gameObject.SetActive(false);
        current_card.Delete_Card();
        yield break;
    }



}
