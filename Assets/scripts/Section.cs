using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Section : MonoBehaviour
{
    public static Text laptext;
    public static Section nowsection;
    public bool end;
    public string stagename;
    static float time;
    public bool start;
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
            if (end)
            {
                if(nowsection != null) nowsection.start = false;
                start = false;
                time = 0;
                for (int i = l; i >= 0; i--)
                    time += laps[i];
                
                
                aus.Play();
                laptext.text = ToTime(laps[0]) + ToTime(laps[1]) + ToTime(laps[2]) + ToTime(laps[3]) + ToTime(laps[4]);
                laptext.text += "  " + ToTime(time);

                if (PlayerPrefs.HasKey(stagename))
                {
                    if (PlayerPrefs.GetFloat(stagename) > time)
                        PlayerPrefs.SetFloat(stagename, time);
                }
                else
                {
                    PlayerPrefs.SetFloat(stagename, time);
                }
                Destroy(this);
            }
            else
            {
                if (nowsection != null) nowsection.start = false;
                nowsection = this;
                aus.Play();
                start = true;
                time = 0;
                if (l == 4)
                    ShiftData();
                else
                    l++;
            }

        }
    }

    private void ShiftData()
    {
        for (int i = 1; i < 5; i++)
            laps[i - 1] = laps[i];
    }
}
