using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NewFileName : MonoBehaviour {

    public ExportData manager;

    public GameObject file_text;

    public GameObject error_text;

    public GameObject pop_up;

    void Start()
    {
        pop_up.SetActive(true);

        manager = GameObject.Find("GameManager").GetComponent<ExportData>();

        if (pop_up == null)
        {
            pop_up = GameObject.Find("ErrorPopUp");
        }
        file_text = GameObject.FindWithTag("FileText");

        error_text = GameObject.FindWithTag("ErrorText");

        pop_up.SetActive(false);
    }

	public void OnClick()
    {
        Text error = error_text.GetComponent<Text>();

        Text name = file_text.GetComponent<Text>();

        string namestr = name.text;


        // check for illegal characters
        if(namestr.Contains("[") || namestr.Contains("]") || namestr.Contains("\\") || namestr.Contains("/") || namestr.Contains("|")
            || namestr.Contains("?") || namestr.Contains(",") || namestr.Contains(":") || namestr.Contains("*") || namestr.Contains("\""))
        {
            error.text = "File Name contains an illegal character\n";
            error.text += "File Name cannot contain: \n";
            error.text += "[, ], \\, /, |, ?, ,, :, *, \"";
            pop_up.SetActive(true);
            throw new ArgumentException("File Name contains an illegal character");
        }
        else
        {
            GameObject.Find("Main Camera").GetComponent<MenuScript>().Show_Message();
        }

        // remove any other file types
        if(namestr.Contains("."))
        {
            Debug.Log(namestr.IndexOf("."));
            namestr = namestr.Substring(0, namestr.IndexOf("."));
        }

        // default name if filename is empty
        if(namestr.Length == 0)
        {
            namestr = "results";
        }

        // default name if filename is too large
        if(namestr.Length > 50)
        {
            namestr = "results";
        }

        manager.Get_Filename(namestr + ".csv");
    }

    public void ErrorClick()
    {
        pop_up = GameObject.Find("ErrorPopUp");

        pop_up.SetActive(false);
    }
}
