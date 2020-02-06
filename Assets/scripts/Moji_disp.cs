﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Moji_disp : MonoBehaviour {
    public GameObject sora, doko,over;
    public Carmain cm;
    public MakeRoad mk;
    public Text all;
    public TextMeshProUGUI tmp;
    public int pinturn;
    public int alltime;
    public bool free, dbp, auto;
    bool disp;
    AudioSource ads;

    Rigidbody rb;

    public int i;
    int time;

	// Use this for initialization
	void Start () {
        ads = GetComponent<AudioSource>();
        alltime = 0;
        time = 40;
        i = 0;
        if(!free)
            tmp.text = time.ToString();
        InvokeRepeating("TimeCount", 4, 1);
        if (PlayerPrefs.GetInt("etext") == 1) disp = true; else disp = false;

        rb = cm.gameObject.GetComponent<Rigidbody>();
    }
	
    void TimeCount()
    {
        alltime++;
        all.text = "TOTAL:"+alltime.ToString();
        time--;
        if (time == -1 && dbp && !free)
        {
            GameObject start = GameObject.Find("Start");
            StartCtrl stc = start.GetComponent<StartCtrl>();
            GameObject rcar = stc.atc.gameObject;
            stc.atc.enabled = false;

            CancelInvoke("TimeCount");
            cm.TIme_Up();
            cm.enabled = false;
            int c;
            c = PlayerPrefs.GetInt("money");

            if (cm.transform.position.y < rcar.transform.position.y)
            {//win
                c += 1000;
                stc.count.text = "win";
            }
            else
            {
                c += 100;
                stc.count.text = "lose";
            }

            PlayerPrefs.SetInt("money", c);
        }
        else if (!free && time == -1)
        {
            CancelInvoke("TimeCount");
            cm.TIme_Up();
            cm.enabled = false;
            int c;
            c = PlayerPrefs.GetInt("money");
            c += alltime;
            PlayerPrefs.SetInt("money", c);
            if(!dbp)
                naichilab.RankingLoader.Instance.SendScoreAndShowRanking(alltime);
        }
        else
        {
            if (!free)
                tmp.text = time.ToString();
        }
    }


    void Setoffsora()
    { 
        sora.SetActive(false);
    }
    void Setoffdoko()
    {
        doko.SetActive(false);
    }


    private void OnTriggerStay(Collider other)
    {
        if (disp)
        {
            if (other.gameObject.tag == "Sora")
            {
                sora.SetActive(true);
                CancelInvoke("Setoffsora");
                Invoke("Setoffsora", 0.8f);
            }
            if (other.gameObject.tag == "Doko")
            {
                doko.SetActive(true);
                CancelInvoke("Setoffdoko");
                Invoke("Setoffdoko", 0.5f);
            }
        }
        if(other.gameObject.tag == "mizo")
        {
            Vector3 vec = other.gameObject.transform.position - transform.position;
            float a = Vector3.Cross(vec, cm.gameObject.transform.forward).y;
            rb.AddForce(cm.transform.right*(a<0 ? 1:-1)*cm.speed*0.25f);
            Debug.Log(cm.speed * 0.25f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Check")
        {
            if (i < 3)
                time += 21 - i*2;
            else
                time += 15;
            i++;
            time += pinturn;
            if (pinturn > 1)
                time++;

            mk.SetRoad();
            ads.Play();
            if(!free)
                tmp.text = time.ToString();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Check")
        {
            if (!dbp)
            {
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;
                other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                mk.Destroy_Road();
            }
               
            if(!auto)
                other.gameObject.tag = "Untagged";
        }
    }
}
