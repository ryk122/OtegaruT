using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetScript : MonoBehaviour
{
    public static int num=0;
    public static int putnum = 0;
    public static GameObject[] target=new GameObject[1000];

    // Start is called before the first frame update
    void Start()
    {
        this.name = "Target"+num.ToString();
        target[putnum] = this.gameObject;
        num++;
        putnum++;
        if (putnum == 1000) putnum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
