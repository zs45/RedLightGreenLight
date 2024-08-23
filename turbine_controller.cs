using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turbine_controller : MonoBehaviour {

    public Animator ani;

    public SpriteRenderer sr;

    public GameObject Wind_Prefab;

    public float wind_speed = 10f;

    AudioManager aman;

    private void Start()
    {
        Hide();
        aman = gameObject.GetComponent<AudioManager>();
    }


    public void Start_Spinning()
    {
        ani.speed = 1;
        ani.SetBool("spinning", true);
        StartCoroutine(Spin());
        aman.playMusic(0);
    }

    //Keep the spin animation going for some time
    IEnumerator Spin()
    {
        for(int t = 0;t < 60; t++) {
            yield return null;
        }
        //Fall back in line
        ani.SetBool("spinning", false);
        yield break;
    }

    IEnumerator Windy()
    {
        GameObject windy_effect_1 = Instantiate(Wind_Prefab);
        GameObject windy_effect_2 = Instantiate(Wind_Prefab);
        GameObject windy_effect_3 = Instantiate(Wind_Prefab);
        windy_effect_1.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 2, 0);
        windy_effect_2.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1, 0);
        windy_effect_3.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 3, 0);

        float ws_1 = wind_speed + Random.Range(-1,15);
        float ws_2 = wind_speed + Random.Range(-1, 15);
        float ws_3 = wind_speed + Random.Range(-1, 15);
        for (int t = 0; t < 70; t++)
        {
            windy_effect_1.transform.Translate(Vector3.down * (ws_1) * Time.deltaTime);
            windy_effect_2.transform.Translate(Vector3.down * (ws_2) * Time.deltaTime);
            windy_effect_3.transform.Translate(Vector3.down * (ws_3) * Time.deltaTime);
            yield return null;
        }
        Destroy(windy_effect_1);
        Destroy(windy_effect_2);
        Destroy(windy_effect_3);
        yield break;

    }

    public void Start_Rising()
    {
        sr.sortingOrder = 1;
        ani.speed = 2;
        StartCoroutine(Windy());
    }

    public void Hide()
    {
        ani.SetBool("spinning", false);
        ani.speed = 0;
        sr.sortingOrder = -11;
    }

}
