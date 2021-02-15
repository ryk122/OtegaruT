using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class PreLoadAd : MonoBehaviour
{
    public static PreLoadAd preAd;
    private InterstitialAd interstitial;

    [SerializeField] GameObject[] photodisableObjects;
    private bool photomode = false;


    private GameObject logoInstance;

    private void Start()
    {
        preAd = this;
        //ad load==========================================
        RequestInterstitial();
    }

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

    public void ShowAd()
    {
        int adb = PlayerPrefs.GetInt("adblock");
        if (adb > 0)
        {
            Debug.Log("adblock!");
            interstitial.Show();
            PlayerPrefs.SetInt("adblock", adb - 1);
        }
    }

}
