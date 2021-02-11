using UnityEngine;

public class ColorCamera : MonoBehaviour
{
    void Start()
    {
        Vector3 pos;
        pos = transform.position;
        float r = (float)Screen.height / (float)Screen.width;
        r *= r;
        transform.position = pos - new Vector3(r * 1.5f, 0, 0);
    }
}
