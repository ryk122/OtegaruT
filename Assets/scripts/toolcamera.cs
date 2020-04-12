using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toolcamera : MonoBehaviour
{
    [SerializeField]
    GameObject c1, c2;
    [SerializeField]
    Transform dir,car;
    [SerializeField]
    Text postext;


    bool s = false;
    // Start is called before the first frame update
    void Start()
    {
        car = c1.transform;
    }

    // Update is called once per frame
    void Update()
    {
        postext.text = "x: " + car.position.x + "\n" + "y: " + car.position.y + "\n" + "z: " + car.position.z;
        dir.eulerAngles = new Vector3(0, 0, 0);
        dir.position = new Vector3(car.position.x, car.position.y + 1, car.position.z);
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
