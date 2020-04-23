using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
    public class TirerotOnline : MonoBehaviour
    {
        [SerializeField]
        CarmainOnline cm;
        public float p;


        private void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 rot = transform.eulerAngles;
            rot.z += cm.speed * cm.back * p * Time.deltaTime;
            transform.eulerAngles = rot;
        }

    }
}
