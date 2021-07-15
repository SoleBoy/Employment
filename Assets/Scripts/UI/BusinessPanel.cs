using MiniJSON;
using Paroxe.PdfRenderer;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BusinessPanel : MonoBehaviour
{
    private RawImage licenseImage;
    private Button backBtn;
    private GameObject attestInfo;
    private Texture2D texture;
    private float m_Page = 0;

    private void Awake()
    {
        attestInfo = transform.Find("InfoText").gameObject;
        licenseImage = transform.Find("License").GetComponent<RawImage>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel(Dictionary<string, object> tokenData)
    {
        UIManager.Instance.loadingPanel.OpenPanel();
        gameObject.SetActive(true);
        attestInfo.SetActive(false);
        licenseImage.gameObject.SetActive(false);
        try
        {
            StartCoroutine(RequestAddress(tokenData["url"].ToString()));
        }
        catch (System.Exception)
        {
            attestInfo.SetActive(true);
            UIManager.Instance.loadingPanel.ClosePanel();
        }
    }
    private IEnumerator RequestAddress(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
            PDFDocument pdfDocument = new PDFDocument(webRequest.downloadHandler.data, "");
            if (pdfDocument.IsValid)
            {
                int pageCount = pdfDocument.GetPageCount();

                PDFRenderer renderer = new PDFRenderer();
                texture = renderer.RenderPageToTexture(pdfDocument.GetPage((int)m_Page % pageCount), 1280, 900);
                texture.filterMode = FilterMode.Bilinear;
                texture.anisoLevel = 8;
                licenseImage.texture = texture;
                licenseImage.SetNativeSize();
                licenseImage.gameObject.SetActive(true);
                UIManager.Instance.loadingPanel.ClosePanel();
            }
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
