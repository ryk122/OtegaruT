using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;


public class EventResult : MonoBehaviour
{
    [SerializeField]
    GameObject awardPanel,loading;
    [SerializeField]
    Text award;

    //private InterstitialAd interstitial;


    // Start is called before the first frame update
    void Start()
    {
        Title.timeCheck--;

        //ad load==========================================
        //RequestInterstitial();

    }

    /*
    //ad https://developers.google.com/admob/unity/start?hl=ja
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-7102752236968696/4649270801";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-7102752236968696/1553907636";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }*/


    public void GiveAward(int rank)
    {
        if (EventScene.IsEventDay() != 2) return;

        if (PlayerPrefs.HasKey(EventScene.EventName() + "award")) return;

        if (rank == -1 || rank > 5)
        {
            //参加賞
            award.text = "Congratulations!\n参加賞\n" + "+2gem";
            PlayerPrefs.SetInt("gem", PlayerPrefs.GetInt("gem") + 2);
            PlayerPrefs.SetInt(EventScene.EventName() + "award", 1);
            awardPanel.SetActive(true);
        }
        else if (rank == 1)
        {
            //1st
            award.text = "Congratulations!\n1位\n" + "+100gem";
            PlayerPrefs.SetInt("gem", PlayerPrefs.GetInt("gem") + 100);
            PlayerPrefs.SetInt(EventScene.EventName() + "award", 1);
            awardPanel.SetActive(true);
        }
        else if (rank <= 5)
        {
            //semi winner
            award.text = "Congratulations!\n"+rank.ToString()+"位\n" + "+10gem";
            PlayerPrefs.SetInt("gem", PlayerPrefs.GetInt("gem") + 10);
            PlayerPrefs.SetInt(EventScene.EventName() + "award", 1);
            awardPanel.SetActive(true);
        }
    }

    public void CloseAward()
    {
        awardPanel.SetActive(false);

    }

    public void RePlayEvent()
    {
        PreLoadAd.preAd.ShowAd();

        loading.SetActive(true);

        if (Title.timeCheck < 0)
        {
            Title.timeReload = true;
            SceneManager.LoadScene("title");
        }
        else
            SceneManager.LoadScene("EventScene");

    }

    public void CloseEvent()
    {
        PreLoadAd.preAd.ShowAd();
        loading.SetActive(true);
        SceneManager.LoadScene("title");
    }
}
