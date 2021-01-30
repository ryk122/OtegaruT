using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Share : MonoBehaviour
{
    [SerializeField]
    GameObject sharePanel;

    public void ShareButton()
    {
        //sharePanel.SetActive(true); 
        Application.OpenURL("https://twitter.com/intent/tweet?text=%23%E5%BE%A1%E6%89%8B%E8%BB%BDT%20%23otegaruT%0Ahttps%3A%2F%2Fryuukun.web.fc2.com%2Fotegaru%2Fdownload.html");
    }

    public void CloseShaer()
    {

    }
}
