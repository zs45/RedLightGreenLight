using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{

    // Use this for initialization

    public List<GameObject> bar_list = new List<GameObject>();

    public Image image;

    public Sprite found_sprite;

    public bool working = false;

    public Image countdown;

    public List<Sprite> cdown_sprites = new List<Sprite>();

    public void startLoading(int timer)
    {
        // This will start timer
        this.gameObject.transform.localPosition = Vector3.zero;
        StartCoroutine(loadScreen(timer));
    }

    IEnumerator loadScreen(int tmer)
    {
        working = true;
        //Initialize the search bar
        foreach (GameObject gm in bar_list)
        {
            gm.SetActive(false);
        }

        //These variables control the flow of the bar animation
        int bar_progress = 0;
        int next_bar = 0;
        bool setting = true;

        int timer = tmer;
        while (timer > 0)
        {
            --timer;

            bar_progress += 1;

            //Every 5 frames, advance the animation
            if (bar_progress % 5 == 0)
            {
                bar_list[next_bar].SetActive(setting);
                next_bar += 1;
                if (next_bar >= bar_list.Count)
                {
                    next_bar = 0;
                    bar_progress = 0;
                    setting = !setting;
                }
            }

            yield return null;

        }

        image.sprite = found_sprite;
        foreach (GameObject gm in bar_list)
        {
            gm.SetActive(false);
        }

        for (int w = 0;w < 200; w++)
        {
            yield return null;
        }
        working = false;

        yield break;
    }
}

