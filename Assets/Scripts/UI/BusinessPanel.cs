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
    public Transform conte;
    private Text nameText;
    private Text firmText;

    private RawImage licenseImage;
    private Button backBtn;
    private GameObject attestInfo;
    private Texture2D texture;
    private float m_Page = 0;
    public bool isStart = true;

    private float width;
    private void Awake()
    {
        width = Screen.width - 300;
        nameText = transform.Find("TopBg/NameText").GetComponent<Text>();
        firmText = transform.Find("TopBg/Text").GetComponent<Text>();

        attestInfo = transform.Find("InfoText").gameObject;//Mask
        licenseImage = transform.Find("Mask/License").GetComponent<RawImage>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel()
    {
        //DataTool.businessPic = "http://salary-file.oos-website-cn.oos-cn.ctyunapi.cn/businessLicense/2021-12-30/dd69858a12e04e599ec5a7a91db93339.jpg";
        //DataTool.businessPic = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=2673183312,4257398740&fm=26&gp=0.jpg";
        gameObject.SetActive(true);
        if(isStart)
        {
            isStart = false;
            licenseImage.gameObject.SetActive(false);
            StartCoroutine(DownSprite());
            firmText.text = DataTool.theCompany;
        }
    }

    private IEnumerator DownSprite()
    {
        UnityWebRequest webRequest = new UnityWebRequest(DataTool.businessPic);
        DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true);
        webRequest.downloadHandler = texD1;
        UIManager.Instance.loadingPanel.OpenPanel();
        yield return webRequest.SendWebRequest();
        UIManager.Instance.loadingPanel.ClosePanel();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            attestInfo.SetActive(true);
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            licenseImage.texture = texD1.texture;
            licenseImage.SetNativeSize();
            licenseImage.gameObject.SetActive(true);

            Vector2 canvasSize = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta;
            //当前画布尺寸长宽比
            float screenxyRate =  canvasSize.x / canvasSize.y ;
            Debug.Log("X:"+ canvasSize.x+"Y:"+ canvasSize.y+"bizhi："+screenxyRate);
            Vector2 licenseSize = licenseImage.GetComponent<RectTransform>().sizeDelta;

            float licenseRate = licenseSize.x/licenseSize.y;
            Debug.Log("X:" + licenseSize.x + "Y:" + licenseSize.y + "bizhi：" + licenseRate);
            if(screenxyRate >= licenseRate)
            {
                float y = canvasSize.y * 0.6f;
                float offef = y / licenseSize.y;
                licenseImage.GetComponent<RectTransform>().sizeDelta = new Vector2(licenseSize.x * offef, y);
            }
            else
            {
                float x = canvasSize.x * 0.6f;
                float offef = x / licenseSize.x;
                licenseImage.GetComponent<RectTransform>().sizeDelta = new Vector2(x, licenseSize.y * offef);
            }
        }
    }
    private void GetData(Vector2 canvasSize, Vector2 licenseSize, float licenseRate, float screenxyRate)
    {
        if (licenseRate > screenxyRate)
        {
            int newSizeY = Mathf.CeilToInt(canvasSize.y);
            int newSizeX = Mathf.CeilToInt((float)newSizeY / licenseSize.y * licenseSize.x);
            licenseImage.GetComponent<RectTransform>().sizeDelta = new Vector2(newSizeX, newSizeY);
        }
        else
        {
            int newVideoSizeX = Mathf.CeilToInt(canvasSize.x);
            int newVideoSizeY = Mathf.CeilToInt((float)newVideoSizeX / licenseSize.x * licenseSize.y);
            licenseImage.GetComponent<RectTransform>().sizeDelta = new Vector2(newVideoSizeX, newVideoSizeY);
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
