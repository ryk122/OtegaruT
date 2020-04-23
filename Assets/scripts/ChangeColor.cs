using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    //public Color newcolor;
    public Color normalcolor;
    public Color noremicolor;
    [SerializeField]
    bool garage;
    Renderer rd;

    Texture2D mainTexture;
    Color[] pixels;


    public void Start()
    {
        rd = GetComponent<Renderer>();

        if (!garage)//whin race is start
        {
            int dcar = PlayerPrefs.GetInt("dcar");
            string save = PlayerPrefs.GetString("car" + dcar);
            if (save.Length > 6)
            {
                string colorcode = save.Substring(save.Length - 7);
                Debug.Log(colorcode);
                Color color;
                if (ColorUtility.TryParseHtmlString(colorcode, out color))
                {
                    // 変換できた時の処理（変換後のColorはcolorに代入されている）
                    SetColor(color);
                }
                else
                {
                    // 変換に失敗した時の処理（colorにはデフォルトの値が入ったまま）
                }
            }
                
        }
    }

    public void SetColor(Color color)
    {
        rd.material.SetColor("_Color", color);
    }

    public void SetEmmison(int x)
    {
        if (x == 0)
            rd.material.SetColor("_EmissionColor", new Color(0, 0, 0));
        else
            rd.material.SetColor("_EmissionColor", noremicolor);
    }

    public void GetEmitColor()
    {
        noremicolor = rd.material.GetColor("_EmissionColor");
        Debug.Log("emi:"+noremicolor);
    }

}
