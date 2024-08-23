using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ErrorPopUp : MonoBehaviour {

    public ExportData manager;      // manager

    public GameObject error_text;   // error message text

    public GameObject pop_up;       // error pop up game object

	// Use this for initialization
	void Start () {
		
        manager = GameObject.Find("GameManager").GetComponent<ExportData>();

        if (pop_up == null)
        {
            pop_up = GameObject.Find("ErrorPopUp");
        }

        error_text = GameObject.FindWithTag("ErrorText");

        //error_text = GameObject.FindWithTag("ErrorText");

        //pop_up.SetActive(false);

	}
	
    // when close button is pressed, disable pop up
    public void ErrorClick()
    {
        //pop_up = GameObject.Find("ErrorPopUp");

        pop_up.SetActive(false);

        if (SceneManager.GetActiveScene().name == "test_room")
        {
            SceneManager.LoadScene("Menu");
        }
    }

    // pop up pop up
    public void PopUp(string message)
    {
        //pop_up = GameObject.Find("ErrorPopUp");

        Text error = error_text.GetComponent<Text>();

        error.text = message;

        pop_up.SetActive(true);

    }
}
