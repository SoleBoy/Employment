using LitJson;
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
    private Button Replenish;

    private RawImage rawImage;

    private GameObject successPanel;
    private GameObject replenishPanel;

    private string taskId;
    private void Awake()
    {
        successPanel = transform.Find("success").gameObject;
        replenishPanel = transform.Find("ClockPanel").gameObject;

        confirmBtn = transform.Find("Confirm").GetComponent<Button>();
        backBtn = transform.Find("Image/BackBtn").GetComponent<Button>();
        sureBtn = transform.Find("success/Sure").GetComponent<Button>();
        cerBtn = transform.Find("ClockPanel/Certificate").GetComponent<Button>();
        cancelBtn = transform.Find("ClockPanel/Cancel").GetComponent<Button>();
        Replenish = transform.Find("Replenish").GetComponent<Button>();

        rawImage = transform.Find("ClockPanel/RawImage").GetComponent<RawImage>();

        confirmBtn.onClick.AddListener(OpenSuccess);
        cerBtn.onClick.AddListener(OpenSuccess);
        sureBtn.onClick.AddListener(AurePanel);
        backBtn.onClick.AddListener(ClosePanel);
        cancelBtn.onClick.AddListener(ClosePanel);
        Replenish.onClick.AddListener(OpenReplenish);
    }

    private void OpenReplenish()
    {
        UIManager.Instance.photographPanel.OpenPanel(false);
    }

    // DataTool.salaryEntry = SalaryEntry.submit;
    public void OpenReplenish(Texture texture)
    {
        replenishPanel.SetActive(true);
        rawImage.texture = ByteToTex2d(DataTool.cheackByte);
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
        replenishPanel.SetActive(false);
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
        StartCoroutine(TaskOrder(DataTool.submitTaskUrl));
    }

    //DataTool.checkAddress
    public void OpenPanel(string taskId)
    {
        this.taskId = taskId;
        gameObject.SetActive(true);
        successPanel.SetActive(false);
        replenishPanel.SetActive(false);
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
        //Debug.Log("数据"+ data.ToJson());
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
            successPanel.SetActive(true);
        }
    }
}
