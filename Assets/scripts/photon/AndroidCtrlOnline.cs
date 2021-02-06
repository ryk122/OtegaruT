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
        public GameObject b1, b2, b3, b4, rcam;
        float trlevel;
        bool r, l, g, b;
        bool accont, controller;
        int camstate;
        Vector3 acc;
        float rot;

        public static GameObject nameui;//set by PlayerManager to on off ui

        // Use this for initialization
        public void Start()
        {
            
            if (PlayerPrefs.GetInt("accont") == 1) accont = true; else accont = false;
            if (PlayerPrefs.GetInt("controller") == 1) controller = true; else controller = false;
            if (accont || controller)
            {
                b1.SetActive(false); b2.SetActive(false);
                if (controller)
                {
                    b3.SetActive(false); b4.SetActive(false);
                }
            }
            trlevel = PlayerPrefs.GetFloat("trlevel");
            //GameObject car = GameObject.Find("car");
            //cm = car.GetComponent<CarmainOnline>();
            r = l = g = b = false;
            cm.android = true;

            rcam.transform.parent = cm.transform;
            rcam.transform.position = cm.transform.position;

            InvokeRepeating("PlayerNumChange", 0, 3);

            camstate = 0;
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
            /*for controll device*/
            if (controller)
            {
                //steer
                rot = Input.GetAxis("Horizontal");


                //up down
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Fire2"))
                {
                    g = true;
                }
                else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetButtonUp("Fire2"))
                {
                    g = false;
                    cm.Acoff();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetButtonDown("Fire1"))
                {
                    b = true;
                    cm.Bon();
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetButtonUp("Fire1"))
                {
                    b = false;
                    cm.Boff();
                }

                //other
                if (Input.GetButtonDown("Fire3"))
                {
                    cm.LightButton();
                }
            }

        }

        private void PlayerNumChange()
        {
            playernum.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString()+" players";
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (accont||controller)
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

        public void CamChange()
        {
            FollowingCamera fcam = cm.cpm.gameObject.GetComponent<FollowingCamera>();
            camstate++;

            if (camstate == 0)//to third person
            {
                nameui.SetActive(false);

                fcam.enabled = false;
                cm.smf.enabled = true;
                

                cm.smf.rotationDamping -= 500;
                cm.smf.heightDamping -= 500;
                cm.cpm.maincam.transform.position = cm.cpm.pos1.position;
                cm.smf.distance = 3;
                cm.smf.height = 0.5f;
                nameui.SetActive(true);
            }
            else if(camstate == 1)//first person
            {
                nameui.SetActive(false);
                cm.smf.rotationDamping += 500;
                cm.smf.heightDamping += 500;
                cm.smf.distance = 2;
                cm.smf.height = 0.2f;
                cm.cpm.maincam.position = cm.cpm.pos2.position;
            }
            else//around camera
            {
                
                fcam.target = cm.gameObject;
                cm.smf.enabled = false;
                fcam.enabled = true;

                
                camstate = -1;
                nameui.SetActive(true);
            }
        }

    }
}