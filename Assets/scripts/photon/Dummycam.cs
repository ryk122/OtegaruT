using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Photon.Pun.Demo.PunBasics
{
    public class Dummycam : MonoBehaviour
    {
        public static Transform t;
        // Start is called before the first frame update
        void Start()
        {
            t = this.transform;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
