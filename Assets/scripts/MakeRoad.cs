using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRoad : MonoBehaviour {
    public GameObject[] Road;
    public GameObject Checkpoint;
    public GameObject Target;
    public HitChecker hitCheck;
    public bool VS;
    int length,j,u;
    GameObject[] index = new GameObject[30];

	// Use this for initialization
	void Start () {
        j = 0;
        u = 0;
        length = Road.Length;
        SetRoad();
        SetRoad();
    }

    public void SetRoad()
    {
        int i, x;
        GameObject Put,Endobj;
        hitCheck.pinturn = 0;
        for (i = 0; i < 10; i++)
        {
            x = Random.Range(0, length * 2);
            if (VS) Instantiate(Target, transform.position, transform.rotation);
            Put = Instantiate(Road[x / 2], transform.position, transform.rotation);
            index[i + j] = Put;
            if (x / 2 == 2)
                hitCheck.pinturn++;
            if (x % 2 == 1)
            {
                Vector3 size = Put.transform.localScale;
                size.y *= -1;
                Put.transform.localScale = size;
            }
            Endobj=Put.transform.Find("End").gameObject;
            transform.position = Endobj.transform.position;
            transform.rotation = Endobj.transform.rotation;
        }
        j += 10;
        if (j == 30)
            j = 0;
        Instantiate(Checkpoint, transform.position, transform.rotation);
    }

    public void Destroy_Road()
    {
        int i;
        for (i = 0; i < 10; i++)
            Destroy(index[i + u]);
        u += 10;
        if (u == 30)
            u = 0;
    }
}
