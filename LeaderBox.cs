using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBox : MonoBehaviour {

    Manager man;

    public InputField inp;

	// Use this for initialization
	void Start () {
        man = GameObject.Find("GameManager").GetComponent<Manager>();

	}
	
    public void EnterUsername()
    {
        man.usrname = inp.text;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
