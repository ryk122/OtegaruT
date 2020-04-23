using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Photon.Pun.Demo.PunBasics
{
    //コピペで解決シリーズと見せかけて
    //ガレージ内で使用されないこと前提の仕様
    //carmainから呼び出されるので、注意
    public class ChangeColorOnline : MonoBehaviour
    {
        //public Color newcolor;
        Color normalcolor;
        Color noremicolor;
        Renderer rd;

        public PhotonView photonView;

        Texture2D mainTexture;
        Color[] pixels;

        string changedcolorcode;

        // Start is called before the first frame update
        public void Start()
        {
            photonView = GetComponent<PhotonView>();


            //Changecolorからデータをとる
            ChangeColor cc = GetComponent<ChangeColor>();
            normalcolor = cc.normalcolor;
            //noremicolor = cc.noremicolor;
            //

            rd = GetComponent<Renderer>();
            GetEmitColor();

            if (photonView == null || !photonView.IsMine)
                return;

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
                    //SetColor(color);
                    changedcolorcode = colorcode;
                    photonView.RPC("SetColor", RpcTarget.AllViaServer,colorcode);
                }
                else
                {
                    // 変換に失敗した時の処理（colorにはデフォルトの値が入ったまま）
                }
            }

        }

        [PunRPC]
        public void SetColor(string colorcode)//punで共有できるのは基本形のみ?
        {
            rd = GetComponent<Renderer>();
            Color color;
            if (ColorUtility.TryParseHtmlString(colorcode, out color))
            {
                // 変換できた時の処理（変換後のColorはcolorに代入されている）
                //SetColor(color);
                rd.material.SetColor("_Color", color);
            }
            else
            {
                // 変換に失敗した時の処理（colorにはデフォルトの値が入ったまま）
            }
            
        }

        public void ReSetColor()
        {
            photonView.RPC("SetColor", RpcTarget.AllViaServer, changedcolorcode);
            //Debug.LogWarning(changedcolorcode);
        }

        public void SetEmmison(int x)
        {
            if (photonView == null)
                photonView = GetComponent<PhotonView>();
            photonView.RPC("SetEmmisionPun", RpcTarget.AllViaServer, x);
        }

        [PunRPC]
        public void SetEmmisionPun(int x)
        {
            rd = GetComponent<Renderer>();
            if (x == 0)
                rd.material.SetColor("_EmissionColor", new Color(0, 0, 0));
            else
                rd.material.SetColor("_EmissionColor", noremicolor);
        }

        public void GetEmitColor()
        {
            noremicolor = rd.material.GetColor("_EmissionColor");
            Debug.Log("emi:" + noremicolor);
        }
    }
}
