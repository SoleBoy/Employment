using LitJson;
using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InvitationPanel : MonoBehaviour
{
    private Button backBtn;
    private Button backBtn_Code;
    private Button updataBtn;

    private InputField codeText;
    private Button codeBtn;

    private GameObject updatePanel;
    private GameObject codePanel;
    private Button confirmBtn;
    private Text identityText;
    private Text nameText;
    private string urlCode = "http://appapi.brilliantnetwork.cn:5002/companyapi/company/getCompanyInfoByInvateCode?invateCode=";
    private void Awake()
    {
        codePanel = transform.Find("CodePanel").gameObject;
        updatePanel = transform.Find("UpdatePanel").gameObject;

        updataBtn = transform.Find("UpdatePanel/Button").GetComponent<Button>();
        confirmBtn = transform.Find("CodePanel/Button").GetComponent<Button>();
        backBtn_Code = transform.Find("CodePanel/BackBtn").GetComponent<Button>();
        identityText = transform.Find("CodePanel/Info/line/Name").GetComponent<Text>();
        nameText = transform.Find("CodePanel/Info/line/Code").GetComponent<Text>();

        codeText = transform.Find("Info/InputField").GetComponent<InputField>();
        codeBtn = transform.Find("Info/Button").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        updataBtn.onClick.AddListener(ClosePanel);
        codeBtn.onClick.AddListener(OpenCode);
        confirmBtn.onClick.AddListener(FirmPanel);
        backBtn_Code.onClick.AddListener(BackCode);
    }

    private void FirmPanel()
    {
        StartCoroutine(RequestCode(DataTool.invateCodeUrl, codeText.text));
    }

    private void BackCode()
    {
        codePanel.SetActive(false);
    }

    private void OpenCode()
    {
        if (codeText.text.Length == 8)
        {
            StartCoroutine(RequestIfo(string.Format("{0}{1}", urlCode, codeText.text)));
        }
        else
        {
            UIManager.Instance.CloningTips("请输入正确的邀请码");
        }
    }

    public void InputSpace(Text showText)
    {
        showText.text = System.Text.RegularExpressions.Regex.Replace(codeText.text, @"(\w{4})", "$1 ").Trim(' ');
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        codeText.text = "";
        codePanel.SetActive(false);
        updatePanel.SetActive(false);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator RequestIfo(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.SetRequestHeader("Authorization", DataTool.token);

        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("邀请码信息" + webRequest.downloadHandler.text);
            Dictionary<string, object> codeInfo = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (codeInfo["msg"].ToString() == "SUCCESS")
            {
                Dictionary<string, object> code = codeInfo["data"] as Dictionary<string, object>;
                codePanel.SetActive(true);
                DataTool.inviteCode = code["invateCode"].ToString();
                DataTool.theCompany = code["companyName"].ToString();
                identityText.text = DataTool.inviteCode;
                nameText.text = DataTool.theCompany;
            }
            else
            {
                UIManager.Instance.CloningTips(codeInfo["msg"].ToString());
            }
        }
    }

    //更新邀请码
    private IEnumerator RequestCode(string url,string code)
    {
        JsonData data = new JsonData();
        data["invateCode"] = code;

        UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
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
            Debug.Log("邀请码信息" + webRequest.downloadHandler.text);
            Dictionary<string, object> codeInfo = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (codeInfo["msg"].ToString() == "SUCCESS")
            {
                updatePanel.SetActive(true);
                UIManager.Instance.unitPanel.UpdateStatus(0, "已输入");
            }
            else
            {
                UIManager.Instance.CloningTips(codeInfo["msg"].ToString());
            }
        }
    }
}
