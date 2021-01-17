using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObject : MonoBehaviour
{
    [SerializeField]
    AudioSource ads;
    [SerializeField]
    ParticleSystem particle;

    bool d;

    // Start is called before the first frame update
    void Start()
    {
        d = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += transform.up * Time.deltaTime *100;
    }

    public void GetCoin()
    {
        if (d) return;

        d = true;
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 10);
        ads.Play();
        particle.Play();
        Destroy(this.gameObject, 0.2f);

    }
}
