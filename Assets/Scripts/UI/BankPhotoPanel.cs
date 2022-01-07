using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BankPhotoPanel : MonoBehaviour
{
    private Text nameText;
    private Text firmText;

    private RawImage licenseImage;
    private Button backBtn;
    private GameObject attestInfo;

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

        attestInfo = transform.Find("InfoText").gameObject;//Mask
        licenseImage = transform.Find("Mask/License").GetComponent<RawImage>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        selfScaling = licenseImage.transform.localScale.x;
        if (isStart)
        {
            isStart = false;
            licenseImage.gameObject.SetActive(false);
            StartCoroutine(DownSprite());
            if (DataTool.theCompany == "")
            {
                firmText.text = string.Format("{0}(自由职业者)", DataTool.roleName);
            }
            else
            {
                firmText.text = string.Format("{0}({1})", DataTool.roleName, DataTool.theCompany);
            }
        }
    }

    private IEnumerator DownSprite()
    {
        UnityWebRequest webRequest = new UnityWebRequest(DataTool.bankCardPic);
        DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true);
        webRequest.downloadHandler = texD1;
        //UIManager.Instance.loadingPanel.OpenPanel();
        yield return webRequest.SendWebRequest();
        //UIManager.Instance.loadingPanel.ClosePanel();
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

            Vector2 licenseSize = licenseImage.GetComponent<RectTransform>().sizeDelta;
            float licenseRate = licenseSize.x / licenseSize.y;

            if (screenxyRate >= licenseRate)
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
        if(Input.touchCount == 2)
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
}
