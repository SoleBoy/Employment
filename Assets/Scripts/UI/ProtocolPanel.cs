using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ProtocolPanel : MonoBehaviour
{
    private Button backBtn;

    public RawImage rawImage;
    public bool isStart;
    private void Awake()
    {
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        if(isStart)
        {
            isStart = false;
            StartCoroutine(DownSprite());
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator DownSprite()
    {
        UnityWebRequest webRequest = new UnityWebRequest(DataTool.signaturePic);
        DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true);
        webRequest.downloadHandler = texD1;
        yield return webRequest.SendWebRequest();
        //UIManager.Instance.loadingPanel.ClosePanel();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("获取签名照片" + webRequest.downloadHandler.text);
            int width = 150;
            int high = 100;
            Texture2D tex = new Texture2D(width, high);
            tex = texD1.texture;

            rawImage.texture = tex;
            //rawImage.SetNativeSize();
            rawImage.gameObject.SetActive(true);
        }
    }
}
