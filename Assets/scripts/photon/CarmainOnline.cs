using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun.UtilityScripts;
using UnityStandardAssets.Utility;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.Demo.PunBasics
{
    public class CarmainOnline : MonoBehaviour
    {
        public static CarmainOnline CARMAIN;
        public float speed, maxs;
        public float b, a, str;
        public int h, back, slip, mizo;
        public float lvec;
        public GameObject tlight, gyaobj, light1, light2;
        public SkinnedMeshRenderer skm;
        public Transform ur, ur2;
        Rigidbody rb;
        public float rl, rl2;
        public GameObject kemuri1, kemuri2, kemuri3, kemuri4;
        public Transform ftr, ftl;
        public Camera efcmr;
        private ParticleSystem k1, k2, k3, k4;
        public int ct = 0, gt = 0;
        public bool turbo, android, auto;
        AudioSource auds, eds, tsound;
        bool gyaa, imgfadeout ,lightstate;

        RaycastHit uhit;
        TuneSetter ts;
        bool k;
        int sliptime;
        float vol;
        [SerializeField]
        GameObject efcam_prefab,adc_prefab,camera;

        private GameObject cameraObject;


        private PhotonView photonView;

        // Use this for initialization
        void Start()
        {
            if (!auto)
                CARMAIN = this;
            k = false;
            sliptime = 0;
            back = 1;
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = new Vector3(0, -1, 0);
            rb.maxAngularVelocity = 0.1f;
            k1 = kemuri1.GetComponent<ParticleSystem>();
            k2 = kemuri2.GetComponent<ParticleSystem>();
            k3 = kemuri3.GetComponent<ParticleSystem>();
            k4 = kemuri4.GetComponent<ParticleSystem>();
            AudioSource[] audioSources = GetComponents<AudioSource>();
            auds = audioSources[0];
            eds = audioSources[1];
            vol = PlayerPrefs.GetFloat("sev");
            //
            vol = 1;
            if (turbo)
            {
                tsound = audioSources[2]; tsound.volume = vol;
            }
            auds.volume = vol * 0.6f; eds.volume = vol;
            gyaa = false;
            imgfadeout = true;

            ts = GetComponent<TuneSetter>();


            foreach (ChangeColor cc in ts.changecolor)
            {
                cc.Start();
                cc.GetEmitColor();
            }

            //if (PlayerPrefs.GetInt("time") == 0)
                lightstate = false;

            LightOnOff();

            photonView = GetComponent<PhotonView>();

            if (photonView.IsMine)
            {
                efcmr = Instantiate(efcam_prefab).GetComponent<Camera>();

                AndroidCtrlOnline adc = Instantiate(adc_prefab).GetComponent<AndroidCtrlOnline>();
                adc.cm = this;
                android = true;
                DontDestroyOnLoad(adc.gameObject);

                SmoothFollow smf = Instantiate(camera).GetComponent<SmoothFollow>();
                smf.target = transform;
                DontDestroyOnLoad(smf.gameObject);
                cameraObject = smf.gameObject;
            }

            
        }

        private void Update()
        {

            if (!auto && efcmr!=null)
                if (speed > 19)
                    efcmr.enabled = true;
                else
                    efcmr.enabled = false;

            Vector3 rot = transform.localEulerAngles;

            if (!(rot.z > 330 || rot.z < 30))
            {
                rot.z = 0;
                transform.localEulerAngles = rot;
            }
            if (!(rot.x > 300 || rot.x < 80))
            {
                rot.x = 0;
                transform.localEulerAngles = rot;
            }

            eds.pitch = speed / 20 + 0.5f + a / 2;

            if (k)
            {
                k1.Play(); k2.Play(); k3.Play(); k4.Play();
            }

            //turbo
            if (turbo && Input.GetKeyUp(KeyCode.UpArrow))
            {
                tsound.Play();
            }

            //roman
            /*
            turn = (Input.mousePosition.x - Screen.width/2)/(Screen.width/2)*2.5f;
            if (Mathf.Abs(turn) > 1) turn /= Mathf.Abs(turn);
            Debug.Log(turn);
            */
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            speed = rb.velocity.magnitude;
            a = 0;
            //Ray uray = new Ray(ur.position, ur.forward);
            Physics.Raycast(ur.position, ur.forward, out uhit, 16);
            //Debug.DrawRay(uray.origin, uray.direction * 16, Color.red, 0.1f,true);
            rl = uhit.distance;

            if (rl > 0.7f)//0.33
            {
                transform.Rotate(new Vector3(1, 0, 0));
                rb.AddForce(transform.up * -1, ForceMode.VelocityChange);
                rb.AddForce(transform.forward * 1, ForceMode.VelocityChange);
            }

            back = 1;
            tlight.SetActive(false);

            if (!android && !auto)
            //if (!auto)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    Run();
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    Back();
                    back = -1;
                }
                else
                {
                    N();
                }


                if (Input.GetKey(KeyCode.RightArrow))
                {
                    TR(1);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    TR(-1);
                }
                else
                {
                    //TRModel(0);
                    photonView.RPC("TRModel", RpcTarget.AllViaServer,0);
                    ct = 0;
                    imgfadeout = true;
                    Vector3 rot = transform.eulerAngles;
                    ftl.eulerAngles = rot;
                    ftr.eulerAngles = rot;

                    if (gyaa)
                    {
                        gt++;
                        if (gt > 5 && back != -1)
                        {
                            auds.Stop();
                            k = false;
                            imgfadeout = true;

                            gyaa = false;
                            gt = 0;
                        }
                    }
                }
            }

            sliptime++;


            //mizo
            if (mizo != 0)
            {
                rb.AddForce(transform.right * mizo * speed * 0.55f);
                rb.AddTorque(transform.forward * mizo * speed * -80f);
                rb.AddTorque(transform.right * mizo * speed * 50f);
                transform.Rotate(new Vector3(0, Mathf.Pow(speed, 0.5f) * mizo * 0.05f, 0));
            }
        }

        public void Run()
        {
            if (h == 0)
                h = slip;
            else
                h = 1;

            a = b * Mathf.Pow(-speed + maxs, 0.5f);
            if (!float.IsNaN((transform.forward * a).x))
                rb.AddForce(transform.forward * a, ForceMode.VelocityChange);
            lvec = a * 0.9f;

        }

        public void Back()
        {
            sliptime = 0;
            tlight.SetActive(true);
            //
            float bspeed = Vector3.Dot(transform.forward, rb.velocity);
            //Debug.Log("speed" + bspeed.ToString());
            if (bspeed > 0 && speed > 5)
            {
                h = 0;
                k = true;
                //k1.Play(); k2.Play(); k3.Play(); k4.Play();

                if (!gyaa)
                {
                    gyaa = true;
                    imgfadeout = false;
                    auds.Play();
                }
            }
            else if (bspeed < 0)
            {
                h = -1;
            }
            a = b * 2 * Mathf.Pow(-speed + maxs - 20, 0.5f);
            if (!float.IsNaN((transform.forward * a).x))
                rb.AddForce(transform.forward * -a, ForceMode.VelocityChange);
            lvec = -a * 0.9f;

        }
        public void N()
        {
            if (sliptime > 10)
                h = 1;

            if (lvec > 0)
            {
                rb.AddForce(transform.forward * lvec, ForceMode.VelocityChange);
                lvec -= 0.01f;
            }
            else
            {
                lvec = 0;
            }

        }

        /*Turn*/
        public void TR(float x)
        {
            /*
            Vector3 rot = ftl.transform.localEulerAngles;
            rot.y = 40 * x;
            ftl.localEulerAngles = rot;
            ftr.localEulerAngles = rot;*/
            //TRModel(x);
            photonView.RPC("TRModel", RpcTarget.AllViaServer, x);

            ct += 1;
            transform.Rotate(new Vector3(0, str * Mathf.Pow(speed, 0.5f) * h * x, 0));
            if (speed > 8)
            {
                if (ct > 20)
                {
                    k = true;
                    gt = 0;
                    if (!gyaa)
                    {
                        gyaa = true;
                        auds.Play();
                    }
                }
            }
            else if (speed < 3 && gyaa)
            {
                k = false;
                gyaa = false;
                auds.Stop();
            }
        }
        [PunRPC]
        public void TRModel(float x)
        {
            Vector3 rot = ftl.transform.localEulerAngles;
            rot.y = 40 * x;
            ftl.localEulerAngles = rot;
            ftr.localEulerAngles = rot;
        }

        public void TIme_Up()
        {
            b = 0;
            gyaa = false;

            efcmr.enabled = false;
            k = false;
            auds.Stop();
            rb.AddForce(transform.up * -5, ForceMode.VelocityChange);
            tlight.SetActive(false);
            eds.Stop();
        }

        public void AndrC()
        {
            ct = 0;
            imgfadeout = true;
            Vector3 rot = transform.eulerAngles;
            ftl.eulerAngles = rot;
            ftr.eulerAngles = rot;

            if (gyaa)
            {
                gt++;
                if (gt > 5 && back != -1)
                {
                    auds.Stop();
                    k = false;
                    imgfadeout = true;

                    gyaa = false;
                    gt = 0;
                }
            }

        }

        public void Acoff()
        {
            if (turbo)
                tsound.Play();
        }

        public void GetSoundSource()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            tsound = audioSources[2]; tsound.volume = vol;
        }

        public void DestroyCamera()
        {
            Destroy(cameraObject);
        }

        [PunRPC]
        public void LightOnOff()
        {
            if (lightstate)
            {
                lightstate = false;
                light1.SetActive(false); light2.SetActive(false);
                if (skm != null)
                {
                    skm.SetBlendShapeWeight(0, 100);
                }

                if (!auto)
                    foreach (ChangeColor cc in ts.changecolor)
                    {
                        cc.SetEmmison(0);
                    }
            }
            else
            {
                lightstate = true;
                light1.SetActive(true); light2.SetActive(true);
                if (skm != null)
                {
                    skm.SetBlendShapeWeight(0, 0);
                }

                if (!auto)
                    foreach (ChangeColor cc in ts.changecolor)
                    {
                        cc.SetEmmison(1);
                    }
            }
        }
        public void LightButton()
        {
            photonView.RPC("LightOnOff", RpcTarget.AllViaServer);
        }

    }
}