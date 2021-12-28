using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using LitJson;

public class TaskPanel : MonoBehaviour
{
    public Color selectedColor;
    public Color clickColor;
    public Color cancelColor;

    public Text[] clickText;

    private Button filterBtn;
    private Button recomBtn;
    private Button assignBtn;
    private Button mineBtn;
    private Button confirmBtn;
    private Button areaBtn;
    private Button typeBtn;

    private Text araeNameText;
    private Text typeNameText;
    private Text filterText;

    public GameObject[] clickHide;
    public GameObject taskItem;
    public GameObject araeItem;

    private GameObject filterPanel;
    private GameObject areaPanel;
    private GameObject typePanel;
    private GameObject filterObject;

    private Transform itemParent;
    private Transform mineParent;
    private Transform areaParent;
    private Transform typeParent;
    private Transform areaIcon;
    private Transform typeIcon;

    private bool isArea;
    private bool isType;
    private int index_click;
    private string provinceId;

    private Vector3 selectedAngle = new Vector3(0,0,90);
    private Vector3 cancelAngle = new Vector3(0,0,180);
    private List<TaskDetails> taskDetails = new List<TaskDetails>();
    private void Awake()
    {
        filterPanel = transform.Find("FilterPanel").gameObject;
        areaPanel = transform.Find("FilterPanel/Area").gameObject;
        typePanel = transform.Find("FilterPanel/Type").gameObject;
        filterObject=transform.Find("FilterBtn").gameObject;

        itemParent = transform.Find("TaskType/Viewport/Content");
        mineParent = transform.Find("MineTask/Viewport/Content");
        areaParent = transform.Find("FilterPanel/Area/Viewport/Content");
        typeParent = transform.Find("FilterPanel/Type/Viewport/Content");
        areaIcon = transform.Find("FilterPanel/AreaBtn/icon");
        typeIcon = transform.Find("FilterPanel/TypeBtn/icon");

        araeNameText = transform.Find("FilterPanel/AreaBtn/Text").GetComponent<Text>();
        typeNameText = transform.Find("FilterPanel/TypeBtn/Text").GetComponent<Text>();
        filterText = transform.Find("FilterBtn/Text").GetComponent<Text>();

        filterBtn = transform.Find("FilterBtn").GetComponent<Button>();
        recomBtn = transform.Find("Header/RecomBtn").GetComponent<Button>();
        assignBtn = transform.Find("Header/AssignBtn").GetComponent<Button>();
        mineBtn = transform.Find("Header/MineBtn").GetComponent<Button>();

        confirmBtn = transform.Find("FilterPanel/Button").GetComponent<Button>();
        areaBtn = transform.Find("FilterPanel/AreaBtn").GetComponent<Button>();
        typeBtn = transform.Find("FilterPanel/TypeBtn").GetComponent<Button>();

        filterBtn.onClick.AddListener(OpenFilter);
        recomBtn.onClick.AddListener(OpenRecommend);
        assignBtn.onClick.AddListener(OpenAssign);
        mineBtn.onClick.AddListener(OpenMine);

        confirmBtn.onClick.AddListener(ConfirmArea);
        areaBtn.onClick.AddListener(RegionSelection);
        typeBtn.onClick.AddListener(TypeSelection);

        StartCoroutine(RequestTaskType(DataTool.typeTaskUrl));
        StartCoroutine(RequestAddress(DataTool.placeTaskUrl));
        //StartCoroutine(RequestSeachTask(DataTool.seachTaskUrl,"0"));
    }

    private void SetTaskType()
    {
        if(index_click == 0)
        {
            filterObject.SetActive(true);
        }
        else if(index_click == 1)
        {
            filterObject.SetActive(false);
        }
        else if(index_click == 2)
        {
            filterObject.SetActive(false);
        }
        
    }

    private void OpenRecommend()
    {
        OpenSubscript(0);
    }

    private void OpenAssign()
    {
        OpenSubscript(1);
    }

    private void OpenMine()
    {
        OpenSubscript(2);
    }

    private void OpenFilter()
    {
        if(filterPanel.activeSelf)
        {
            filterText.color = cancelColor;
            filterPanel.SetActive(false);
        }
        else
        {
            isArea = true;
            isType = true;
            filterText.color = selectedColor;
            areaIcon.localEulerAngles = cancelAngle;
            typeIcon.localEulerAngles = cancelAngle;
            araeNameText.text = "地区";
            typeNameText.text = "类型";
            filterPanel.SetActive(true);
            areaPanel.SetActive(false);
            typePanel.SetActive(false);
        }
    }

    private void ConfirmArea()
    {
        if(araeNameText.text != "地区" && typeNameText.text != "类型")
        {
            StartCoroutine(RequestSeachTask(DataTool.seachTaskUrl,"0", provinceId, typeNameText.text));
        }
        filterPanel.SetActive(false);
    }

    private void RegionSelection()
    {
        if(isArea)
        {
            isArea = false;
            areaPanel.SetActive(true);
            areaIcon.localEulerAngles = selectedAngle;
        }
        else
        {
            isArea = true;
            areaPanel.SetActive(false);
            areaIcon.localEulerAngles = cancelAngle;
        }
    }

    private void TypeSelection()
    {
        if (isType)
        {
            isType = false;
            typePanel.SetActive(true);
            typeIcon.localEulerAngles = selectedAngle;
        }
        else
        {
            isType = true;
            typePanel.SetActive(false);
            typeIcon.localEulerAngles = cancelAngle;
        }
    }

    public void OpenSubscript(int index)
    {
        for (int i = 0; i < clickHide.Length; i++)
        {
            clickHide[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < clickText.Length; i++)
        {
            clickText[i].color = cancelColor;
        }
        index_click = index;
        clickHide[index_click].gameObject.SetActive(true);
        clickText[index_click].color = clickColor;
        SetTaskType();
        StartCoroutine(RequestSeachTask(DataTool.seachTaskUrl, index_click.ToString()));
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        filterPanel.SetActive(false);
        filterObject.SetActive(true);
        index_click = 0;
        filterText.color = cancelColor;
        OpenRecommend();
    }


    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    //筛选当前任务
    public void FilterSelection(string messgName,string provinceId)
    {
        if(!isArea)
        {
            isArea = true;
            areaPanel.SetActive(false);
            araeNameText.text = messgName;
            this.provinceId = provinceId;
        }
        else if (!isType)
        {
            isType = true;
            typePanel.SetActive(false);
            typeNameText.text = messgName;
        }
       
    }

    //获取任务类型列表
    private IEnumerator RequestTaskType(string url)
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
            //Debug.Log("任务类型" + webRequest.downloadHandler.text);
            Dictionary<string, object> taskType = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (taskType["msg"].ToString() == "SUCCESS")
            {
                List<object> taskInfo = taskType["data"] as List<object>;
                for (int i = 0; i < taskInfo.Count; i++)
                {
                    var item = Instantiate(araeItem);
                    item.SetActive(true);
                    item.transform.SetParent(typeParent);
                    item.transform.localScale = Vector3.one;
                    AraeClass arae = new AraeClass(item.transform);
                    arae.SetName(taskInfo[i].ToString(),"");
                }
                float typeY = taskInfo.Count * 100;
                typeParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, typeY);
            }
        }
    }

    //获取任务地址列表
    private IEnumerator RequestAddress(string url)
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
            //Debug.Log("行政区" + webRequest.downloadHandler.text);
            Dictionary<string, object> taskType = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (taskType["msg"].ToString() == "SUCCESS")
            {
                List<object> taskInfo = taskType["data"] as List<object>;
                for (int i = 0; i < taskInfo.Count; i++)
                {
                    Dictionary<string,object> info= taskInfo[i] as Dictionary<string, object>;
                    var item = Instantiate(araeItem);
                    item.SetActive(true);
                    item.transform.SetParent(areaParent);
                    item.transform.localScale = Vector3.one;
                    AraeClass arae = new AraeClass(item.transform);
                    arae.SetName(info["name"].ToString(), info["id"].ToString());
                }
                float araeY = taskInfo.Count * 100;
                areaParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, araeY);
            }
        }
    }

    //筛选任务
    private IEnumerator RequestSeachTask(string url,string taskType, string provinceId = "", string taskTypeName = "")
    {
        JsonData data = new JsonData();
        data["page"] = "1";
        data["pageSize"] = "100";
        data["taskType"] = taskType;
        data["provinceId"] = provinceId;
        data["taskTypeName"] = taskTypeName;
        if(taskType == "0")
        {
            data["lat"] = DataTool.latitude;
            data["lgn"] = DataTool.longitude;
        }
        else
        {
            data["lat"] = "";
            data["lgn"] = "";
        }

        UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        webRequest.SetRequestHeader("Authorization", DataTool.token);

        byte[] postBytes = System.Text.Encoding.Default.GetBytes(data.ToJson());
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        UIManager.Instance.MaskTest(true);
        yield return webRequest.SendWebRequest();
        UIManager.Instance.MaskTest(false);
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("任务"+ taskType + webRequest.downloadHandler.text);
            bool istaskType = taskType == "2";
            for (int i = 0; i < taskDetails.Count; i++)
            {
                taskDetails[i].gameObject.SetActive(false);
            }
            Dictionary<string, object> taskTotal = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (taskTotal["msg"].ToString() == "SUCCESS")
            {
                Dictionary<string, object> taskData = taskTotal["data"] as Dictionary<string, object>;
                List<object> taskList = taskData["list"] as List<object>;
                for (int i = 0; i < taskList.Count; i++)
                {
                    Dictionary<string, object> info = taskList[i] as Dictionary<string, object>;
                    if (i >= taskDetails.Count)
                    {
                        var item = Instantiate(taskItem);
                        item.SetActive(true);
                        item.transform.SetParent(itemParent);
                        item.transform.localScale = Vector3.one;
                        item.GetComponent<TaskDetails>().SetInfo(istaskType, taskList[i] as Dictionary<string, object>,i == taskList.Count-1);
                        taskDetails.Add(item.GetComponent<TaskDetails>());
                    }
                    else
                    {
                        taskDetails[i].gameObject.SetActive(true);
                        taskDetails[i].SetInfo(istaskType,taskList[i] as Dictionary<string, object>,i == taskList.Count - 1);
                    }
                }
                float maxY = taskList.Count * 180 + taskList.Count * 100;
                itemParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
            }
        }
    }
    class AraeClass
    {
        private Text nameText;
        private Button areaBtn;
        private string provinceId;
        public AraeClass(Transform parent)
        {
            areaBtn = parent.GetComponent<Button>();
            nameText = parent.GetComponent<Text>();
            areaBtn.onClick.AddListener(ClickEvent);
        }

        private void ClickEvent()
        {
            UIManager.Instance.taskPanel.FilterSelection(nameText.text,provinceId);
        }

        public void SetName(string messgName,string messgId)
        {
            provinceId = messgId;
            nameText.text = messgName;
        }

    }
}
