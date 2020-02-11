using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Networking;


public class Title : MonoBehaviour {
    public GameObject howto,dpd,timep,loadtext,hill;
    int game;

    [SerializeField]
    GameObject cmas,maincam,logb;
    private InterstitialAd interstitial;


    private void Start()
    {
        //ad load==========================================
        RequestInterstitial();
        //=================================================
        //is save data here
        //if there is not date ,set value.
        if (!PlayerPrefs.HasKey("money"))
        {
            PlayerPrefs.SetInt("money", 1000);
            Howto();
        }
        if (!PlayerPrefs.HasKey("dcar"))
            PlayerPrefs.SetInt("dcar", 0);
        if (!PlayerPrefs.HasKey("gcar"))
            PlayerPrefs.SetInt("gcar", 1);
        if (!PlayerPrefs.HasKey("bgm"))
            PlayerPrefs.SetInt("bgm", 1);
        if (!PlayerPrefs.HasKey("etext"))
            PlayerPrefs.SetInt("etext", 1);
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
            PlayerPrefs.SetFloat("sev", 1.0f);
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
        StartCoroutine(GetText());

    }

    public void MainGame()
    {
        //SceneManager.LoadScene("main");
        game = 0;
        SetTime();
    }

    public void Free()
    {
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


    public void TechTubu()
    {
        Application.OpenURL("https://youtu.be/uJ6LTdyRyyQ");
    }

    public void newswiki()
    {
        Application.OpenURL("https://otegarut.wiki.fc2.com/wiki/update%20news");
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
        UnityWebRequest request = UnityWebRequest.Get("https://ntp-a1.nict.go.jp/cgi-bin/ntp");
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
        int start = text.IndexOf("<BODY>") + 6;
        int end = text.IndexOf("</BODY>") - 1;

        if (float.TryParse(text.Substring(start, end - start + 1), out t))
        {
            DateTime now = new DateTime(1900, 1, 1).AddMilliseconds(t*1000).ToLocalTime();
            Debug.Log(now);
            TimeEvent(now);
        }

    }

    private void TimeEvent(DateTime today)
    {
        if(today.Month == 12 && today.Day >= 20 && today.Day < 28)
        {
            StartCoroutine(CmasSound());
            Instantiate(cmas);
            maincam.SetActive(false);
        }
        if(!PlayerPrefs.HasKey("login") || !PlayerPrefs.GetString("login").Equals(today.Month.ToString() + today.Day.ToString()))
        {
            int c = PlayerPrefs.GetInt("money");
            PlayerPrefs.SetInt("money", c + 50);
            PlayerPrefs.SetString("login", today.Month.ToString() + today.Day.ToString());
            logb.SetActive(true);
        }
    }

    private IEnumerator CmasSound()
    {


        string url = "http://ryuukun.at-ninja.jp/wewish.mp3";

        WWW www = new WWW(url);
        yield return www;

        AudioSource audios = GetComponent<AudioSource>();
        audios = GetComponent<AudioSource>();
        audios.clip = www.GetAudioClip(false, true);//二つ目の引数がtureで読込中の再生可能
        audios.Play();

    }

    public void CloseLogB()
    {
        logb.SetActive(false);
    }
}
