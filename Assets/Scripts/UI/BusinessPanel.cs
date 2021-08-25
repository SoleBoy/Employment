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

    public void OpenPanel(Dictionary<string, object> tokenData)
    {
        gameObject.SetActive(true);
        nameText.text = DataTool.roleName;
        firmText.text = DataTool.theCompany;
        UIManager.Instance.loadingPanel.OpenPanel();
        licenseImage.gameObject.SetActive(false);
        if (tokenData["code"].ToString() == "200")
        {
            attestInfo.SetActive(false);
            StartCoroutine(RequestAddress(tokenData["url"].ToString()));
        }
        else
        {
            attestInfo.SetActive(true);
            UIManager.Instance.loadingPanel.ClosePanel();
        }
    }
    private IEnumerator RequestAddress(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();
        UIManager.Instance.loadingPanel.ClosePanel();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            attestInfo.SetActive(true);
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
                texture = renderer.RenderPageToTexture(pdfDocument.GetPage((int)m_Page % pageCount), 834, 640);
                texture.filterMode = FilterMode.Bilinear;
                texture.anisoLevel = 8;
                licenseImage.texture = texture;
                licenseImage.SetNativeSize();
                licenseImage.gameObject.SetActive(true);
            }
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
