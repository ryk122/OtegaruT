﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
    //コピペで解決シリーズと見せかけて
    //ガレージ内で使用されないこと前提の仕様
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

        public ChangeColor[] changecolor;

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
            changecolor = ts.changecolor;
            //

            if (!photonView.IsMine)
            {
                //追加された側から、追加された側への通信は有効
                //ローカルインスタンス 呼びかけ=> 実体、実体 呼びかけ=>全ローカルインスタンス 
                photonView.RPC("NewCar", RpcTarget.AllViaServer);
                return;
            }

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
            if (photonView.IsMine &&added)
            {
                photonView.RPC("SetTune", RpcTarget.AllViaServer, tune_num, m_num);
                Debug.LogWarning("Tune!");
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
        public void NewCar()
        {
            added = true;
        }

    }
}