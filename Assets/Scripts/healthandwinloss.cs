using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthandwinloss : MonoBehaviour {

    public Text healthText;
    public Text endText;
    public int health;

	// Use this for initialization
	void Start () {
        health = 100;
        healthText.text = "Health: " + health;
        endText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        CheckLose();
	}

    void CheckLose()
    {
        healthText.text = "Health: " + health;
        if (health == 0)
        {
            endText.text = "You have lost all your health!";
        }
    }
}
