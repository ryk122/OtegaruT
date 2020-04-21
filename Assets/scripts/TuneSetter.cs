using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//carに適用 レース開始時に自動で変更する。
//garage中ではGarageTuneからSettuneを使う
public class TuneSetter : MonoBehaviour
{
    [SerializeField]
    Carmain cm;
    [SerializeField]
    public float pmaxs, pslip, pstr;
    [SerializeField]
    public bool skinmesh,bodytune,turbo,garage;
    [SerializeField]
    public GameObject nomal, tuned;
    [SerializeField]
    public GameObject[] wheel;
    public Material[] material;

    public ChangeColor[] changecolor;

    // Start is called before the first frame update
    void Start()
    {
        if (wheel.Length == 0)
            Debug.LogError("No Wheel Material");
        if (garage)
            return;
        int tune;
        int dcar = PlayerPrefs.GetInt("dcar");
        if (PlayerPrefs.HasKey("car" + dcar)){
            string data = PlayerPrefs.GetString("car" + dcar);
            tune = data[0] - '0';
            int m = data[1] - '0';
            if (m < material.Length)
                SetWheel(material[m]);


        }
        else
        {
            PlayerPrefs.SetString("car" + dcar, "00#nnnnnn");//[0]=body,[0]=wheel
            tune = 0;
        }

        cm = GetComponent<Carmain>();
        if(bodytune)
            SetTune(tune);
        if (skinmesh)
        {
            if (tune == 1)
                cm.skm = tuned.GetComponent<SkinnedMeshRenderer>();
            else
                cm.skm = nomal.GetComponent<SkinnedMeshRenderer>();
            cm.LightOnOff(); cm.LightOnOff();
        }
    }

    public void SetTune(int tune)
    {
        Debug.Log("tune :" + tune);
        if (tune == 0)
        {
            tuned.SetActive(false);
            nomal.SetActive(true);
        }
        else if (tune == 1)
        {
            tuned.SetActive(true);
            nomal.SetActive(false);
            if (cm != null)
            {
                cm.maxs += pmaxs;
                cm.slip += (int)pslip;
                cm.str += pstr;
                cm.turbo = turbo;
                cm.GetSoundSource();
            }
        }
    }

    public void SetWheel(Material m)
    {
        foreach(GameObject t in wheel)
        {
            t.GetComponent<Renderer>().material = m;
        }
    }

}
