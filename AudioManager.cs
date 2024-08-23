using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource asrc;

    public List<AudioClip> alist = new List<AudioClip>();

	// Use this for initialization
	void Start () {
		
	}
	
    public void playMusic(int index)
    {
        asrc.clip = alist[index];
        asrc.Play();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
