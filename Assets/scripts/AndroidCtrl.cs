using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidCtrl : MonoBehaviour {
    public Carmain cm;
    public GameObject b1, b2, b3, b4;
    float trlevel;
    bool r, l, g, b;
    bool accont, controller;
    Vector3 acc;
    float rot;

	// Use this for initialization
	public void Start () {
        if (PlayerPrefs.GetInt("accont") == 1) accont = true; else accont = false;
        if (PlayerPrefs.GetInt("controller") == 1) controller = true; else controller = false;
        if (accont||controller)
        {
            b1.SetActive(false);b2.SetActive(false);
            if (controller)
            {
                b3.SetActive(false);b4.SetActive(false);
            }
        }
        trlevel = PlayerPrefs.GetFloat("trlevel");
        GameObject car = GameObject.Find("car");
        cm = car.GetComponent<Carmain>();
        r = l = g = b = false;
        cm.android = true;
	}

    private void Update()
    {
        if (accont)
        {
            acc = Input.acceleration;
            rot = acc.x * trlevel * 1.5f;
            if (rot > 1) rot = 1;
            else if (rot < -1) rot = -1;
        }

        /*for controll device*/
        if (controller)
        {
            //steer
            rot = Input.GetAxis("Horizontal");


            //up down
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Fire2"))
            {
                g = true;
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetButtonUp("Fire2"))
            {
                g = false;
                cm.Acoff();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetButtonDown("Fire1"))
            {
                b = true;
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetButtonUp("Fire1"))
            {
                b = false;
            }

            //other
            if (Input.GetButtonDown("Fire3"))
            {
                cm.LightOnOff();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (accont||controller)
        {
            if(Mathf.Abs(rot)>0.4)
                cm.TR(rot);
            else
                cm.AndrC();
        }
        else
        {
            if (r)
                cm.TR(1);
            else if (l)
                cm.TR(-1);
            else
            {
                cm.AndrC();
            }
        }


        if (g)
            cm.Run();
        else if (b)
        {
            cm.back = -1;
            cm.Back();
        }
        else
        {
            cm.N();

        }

	}

    public void RightD()
    {
        r = true;
    }
    public void LeftD()
    {
        l = true;
    }
    public void RunD()
    {
        g = true;
    }
    public void BackD()
    {
        b = true;
    }
    public void RightU()
    {
        r = false;
    }
    public void LeftU()
    {
        l = false;
    }
    public void RunU()
    {
        g = false;
        cm.Acoff();
    }
    public void BackU()
    {
        b = false;
    }

}
