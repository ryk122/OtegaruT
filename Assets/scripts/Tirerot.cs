using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tirerot : MonoBehaviour {
    public Carmain cm;
    public Carmain_2p cm2;
    public bool Dbp;
    public float p;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!Dbp)
        {
            Vector3 rot = transform.eulerAngles;
            rot.z += cm.speed * cm.back * p * Time.deltaTime;
            transform.eulerAngles = rot;
        }
        else
        {
            Vector3 rot = transform.eulerAngles;
            rot.z += cm2.speed * cm2.back * p * Time.deltaTime;
            transform.eulerAngles = rot;
        }
    }
}
