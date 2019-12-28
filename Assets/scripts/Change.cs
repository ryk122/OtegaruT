using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change : MonoBehaviour {
    [Range(0,1)]
    public float speed;
    public static Carmain cm;
    public bool dbp;
    Renderer rd;

	// Use this for initialization
	void Start () {
        rd = GetComponent<Renderer>();
        GameObject car = GameObject.Find("car");
        cm = car.GetComponent<Carmain>();
    }

    public static void Reset()
    {
        GameObject car = GameObject.Find("car");
        cm = car.GetComponent<Carmain>();
    }
    
    // Update is called once per frame
    void Update () {
        rd.material.SetFloat("speed", (cm.maxs-cm.speed)/cm.maxs-0.05f);
        //rd.material.SetFloat("speed", speed);
    }
}
