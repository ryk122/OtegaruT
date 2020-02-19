using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedmeter : MonoBehaviour
{
    [SerializeField]
    Carmain cm;
    [SerializeField]
    Text meter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        meter.text = ((int)cm.speed*5).ToString();
    }
}
