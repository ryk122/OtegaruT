﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSeteer : MonoBehaviour
{
    public Material day, sunset, night;
    public Light light;


    // Start is called before the first frame update
    void Start()
    {
        int time;
        //PlayerPrefs.SetInt("time", 0);
        time = PlayerPrefs.GetInt("time");
        
        //0 day  1 sunset  2 night
        if (time == 0)
        {
            light.intensity = 1.0f;
            RenderSettings.ambientIntensity = 1.5f;
            RenderSettings.skybox = day;
        }
        else if (time == 1)
        {
            light.intensity = 0.5f;
            light.color = new Color32(255,128,0,1);
            RenderSettings.ambientIntensity = 0.5f;
            RenderSettings.skybox = sunset;
        }
        else if (time == 2)
        {
            light.intensity = 0.1f;
            RenderSettings.ambientIntensity = 0.2f;
            RenderSettings.skybox = night;
        }
        else
        {
            PlayerPrefs.SetInt("time", 2);
            Start();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
