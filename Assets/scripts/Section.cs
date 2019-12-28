using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Section : MonoBehaviour
{
    public static Text laptext;
    float time;
    bool start;
    static float[] laps;
    static int l;
    private AudioSource aus;

    // Start is called before the first frame update
    void Start()
    {
        l = 0;
        start = false;
        time = 0;
        laps = new float[5];
        laptext = UserStage.ltext;
        aus = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            time += Time.deltaTime;
            laps[l] = time;
            //laptext.text = laps[0] + "\n" + laps[1] + "\n" + laps[2] + "\n" + laps[3];
            laptext.text = ToTime(laps[0])+ ToTime(laps[1])+ ToTime(laps[2])+ ToTime(laps[3]) + ToTime(laps[4]);
        }
    }

    string ToTime(float time)
    {
        if (time == 0)
            return null;
        int min, sec, msc;
        min = (int)time / 60;
        sec = (int)time % 60;
        msc = (int)(time*1000 % 1000);

        return min.ToString("D2") + ":" + sec.ToString("D2") + "." + msc.ToString("D3")+"\n";
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Car")//拡張性無視の激やば実装
        {//startctrl問い合わせてプレイヤのcarかの判断すればいけるか・・
            aus.Play();
            start = true;
            time = 0;
            if (l == 4)
                ShiftData();
            else
                l++;

        }
    }

    private void ShiftData()
    {
        for (int i = 1; i < 5; i++)
            laps[i - 1] = laps[i];
    }
}
