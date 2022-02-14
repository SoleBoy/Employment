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
    private Text attestInfo;

    private RawImage licenseImage;
    private Button backBtn;
  
    private Vector2 nTouchPos1;
    private Vector2 nTouchPos2;
    private Vector2 oTouchPos1;
    private Vector2 oTouchPos2;

    private float selfScaling = 1;

    public bool isStart = true;

    private float screenxyRate;
    private void Awake()
    {
        screenxyRate = DataTool.canvasSize.x / DataTool.canvasSize.y;
        nameText = transform.Find("TopBg/NameText").GetComponent<Text>();
        firmText = transform.Find("TopBg/Text").GetComponent<Text>();

        attestInfo = transform.Find("InfoText").GetComponent<Text>();
        licenseImage = transform.Find("Mask/License").GetComponent<RawImage>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel()
    {
        //DataTool.businessPic = "http://salary-file.oos-website-cn.oos-cn.ctyunapi.cn/businessLicense/2021-12-30/dd69858a12e04e599ec5a7a91db93339.jpg";
        //DataTool.businessPic = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=2673183312,4257398740&fm=26&gp=0.jpg";
        gameObject.SetActive(true);
        selfScaling = licenseImage.transform.localScale.x;
        if (isStart)
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
        //UIManager.Instance.loadingPanel.OpenPanel();
        yield return webRequest.SendWebRequest();
        //UIManager.Instance.loadingPanel.ClosePanel();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            attestInfo.gameObject.SetActive(true);
            attestInfo.text = webRequest.error;
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            attestInfo.gameObject.SetActive(false);
            licenseImage.texture = texD1.texture;
            licenseImage.SetNativeSize();
            licenseImage.gameObject.SetActive(true);

            Vector2 licenseSize = licenseImage.GetComponent<RectTransform>().sizeDelta;
            float licenseRate = licenseSize.x/licenseSize.y;
            //Debug.Log("X:" + licenseSize.x + "Y:" + licenseSize.y + "bizhi：" + licenseRate);
            if(screenxyRate >= licenseRate)
            {
                float y = DataTool.canvasSize.y * 0.6f;
                float offef = y / licenseSize.y;
                licenseImage.GetComponent<RectTransform>().sizeDelta = new Vector2(licenseSize.x * offef, y);
            }
            else
            {
                float x = DataTool.canvasSize.x * 0.6f;
                float offef = x / licenseSize.x;
                licenseImage.GetComponent<RectTransform>().sizeDelta = new Vector2(x, licenseSize.y * offef);
            }
        }
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            ZoomInOut();
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                selfScaling -= 0.1f;
                selfScaling = Mathf.Clamp(selfScaling, 0.1f, 5);
                licenseImage.transform.localScale = Vector3.one * selfScaling;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                selfScaling += 0.1f;
                selfScaling = Mathf.Clamp(selfScaling, 0.1f, 5);
                licenseImage.transform.localScale = Vector3.one * selfScaling;
            }
        }
    }

    void ZoomInOut()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            nTouchPos1 = Input.GetTouch(0).position;
            nTouchPos2 = Input.GetTouch(1).position;

            if (IsEnLarge(nTouchPos1, nTouchPos2, oTouchPos1, oTouchPos2))
            {
                selfScaling += 0.05f;
                selfScaling = Mathf.Clamp(selfScaling, 0.5f, 5);
                licenseImage.transform.localScale = Vector3.one * selfScaling;
            }
            else
            {
                selfScaling -= 0.05f;
                selfScaling = Mathf.Clamp(selfScaling, 0.5f, 5);
                licenseImage.transform.localScale = Vector3.one * selfScaling;
            }

            oTouchPos1 = nTouchPos1;
            oTouchPos2 = nTouchPos2;
        }
    }

    bool IsEnLarge(Vector2 nPos1, Vector2 nPos2, Vector2 oPos1, Vector2 oPos2)
    {
        float nDis = Vector2.Distance(nPos1, nPos2);
        float oDis = Vector2.Distance(oPos1, oPos2);

        if (nDis < oDis)
        {
            return false;
        }
        else
        {
            return true;
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

}
