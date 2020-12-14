using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
    //コピペで解決シリーズと見せかけて
    //ガレージ内で使用されないこと前提の仕様
    //CarmainOnlineで、新規ログインを検知。updateないで判定してます
    public class TuneSetterOnline : MonoBehaviour
    {
        [SerializeField]
        CarmainOnline cm;
        [SerializeField]
        float pmaxs, pslip, pstr;
        [SerializeField]
        bool skinmesh, bodytune, turbo;
        [SerializeField]
        GameObject nomal, tuned;
        [SerializeField]
        GameObject[] wheel;
        public Material[] material;

        public ChangeColorOnline[] changecolor;

        PhotonView photonView;

        private int tune_num, m_num;
        private bool added = false;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();

            cm = GetComponent<CarmainOnline>();
            //tunesetterから読み取りたい
            TuneSetter ts = GetComponent<TuneSetter>();
            pmaxs = ts.pmaxs; pslip = ts.pslip; pstr = ts.pstr;
            skinmesh = ts.skinmesh; bodytune = ts.bodytune; turbo = ts.turbo;
            nomal = ts.nomal; tuned = ts.tuned;
            wheel = ts.wheel;
            material = ts.material;
            //changecolor = ts.changecolor;
            //

            if (!photonView.IsMine)
            {
                //追加された側から、追加された側への通信は有効
                //ローカルインスタンス 呼びかけ=> 実体、実体 呼びかけ=>全ローカルインスタンス 
                photonView.RPC("NewCar", RpcTarget.AllViaServer);
                return;
            }

            //自分が入室したので、既にいた他人に自分の状態を知らせる&自分の状態変化

            if (wheel.Length == 0)
                Debug.LogError("No Wheel Material");

            int tune;
            int dcar = PlayerPrefs.GetInt("dcar");
            if (PlayerPrefs.HasKey("car" + dcar))
            {
                string data = PlayerPrefs.GetString("car" + dcar);
                tune = data[0] - '0';
                int m = data[1] - '0';
                if (m < material.Length)
                    //SetWheel(material[m]);
                    photonView.RPC("SetWheel", RpcTarget.AllViaServer, m);
                m_num = m;

                float shakou = PlayerPrefs.GetFloat("shakou" + dcar);
                SetShakou(shakou);
                //photonView.RPC("SetShakou", RpcTarget.AllViaServer, shakou);

                float camber = PlayerPrefs.GetFloat("camber" + dcar);
                SetCamber(camber);
                //photonView.RPC("SetCamber", RpcTarget.AllViaServer, camber);

                float width = PlayerPrefs.GetFloat("width" + dcar);
                SetWidth(width);
                //photonView.RPC("SetWidth", RpcTarget.AllViaServer, width);
            }
            else
            {
                PlayerPrefs.SetString("car" + dcar, "00#nnnnnn");//[0]=body,[0]=wheel
                tune = 0;
            }

            tune_num = tune;

            if (bodytune)
                //SetTune(tune);
                photonView.RPC("SetTune", RpcTarget.AllViaServer, tune);
            if (skinmesh)
            {
                if (tune == 1)
                    cm.skm = tuned.GetComponent<SkinnedMeshRenderer>();
                else
                    cm.skm = nomal.GetComponent<SkinnedMeshRenderer>();
                //cm.LightButton(); cm.LightButton();
            }

        }

        private void Update()
        {
            if (photonView.IsMine &&added)//呼びかけの実体
            {
                //他人が入室してきたとき、他人に自分のステータスを知らせる。
                photonView.RPC("SetTune", RpcTarget.AllViaServer, tune_num, m_num);

                foreach(ChangeColorOnline cc in changecolor)
                {
                    cc.ReSetColor();
                }
                added = false;

                
                int dcar = PlayerPrefs.GetInt("dcar");
                float shakou = PlayerPrefs.GetFloat("shakou" + dcar);
                photonView.RPC("SetShakou", RpcTarget.AllViaServer, shakou);


                float camber = PlayerPrefs.GetFloat("camber" + dcar);
                photonView.RPC("SetCamber", RpcTarget.AllViaServer, camber);

                float width = PlayerPrefs.GetFloat("width" + dcar);
                photonView.RPC("SetWidth", RpcTarget.AllViaServer, width);

                //自分だけまき戻す
                SetShakou(-shakou);
                SetWidth(-width);

                //Debug.LogWarning("Tune!");
            }
        }

        [PunRPC]
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

        [PunRPC]
        public void SetWheel(int m)
        {
            foreach (GameObject t in wheel)
            {
                t.GetComponent<Renderer>().material = material[m];
            }
        }

        [PunRPC]
        public void SetTune(int tune,int m)
        {
            if (tune == 0)
            {
                tuned.SetActive(false);
                nomal.SetActive(true);
            }
            else if (tune == 1)
            {
                tuned.SetActive(true);
                nomal.SetActive(false);
            }

            foreach (GameObject t in wheel)
            {
                t.GetComponent<Renderer>().material = material[m];
            }
        }

        [PunRPC]
        public void SetCamber(float angle)
        {
            wheel[0].transform.localEulerAngles = new Vector3(-angle, 90, 0);
            wheel[1].transform.localEulerAngles = new Vector3(-angle, -90, 0);
            wheel[2].transform.localEulerAngles = new Vector3(-angle, 90, 0);
            wheel[3].transform.localEulerAngles = new Vector3(-angle, -90, 0);
        }

        [PunRPC]
        public void SetWidth(float widthDiff)
        {
            wheel[0].transform.localPosition += new Vector3(widthDiff, 0, 0);
            wheel[1].transform.localPosition += new Vector3(-widthDiff, 0, 0);
            wheel[2].transform.localPosition += new Vector3(widthDiff, 0, 0);
            wheel[3].transform.localPosition += new Vector3(-widthDiff, 0, 0);
        }

        [PunRPC]
        public void SetShakou(float diff)
        {
            nomal.transform.localPosition += new Vector3(0, diff, 0);
            if (tuned != null)
                tuned.transform.localPosition += new Vector3(0, diff, 0);

            GetComponent<Carmain>().tlight.transform.localPosition += new Vector3(0, diff, 0);
        }


        [PunRPC]
        public void NewCar()//carmianonlineから呼び出し
        {
            added = true;
        }

    }
}