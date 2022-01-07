using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using MiniJSON;
using UnityEngine.Networking;
using LitJson;
using System;

public class UIManager : MonoSingleton<UIManager>
{
    public LoadTxt loadTxt;

    public PhotographPanel photographPanel;
    public HallPanel hallPanel;
    public HomePanel homePanel;
    public IncomePanel incomePanel;
    public MainPanel mainPanel;
    public DropPanel dropPanel;
    public PersonalPanel personalPanel;
    public UnitPanel unitPanel;
    public BusinessPanel businessPanel;
    public PrivacyPanel privacyPanel;
    public TermsPanel termsPanel;
    public LoadingPanel loadingPanel;
    public Employerpanel employerpanel;
    public ServicePanel servicePanel;
    public CheckPanel checkPanel;
    public TaskPanel taskPanel;
    public TaskConfirmPanel taskConfirmPanel;
    public ProtocolPanel protocolPanel;
    public BankCardPanel bankCardPanel;
    public InvitationPanel invitationPanel;
    public TaskDetailsPanel taskDetailsPanel;
    public SuccessCodePanel successCodePanel;
    public TaskSubmitPanel taskSubmitPanel;
    public ClockinPanel clockinPanel;
    public BankPhotoPanel bankPhotoPanel;

    private GameObject maskPanel;
    private Transform tipPanel;
    private Transform bloodParent;

    private bool isInit;
    public override void Init()
    {
        Debug.Log("初始信息");
        if(Application.platform == RuntimePlatform.Android)
        {
            AndroidStatusBar.statusBarState = AndroidStatusBar.States.Visible;//显示状态栏，占用屏幕最上方的一部分像素
            //AndroidStatusBar.statusBarState = AndroidStatusBar.States.VisibleOverContent;//悬浮显示状态栏，不占用屏幕像素
            //AndroidStatusBar.statusBarState = AndroidStatusBar.States.TranslucentOverContent;//透明悬浮显示状态栏，不占用屏幕像素
            //AndroidStatusBar.statusBarState = AndroidStatusBar.States.Hidden;//隐藏状态栏
        }
        FindPanel();
#if UNITY_EDITOR
        AcceptData_Android(loadTxt.TxtFile[9].ToString());
#else
                  DataTool.StartActivity(0);
#endif
    }
    private void Start()
    {
        DataTool.InitData();
        DataTool.canvasSize = this.GetComponent<RectTransform>().sizeDelta;
        maskPanel.SetActive(false);
    }
    //查找面板
    private void FindPanel()
    {
        maskPanel = transform.Find("MaskPanel").gameObject;
        tipPanel = transform.Find("TipPanel");
        bloodParent = transform.Find("DriftingBlood");
        maskPanel.SetActive(true);

        hallPanel = transform.Find("HallPanel").GetComponent<HallPanel>();
        homePanel = transform.Find("HomePanel").GetComponent<HomePanel>();
        incomePanel = transform.Find("IncomePanel").GetComponent<IncomePanel>();
        mainPanel = transform.Find("MainPanel").GetComponent<MainPanel>();
        dropPanel = transform.Find("DropPanel").GetComponent<DropPanel>();
        personalPanel = transform.Find("Employerpanel/PersonalPanel").GetComponent<PersonalPanel>();
        unitPanel = transform.Find("UnitPanel").GetComponent<UnitPanel>();
        businessPanel = transform.Find("BusinessPanel").GetComponent<BusinessPanel>();
        privacyPanel = transform.Find("PrivacyPanel").GetComponent<PrivacyPanel>();
        termsPanel = transform.Find("TermsPanel").GetComponent<TermsPanel>();
        loadingPanel = transform.Find("LoadingPanel").GetComponent<LoadingPanel>();
        employerpanel = transform.Find("Employerpanel").GetComponent<Employerpanel>();
        servicePanel = transform.Find("ServicePanel").GetComponent<ServicePanel>();
        checkPanel = transform.Find("CheckPanel").GetComponent<CheckPanel>();
        taskPanel = transform.Find("TaskPanel").GetComponent<TaskPanel>();
        taskConfirmPanel = transform.Find("TaskConfirmPanel").GetComponent<TaskConfirmPanel>();
        protocolPanel = transform.Find("ProtocolPanel").GetComponent<ProtocolPanel>();
        bankCardPanel = transform.Find("BankCardPanel").GetComponent<BankCardPanel>();
        invitationPanel = transform.Find("InvitationPanel").GetComponent<InvitationPanel>();
        taskDetailsPanel = transform.Find("TaskDetailsPanel").GetComponent<TaskDetailsPanel>();
        successCodePanel = transform.Find("SuccessCodePanel").GetComponent<SuccessCodePanel>();
        taskSubmitPanel = transform.Find("TaskSubmitPanel").GetComponent<TaskSubmitPanel>();
        clockinPanel = transform.Find("ClockinPanel").GetComponent<ClockinPanel>();
        bankPhotoPanel = transform.Find("BankPhotoPanel").GetComponent<BankPhotoPanel>();

        hallPanel.Init();
        homePanel.Init();
        unitPanel.Init();
        personalPanel.Init();
        bankCardPanel.Init();
    }
    //遮罩面板
    public void MaskTest(bool isHide)
    {
        maskPanel.SetActive(isHide);
    }
    //获取点击对象
    private GameObject ClickOnUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        this.GetComponent<GraphicRaycaster>().Raycast(eventDataCurrentPosition, results);
        if (results.Count > 0)
        {
            return results[0].gameObject;
        }
        return null;
    }
    //生成提示
    public void CloningTips(string messg)
    {
        var tip = ObjectPool.Instance.CreateObject("TipPanel",tipPanel.gameObject);
        tip.gameObject.SetActive(true);
        tip.transform.SetParent(transform,false);
        tip.transform.localPosition = Vector3.zero;
        tip.GetComponent<TipPanel>().StartAnimal(messg);
    }
    //打卡提示
    public void SubmitTip(bool isClock)
    {
        if(isClock)
        {
            DataTool.salaryEntry = SalaryEntry.clock;
            #if UNITY_EDITOR
                  Location_Android(loadTxt.TxtFile[11].ToString());
            #else
                  DataTool.CallClockInfo();
            #endif
        }
        else
        {
            gameObject.SetActive(true);
            CloningTips("获取相机权限失败");
        }
    }

    ////切换后台
    //private void OnApplicationPause(bool focus)
    //{
    //    if (focus)
    //    {
    //        battlePanel.ServiceData();
    //        Debug.Log("OnApplicationPause保存数据");
    //    }
    //    else
    //    {
    //        //if(!isInit)
    //        //{
    //        //    DataTool.StartActivity(0);
    //        //}
    //    }
    //}
    //private void OnApplicationQuit()
    //{
    //    battlePanel.ServiceData();
    //    Debug.Log("OnApplicationQuit保存数据");
    //}
    private void InitData()
    {
        DataTool.token = "";
        DataTool.roleType = "";
        DataTool.bankName = "";
        DataTool.bankNo = "";
        DataTool.roleName = "";
        DataTool.theCompany = "";
        DataTool.latitude = "";
        DataTool.longitude = "";
        DataTool.clockInAddress = "";
        DataTool.currentTask = "";
        DataTool.taskDuration = "0";
        DataTool.businessPic = "";
        DataTool.bankCardPic = "";
        DataTool.signaturePic = "";
        DataTool.inviteCode = "";
        DataTool.information.Clear();
    }
    //雇主 eyJ1c2VyTmFtZSI6IjEyMyIsImFsZyI6IkhTMjU2In0.eyJqdGkiOiJmY29pbmp3dCIsImlhdCI6MTY0MTI4MjgxNCwic3ViIjoie1wiY29tcGFueUlkXCI6ODIsXCJ1c2VySWRcIjozNCxcInVzZXJUeXBlXCI6MX0iLCJleHAiOjE2NDM4NzQ4MTR9.YNNj2RxWuOJaEONQXRNTeIqwP7aHtPZ3mEUDYTFzr78
    //个人 eyJ1c2VyTmFtZSI6IjEyMyIsImFsZyI6IkhTMjU2In0.eyJqdGkiOiJmY29pbmp3dCIsImlhdCI6MTY0MTI4NDAzMywic3ViIjoie1widXNlcklkXCI6NzEsXCJ1c2VyVHlwZVwiOjB9IiwiZXhwIjoxNjQzODc2MDMzfQ.44NRaebSwXUWTWSLPlc69KBNMhvpaIQnb0zugmgivmk
    //接受安卓数据  {"name":"张大牛","goto":"个体户"}
    public void AcceptData_Android(string messgInfo)
    {
        isInit = true;
        maskPanel.SetActive(false);
        try
        {
            Debug.Log("安卓初始数据" + messgInfo);
            InitData();
            Dictionary<string, object> information = Json.Deserialize(messgInfo) as Dictionary<string, object>;
            DataTool.information = information;
            DataTool.roleType = information["goto"].ToString();
            DataTool.token = information["token"].ToString();
            if (DataTool.roleType.Contains("雇主"))
            {
                businessPanel.isStart = true;
                DataTool.theCompany = "";
                DataTool.businessPic = "";
                DataTool.inviteCode = "";
                StartCoroutine(BusinessLicense(DataTool.businessUrl));
            }
            else
            {
                DataTool.latitude = information["lat"].ToString();
                DataTool.longitude = information["lgn"].ToString();
                DataTool.clockInAddress = information["locationAddress"].ToString();

                taskPanel.isSuccess = true;
                bankPhotoPanel.isStart = true;
                protocolPanel.isStart = true;
                hallPanel.gameObject.SetActive(true);
                homePanel.gameObject.SetActive(true);
                employerpanel.gameObject.SetActive(false);
                StartCoroutine(RequestAddress(DataTool.currentTaskUrl,false));
                StartCoroutine(WorkerInfo(DataTool.workerInfo));
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("数据解析错误"+e.ToString());
        }
    }
    //接收经纬度信息
    public void Location_Android(string messg)
    {
        Debug.Log("获取经纬度信息:" + messg);
        Dictionary<string, object> clockData = Json.Deserialize(messg) as Dictionary<string, object>;
        if (clockData["errorCode"].ToString() == "0")
        {
            if(clockData["clockInAddress"] != null)
                DataTool.clockInAddress = clockData["clockInAddress"].ToString();
            if (clockData["lat"] != null)
                DataTool.latitude = clockData["lat"].ToString();
            if (clockData["lgn"] != null)
                DataTool.longitude = clockData["lgn"].ToString();
        }
        else
        {
            DataTool.clockInAddress = "";
            DataTool.latitude = "";
            DataTool.longitude = "";
        }

        switch (DataTool.salaryEntry)
        {
            case SalaryEntry.submit:
                taskSubmitPanel.OpenSubmit();
                break;
            case SalaryEntry.clock:
                StartCoroutine(RequestClockIn(true,"1"));
                break;
            default:
                break;
        }
    }
    //接收收入信息
    public void Acceptance_Android(string messg)
    {
        Debug.Log("更新银行卡:"+ messg);

    }
    //获取雇主用户信息
    private IEnumerator BusinessLicense(string url)
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
            Debug.Log("雇主信息" + webRequest.downloadHandler.text);
            Dictionary<string, object> pageData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (pageData["code"].ToString() == "0")
            {
                Dictionary<string, object> employerInfo = pageData["data"] as Dictionary<string, object>;
                if (employerInfo["companyName"] != null)
                    DataTool.theCompany = employerInfo["companyName"].ToString();
                if (employerInfo["buzLicensePic"] != null)
                    DataTool.businessPic = employerInfo["buzLicensePic"].ToString();
            }
            hallPanel.gameObject.SetActive(false);
            homePanel.gameObject.SetActive(false);
            StartCoroutine(InvitationCode(DataTool.invitationCode));
        }
    }
    //获取雇主邀请码
    private IEnumerator InvitationCode(string url)
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
            Debug.Log("雇主邀请码" + webRequest.downloadHandler.text);
            Dictionary<string, object> pageData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (pageData["code"].ToString() == "0")
            {
                if (pageData["data"] != null)
                    DataTool.inviteCode = pageData["data"].ToString();
            }
            employerpanel.OpenPanel();
        }
    }
    //获取个人用户信息
    private IEnumerator WorkerInfo(string url)
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
            Debug.Log("个人用户信息" + webRequest.downloadHandler.text);
            Dictionary<string, object> pageData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (pageData["code"].ToString() == "0")
            {
                Dictionary<string, object> data = pageData["data"] as Dictionary<string, object>;
                if (data["name"] != null)
                    DataTool.roleName = data["name"].ToString();
                if (data["invitationCompayName"] != null)
                    DataTool.theCompany = data["invitationCompayName"].ToString();
                if (data["invitationCode"] != null)
                    DataTool.inviteCode = data["invitationCode"].ToString();
                if (data["bankName"] != null)
                    DataTool.bankName = data["bankName"].ToString();
                if (data["bankNo"] != null)
                    DataTool.bankNo = data["bankNo"].ToString();
                if (data["signaturePic"] != null)
                    DataTool.signaturePic = data["signaturePic"].ToString();
                if (data["bankPic"] != null)
                    DataTool.bankCardPic = data["bankPic"].ToString();
            }
            hallPanel.InitData();
            homePanel.InitData();
            unitPanel.InitData();
            bankCardPanel.InitData();
            hallPanel.CheckRecord(DataTool.isClock);
        }
    }
    //获取当前任务信息
    public IEnumerator RequestAddress(string url,bool isUdata)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.SetRequestHeader("Authorization", DataTool.token);

        DataTool.currentTask = "";
        DataTool.taskDuration = "";
        yield return webRequest.SendWebRequest();
        if(isUdata)
        {
            MaskTest(false);//loadingPanel.ClosePanel();
        }
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("当前任务信息" + webRequest.downloadHandler.text);
            Dictionary<string, object> taskData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (taskData["code"].ToString() == "0")
            {
                Dictionary<string, object>  taskInfo = taskData["data"] as Dictionary<string, object>;
                if(taskInfo["id"] != null)
                    DataTool.currentTask = taskInfo["id"].ToString();
                if (taskInfo["currentMonthTime"] != null)
                    DataTool.taskDuration = taskInfo["currentMonthTime"].ToString();
            }
            if(isUdata)
            {
                hallPanel.UpdateTask();
            }
            else
            {
                hallPanel.InitData();
                StartCoroutine(RequestClockIn(false,"0"));
            }
        }
    }
    //打卡  ///0 登录 1.主动 2.已接单 4：待开始 5：进行中
    public IEnumerator RequestClockIn(bool isPicture, string lockType,string currentTask = "")
    {
        if(isPicture)
        {
            WWWForm form = new WWWForm();

            form.AddBinaryData("file", DataTool.cheackByte, "ClockIn.png", DataTool.filePath);
            UnityWebRequest webRequest = UnityWebRequest.Post(DataTool.pictureUrl, form);

            yield return webRequest.SendWebRequest();
            //loadingPanel.ClosePanel();
            if (webRequest.isNetworkError || webRequest.error != null)
            {
                Debug.Log("请求网络错误:" + webRequest.error);
            }
            else
            {
                Debug.Log("打卡获取" + webRequest.downloadHandler.text);
                Dictionary<string, object> pageData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
                if (pageData["code"].ToString() == "0")
                {
                    if (pageData["data"] != null)
                        DataTool.checkAddress = pageData["data"].ToString();
                }
            }
            StartCoroutine(ClockRequest(lockType, currentTask));
        }
        else
        {
            StartCoroutine(ClockRequest(lockType, currentTask));
        }
    }

    private IEnumerator ClockRequest(string lockType, string currentTask = "")
    {
        JsonData data = new JsonData();
        data["lat"] = DataTool.latitude;
        data["lgn"] = DataTool.longitude;
        data["lockType"] = lockType;
        data["pic"] = DataTool.checkAddress;
        data["clockInAddress"] = DataTool.clockInAddress;
        if (lockType == "1" || lockType == "0")
            data["taskId"] = DataTool.currentTask;
        else
            data["taskId"] = currentTask;

        UnityWebRequest webRequest = new UnityWebRequest(DataTool.clockUrl, "POST");
        webRequest.SetRequestHeader("Authorization", DataTool.token);

        byte[] postBytes = System.Text.Encoding.Default.GetBytes(data.ToJson());
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();
        MaskTest(false);//loadingPanel.ClosePanel();

        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log(lockType + "打卡成功" + webRequest.downloadHandler.text);
            Dictionary<string, object> clockData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (clockData["code"].ToString() == "0")
            {
                if (lockType == "1")
                {
                    gameObject.SetActive(true);
                    string messgInfo = string.Format("{0:D2}-{1:D2}-{2:D2} " + " {3:D2}:{4:D2}  ", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                    PlayerPrefs.SetString("ClockInTime", messgInfo);
                    PlayerPrefs.SetString("ClockInAddress", DataTool.clockInAddress);
                    clockinPanel.SuccessCheck();
                    hallPanel.CheckRecord(true);
                    checkPanel.OpenPanel();
                }
                else if (lockType == "2" || lockType == "4" || lockType == "5")
                {
                    taskSubmitPanel.CheckSuccess();
                    StartCoroutine(RequestAddress(DataTool.currentTaskUrl, true));
                }
            }
            else
            {
                CloningTips(clockData["msg"].ToString());
            }
        }
    }

    //打卡上传图
    private IEnumerator CheckAddress(string url)
    {
        WWWForm form = new WWWForm();

        form.AddBinaryData("file", DataTool.cheackByte, "ClockIn.png", DataTool.filePath);
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
        
        yield return webRequest.SendWebRequest();
        //loadingPanel.ClosePanel();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("打卡获取" + webRequest.downloadHandler.text);
            Dictionary<string, object> pageData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (pageData["code"].ToString() == "0")
            {
                if(pageData["data"] != null)
                    DataTool.checkAddress = pageData["data"].ToString();
            }
        }
    }
   
}
