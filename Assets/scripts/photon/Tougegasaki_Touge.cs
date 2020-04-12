using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tougegasaki_Touge : UserStage
{

    [SerializeField]
    TextAsset tougegasaki;



    string stagedata;

    private void Start()
    {
        err = false;
        ltext = laptext;
        scale = 1;
        Load();

    }

    public void Load()
    {
        stagedata = tougegasaki.text;
        GameStart();
    }


    void GameStart()
    {
        data = stagedata.Split(',');
        SetRoad();
        if (!err)
        {
            Debug.Log("ok");
            /*
            lightb.SetActive(true);
            starttext.SetActive(true);
            strtctrl.enabled = true;
            lcanvas.SetActive(false);
            if (!pc)
                android.SetActive(true);
            */
        }
        else
        {
            Debug.LogWarning("stage data err");
        }
    }


}
