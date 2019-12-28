using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDisp : MonoBehaviour
{
    public GameObject[] button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            button[1].SetActive(true);
        else
            button[1].SetActive(false);

        if (Input.GetKey(KeyCode.LeftArrow))
            button[0].SetActive(true);
        else
            button[0].SetActive(false);

        if (Input.GetKey(KeyCode.DownArrow))
            button[2].SetActive(true);
        else
            button[2].SetActive(false);

        if (Input.GetKey(KeyCode.UpArrow))
            button[3].SetActive(true);
        else
            button[3].SetActive(false);

    }
}
