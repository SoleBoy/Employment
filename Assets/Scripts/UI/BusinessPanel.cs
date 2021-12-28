using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BusinessPanel : MonoBehaviour
{

    private Text nameText;
    private Text firmText;

    private RawImage licenseImage;
    private Button backBtn;
    private GameObject attestInfo;
    private Texture2D texture;
    private float m_Page = 0;

    private void Awake()
    {
        nameText = transform.Find("TopBg/NameText").GetComponent<Text>();
        firmText = transform.Find("TopBg/Text").GetComponent<Text>();

        attestInfo = transform.Find("InfoText").gameObject;
        licenseImage = transform.Find("License").GetComponent<RawImage>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel(string tokenUrl)
    {
        //tokenUrl = "https://img-blog.csdnimg.cn/20200114151229684.jpg?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80NTAyMzMyOA==,size_16,color_FFFFFF,t_70";
        gameObject.SetActive(true);
        //nameText.text = DataTool.roleName;
        firmText.text = DataTool.theCompany;
        UIManager.Instance.loadingPanel.OpenPanel();
        licenseImage.gameObject.SetActive(false);
        StartCoroutine(DownSprite(tokenUrl));
    }

    private IEnumerator DownSprite(string url)
    {
        UnityWebRequest webRequest = new UnityWebRequest(url);
        DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true);
        webRequest.downloadHandler = texD1;
        yield return webRequest.SendWebRequest();
        UIManager.Instance.loadingPanel.ClosePanel();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            attestInfo.SetActive(true);
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            int width = 834;
            int high = 640;
            Texture2D tex = new Texture2D(width, high);
            tex = texD1.texture;

            licenseImage.texture = tex;
            licenseImage.SetNativeSize();
            licenseImage.gameObject.SetActive(true);
        }
    }


    //public void OpenPanel(Dictionary<string, object> tokenData)
    //{
    //    gameObject.SetActive(true);
    //    //nameText.text = DataTool.roleName;
    //    firmText.text = DataTool.theCompany;
    //    UIManager.Instance.loadingPanel.OpenPanel();
    //    licenseImage.gameObject.SetActive(false);
    //    if (tokenData["code"].ToString() == "200")
    //    {
    //        attestInfo.SetActive(false);
    //        StartCoroutine(RequestAddress(tokenData["url"].ToString()));
    //    }
    //    else
    //    {
    //        attestInfo.SetActive(true);
    //        UIManager.Instance.loadingPanel.ClosePanel();
    //    }
    //}
    //private IEnumerator RequestAddress(string url)
    //{
    //    UnityWebRequest webRequest = UnityWebRequest.Get(url);
    //    yield return webRequest.SendWebRequest();
    //    UIManager.Instance.loadingPanel.ClosePanel();
    //    if (webRequest.isNetworkError || webRequest.error != null)
    //    {
    //        attestInfo.SetActive(true);
    //        Debug.Log("请求网络错误:" + webRequest.error);
    //    }
    //    else
    //    {
    //        Debug.Log(webRequest.downloadHandler.text);
    //        PDFDocument pdfDocument = new PDFDocument(webRequest.downloadHandler.data, "");
    //        if (pdfDocument.IsValid)
    //        {
    //            int pageCount = pdfDocument.GetPageCount();

    //            PDFRenderer renderer = new PDFRenderer();
    //            texture = renderer.RenderPageToTexture(pdfDocument.GetPage((int)m_Page % pageCount), 834, 640);
    //            texture.filterMode = FilterMode.Bilinear;
    //            texture.anisoLevel = 8;
    //            licenseImage.texture = texture;
    //            licenseImage.SetNativeSize();
    //            licenseImage.gameObject.SetActive(true);
    //        }
    //    }
    //}

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
