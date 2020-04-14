using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class News : MonoBehaviour
{
    public GameObject newspanel;
    [SerializeField]
    int buildnum;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("news"))
            PlayerPrefs.SetInt("news", 0);

        int n = PlayerPrefs.GetInt("news");
        if (n != buildnum)
        {
            if (buildnum == 413)
            {
                int c = PlayerPrefs.GetInt("money");
                PlayerPrefs.SetInt("money", c + 100);
            }
            newspanel.SetActive(true);
            PlayerPrefs.SetInt("news", buildnum);
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
