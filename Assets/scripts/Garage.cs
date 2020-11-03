using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using GoogleMobileAds.Api;

public class Garage : MonoBehaviour {
    public int dcar;
    int max = 6;
    public GameObject[] car;
    public TextMeshProUGUI coin;
    public Text buttontex,value,strengthvalue,sevol,levellabel;
    public Slider slider,slider2,slider3,expslider;
    public GameObject ad,opt;
    public Toggle toggle_b,toggle_e,toggle_a,toggle_m;
    bool having,bgm,effect,accont,mir;
    int unitytime, sm, ss ,mm,ms;
    int sw, sh, tuneTabState;

    [SerializeField]
    GarageTune gt;
    [SerializeField]
    GameObject tunebt, tunepl, polishpl, opbt, rb, lb, picker, colorbt;
    [SerializeField]
    Dropdown drop;
    [SerializeField]
    GarageColorSetter gcsetter;
    [SerializeField]
    GarageColorSetter gcs;
    [SerializeField]
    ParticleSystem polisheffect;

    [SerializeField]
    GameObject[] TuneContents;

    Transform carobj;

    private RewardedAd rewardedAd;

    const int TUNE_TAB = 3;


    //ひどい変数
    public static bool showOption=false;


    // Use this for initialization
    void Start () {
        max = car.Length -1;
        dcar = PlayerPrefs.GetInt("dcar");
        if(dcar== car.Length)//bug fix
        {
            dcar = 0;
            PlayerPrefs.SetInt("dcar", 0);
        }
        if (PlayerPrefs.GetInt("bgm") == 1) bgm = true; else bgm = false;
        if (PlayerPrefs.GetInt("etext") == 1) effect = true; else effect = false;
        if (PlayerPrefs.GetInt("accont") == 1) accont = true; else accont = false;
        if (PlayerPrefs.GetInt("mir") == 1) mir = true; else mir = false;
        slider.value = PlayerPrefs.GetFloat("trlevel");
        slider2.value = PlayerPrefs.GetFloat("cpstren");
        slider3.value = PlayerPrefs.GetFloat("sev");
        toggle_b.isOn = bgm;
        toggle_e.isOn = effect;
        toggle_a.isOn = accont;
        toggle_m.isOn = mir;
        car[dcar].SetActive(true);
        carobj = car[dcar].transform;
        having = true;
        DispCoin(PlayerPrefs.GetInt("money"));
        sw = Screen.width;
        sh = Screen.height;

        gt.ts = carobj.GetComponent<TuneSetter>();

        OnOffTune();
        DispCarLevel(dcar);

        gcsetter.GCStart(dcar, gt.ts);//g-color setterにtsとdcar伝達

        //オプションを表示
        if (showOption)
        {
            Option();
            showOption = false;
        }

        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-7102752236968696/8032259842";
#elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        adUnitId = "unexpected_platform";
#endif
        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    // Update is called once per frame
    void Update () {
        int v = (int)(slider.value * 10);
        slider.value = ((float)v) / 10;
        value.text = slider.value.ToString();
        sevol.text = slider3.value.ToString();

        v = (int)(slider2.value * 10);
        slider2.value = ((float)v) / 10;
        strengthvalue.text = slider2.value.ToString();

        Vector3 rot = carobj.eulerAngles;
        rot.y += Time.deltaTime * 20;
        carobj.eulerAngles = rot;
	}

    public void ChangeR()
    {
        car[dcar].SetActive(false);
        if (dcar == max)
            dcar = 0;
        else
            dcar++;
        ChangeCar();
    }
    public void ChangeL()
    {
        car[dcar].SetActive(false);
        if (dcar == 0)
            dcar = max;
        else
            dcar--;
        ChangeCar();
    }

    void ChangeCar()
    {
        int gc,i,p;
        gc = PlayerPrefs.GetInt("gcar");
        p = 1;
        for (i = 0; i < dcar; i++)
            p *= 2;
        if ((gc & p) != p)
        {
            having = false;
            buttontex.text = "Get:1000coin";
        }
        else
        {
            having = true;
            buttontex.text = "OK";
        }

        car[dcar].SetActive(true);
        carobj = car[dcar].transform;
        gt.ts = carobj.GetComponent<TuneSetter>();//tune対象を伝達
        gcsetter.GCStart(dcar, gt.ts);//g-color setterにtsとdcar伝達
        OnOffTune();
        DispCarLevel(dcar);
    }


    public void OKButton()
    {
        if (having)
        {
            PlayerPrefs.SetInt("dcar", dcar);
            Debug.Log("save:" + dcar);
            SceneManager.LoadScene("title");
        }
        else
        {
            int c;
            c = PlayerPrefs.GetInt("money");
            if (c >= 1000)
            {
                int gc, i, p;
                gc = PlayerPrefs.GetInt("gcar");
                p = 1;
                for (i = 0; i < dcar; i++)
                    p *= 2;
                gc += p;
                PlayerPrefs.SetInt("gcar", gc);
                having = true;
                c -= 1000;
                PlayerPrefs.SetInt("money", c);
                DispCoin(c);
                ChangeCar();
            }
        }
    }

    public void Nochange()
    {
        SceneManager.LoadScene("title");
    }

    public void DispCoin(int c)
    {
        //int c;
        //c = PlayerPrefs.GetInt("money");
        coin.text = "coin:" + c.ToString(); ;
    }

    private void DispCarLevel(int carnum)
    {
        if (!PlayerPrefs.HasKey("carlev" + carnum))
        {
            PlayerPrefs.SetInt("carlev" + carnum, 1);
        }
        int carlevel = PlayerPrefs.GetInt("carlev" + carnum);
        int thisLevelExp = (int)Mathf.Pow(carlevel, 1.2f) * 100;
        expslider.maxValue = thisLevelExp;
        expslider.value = PlayerPrefs.GetInt("carexp" + carnum);
        levellabel.text = "Level " + carlevel;
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        int c;
        c = PlayerPrefs.GetInt("money");
        c += 100;
        PlayerPrefs.SetInt("money", c);
        DispCoin(c);
    }

    public void CoinGet()
    {
        //ad.SetActive(true);
        if (this.rewardedAd.IsLoaded())
        {
            ad.SetActive(false);
            this.rewardedAd.Show();
        }
    }

    public void Close()
    {
        ad.SetActive(false);
        CancelInvoke("TimeCount");
    }

    public void Option()
    {
        opt.SetActive(true);
    }

    public void LevelRankingButton()
    {
        int highlevel = 1;
        for(int i = 0; i < car.Length; i++)
        {
            int carlevel = PlayerPrefs.GetInt("carlev" + i);
            if (carlevel > highlevel)
                highlevel = carlevel;
        }
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(highlevel, 1);
    }

    /*
    public void OptSet()
    {
        
        float res;
        int width, height;
        res = kaizoudo.value;
        float screenRate = (float)res / Screen.height;
        width = sw;
        height = sh;
        if (screenRate < 1)
        {
            width = (int)(Screen.width * screenRate);
            height = (int)(Screen.height * screenRate);
        }
        Screen.SetResolution(width, height, true, 15);
        
    }*/

    void OnOffTune()
    {

        int t, m;
        if (PlayerPrefs.HasKey("car" + dcar))
        {
            string s = PlayerPrefs.GetString("car" + dcar);
            Debug.Log("save s:"+s);
            t = s[0] - '0';
            m = s[1] - '0';
        }
        else
        {
            PlayerPrefs.SetString("car" + dcar, "00");
            t = 0; m = 0;
        }

        if (dcar == 0 || dcar == 2  || dcar == 4 || dcar == 5 || dcar == 8 || dcar == 12 || dcar == 15)
        {
            gt.ts.SetTune(t);
            tunebt.SetActive(true);
        }
        else
            tunebt.SetActive(false);
        gt.ts.SetWheel(gt.ts.material[m]);
        drop.value = m;

    }

    public void OpenTune()
    {
        tunepl.SetActive(true);
        opbt.SetActive(false);
        rb.SetActive(false);
        lb.SetActive(false);
        gt.TurnOnTunePanel();

        tuneTabState = 0;
        TuneContents[tuneTabState].SetActive(true);
    }

    public void CloseTune()
    {
        tunepl.SetActive(false);
        opbt.SetActive(true);
        rb.SetActive(true);
        lb.SetActive(true);
        gt.TurnOnTunePanel();

        foreach(GameObject g in TuneContents)
        {
            g.SetActive(false);
        }
    }

    public void ChangeTuneTab(int x)
    {
        TuneContents[tuneTabState].SetActive(false);
        if (x == 1)
        {
            tuneTabState++;
            if (tuneTabState >= TUNE_TAB)
                tuneTabState -= TUNE_TAB;
        }
        else if(x==-1)
        {
            tuneTabState--;
            if (tuneTabState < 0)
                tuneTabState += TUNE_TAB;
        }
        TuneContents[tuneTabState].SetActive(true);
    }

    public void CloseOption()
    {
        PlayerPrefs.SetInt("bgm", toggle_b.isOn ? 1 : 0);
        PlayerPrefs.SetInt("etext", toggle_e.isOn ? 1 : 0);
        PlayerPrefs.SetInt("accont", toggle_a.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("trlevel", slider.value);
        PlayerPrefs.SetFloat("cpstren", slider2.value);
        PlayerPrefs.SetFloat("sev", slider3.value);
        PlayerPrefs.SetInt("mir", toggle_m.isOn ? 1 : 0);
        opt.SetActive(false);
    }

    public void OpenColor()
    {
        tunepl.SetActive(false);
        picker.SetActive(true);
        colorbt.SetActive(true);
        gcs.changing = true;
    }

    public void CloseColor()
    {
        tunepl.SetActive(true);
        picker.SetActive(false);
        colorbt.SetActive(false);
        gcs.changing = false ;
        gcs.Closed();
    }

    public void OpenPolish()
    {
        tunepl.SetActive(false);
        polishpl.SetActive(true);
    }

    public void ClosePolish()
    {
        tunepl.SetActive(true);
        polishpl.SetActive(false);
    }

    public void PolishCar()
    {
        int c = PlayerPrefs.GetInt("money");
        if (c > 500)
            c -= 500;
        else
            return;

        DispCoin(c);
        polisheffect.Play();
        PlayerPrefs.SetInt("money", c);

        int carlevel = PlayerPrefs.GetInt("carlev" + dcar);
        int carexp = PlayerPrefs.GetInt("carexp" + dcar);

        carexp += 50;



        int thisLevelExp = (int)Mathf.Pow(carlevel, 1.2f) * 100;

        while (carexp >= thisLevelExp)
        {
            carexp -= thisLevelExp;
            carlevel++;
            thisLevelExp = (int)Mathf.Pow(carlevel, 1.2f) * 100;
            //expslider.maxValue = thisLevelExp;
        }


        PlayerPrefs.SetInt("carlev" + dcar, carlevel);
        PlayerPrefs.SetInt("carexp" + dcar, carexp);

        DispCarLevel(dcar);
        StartCoroutine(SliderAnime(carexp));
    }

    private IEnumerator SliderAnime(int dest)
    {

        while (expslider.value != dest)
        {
            yield return new WaitForSeconds(0.01f);
            if (expslider.value == expslider.maxValue)
            {
                expslider.value = 0;
            }
            expslider.value++;
        }
    }

}
