using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(StartCtrl))]
public class PutCar : MonoBehaviour
{
    [SerializeField]
    GameObject[] car;

    [SerializeField]
    GameObject gya;

    [SerializeField]
    Camera ecm;

    [SerializeField]
    public Moji mojiDisp;

    [System.Serializable]
    public class Moji{   
    public GameObject sora, doko, over,mizor,mizol;
    public Carmain cm;
    public MakeRoad mk;
    public Text all,speed;
    public TextMeshProUGUI tmp;
    public bool free, dbp, auto;
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCtrl sctrl = GetComponent<StartCtrl>();
        int dcar = PlayerPrefs.GetInt("dcar");
        GameObject racecar = Instantiate(car[dcar], transform.position, transform.rotation);
        racecar.name = "car";
        sctrl.car[dcar] = racecar;
        Transform hitbox = racecar.transform.Find("hitbox");
        HitChecker mj = hitbox.GetComponent<HitChecker>();

        racecar.GetComponent<Carmain>().speedText = mojiDisp.speed;
       

        mj.sora = mojiDisp.sora;
        mj.doko = mojiDisp.doko;
        mj.over = mojiDisp.over;
        mj.mizor = mojiDisp.mizor;
        mj.mizol = mojiDisp.mizol;

        mj.cm = racecar.GetComponent<Carmain>();
        mj.mk = mojiDisp.mk;
        mj.all = mojiDisp.all;
        mj.tmp = mojiDisp.tmp;
        mj.free = mojiDisp.free;
        mj.dbp = mojiDisp.dbp;
        mj.auto = mojiDisp.auto;

        mj.cm.gyaobj = gya;
        mj.cm.efcmr = ecm;

        mj.mk.hitCheck = mj;
    }

}
