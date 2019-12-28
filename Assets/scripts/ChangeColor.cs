using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    //public Color newcolor;
    public Color normalcolor;
    [SerializeField]
    bool garage;
    Renderer rd;

    Texture2D mainTexture;
    Color[] pixels;


    public void Start()
    {
        rd = GetComponent<Renderer>();
        //mainTexture = (Texture2D)GetComponent<Renderer>().material.GetTexture("_SubTex");
        //pixels = mainTexture.GetPixels();
        //normalcolor = new Color(pixels[0].r, pixels[0].g, pixels[0].b);

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
        /*
        // 書き換え用テクスチャ用配列の作成
        Color[] change_pixels = new Color[1];

        change_pixels.SetValue(color, 0);

        // 書き換え用テクスチャの生成
        Texture2D change_texture = new Texture2D(1,1, TextureFormat.RGBA32, false);
        change_texture.filterMode = FilterMode.Point;
        change_texture.SetPixels(change_pixels);
        change_texture.Apply();

        // テクスチャを貼り替える
        GetComponent<Renderer>().material.SetTexture("_SubTex", change_texture);
        */
        rd.material.SetColor("_Color", color);
    }

}
