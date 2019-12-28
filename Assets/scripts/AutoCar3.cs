using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoCar3 : MonoBehaviour
{
    public GameObject NextTarget;
    public Carmain cm;
    private int targetnum;
    private float deg;

    int t=0;

    // Start is called before the first frame update
    void Start()
    {
        float stren = PlayerPrefs.GetFloat("cpstren");
        if (stren < 0.9f) stren -= 1;
        cm.maxs *= 0.9f + (stren * 0.1f);
        targetnum = 0;
        ChangeTarget();
    }

    // Update is called once per frame
    void Update()
    {

        //calc deg
        Vector3 t = (NextTarget.transform.position - transform.position);

        float dot = Vector3.Dot(t, transform.forward) / ( Vector3.Magnitude(t)*Vector3.Magnitude(transform.forward) );
        float theata = Mathf.Acos(dot);

        float dot2 = Vector3.Dot(t, transform.right) / (Vector3.Magnitude(t) * Vector3.Magnitude(transform.right));

        deg = theata * 180 / Mathf.PI;
        if (dot2 < 0) deg *= -1;
        //Debug.Log(deg);


    }

    private void FixedUpdate()
    {
        //drive car
        //=turn
        if (deg > 3) cm.TR(1);
        else if (deg < -3) cm.TR(-1);
        else cm.AndrC();

       if (t>80 && Mathf.Abs(deg) > 30) { cm.back = -1; cm.Back(); t = 0; cm.Run(); }
       else { t++;  if (Mathf.Abs(deg) < 30 || cm.speed < 5) cm.Run(); else cm.N(); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == NextTarget.name)
        {
            targetnum++;
            if (targetnum == 1000) targetnum = 0;
            ChangeTarget();
        }
    }

    void ChangeTarget()
    {

        //NextTarget= GameObject.Find("Target" + targetnum.ToString()).gameObject;
        NextTarget = TargetScript.target[targetnum];
        if (NextTarget == null) { ChangeTarget(); }
        Debug.Log("Target" + targetnum.ToString()+";"+targetnum);
    }
}
