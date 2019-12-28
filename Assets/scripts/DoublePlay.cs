using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoublePlay : MonoBehaviour {
    public Transform car0, car1;
    public Carmain cm;
    public Carmain_2p cm2;
    public TextMeshProUGUI p1, p2,time;
    int t = 180;
	// Use this for initialization
	void Start () {
        InvokeRepeating("Count", 4, 1);
	}

    void Count()
    {
        t--;
        time.text = t.ToString();
        if (t == -1) {
            cm.TIme_Up();
            cm.b = 0;
            cm2.b = 0;
            if (car0.position.y < car1.position.y)
            {
                time.text = "Win  Lose";
            }
            else
            {
                time.text = "Lose  Win";
            }
            CancelInvoke("Count");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (car0.position.y > car1.position.y)
        {
            p1.text = "2";
            p2.text = "1";
        }
        else
        {
            p1.text = "1";
            p2.text = "2";
        }
	}
}
