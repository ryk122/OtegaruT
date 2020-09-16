using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HitChecker : MonoBehaviour {
    public GameObject sora, doko,over,mizor,mizol;
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

        float vol = PlayerPrefs.GetFloat("sev");
        ads.volume = vol;
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
                GetExp(100, PlayerPrefs.GetInt("dcar"));
            }
            else
            {
                c += 100;
                stc.count.text = "lose";
                GetExp(10, PlayerPrefs.GetInt("dcar"));
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
                naichilab.RankingLoader.Instance.SendScoreAndShowRanking(alltime,0);

            GetExp(alltime, PlayerPrefs.GetInt("dcar"));
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
            if (other.CompareTag("Sora"))
            {
                sora.SetActive(true);
                CancelInvoke("Setoffsora");
                Invoke("Setoffsora", 0.8f);
            }
            if (other.CompareTag("Doko"))
            {
                doko.SetActive(true);
                CancelInvoke("Setoffdoko");
                Invoke("Setoffdoko", 0.5f);
            }
        }
        if (other.CompareTag("mizo"))
        {
            Vector3 vec = other.gameObject.transform.position - cm.transform.position;
            float a = Vector3.Cross(vec, cm.gameObject.transform.forward).y;
            if (a < 0)
            {
                cm.mizo = 1;
                if (disp)
                    mizor.SetActive(true);
            }
            else
            {
                cm.mizo = -1;
                if (disp)
                    mizol.SetActive(true);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Check")
        if(other.CompareTag("Check"))
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

        if (other.CompareTag("wall"))
        {
            //Debug.Log("wall");
            //壁衝突中は、溝効果0
            cm.mizo = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == "Check")
        if (other.CompareTag("Check"))
        {
            if (!dbp)
            {
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;
                other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                mk.Destroy_Road();
            }

            if (!auto)
                other.gameObject.tag = "Untagged";
        }
        if (other.CompareTag("mizo"))
        {
            cm.mizo = 0;
            mizor.SetActive(false);
            mizol.SetActive(false);
        }
    }

    static private void GetExp(int exp,int dcar)
    {
        //carlev:level carexp now
        //int dcar = PlayerPrefs.GetInt("dcar");

        if (!PlayerPrefs.HasKey("carlev" + dcar))
        {
            PlayerPrefs.SetInt("carlev" + dcar, 1);
        }
        int carlevel = PlayerPrefs.GetInt("carlev" + dcar);
        int carexp = PlayerPrefs.GetInt("carexp" + dcar);

        carexp += exp;

        int thisLevelExp = (int)Mathf.Pow(carlevel, 1.2f)*100;

        while(carexp >= thisLevelExp)
        {
            carexp -= thisLevelExp;
            carlevel++;
            thisLevelExp = (int)Mathf.Pow(carlevel, 1.2f) * 100;
        }

        PlayerPrefs.SetInt("carlev" + dcar, carlevel);
        PlayerPrefs.SetInt("carexp" + dcar, carexp);
     
    }
}
