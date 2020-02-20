using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carmain_2p : MonoBehaviour
{
    public float speed;
    public float maxs;
    public float b, a, str;
    public int h, back;
    public float lvec;
    public GameObject tlight, gyaobj;
    public Transform ur;
    Rigidbody rb;
    public float rl, rl2;
    public GameObject kemuri1, kemuri2, kemuri3, kemuri4;
    public Transform ftr, ftl;
    public Camera cmr;
    private ParticleSystem k1, k2, k3, k4;
    public int ct = 0, gt = 0;
    AudioSource auds, eds;
    bool gyaa, imgfadeout;
    Image gyaimg;
    float w;
    public UnityStandardAssets.Utility.SmoothFollow smf;

    // Use this for initialization
    void Start()
    {
        Carmain cms=GetComponent<Carmain>();
        maxs = cms.maxs;b = cms.b;str = cms.str;
        tlight = cms.tlight;gyaobj = cms.gyaobj;ur = cms.ur;
        kemuri1 = cms.kemuri1;kemuri2 = cms.kemuri2;kemuri3 = cms.kemuri3;kemuri4 = cms.kemuri4;
        ftr = cms.ftr;ftl = cms.ftl;cmr = cms.efcmr;
        cms.enabled = false;

        smf.target = this.transform;

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
        gyaimg = gyaobj.GetComponent<Image>();
        gyaa = false;
        imgfadeout = true;
        w = b;
        b = 0;
        StartStop();
    }

    void StartStop()
    {
        b = w;
    }

    private void Update()
    {
        if (imgfadeout)
        {
            Color cl = gyaimg.color;
            cl.a -= 0.1f;
            gyaimg.color = cl;
        }

        if (speed > 19)
            cmr.enabled = true;
        else
            cmr.enabled = false;

        Vector3 rot = transform.localEulerAngles;
        if (rot.z > 30 || rot.z < -30)
        {
            rot.z = 0;
            transform.localEulerAngles = rot;
        }
        if (rot.x > 80 || rot.x < -80)
        {
            rot.x = 0;
            transform.localEulerAngles = rot;
        }

        //Engine();
        eds.pitch = speed / 20 + 0.5f + a / 2;


    }


    // Update is called once per frame
    void FixedUpdate()
    {
        speed = rb.velocity.magnitude;
        a = 0;
        Ray uray = new Ray(ur.position, ur.forward);
        RaycastHit uhit;
        Physics.Raycast(ur.position, ur.forward, out uhit, 16);
        Debug.DrawRay(uray.origin, uray.direction * 16, Color.red, 0.1f, true);
        rl = uhit.distance;

        if (rl > 0.7f)//0.33
        {

            transform.Rotate(new Vector3(1, 0, 0));
            rb.AddForce(transform.up * -1, ForceMode.VelocityChange);
            rb.AddForce(transform.forward * 1, ForceMode.VelocityChange);

        }

        back = 1;
        tlight.SetActive(false);

        if (Input.GetKey(KeyCode.W))
        {
            Run();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Back();
            back = -1;
        }
        else
        {
            N();
        }

        if (Input.GetKey(KeyCode.D))
        {
            TR(1);
            Vector3 rot = ftl.transform.localEulerAngles;
            rot.y = 40;
            ftl.localEulerAngles = rot;
            ftr.localEulerAngles = rot;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            TR(-1);
            Vector3 rot = ftl.transform.localEulerAngles;
            rot.y = -40;
            ftl.localEulerAngles = rot;
            ftr.localEulerAngles = rot;
        }
        else
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

                    imgfadeout = true;

                    gyaa = false;
                    gt = 0;
                }
            }
        }
    }

    void Run()
    {
        if (h == 0)
            h = 18;
        else
            h = 1;

        a = b * Mathf.Pow(-speed + maxs, 0.5f);
        if (!float.IsNaN((transform.forward * a).x))
            rb.AddForce(transform.forward * a, ForceMode.VelocityChange);
        lvec = a * 0.9f;

    }

    void Back()
    {
        tlight.SetActive(true);
        if (speed > 5)
        {
            h = 0;
            k1.Play(); k2.Play(); k3.Play(); k4.Play();

            if (!gyaa)
            {
                gyaa = true;
                imgfadeout = false;
                auds.Play();
            }
        }
        else
        {
            h = -1;
        }
        a = b * 2 * Mathf.Pow(-speed + maxs - 20, 0.5f);
        if (!float.IsNaN((transform.forward * a).x))
            rb.AddForce(transform.forward * -a, ForceMode.VelocityChange);
        lvec = -a * 0.9f;

    }
    void N()
    {
        if (lvec > 0)
        {
            rb.AddForce(transform.forward * lvec, ForceMode.VelocityChange);
            lvec -= 0.01f;
        }
        else { lvec = 0; }

    }

    /*Turn*/
    void TR(int x)
    {
        ct += 1;
        transform.Rotate(new Vector3(0, str * Mathf.Pow(speed, 0.5f) * h * x, 0));
        if (speed > 8)
        {
            if (ct > 20)
            {
                k1.Play(); k2.Play(); k3.Play(); k4.Play();
                gyaimg.color = new Color(1, 1, 1, 1);//(float)(ct-5)/10
                Vector3 rot = gyaobj.transform.eulerAngles;
                rot.z = Random.Range(-3.0f, 3.0f);
                gyaobj.transform.eulerAngles = rot;
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
            gyaa = false;
            auds.Stop();
        }
    }

    public void TIme_Up()
    {
        b = 0;
        gyaa = false;
        gyaimg.color = new Color(1, 1, 1, 0);
        cmr.enabled = false;
        auds.Stop();
        rb.AddForce(transform.up * -5, ForceMode.VelocityChange);
        tlight.SetActive(false);
        eds.Stop();
    }

}
