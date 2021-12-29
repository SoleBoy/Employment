using LitJson;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TaskDetailsPanel : MonoBehaviour
{
    private Text taskType;
    private Text taskAmount;
    private Text taskCycle;
    private Text taskUnivalent;
    private Text taskInfo;

    private Button backBtn;

    private void Awake()
    {
        taskType = transform.Find("TopBg/DateText").GetComponent<Text>();
        taskAmount = transform.Find("TopBg/WageText").GetComponent<Text>();
        taskCycle = transform.Find("Header/cycle").GetComponent<Text>();
        taskUnivalent = transform.Find("Header/univalent ").GetComponent<Text>();
        taskInfo = transform.Find("Header/info").GetComponent<Text>();

        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel(string taskId)
    {
        gameObject.SetActive(true);
        StartCoroutine(InfoTask(DataTool.salaryDetailsUrl, taskId));
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    //infoTaskUrl
    private IEnumerator InfoTask(string url,string taskid)
    {
        string dataUrl = string.Format("{0}{1}", url, taskid);
        UnityWebRequest webRequest = UnityWebRequest.Get(dataUrl);
        webRequest.SetRequestHeader("Authorization", DataTool.token);

        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("任务详情" + webRequest.downloadHandler.text);
            Dictionary<string, object> pageData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (pageData["msg"].ToString() == "SUCCESS")
            {
                Dictionary<string, object> infoData = pageData["data"] as Dictionary<string, object>;
                taskType.text = infoData["title"].ToString();
                taskAmount.text = string.Format("￥{0:N2}", infoData["fee"].ToString());

                taskCycle.text = infoData["time"].ToString();
                taskUnivalent.text = string.Format("￥{0:N2}/{1}", infoData["unitAmount"].ToString(), infoData["billingUnit"].ToString());
                taskInfo.text = infoData["taskResult"].ToString();
            }
            else
            {
                taskType.text = "";
                taskAmount.text = "";
                taskCycle.text = "";
                taskUnivalent.text = "";
                taskInfo.text = "";
            }
        }
    }
    
}
