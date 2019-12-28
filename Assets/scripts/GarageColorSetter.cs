using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageColorSetter : MonoBehaviour
{
    public bool changing=false;
    [SerializeField]
    ColorPickerAdvanced colorpicker;
    TuneSetter ts;
    int car;
    Color color;
    [SerializeField]
    Garage gg;

    public void GCStart(int car,TuneSetter ts)//garageより表示中の車番号をもらえる
    {
        this.car = car;
        this.ts = ts;
        string save = PlayerPrefs.GetString("car" + car);
        
        if (save.Length < 7)//hasnt color code
        {
            if (save.Length < 2)
                save += "00";
            save += "#nnnnnn";
            Debug.Log("gcs:" + save);
            PlayerPrefs.SetString("car" + car, save);
        }
        string colorcode = save.Substring(save.Length - 7);
        if (ColorUtility.TryParseHtmlString(colorcode, out color))
        {
            // 変換できた時の処理（変換後のColorはcolorに代入されている）
            foreach (ChangeColor cc in ts.changecolor)
            {
                if (cc != null)
                {
                    cc.Start();
                    cc.SetColor(color);
                    colorpicker.Color = color;
                }
            }
        }
        else
        {
            // 変換に失敗した時の処理（colorにはデフォルトの値が入ったまま）
            //color = ts.changecolor[0].normalcolor;
            //colorpicker.Color = color;
            foreach (ChangeColor cc in ts.changecolor)
            {
                if (cc != null)
                {
                    cc.Start();
                    //cc.SetColor(color);
                }
            }
        }
    }

    void Preview()
    {
        foreach (ChangeColor cc in ts.changecolor)
        {
            if (cc != null)
            {
                cc.SetColor(colorpicker.Color);
            }
        }
    }

    public void Paint()
    {
        int c = PlayerPrefs.GetInt("money");
        if (c < 500)
            return;
        PlayerPrefs.SetInt("money", c - 500);
        string save = PlayerPrefs.GetString("car" + car);
        string newsave = save.Substring(0, save.Length - 7) +"#"+ ColorUtility.ToHtmlStringRGB(colorpicker.Color);
        Debug.Log(newsave);
        PlayerPrefs.SetString("car" + car, newsave);
        gg.CloseColor();
    }

    public void Reset()
    {
        string save = PlayerPrefs.GetString("car" + car);
        string newsave = save.Substring(0, save.Length - 7) + "#nnnnnn";

        foreach (ChangeColor cc in ts.changecolor)
        {
            if (cc != null)
            {
                cc.SetColor(ts.changecolor[0].normalcolor);
                colorpicker.Color = ts.changecolor[0].normalcolor;
            }
        }
    }

    public void Closed()
    {
        string save = PlayerPrefs.GetString("car" + car);
        Debug.Log(save);
        string colorcode = save.Substring(save.Length - 7);
        if (ColorUtility.TryParseHtmlString(colorcode, out color))
        {
            // 変換できた時の処理（変換後のColorはcolorに代入されている）
            foreach (ChangeColor cc in ts.changecolor)
            {
                if (cc != null)
                {
                    cc.SetColor(color);
                }
            }
        }
        else
        {
            // 変換に失敗した時の処理（colorにはデフォルトの値が入ったまま）
            //ts.changecolor[0].Start();
            color = ts.changecolor[0].normalcolor;
            foreach (ChangeColor cc in ts.changecolor)
            {
                if (cc != null)
                {
                    cc.SetColor(color);
                    colorpicker.Color = color;
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (changing)
            Preview();
    }
}
