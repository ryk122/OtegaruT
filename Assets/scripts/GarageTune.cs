using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Garage内でのチューン設定時に使用
//
public class GarageTune : MonoBehaviour
{
    static int dcar,tune;
    string data;
    [SerializeField]
    Text btext;
    [SerializeField]
    Garage g;
    [SerializeField]
    Dropdown drop;

    public TuneSetter ts;//Garageから伝達
    [SerializeField]
    Garage gg;

    public void TurnOnTunePanel()
    {
        GetData();
    }

    void GetData()
    {
        dcar = g.dcar;
        if (PlayerPrefs.HasKey("car" + dcar))
        {
            data = PlayerPrefs.GetString("car" + dcar);
            tune = data[0] - '0';

            int m = data[1] - '0';
            if (m < ts.material.Length)
                ts.SetWheel(ts.material[m]);
        }
        else
        {
            PlayerPrefs.SetString("car" + dcar, "00");
            data = "00";
            tune = 0;
        }

        TextChange();
    }

    void TextChange()
    {
        if (tune == 0)
            btext.text = " Tune Up : 500coin ";
        else
            btext.text = " Normal : 100coin ";
    }

    public void TuneButton()
    {
        int c;
        c = PlayerPrefs.GetInt("money");
        if (tune == 0)
        {
            if (c > 500)
            {
                tune = 1;
                c -= 500;
                PlayerPrefs.SetInt("money", c);
                PlayerPrefs.SetString("car" + dcar,"1"+data.Substring(1,data.Length-1));
            }
        }
        else
        {
            if (c > 100)
            {
                tune = 0;
                c -= 100;
                PlayerPrefs.SetInt("money", c);
                PlayerPrefs.SetString("car" + dcar, "0" +data.Substring(1, data.Length - 1));
            }
        }
        g.DispCoin(c);
        TextChange();
        ts.SetTune(tune);
    }

    public void SelectWheel()
    {
        ts.SetWheel(ts.material[drop.value]);
        //PlayerPrefs.SetString("car" + dcar, data[0] +d.value.ToString() );
    }

    public void SetSelectWheel()
    {
        int c = PlayerPrefs.GetInt("money");
        if (c > 10)
        {
            c -= 10;
            PlayerPrefs.SetInt("money", c);
            g.DispCoin(c);
            data = PlayerPrefs.GetString("car" + dcar);
            PlayerPrefs.SetString("car" + dcar, data[0] + drop.value.ToString() +data.Substring(2, data.Length - 2));
            gg.CloseTune();
        }
    }
}
