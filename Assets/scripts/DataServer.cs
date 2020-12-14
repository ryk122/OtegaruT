using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;



public class DataServer : MonoBehaviour
{
    Data sendData;

    [SerializeField]
    class Data
    {
        [SerializeField]
        int coin;
        [SerializeField]
        string os;

        public void Setcoin(int c)
        {
            coin = c;
        }

        public void Setos(string osname)
        {
            os = osname;
        }

        public Data()
        {
            coin = 0;
            os = "unknow";
        }
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckServerData()
    {
        StartCoroutine(GetRequest("https://ryuukun.web.fc2.com/otegaru/otegarudata.txt"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nServerData Received: " + webRequest.downloadHandler.text);
                if (webRequest.downloadHandler.text.Equals("y"))
                {
                    Debug.Log("Sending Log");
                    SetData();
                }
            }
        }
    }

    private void SetData()
    {
        sendData = new Data();
        sendData.Setcoin(PlayerPrefs.GetInt("money"));
#if UNITY_ANDROID
        sendData.Setos("android");
#elif UNITY_IPHONE
        sendData.Setos("ios");
#else
        sendData.Setos("unknown");
#endif
        StartCoroutine(SendJson());
    }

    public IEnumerator SendJson()
    {
        var json = JsonUtility.ToJson(sendData, true);

        //POSTメソッドのリクエストを作成
        UnityWebRequest request = new UnityWebRequest("https://us-central1-otegaru-api.cloudfunctions.net/active_logs", "POST");

        //json(string)をbyte[]に変換
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        //jsonを設定
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        //ヘッダーにタイプを設定
        request.SetRequestHeader("Content-Type", "application/json");


        yield return request.SendWebRequest();

        //3.isNetworkErrorとisHttpErrorでエラー判定
        if (request.isHttpError || request.isNetworkError)
        {
            //4.エラー確認
            Debug.Log(request.error);
        }
        else
        {
            //4.結果確認
            Debug.Log(request.downloadHandler.text);
        }
    }
}
