using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Notice : MonoBehaviour
{
    [SerializeField]
    int noticenum;
    [SerializeField]
    GameObject noticepanel;

    void Start()
    {
        if (!PlayerPrefs.HasKey("notice"))
            PlayerPrefs.SetInt("notice", 0);
        int n = PlayerPrefs.GetInt("notice");

        if (n != noticenum)
        {
            noticepanel.SetActive(true);
            PlayerPrefs.SetInt("notice", noticenum);
        }
    }


    public void AgreeNotice()
    {
        noticepanel.SetActive(false);
    }
    public void DegreeNotice()
    {
        SceneManager.LoadScene("title");
    }
}
