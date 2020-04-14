using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyAdDisp : MonoBehaviour
{
    [SerializeField]
    string URI = "https://4.bp.blogspot.com/-4xxTe_qeV1E/Vd7FkNUlwjI/AAAAAAAAxFc/8u9MNKtg7gg/s800/syachiku.png";

    [SerializeField] Shader shader;

    IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URI);

        //画像を取得できるまで待つ
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //取得した画像のテクスチャをRawImageのテクスチャに張り付ける
            //_image.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Renderer rend = GetComponent<Renderer>();
            rend.material = new Material(shader);
            rend.material.mainTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            //_image.SetTexture("_MainTex", ((DownloadHandlerTexture)www.downloadHandler).texture);
            Debug.Log("ok");
        }
    }
}