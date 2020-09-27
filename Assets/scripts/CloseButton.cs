using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class CloseButton : MonoBehaviour {

    [SerializeField] GameObject stopmenu,cam;
    private InterstitialAd interstitial;

    private bool photomode = false;

    private void Start()
    {
        //ad load==========================================
        RequestInterstitial();
    }

    //ad https://developers.google.com/admob/unity/start?hl=ja
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

    public void Close()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        SceneManager.LoadScene("title");
    }

    public void TempStop()
    {
        if (!photomode)
        {
            stopmenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            CancelPhotoMode();
        }
    }

    public void TakePhotoMode()
    {
        photomode = true;
        stopmenu.SetActive(false);
        cam.GetComponent<UnityStandardAssets.Utility.SmoothFollow>().enabled = false;
        cam.GetComponent<FollowingCamera>().enabled = true;
        cam.GetComponent<FollowingCamera>().target = cam.GetComponent<UnityStandardAssets.Utility.SmoothFollow>().target.gameObject;
    }

    public void CancelPhotoMode()
    {
        photomode = false;
        stopmenu.SetActive(true);
        cam.GetComponent<UnityStandardAssets.Utility.SmoothFollow>().enabled = true;
        cam.GetComponent<FollowingCamera>().enabled = false;
    }

    public void CancelStop()
    {
        Time.timeScale = 1.0f;
        stopmenu.SetActive(false);
    }

    public void CloseLevelRanking()
    {
        SceneManager.LoadScene("garage");
    }
}
