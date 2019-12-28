using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleStart : MonoBehaviour {
    public GameObject[] car1;
    public GameObject[] car2;
    public GameObject starter;
    public int n;
    int a,b;

	// Use this for initialization
	void Start () {
        a = 3;
        b = 3;
        car1[a].SetActive(true);
        car2[b].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D))
            PlayerB(1);
        else if (Input.GetKeyDown(KeyCode.A))
            PlayerB(0);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            PlayerA(1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            PlayerA(0);

    }

    public void PlayerA(int x)
    {
        car1[a].SetActive(false);
        if (x > 0)
        {
            if (a == n)
                a = 0;
            else
                a++;
        }
        else
        {
            if (a == 0)
                a = n;
            else
                a--;
        }
        car1[a].SetActive(true);
    }

    public void PlayerB(int x)
    {
        car2[b].SetActive(false);
        if (x > 0)
        {
            if (b == n)
                b = 0;
            else
                b++;
        }
        else
        {
            if (b == 0)
                b = n;
            else
                b--;
        }
        car2[b].SetActive(true);
    }

    public void Decide()
    {
        car1[a].GetComponent<Carmain>().enabled = true;
        car1[a].GetComponent<SmcamSel>().enabled = true;
        car2[b].GetComponent<Carmain>().enabled = true;
        car2[b].GetComponent<Carmain_2p>().enabled = true;
        starter.SetActive(false);
        GetComponent<DoubleStart>().enabled = false;
    }

}
