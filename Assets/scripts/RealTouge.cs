using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealTouge : UserStage
{
    [SerializeField]
    TextAsset haruna;
    [SerializeField]
    GameObject haruna_objects;
    [SerializeField]
    Image waku;
    [SerializeField]
    Button b1, b2, b3;

    [SerializeField]
    public StageScore[] ssc;

    string stagedata;

    private void Start()
    {
        TimeWaku(PlayerPrefs.GetInt("time"));
        err = false;
        ltext = laptext;
        scale = 1;

        foreach(StageScore s in ssc){
            s.Disp();
        }
    }

    public void Haruna()
    {
        stagedata = haruna.text;
        GameObject h = Instantiate(haruna_objects);
        h.transform.position = new Vector3(0, 0, 0);
        GameStart();
    }

    void GameStart()
    {
        data = stagedata.Split(',');
        SetRoad();
        if (!err)
        {
            Debug.Log("ok");
            lightb.SetActive(true);
            starttext.SetActive(true);
            strtctrl.enabled = true;
            lcanvas.SetActive(false);
            if (!pc)
                android.SetActive(true);
        }
    }

    public void TimeWaku(int t)
    {
        Vector3 pos = waku.rectTransform.position;
        switch (t)
        {
            case 0: pos.x = b1.transform.position.x; break;
            case 1: pos.x = b2.transform.position.x; break;
            case 2: pos.x = b3.transform.position.x; break;
        }
        waku.rectTransform.position = pos;
    }

    [System.Serializable]
    public class StageScore{
        public string stagename;
        public Text timelabel;
        
        public void Disp()
        {
            if (PlayerPrefs.HasKey(stagename))
                timelabel.text = ToTime(PlayerPrefs.GetFloat(stagename));
            else
                timelabel.text = "--:--:---";
        }

        string ToTime(float time)
        {
            if (time == 0)
                return null;
            int min, sec, msc;
            min = (int)time / 60;
            sec = (int)time % 60;
            msc = (int)(time * 1000 % 1000);

            return min.ToString("D2") + ":" + sec.ToString("D2") + "." + msc.ToString("D3") + "\n";
        }
    }

    
}
