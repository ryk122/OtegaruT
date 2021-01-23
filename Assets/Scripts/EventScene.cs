using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventScene : UserStage
{
    [SerializeField]
    TextAsset haruna, akagi;


    string stagedata;

    private void Start()
    {
        err = false;
        ltext = laptext;
        scale = 1;

    }

    public void Haruna()
    {
        stagedata = haruna.text;
        GameStart();
    }

    public void Akagi()
    {
        stagedata = akagi.text;
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
        else
        {
            Debug.LogWarning("stage data err");
        }
    }


    [System.Serializable]
    public class StageScore
    {
        public string stagename;
        public Text timelabel;

        public void Disp()
        {
            Debug.Log(PlayerPrefs.GetFloat(stagename));
            if (PlayerPrefs.HasKey(stagename))
                timelabel.text = ToTime(PlayerPrefs.GetFloat(stagename));
            else
                timelabel.text = "--:--:---";
        }

        public void DeleteRecord()
        {
            PlayerPrefs.DeleteKey(stagename);
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
