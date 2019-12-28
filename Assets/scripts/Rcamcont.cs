using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rcamcont : MonoBehaviour
{
    public GameObject bt;
    Camera cam;
    int on;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        on = PlayerPrefs.GetInt("mir");
        if (on == 1)
        {
            cam.enabled = true;
            bt.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (on == 0)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                cam.enabled = true;
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                cam.enabled = false;
            }
        }
    }

    public void U()
    {
        if (on == 0)
            cam.enabled = false;
    }

    public void D()
    {
        cam.enabled = true;
    }
}
