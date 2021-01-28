using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;


public class EventResult : MonoBehaviour
{
    [SerializeField]
    GameObject awardPanel;
    [SerializeField]
    Text award;

    private InterstitialAd interstitial;


    // Start is called before the first frame update
    void Start()
    {
        //ad load==========================================
        RequestInterstitial();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
    }


    public void GiveAward(int rank)
    {
        if (EventScene.IsEventDay() != 2) return;

        if (PlayerPrefs.HasKey(EventScene.EventName() + "award")) return;

        if (rank == -1 || rank > 5)
        {
            //参加賞
            award.text = "Congratulations!\n参加賞\n" + "+100coin";
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 100);
            PlayerPrefs.SetInt(EventScene.EventName() + "award", 1);
            awardPanel.SetActive(true);
        }
        else if (rank == 1)
        {
            //1st
            award.text = "Congratulations!\n1位\n" + "+1000coin";
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 1000);
            PlayerPrefs.SetInt(EventScene.EventName() + "award", 1);
            awardPanel.SetActive(true);
        }
        else if (rank <= 5)
        {
            //semi winner
            award.text = "Congratulations!\n"+rank.ToString()+"位\n" + "+500coin";
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 500);
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
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        SceneManager.LoadScene("EventScene");

    }
}
