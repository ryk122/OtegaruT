using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolButton : MonoBehaviour
{
    [SerializeField]
    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void camUp()
    {
        cam.position += new Vector3(0, 10, 0);

    }

    public void camDn()
    {
        cam.position += new Vector3(0, -10, 0);
    }

    public void Exit()
    {
        SceneManager.LoadScene("gasaki_make");
    }
}
