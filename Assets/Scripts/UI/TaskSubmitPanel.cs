using LitJson;
using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TaskSubmitPanel : MonoBehaviour
{
    private Button confirmBtn;
    private Button backBtn;
    private Button sureBtn;
    private Button cerBtn;
    private Button cancelBtn;
    private Button replenish;

    private Text cerText;
    private RawImage rawImage;

    private GameObject successPanel;
    private GameObject replenishPanel;

    private string taskId;
    private string taskType;
    private TaskStatus taskStatus;
    private void Awake()
    {
        successPanel = transform.Find("success").gameObject;
        replenishPanel = transform.Find("ClockPanel").gameObject;

        cerText = transform.Find("ClockPanel/Certificate/Text").GetComponent<Text>();
        confirmBtn = transform.Find("Confirm").GetComponent<Button>();
        backBtn = transform.Find("Image/BackBtn").GetComponent<Button>();
        sureBtn = transform.Find("success/Sure").GetComponent<Button>();
        cerBtn = transform.Find("ClockPanel/Certificate").GetComponent<Button>();
        cancelBtn = transform.Find("ClockPanel/Cancel").GetComponent<Button>();
        replenish = transform.Find("Replenish").GetComponent<Button>();

        rawImage = transform.Find("ClockPanel/RawImage").GetComponent<RawImage>();

        confirmBtn.onClick.AddListener(OpenSuccess);
        cerBtn.onClick.AddListener(OpenSuccess);
        sureBtn.onClick.AddListener(AurePanel);
        backBtn.onClick.AddListener(ClosePanel);
        cancelBtn.onClick.AddListener(ClosePanel);
        replenish.onClick.AddListener(OpenReplenish);
    }

    private void OpenReplenish()
    {
        UIManager.Instance.photographPanel.OpenPanel(false, "请拍摄相关照片并提交");
    }

    // DataTool.salaryEntry = SalaryEntry.submit;
    public void OpenReplenish(bool isFlip)
    {
        replenishPanel.SetActive(true);
        rawImage.texture = ByteToTex2d(DataTool.cheackByte);
        if (isFlip)
        {
            rawImage.transform.localEulerAngles = DataTool.frontAngle;
        }
        else
        {
            rawImage.transform.localEulerAngles = DataTool.rearAngle;
        }
    }
    public static Texture2D ByteToTex2d(byte[] bytes)
    {
        Texture2D tex = new Texture2D(1015, 850);
        tex.LoadImage(bytes);
        return tex;
    }
    //
    private void OpenSuccess()
    {
        DataTool.salaryEntry = SalaryEntry.submit;
        if (Application.platform == RuntimePlatform.Android)
        {
            DataTool.CallClockInfo();
        }
        else
        {
            UIManager.Instance.Location_Android(UIManager.Instance.loadTxt.TxtFile[11].ToString());
        }
        //successPanel.SetActive(true);
    }

    public void OpenSubmit()
    {
        switch (taskStatus)
        {
            case TaskStatus.start:
                StartCoroutine(RequestAddress());
                break;
            case TaskStatus.submit:
                StartCoroutine(TaskOrder(DataTool.submitTaskUrl));
                break;
            default:
                break;
        }
       
    }

    //DataTool.checkAddress
    public void OpenPanel(TaskStatus taskStatus, string taskId, string taskType)
    {
        this.taskId = taskId;
        this.taskType = taskType; 
        this.taskStatus = taskStatus;
        gameObject.SetActive(true);
        successPanel.SetActive(false);
        switch (taskStatus)
        {
            case TaskStatus.start:
                cerText.text = "提交开始凭证";
                replenishPanel.SetActive(true);
                UIManager.Instance.photographPanel.OpenPanel(false, "请拍摄相关照片并提交确认开始任务");
                break;
            case TaskStatus.submit:
                cerText.text = "提交任务凭证";
                replenishPanel.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void AurePanel()
    {
        gameObject.SetActive(false);
        UIManager.Instance.taskPanel.OpenSubscript(2);
    }

    //提交任务
    private IEnumerator TaskOrder(string url)
    {
        JsonData data = new JsonData();
        data["address"] = DataTool.clockInAddress; 
        data["id"] = taskId; 
        data["lat"] = DataTool.latitude;
        data["lgn"] = DataTool.longitude;
        data["pic"] = DataTool.checkAddress;
        Debug.Log("数据"+ data.ToJson());
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
            Debug.Log("提交任务" + webRequest.downloadHandler.text);
            Dictionary<string, object> codeInfo = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (codeInfo["msg"].ToString() == "SUCCESS")
            {
                replenishPanel.SetActive(false);
                successPanel.SetActive(true);
                StartCoroutine(UIManager.Instance.CurretAddress(DataTool.currentTaskUrl));
                UIManager.Instance.hallPanel.UpdateTask();
            }
            else
            {
                UIManager.Instance.CloningTips(codeInfo["msg"].ToString());
            }
        }
    }

    private IEnumerator RequestAddress()
    {
        JsonData data = new JsonData();
        data["lat"] = DataTool.latitude;
        data["lgn"] = DataTool.longitude;
        data["lockType"] = "1";   ///0 登录 1.主动 2.已接单 4：待开始 5：进行中
        data["pic"] = DataTool.checkAddress;
        data["clockInAddress"] = DataTool.clockInAddress;
        data["taskId"] = taskId;
        Debug.Log("数据" + data.ToJson());

        UnityWebRequest webRequest = new UnityWebRequest(DataTool.clockUrl, "POST");
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
            Debug.Log("任务开始" + webRequest.downloadHandler.text);
            Dictionary<string, object> clockData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (clockData["msg"].ToString() == "SUCCESS")
            {
                replenishPanel.SetActive(false);
                successPanel.SetActive(true);
                StartCoroutine(UIManager.Instance.CurretAddress(DataTool.currentTaskUrl));
            }
            else
            {
                UIManager.Instance.CloningTips(clockData["msg"].ToString());
            }
        }
    }
}
