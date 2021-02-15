using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LicensePlate : MonoBehaviour
{
    [SerializeField]
    Text num, hira, subNum;
    string[] data;

    // Start is called before the first frame update
    void Start()
    {
        int dcar = PlayerPrefs.GetInt("dcar");
        string s = PlayerPrefs.GetString("license" + dcar);
        data = s.Split(',');

        if (data.Length >= 3)
        {
            num.text = data[0];
            hira.text = data[1];
            subNum.text = data[2];
        }
    }
}
