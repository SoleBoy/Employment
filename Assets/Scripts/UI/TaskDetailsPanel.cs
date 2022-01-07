using LitJson;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TaskDetailsPanel : MonoBehaviour
{
    public Transform toolParent;
    private Text taskType;
    private Text taskAmount;
    private Text taskCycle;
    private Text taskUnivalent;
    private Text taskInfo;

    private Button backBtn;
    private string currentId;
    private void Awake()
    {
        taskType = transform.Find("TopBg/DateText").GetComponent<Text>();
        taskAmount = transform.Find("TopBg/WageText").GetComponent<Text>();
        taskCycle = transform.Find("Header/cycle").GetComponent<Text>();
        taskUnivalent = transform.Find("Header/univalent ").GetComponent<Text>();
        taskInfo = toolParent.Find("Text").GetComponent<Text>();

        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel(string taskId)
    {
        gameObject.SetActive(true);
        if(currentId != taskId)
        {
            currentId = taskId;
            taskType.text = "";
            taskAmount.text = "";
            taskCycle.text = "";
            taskUnivalent.text = "";
            taskInfo.text = "";
            StartCoroutine(InfoTask(DataTool.salaryDetailsUrl));
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    //infoTaskUrl
    private IEnumerator InfoTask(string url)
    {
        string dataUrl = string.Format("{0}{1}", url, currentId);
        UnityWebRequest webRequest = UnityWebRequest.Get(dataUrl);
        webRequest.SetRequestHeader("Authorization", DataTool.token);

        //UIManager.Instance.loadingPanel.OpenPanel();
        yield return webRequest.SendWebRequest();
        //UIManager.Instance.loadingPanel.ClosePanel();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("任务详情" + webRequest.downloadHandler.text);
            Dictionary<string, object> pageData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (pageData["code"].ToString() == "0")
            {
                Dictionary<string, object> infoData = pageData["data"] as Dictionary<string, object>;
                if(infoData["title"] != null)
                    taskType.text = infoData["title"].ToString();
                if (infoData["fee"] != null)
                    taskAmount.text = string.Format("￥{0:N2}", float.Parse(infoData["fee"].ToString()));
                if (infoData["time"] != null && infoData["timeHHmm"] != null)
                    taskCycle.text = string.Format("{0}\n{1}", infoData["time"].ToString(), infoData["timeHHmm"].ToString()); ;//
                if (infoData["unitAmount"] != null && infoData["billingUnit"] != null)
                    taskUnivalent.text = string.Format("￥{0:N2}元/{1}", float.Parse(infoData["unitAmount"].ToString()), infoData["billingUnit"].ToString());
                if (infoData["taskResult"] != null)
                    taskInfo.text = infoData["taskResult"].ToString();
            }
            Invoke("Dely", 1);
        }
    }
    void Dely()
    {
        //Debug.Log(taskInfo.GetComponent<RectTransform>().sizeDelta.y);
        toolParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, taskInfo.GetComponent<RectTransform>().sizeDelta.y);
    }
}
