using LitJson;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PhonePanel : MonoBehaviour
{
    public Color colorNor;
    public Color colorHig;

    private Button backBtn;
    private Button replaceBtn;
    private Button getBtn;
    private Button nextBtn;
    private Button newBtn;
    private Button noBtn;
    private Button yesBtn;
    private Button successBtn;

    private Text titleText;
    private Text originalText;
    private Text replaceText;
    private Text confirmText;
    private Text timeText;
    private InputField inputCode;
    private InputField inputNew;
    private Image getImage;

    private GameObject originalPanel;
    private GameObject replacePanel;
    private GameObject newPanel;
    private GameObject confirmPanel;
    private GameObject successPanel;
    private string smstoken = "";
    private float lastTime;
    private float curretTime;
    private int indexPanel;
    private void Awake()
    {
        originalPanel = transform.Find("OriginalPanel").gameObject;
        replacePanel = transform.Find("ReplacePanel").gameObject;
        newPanel = transform.Find("NewPanel").gameObject;
        confirmPanel = transform.Find("ConfirmPanel").gameObject;
        successPanel = transform.Find("SuccessPanel").gameObject;

        titleText = transform.Find("TitleText").GetComponent<Text>();
        originalText = transform.Find("OriginalPanel/bg/Phone").GetComponent<Text>();
        replaceText = transform.Find("ReplacePanel/bg1/Phone").GetComponent<Text>();
        timeText = transform.Find("ReplacePanel/bg2/GetBtn/Text").GetComponent<Text>();
        confirmText = transform.Find("ConfirmPanel/bg1/Phone").GetComponent<Text>();

        inputCode = transform.Find("ReplacePanel/bg2/InputCode").GetComponent<InputField>();
        inputNew = transform.Find("NewPanel/bg1/InputNew").GetComponent<InputField>();
        getImage = transform.Find("ReplacePanel/bg2/GetBtn").GetComponent<Image>();

        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        replaceBtn = transform.Find("OriginalPanel/replace").GetComponent<Button>();
        getBtn = transform.Find("ReplacePanel/bg2/GetBtn").GetComponent<Button>();
        nextBtn = transform.Find("ReplacePanel/Next").GetComponent<Button>();
        newBtn = transform.Find("NewPanel/new").GetComponent<Button>();
        noBtn = transform.Find("ConfirmPanel/bg1/NoBtn").GetComponent<Button>();
        yesBtn = transform.Find("ConfirmPanel/bg1/YesBtn").GetComponent<Button>();
        successBtn = transform.Find("SuccessPanel/LogIn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        replaceBtn.onClick.AddListener(OpenReplace);
        getBtn.onClick.AddListener(GetVerification);
        nextBtn.onClick.AddListener(OpenNewPanel);
        newBtn.onClick.AddListener(OpenConfirm);
        noBtn.onClick.AddListener(CloseConfirm);
        yesBtn.onClick.AddListener(SendVerification);
        successBtn.onClick.AddListener(Success);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        originalPanel.SetActive(true);
        replacePanel.SetActive(false);
        newPanel.SetActive(false);
        confirmPanel.SetActive(false);
        successPanel.SetActive(false);
        lastTime = -60;
        indexPanel = 0;
        getBtn.enabled = true;
        inputCode.text = "";
        titleText.text = "登录手机号";
        getImage.color = colorHig;
        timeText.text = "获取验证码";
        originalText.text = string.Format("+86 {0}",DataTool.loginPhone);
    }

    public void ClosePanel()
    {
        if(indexPanel == 0)
        {
            gameObject.SetActive(false);
        }
        else if (indexPanel == 1)
        {
            indexPanel = 0;
            titleText.text = "登录手机号";
            replacePanel.SetActive(false);
        }
        else if (indexPanel == 2)
        {
            indexPanel = 1;
            titleText.text = "更换手机号";
            newPanel.SetActive(false);
        }
        else if (indexPanel == 3)
        {
            indexPanel = 2;
            confirmPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (lastTime >= 0)
        {
            curretTime += Time.deltaTime;
            if(curretTime >= 1)
            {
                curretTime = 0;
                lastTime -= 1;
                timeText.text = lastTime.ToString();
                if(lastTime <= 0)
                {
                    getBtn.enabled = true;
                    getImage.color = colorHig;
                    timeText.text = "获取验证码";
                }
            }
        }
    }

    private void OpenReplace()
    {
        indexPanel = 1;
        titleText.text = "更换手机号";
        replaceText.text = string.Format("+86 {0}", DataTool.loginPhone);
        replacePanel.SetActive(true);
    }

    private void GetVerification()
    {
        StartCoroutine(RequestSendsms(DataTool.sendsms));
    }

    private void OpenNewPanel()
    {
        if (inputCode.text == "")
        {
            UIManager.Instance.CloningTips("验证码不能为空");
        }
        else
        {
            StartCoroutine(RequestSmstoken());
        }
    }

    private void OpenConfirm()
    {
        if (inputNew.text == "")
        {
            UIManager.Instance.CloningTips("手机号不能为空");
        }
        else
        {
            indexPanel = 3;
            confirmPanel.SetActive(true);
            confirmText.text = string.Format("{0}", inputNew.text);
        }
    }

    private void CloseConfirm()
    {
        confirmPanel.SetActive(false);
    }

    private void SendVerification()
    {
        StartCoroutine(RequestMobile());
    }

    private void Success()
    {
        DataTool.StartActivity(1);
        gameObject.SetActive(false);
        UIManager.Instance.unitPanel.ClosePanel();
        UIManager.Instance.homePanel.OpenHome(); ;
    }

    //获取验证码
    public IEnumerator RequestSendsms(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(string.Format("{0}{1}", url,DataTool.loginPhone));
        webRequest.SetRequestHeader("Authorization", DataTool.token);
        smstoken = "";
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("当前验证码信息" + webRequest.downloadHandler.text);
            Dictionary<string, object> taskData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (taskData["code"].ToString() == "0")
            {
                if (taskData["data"] != null)
                    smstoken = taskData["data"].ToString();
                lastTime = 60;
                timeText.text = "60";
                getBtn.enabled = false;
                getImage.color = colorNor;
            }
            else
            {
                UIManager.Instance.CloningTips(taskData["msg"].ToString());
            }
        }
    }
    //验证验证码
    public IEnumerator RequestSmstoken()
    {
        string url = string.Format(DataTool.verifyCode, smstoken,inputCode.text, DataTool.loginPhone);
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.SetRequestHeader("Authorization", DataTool.token);

        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("验证验证码信息" + webRequest.downloadHandler.text);
            Dictionary<string, object> taskData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (taskData["code"].ToString() == "0")
            {
                lastTime = -10;
                indexPanel = 2;
                inputNew.text = "";
                newPanel.SetActive(true);
            }
            else
            {
                UIManager.Instance.CloningTips(taskData["msg"].ToString());
            }
        }
    }
    //更新手机号updateMobile
    public IEnumerator RequestMobile()
    {
        JsonData data = new JsonData();
        data["mobile"] = inputNew.text;

        UnityWebRequest webRequest = new UnityWebRequest(DataTool.updateMobile, "POST");
        webRequest.SetRequestHeader("Authorization", DataTool.token);

        byte[] postBytes = System.Text.Encoding.Default.GetBytes(data.ToJson());
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("更新手机号信息" + webRequest.downloadHandler.text);
            Dictionary<string, object> taskData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (taskData["code"].ToString() == "0")
            {
                DataTool.loginPhone = inputNew.text;
                successPanel.SetActive(true);
            }
            else
            {
                UIManager.Instance.CloningTips(taskData["msg"].ToString());
            }
        }
    }
}
