using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class EventScene : UserStage
{
    [SerializeField]
    TextAsset localdata;

    [SerializeField]
    GameObject errorPanel;

    [SerializeField]
    GameObject[] gameinfo;

    [SerializeField]
    AudioClip crash, driftscore;

    [SerializeField]
    Text scoreText,period;

    [SerializeField]
    Transform tofuicon, pointicon;
    [SerializeField]
    GameObject startButton;

    string stagedata;

    public static int tofu;
    public static float driftPoint;

    [SerializeField]
    AudioSource ads;


    int scoretmp;
    int eventType;

    int t = 0;
    Vector3 iconpos;

    private void Start()
    {
        if (Title.DAY == 0)
            SceneManager.LoadScene("title");

        err = false;
        ltext = laptext;
        scale = 1;

        naichilab.RankingSceneManager.eventRankingData = true;

        period.text = "~" + Title.YEAR.ToString() + "/" + Title.MONTH.ToString() + "/" + ((Title.DAY < 16) ? 10 : 25).ToString();

        eventType = EventType();
        if (eventType == 0)
        {
            scoretmp = 100;
            tofu = 100;
            scoreText.text = "100/100";
            InvokeRepeating("TofuScoreUpdate", 0.1f, 0.1f);
            tofuicon.gameObject.SetActive(true);
        }
        else if(eventType == 1)
        {
            scoretmp = 0;
            driftPoint = 0;
            scoreText.text = "0";
            InvokeRepeating("DriftScoreUpdate", 0.1f, 0.2f);
            pointicon.gameObject.SetActive(true);
            iconpos = pointicon.transform.position;
        }
        else if(eventType == 2)
        {

        }
        gameinfo[eventType].SetActive(true);

        if (IsEventDay() == 2)
        {
            startButton.SetActive(false);
            int score = 0;
            if (PlayerPrefs.HasKey(EventName()))
            {
                score = PlayerPrefs.GetInt("local" + EventName());
            }
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score, 2);
        }
    }

    void TofuScoreUpdate()
    {
        int ns = tofu;
        if(ns != scoretmp)
        {
            ads.PlayOneShot(crash);
            scoretmp = ns;
            scoreText.text = ns.ToString() + "/100";
            InvokeRepeating("ShakeTofu", 0, 0.05f);
        }
    }
    void ShakeTofu()
    {
        float r = Random.Range(-30.0f, 30.0f);
        tofuicon.eulerAngles = new Vector3(0, 0, r);
        t++;
        if (t > 11)
        {
            t = 0;
            CancelInvoke("ShakeTofu");
            tofuicon.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    void DriftScoreUpdate()
    {
        int ns = (int)driftPoint;
        if (ns != scoretmp)
        {
            ads.PlayOneShot(driftscore);
            scoretmp = ns;
            scoreText.text = ns.ToString();
            InvokeRepeating("PointIcon", 0, 0.01f);
            pointicon.transform.position = -new Vector3(0, 20, 0) + iconpos;
        }
    }
    void PointIcon()
    {
        pointicon.transform.position += new Vector3(0, 2, 0);
        t++;
        if (t > 9)
        {
            CancelInvoke("PointIcon");
            t = 0;
            pointicon.position = iconpos;
        }
    }


    public void StartEvent()
    {
        Invoke("TimeOut",20);

        if (localdata != null)
        {
            stagedata = localdata.text;
            GameStart();
            CancelInvoke("TimeOut");
        }
        else
        {
            StartCoroutine(LoadEventStage("https://ryuukun.web.fc2.com/otegaru/eventstagedata.txt"));
        }
    }

    public void DisplayRanking()
    {
        int score = 0;
        if (PlayerPrefs.HasKey(EventName()))
        {
            score = PlayerPrefs.GetInt("local" + EventName());
        }

        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score, 2);

    }

        IEnumerator LoadEventStage(string uri)
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                //Debug.Log(pages[page] + ":\nServerData Received: " + webRequest.downloadHandler.text);
                stagedata = webRequest.downloadHandler.text;
                GameStart();
                CancelInvoke("TimeOut");
            }
        }
    }

    void TimeOut()
    {
        errorPanel.SetActive(true);
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

    public static void EventGameOver()
    {

        int score = 0;
        if (PlayerPrefs.HasKey(EventName()))
        {
            score = PlayerPrefs.GetInt("local"+EventName());
        }

        if (EventType() == 0)
        {
            score += tofu;
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + tofu);
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score, 2);
        }
        else if (EventType() == 1)
        {
            score += (int)driftPoint;
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + (int)driftPoint);
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score, 2);
        }
        else if(EventType() == 2)
        {
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score+1, 2);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 100);
        }
    }


    public static int IsEventDay()
    {
        //1:on event
        //2:result only
        
        if(Title.DAY<11 || (Title.DAY>15 && Title.DAY < 26))
        {
            return 1;
        }
        else
        {
            //return 1;
            return 2;
        }
    }

    public static int EventType()
    {
        //0:tofu transport
        //1:drift charenge
        //2:Number of drive
        int[] e = { 0, 2, 1, 0, 2, 1, 0, 2, 1, 0, 2, 1};
        int index = Title.MONTH * 2 + ((Title.DAY < 16) ? -1 : 0);
        return e[index-1];
    }

    public static string EventName()
    {
        string term;
        term = (Title.DAY < 16) ? "a" : "b";
        return "event" + Title.YEAR.ToString() + Title.MONTH.ToString() + term;
    }

    public void ReloadButton()
    {
        SceneManager.LoadScene("EventScene");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("title");
    }

}
