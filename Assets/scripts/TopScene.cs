using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#pragma warning disable 649

public class TopScene : MonoBehaviour
{
    [SerializeField]
    GameObject loading;
    // Start is called before the first frame update
    void Start()
    {
        //rot
        //
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToPortrait = false; // 縦
        Screen.autorotateToLandscapeLeft = true; // 左
        Screen.autorotateToLandscapeRight = true; // 右
        Screen.autorotateToPortraitUpsideDown = false; // 上下逆
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            loading.SetActive(true);
            SceneManager.LoadScene("title");
        }
    }
}
