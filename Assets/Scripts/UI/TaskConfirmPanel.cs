using LitJson;
using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TaskConfirmPanel : MonoBehaviour
{
    private Button fixBtn;
    private Button backBtn;
    private Button cloceBtn;

    private GameObject successPanel;

    private string taskId;
    private void Awake()
    {
        successPanel = transform.Find("success").gameObject;

        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        fixBtn = transform.Find("Button").GetComponent<Button>();
        cloceBtn = transform.Find("success/Button").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        fixBtn.onClick.AddListener(OpenSuccess);
        cloceBtn.onClick.AddListener(BackTask);
    }

    private void OpenSuccess()
    {
        StartCoroutine(TaskOrder(DataTool.receiverTaskUrl));
    }

    private void BackTask()
    {
        gameObject.SetActive(false);
        UIManager.Instance.taskPanel.OpenSubscript(2);
    }

    public void OpenPanel(string taskId)
    {
        this.taskId = taskId;
        gameObject.SetActive(true);
        successPanel.SetActive(false);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }


    //筛选任务
    private IEnumerator TaskOrder(string url)
    {
        JsonData data = new JsonData();
        data["taskId"] = taskId;

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
            Debug.Log("接单"+webRequest.downloadHandler.text);
            Dictionary<string, object> clockData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (clockData["msg"].ToString() == "SUCCESS")
            {
                successPanel.SetActive(true);
            }
            else
            {
                UIManager.Instance.CloningTips(clockData["msg"].ToString());
            }
        }
    }
}
