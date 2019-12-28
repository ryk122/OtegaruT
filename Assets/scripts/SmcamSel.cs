using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmcamSel : MonoBehaviour {
    public UnityStandardAssets.Utility.SmoothFollow smf;
    // Use this for initialization
    void Start () {
        smf.target = transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
