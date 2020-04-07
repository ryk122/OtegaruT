using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolcamera : MonoBehaviour
{
    [SerializeField]
    GameObject c1, c2;

    bool s = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeC()
    {
        if (s)
        {
            c2.SetActive(true);c1.SetActive(false);
            s = false;
        }
        else
        {
            c1.SetActive(true);c2.SetActive(false);
            s = true;
        }
    }
}
