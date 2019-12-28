using UnityEngine;
using UnityEngine.UI;

public class OptionSetter : MonoBehaviour
{
    public AudioSource ads;
    public Image s,s2, d, g;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("bgm") == 0)
            ads.enabled = false;
        if (PlayerPrefs.GetInt("etext") == 0)
        {
            s.enabled = false;d.enabled = false;g.enabled = false;s2.enabled = false;
        }
    }

}
