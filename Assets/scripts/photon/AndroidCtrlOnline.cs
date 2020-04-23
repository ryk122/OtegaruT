using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Photon.Pun.Demo.PunBasics
{
    public class AndroidCtrlOnline : MonoBehaviour
    {
        [SerializeField]
        Text playernum;
        public CarmainOnline cm;
        public GameObject b1, b2;
        float trlevel;
        bool r, l, g, b;
        bool accont;
        Vector3 acc;
        float rot;

        // Use this for initialization
        public void Start()
        {
            
            if (PlayerPrefs.GetInt("accont") == 1) accont = true; else accont = false;
            if (accont)
            {
                b1.SetActive(false); b2.SetActive(false);
            }
            trlevel = PlayerPrefs.GetFloat("trlevel");
            //GameObject car = GameObject.Find("car");
            //cm = car.GetComponent<CarmainOnline>();
            r = l = g = b = false;
            cm.android = true;

            InvokeRepeating("PlayerNumChange", 0, 3);
        }

        private void Update()
        {
            if (accont)
            {
                acc = Input.acceleration;
                rot = acc.x * trlevel * 1.5f;
                if (rot > 1) rot = 1;
                else if (rot < -1) rot = -1;
            }

        }

        private void PlayerNumChange()
        {
            playernum.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString()+" players";
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (accont)
            {
                if (Mathf.Abs(rot) > 0.4)
                    cm.TR(rot);
                else
                    cm.AndrC();
            }
            else
            {
                if (r)
                    cm.TR(1);
                else if (l)
                    cm.TR(-1);
                else
                {
                    cm.AndrC();
                }
            }


            if (g)
                cm.Run();
            else if (b)
            {
                cm.back = -1;
                cm.Back();
            }
            else
            {
                cm.N();

            }

        }

        public void RightD()
        {
            r = true;
        }
        public void LeftD()
        {
            l = true;
        }
        public void RunD()
        {
            g = true;
        }
        public void BackD()
        {
            b = true;
            cm.Bon();
        }
        public void RightU()
        {
            r = false;
        }
        public void LeftU()
        {
            l = false;
        }
        public void RunU()
        {
            g = false;
            cm.Acoff();
        }
        public void BackU()
        {
            b = false;
            cm.Boff();
        }

        public void LeaveRoom()
        {
            cm.DestroyCamera();
            Destroy(gameObject);
            PhotonNetwork.LeaveRoom();
            //SceneManager.LoadScene("PunBasics-Launcher");
        }

        public void Light()
        {
            cm.LightButton();
        }

    }
}