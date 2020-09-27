//https://qiita.com/sakano/items/918c090f484c0610619d
//and
//https://qiita.com/a_nishimura/items/c10a10e95f0ea1a27009
//edited by ryk


using UnityEngine;

/// <summary>
/// The camera added this script will follow the specified object.
/// The camera can be moved by left mouse drag and mouse wheel.
/// </summary>
[ExecuteInEditMode, DisallowMultipleComponent]
public class FollowingCamera : MonoBehaviour
{
    public GameObject target; // an object to follow
    public Vector3 offset; // offset form the target object

    [SerializeField] private float distance = 4.0f; // distance from following object
    [SerializeField] private float polarAngle = 45.0f; // angle with y-axis
    [SerializeField] private float azimuthalAngle = 45.0f; // angle with x-axis

    [SerializeField] private float minDistance = 1.0f;
    [SerializeField] private float maxDistance = 7.0f;
    [SerializeField] private float minPolarAngle = 5.0f;
    [SerializeField] private float maxPolarAngle = 75.0f;
    [SerializeField] private float mouseXSensitivity = 5.0f;
    [SerializeField] private float mouseYSensitivity = 5.0f;
    [SerializeField] private float scrollSensitivity = 5.0f;

    private Vector2 mousepos;

    //カメラ視覚の範囲
    float viewMin = 20.0f;
    float viewMax = 60.0f;

    //直前の2点間の距離.
    private float backDist = 0.0f;
    //初期値
    float view = 60.0f;
    float v = 4.0f;

    private void Start()
    {
        mousepos = new Vector2(0, 0);
    }

    void Update()
    {
        if (Input.touchCount >= 2)
        {
            // タッチしている２点を取得
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            //2点タッチ開始時の距離を記憶
            if (t2.phase == TouchPhase.Began)
            {
                backDist = Vector2.Distance(t1.position, t2.position);
            }
            else if (t1.phase == TouchPhase.Moved && t2.phase == TouchPhase.Moved)
            {
                // タッチ位置の移動後、長さを再測し、前回の距離からの相対値を取る。
                float newDist = Vector2.Distance(t1.position, t2.position);
                view = view + (backDist - newDist) / 100.0f;
                v = v - (newDist - backDist) / 1000.0f;

                // 限界値をオーバーした際の処理
                if (v > maxDistance)
                {
                    v = maxDistance;
                }
                else if (v < minDistance)
                {
                    v = minDistance;
                }

                // 相対値が変更した場合、カメラに相対値を反映させる
                if (v != 0)
                {
                    //map.transform.localScale = new Vector3(v, v, 1.0f);
                    distance = v;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {

            //updateAngle(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); only this line...
            if(mousepos.x!=0)
                updateAngle((Input.mousePosition.x-mousepos.x)*0.1f,(Input.mousePosition.y-mousepos.y)*0.1f);
            mousepos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mousepos = new Vector2(0, 0);
        }
        updateDistance(Input.GetAxis("Mouse ScrollWheel"));

        var lookAtPos = target.transform.position + offset;
        updatePosition(lookAtPos);
        transform.LookAt(lookAtPos);
    }

    void updateAngle(float x, float y)
    {
        x = azimuthalAngle - x * mouseXSensitivity;
        azimuthalAngle = Mathf.Repeat(x, 360);

        y = polarAngle + y * mouseYSensitivity;
        polarAngle = Mathf.Clamp(y, minPolarAngle, maxPolarAngle);
    }

    void updateDistance(float scroll)
    {
        scroll = distance - scroll * scrollSensitivity;
        distance = Mathf.Clamp(scroll, minDistance, maxDistance);
    }

    void updatePosition(Vector3 lookAtPos)
    {
        var da = azimuthalAngle * Mathf.Deg2Rad;
        var dp = polarAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(
            lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),
            lookAtPos.y + distance * Mathf.Cos(dp),
            lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
    }
}
