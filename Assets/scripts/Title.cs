using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Title : MonoBehaviour {
    public GameObject howto,dpd,timep,loadtext,hill,credits;
    int game;

    [SerializeField]
    GameObject cmas,maincam,logb;
    private InterstitialAd interstitial;
    [SerializeField]
    GameObject policypanel;
    [SerializeField]
    int policy_ver;

    [SerializeField]
    GameObject[] rankingobj;

    [SerializeField]
    GameObject eventButton;

    [SerializeField]
    Text eventText;

    [SerializeField]
    Sprite[] eventbanner;

    [SerializeField]
    bool samplegame;
    [SerializeField]
    GameObject buyfullPanel;

    public static int timeCheck = 0;

    public static bool timeReload = false;

    public static int YEAR,MONTH, DAY;


    private void Start()
    {
        //ad load==========================================
        RequestInterstitial();
        //=================================================
        //is save data here
        //if there is not date ,set value.
        if (!PlayerPrefs.HasKey("money"))
        {
            PlayerPrefs.SetInt("money", 2000);
            Howto();
        }
        if (!PlayerPrefs.HasKey("dcar"))
            PlayerPrefs.SetInt("dcar", 0);
        if (!PlayerPrefs.HasKey("gcar"))
            PlayerPrefs.SetInt("gcar", 1);
        if (!PlayerPrefs.HasKey("bgm"))
            PlayerPrefs.SetInt("bgm", 1);
        if (!PlayerPrefs.HasKey("etext"))
#if UNITY_ANDROID
            PlayerPrefs.SetInt("etext", 1);
#elif UNITY_IPHONE
            PlayerPrefs.SetInt("etext", 0);
#else
            PlayerPrefs.SetInt("etext", 1);
#endif
        if (!PlayerPrefs.HasKey("accont"))
            PlayerPrefs.SetInt("accont", 0);
        if (!PlayerPrefs.HasKey("trlevel"))
            PlayerPrefs.SetFloat("trlevel", 1.2f);
        if (!PlayerPrefs.HasKey("cpstren"))
            PlayerPrefs.SetFloat("cpstren", 0.8f);
        if (!PlayerPrefs.HasKey("time"))
            PlayerPrefs.SetFloat("time", 2);
        if (!PlayerPrefs.HasKey("mir"))
            PlayerPrefs.SetInt("mir", 0);
        if (!PlayerPrefs.HasKey("sev"))
            PlayerPrefs.SetFloat("sev", 0.5f);
        if (!PlayerPrefs.HasKey("adblock"))
            PlayerPrefs.SetInt("adblock", 0);
        if (!PlayerPrefs.HasKey("gem"))
            PlayerPrefs.SetInt("gem", 0);
        
#if UNITY_STANDALONE_WIN
        if (!PlayerPrefs.HasKey("controller"))
            PlayerPrefs.SetInt("controller", 1);
        if(!samplegame)
            PlayerPrefs.SetInt("money", 1000000);

#endif

        if (PlayerPrefs.HasKey("policy"))
        {
            int s = PlayerPrefs.GetInt("policy");
            if (s != policy_ver)
                policypanel.SetActive(true);
        }
        else
            policypanel.SetActive(true);
        /*
         name   dcar    gcar
         
         86     0       1
         eg6    1       2
         gtr    2       4
         evo    3       8
         fd     4       16
         fc     5       32
         s2k
         s13
         gc8
         */

        //resolution change
        /*
        float screenRate = (float)100 / Screen.height;
        if (screenRate > 1) screenRate = 1;
        int width = (int)(Screen.width * screenRate);
        int height = (int)(Screen.height * screenRate);

        Screen.SetResolution(width, height, true, 15);
        */

        //Check Day
        if (timeCheck < 1)
        {
            StartCoroutine(GetText());
            timeCheck = 5;
        }
        else
        {
            timeCheck--;
            //event check
            StartCoroutine(CheckEvent("https://ryuukun.web.fc2.com/otegaru/event.txt"));
        }

        naichilab.RankingSceneManager.eventRankingData = false;

        if (timeReload)
        {
            Loading();
            naichilab.RankingSceneManager.eventRankingData = true;
            timeReload = false;
            SceneManager.LoadScene("EventScene");
        }
    }

    /*
    private void Update()
    {
        MONTH = 3;
        DAY = 12;
        Debug.LogError("update");
    }*/
    

    public void MainGame()
    {
        //SceneManager.LoadScene("main");
        game = 0;
        SetTime();
    }

    public void Free()
    {
        if (samplegame) { BuyFullVer(true); return; }
        //SceneManager.LoadScene("free");
        game = 1;
        //climb test
        hill.SetActive(true);

        
    }

    public void hillclimb()
    {
        game = 3;
        hill.SetActive(false);
        SetTime();
    }
    public void downhill()
    {
        game = 1;
        hill.SetActive(false);
        SetTime();
    }

    public void loadstage()
    {
        if (samplegame) { BuyFullVer(true); return; }
        game = 4;
        SetTime();
    }

    public void Howto()
    {
        howto.SetActive(true);
    }

    public void HowtoC()
    {
        howto.SetActive(false);
    }

    public void Credits()
    {
        credits.SetActive(true);
    }
    public void CreditsClose()
    {
        credits.SetActive(false);
    }

    public void DPDisp()
    {
        dpd.SetActive(true);
    }
    public void DPDC()
    {
        dpd.SetActive(false);
    }
    public void DPStart()
    {
        SceneManager.LoadScene("double");
    }
    public void GoGarage()
    {
        SceneManager.LoadScene("garage");
    }
    public void vs()
    {
        if (samplegame) { BuyFullVer(true); return; }
        //SceneManager.LoadScene("vs");
        game = 2;
        SetTime();
    }

    void SetTime()
    {
        timep.SetActive(true);
    }

    public void SetTimeClose()
    {
        timep.SetActive(false);
    }

    public void Day() { PlayerPrefs.SetInt("time", 0); GameStart(); }
    public void SunSet() { PlayerPrefs.SetInt("time", 1); GameStart(); }
    public void Night() { PlayerPrefs.SetInt("time", 2); GameStart(); }

    void GameStart()
    {
        Loading();
        switch (game)
        {
            case 0: SceneManager.LoadScene("main"); break;
            case 1: SceneManager.LoadScene("free"); break;
            case 2: SceneManager.LoadScene("vs"); break;
            case 3: SceneManager.LoadScene("hillclimb");break;
            case 4: SceneManager.LoadScene("loadstage");break;
            default:break;
        }
    }

    void Loading()
    {
        loadtext.SetActive(true);
        //ShowAd();
    }

    public void RealTouge()
    {
        if (samplegame) { BuyFullVer(true);return; }
        Loading();
        SceneManager.LoadScene("RealTouge");
    }

    public void OnlineGame()
    {
        Loading();
        SceneManager.LoadScene("PunBasics-Launcher");
    }

    public void TechTubu()
    {
        Application.OpenURL("https://youtu.be/uJ6LTdyRyyQ");
    }

    public void Option()
    {
        //一時的にGarageのものを使用
        Garage.showOption = true;
        GoGarage();
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void newswiki()
    {
        Application.OpenURL("https://otegarut.wiki.fc2.com/wiki/update%20news");
    }

    public void Buynow()
    {
        Application.OpenURL("https://ryuukunkoubou.booth.pm/items/2732831");
    }

    //ad
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-7102752236968696/4649270801";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    private void ShowAd()
    {
        Debug.Log("showad");
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }

    IEnumerator GetText()
    {
        //UnityWebRequest request = UnityWebRequest.Get("https://ntp-a1.nict.go.jp/cgi-bin/ntp");
        UnityWebRequest request = UnityWebRequest.Get("https://asia-northeast1-otegaru-api.cloudfunctions.net/get_time");
        // 下記でも可
        // UnityWebRequest request = new UnityWebRequest("http://example.com");
        // methodプロパティにメソッドを渡すことで任意のメソッドを利用できるようになった
        // request.method = UnityWebRequest.kHttpVerbGET;

        // リクエスト送信
        yield return request.Send();

        // 通信エラーチェック
        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                // UTF8文字列として取得する
                string text = request.downloadHandler.text;
                Totime(text);

                // バイナリデータとして取得する
                byte[] results = request.downloadHandler.data;
            }
        }
    }

    private void Totime(string text)
    {
        float t;
        /*
        int start = text.IndexOf("<BODY>") + 6;
        int end = text.IndexOf("</BODY>") - 1;
        */
        int start = 8;
        int end = text.Length;

        if (float.TryParse(text.Substring(start, end - start -1), out t))
        {
            DateTime now = new DateTime(1970, 1, 1).AddMilliseconds(t+ 32400000);
            Debug.Log(now);
            TimeEvent(now);

            YEAR = now.Year;
            MONTH = now.Month;
            DAY = now.Day;
        }

        //event check
        StartCoroutine(CheckEvent("https://ryuukun.web.fc2.com/otegaru/event.txt"));

    }

    private void TimeEvent(DateTime today)
    {
        /*C mas Event*/
        if(today.Month == 12 && today.Day >= 20 && today.Day < 28)
        {
            StartCoroutine(CmasSound());
            Instantiate(cmas);
            maincam.SetActive(false);
        }
        /*Login Bonus*/
        if(!PlayerPrefs.HasKey("login") || !PlayerPrefs.GetString("login").Equals(today.Month.ToString() + today.Day.ToString()))
        {
            GetComponent<DataServer>().CheckServerData();

            int g = PlayerPrefs.GetInt("gem");
            PlayerPrefs.SetInt("gem", g + 1);
            PlayerPrefs.SetString("login", today.Month.ToString() + today.Day.ToString());
            logb.SetActive(true);
        }
        /*Online time setter*/
        if (today.Hour >= 6 && today.Hour <= 15)
            PlayerPrefs.SetInt("on_time", 0);
        else if(today.Hour >= 4 && today.Hour < 6 || today.Hour > 15 && today.Hour < 18)
            PlayerPrefs.SetInt("on_time", 1);
        else
            PlayerPrefs.SetInt("on_time", 2);
    }

    private IEnumerator CmasSound()
    {
        string url = "https://ryuukun.at-ninja.jp/wewish.mp3";

        WWW www = new WWW(url);
        yield return www;

        AudioSource audios = GetComponent<AudioSource>();
        audios = GetComponent<AudioSource>();
        audios.clip = www.GetAudioClip(false, true);//二つ目の引数がtureで読込中の再生可能
        audios.Play();

    }

    public void BuyFullVer(bool b)
    {
        buyfullPanel.SetActive(b);
    }

    public void CloseLogB()
    {
        logb.SetActive(false);
    }

    public void OpenPolicy()
    {
        Application.OpenURL("http://ryuukun.web.fc2.com/prcy/ote/");
    }
    public void Agree()
    {
        PlayerPrefs.SetInt("policy", policy_ver);
        policypanel.SetActive(false);
    }
    public void DisAgree()
    {
        
        SceneManager.LoadScene("title");
    }

    public void ButtonOfNewsContent()
    {
        Application.OpenURL("https://github.com/ryk122/OtegaruT");
    }

    public void GitHubLink()
    {
        Application.OpenURL("https://github.com/ryk122/OtegaruT");
    }

    public void DiscordLink()
    {
        Application.OpenURL("https://discord.gg/MN3DfCPMSf");
    }

    public void EventButton()
    {
        naichilab.RankingSceneManager.eventRankingData = true;
        Loading();
        PlayerPrefs.SetInt("time", 2);
        SceneManager.LoadScene("EventScene");
    }

    IEnumerator CheckEvent(string uri)
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
                Debug.Log(pages[page] + ":\nServerData Received: " + webRequest.downloadHandler.text);
                if (webRequest.downloadHandler.text.Equals("0"))
                {
                    //no event
                    eventButton.SetActive(false);
                }
                else
                {
                    if (webRequest.downloadHandler.text.Equals("1"))
                    {
                        eventButton.SetActive(true);
                        eventText.text = "~" + MONTH.ToString() + "/" + ((Title.DAY < 16) ? 10 : 25).ToString();
                        if (EventScene.IsEventDay() == 2)
                            eventText.text = "Result";
                        eventButton.GetComponent<Image>().sprite = eventbanner[EventScene.EventType()];
                        Debug.Log("do event");
                    }
                    else if (webRequest.downloadHandler.text.Equals("2"))
                    {
                        //結果発表期間のみ表示
                        if (EventScene.IsEventDay() == 2)
                        {
                            eventButton.SetActive(true);
                            eventButton.GetComponent<Image>().sprite = eventbanner[EventScene.EventType()];
                            eventText.text = "Result";
                            //eventText.text = "~" + MONTH.ToString() + "/" + ((Title.DAY < 16) ? 10 : 25).ToString();
                        }
                    }
                    else if (webRequest.downloadHandler.text.Equals("3"))
                    {
                        //イベント実施期間のみ表示
                        if (EventScene.IsEventDay() == 1)
                        {
                            eventButton.SetActive(true);
                            eventButton.GetComponent<Image>().sprite = eventbanner[EventScene.EventType()];
                            eventText.text = "~" + MONTH.ToString() + "/" + ((Title.DAY < 16) ? 10 : 25).ToString();
                            Debug.Log("do event");

                        }
                    }
                }
            }
        }
    }


}
