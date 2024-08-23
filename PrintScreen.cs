using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintScreen : MonoBehaviour {

    public ExportData manager;

    public GameObject mob_text;

	public void OnClick()
    {
        manager = GameObject.Find("GameManager").GetComponent<ExportData>();

        mob_text = GameObject.FindWithTag("MobileText");

        Text text = mob_text.GetComponent<Text>();

        text.text = manager.txt;
    }
}
