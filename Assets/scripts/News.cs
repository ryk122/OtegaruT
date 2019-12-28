using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class News : MonoBehaviour
{
    public GameObject newspanel;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("news"))
            PlayerPrefs.SetInt("news", 0);

        int n = PlayerPrefs.GetInt("news");
        if (n != 28)
        {
            newspanel.SetActive(true);
            PlayerPrefs.SetInt("news", 28);
        }
    }

    public void Close()
    {
        newspanel.SetActive(false);
    }

    public void Open()
    {
        newspanel.SetActive(true);
    }

}
