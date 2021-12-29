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
    public LocationService location;

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
    }
    private void Start()
    {
        maskPanel.SetActive(false);
        DataTool.StartActivity(0);
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

        hallPanel.Init();
        homePanel.Init();
        unitPanel.Init();
        personalPanel.Init();
        bankCardPanel.Init();
    }
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
        //gameObject.SetActive(true);
        if(isClock)
        {
            //location.UpdateGps();
            DataTool.salaryEntry = SalaryEntry.clock;
            if (Application.platform == RuntimePlatform.Android)
            {
                DataTool.CallClockInfo();
            }
            else
            {
                Location_Android(loadTxt.TxtFile[11].ToString());
            }
        }
        else
        {
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
    //雇主 eyJ1c2VyTmFtZSI6IjEyMyIsImFsZyI6IkhTMjU2In0.eyJqdGkiOiJmY29pbmp3dCIsImlhdCI6MTY0MDE2MTIzMSwic3ViIjoie1wiY29tcGFueUlkXCI6MzQsXCJ1c2VySWRcIjoxLFwidXNlclR5cGVcIjoxfSIsImV4cCI6MTY0Mjc1MzIzMX0.amlxYbYn2nfzkOlW09n-EndvVcsTdzzBjissfkmc1Bc
    //个人 eyJ1c2VyTmFtZSI6IjEyMyIsImFsZyI6IkhTMjU2In0.eyJqdGkiOiJmY29pbmp3dCIsImlhdCI6MTY0MDQ5NTYxMCwic3ViIjoie1widXNlcklkXCI6MjksXCJ1c2VyVHlwZVwiOjB9IiwiZXhwIjoxNjQzMDg3NjEwfQ.GGzpd-u1EcwE0V0-RW4PyoPm4f7MwBPuMpuOoDgR8iU
    //接受安卓数据  {"name":"张大牛","goto":"个体户"}
    public void AcceptData_Android(string messgInfo)
    {
        isInit = true;
        maskPanel.SetActive(false);
        try
        {
            Debug.Log("安卓初始数据" + messgInfo);
            Dictionary<string, object> information = Json.Deserialize(messgInfo) as Dictionary<string, object>;
            DataTool.information = information;
            DataTool.InitData();
            DataTool.roleType = information["goto"].ToString();
            DataTool.token = information["token"].ToString();
            if (DataTool.roleType.Contains("雇主"))
            {
                StartCoroutine(BusinessLicense(DataTool.businessUrl));
            }
            else
            {
                DataTool.bankName = information["bankName"].ToString();
                DataTool.bankNo = information["bankNo"].ToString();
                DataTool.roleName = information["name"].ToString();
                DataTool.latitude = information["lat"].ToString();
                DataTool.longitude = information["lgn"].ToString();
                DataTool.clockInAddress = information["locationAddress"].ToString();
                
                hallPanel.gameObject.SetActive(true);
                homePanel.gameObject.SetActive(true);
                employerpanel.gameObject.SetActive(false);
                StartCoroutine(RequestAddress(DataTool.currentTaskUrl));
                StartCoroutine(WorkerInfo(DataTool.workerInfo));
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("数据解析错误"+e.ToString());
        }
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
            if (pageData["msg"].ToString() == "SUCCESS")
            {
                Dictionary<string, object> employerInfo = pageData["data"] as Dictionary<string, object>;
                if (employerInfo["companyName"] == null)
                {
                    DataTool.theCompany = ""; //
                }
                else
                {
                    DataTool.theCompany = employerInfo["companyName"].ToString(); //"cust_name";
                }
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
            if (pageData["msg"].ToString() == "SUCCESS")
            {
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
            if (pageData["msg"].ToString() == "SUCCESS")
            {
                Dictionary<string, object> data = pageData["data"] as Dictionary<string, object>;
                if (data["invitationCompayName"] == null)
                {
                    DataTool.theCompany = "";
                }
                else
                {
                    DataTool.theCompany = data["invitationCompayName"].ToString();
                }
                if (data["invitationCode"] == null || data["invitationCode"].ToString() == "")
                {
                    DataTool.inviteCode = "-1";
                }
                else
                {
                    DataTool.inviteCode = data["invitationCode"].ToString();
                }
                protocolPanel.GetSignature(data["signaturePic"].ToString());
            }
            hallPanel.InitData();
            homePanel.InitData();
            unitPanel.InitData();
            bankCardPanel.InitData();
            hallPanel.CheckRecord(DataTool.isClock);

        }
    }
    //获取当前任务信息
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
            Debug.Log("当前任务信息" + webRequest.downloadHandler.text);
            Dictionary<string, object> taskData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (taskData["msg"].ToString() == "SUCCESS")
            {
                Dictionary<string, object>  taskInfo = taskData["data"] as Dictionary<string, object>;
                if(taskInfo["id"] != null)
                    DataTool.currentTask = taskInfo["id"].ToString();
                if (taskInfo["currentMonthTime"] != null)
                    DataTool.taskDuration = taskInfo["currentMonthTime"].ToString();
            }
            hallPanel.InitData();
            StartCoroutine(RequestAddress(DataTool.latitude, DataTool.longitude, "0"));
        }
    }

    private IEnumerator RequestAddress(string lat, string lgn,string lockType)
    {
        JsonData data = new JsonData();
        data["lat"] = lat;
        data["lgn"] = lgn;
        data["lockType"] = lockType;
        data["pic"] = DataTool.checkAddress;
        data["taskId"] = DataTool.currentTask;
        data["clockInAddress"] = DataTool.clockInAddress;

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
            Debug.Log(lockType + "打卡成功" + webRequest.downloadHandler.text);
            Dictionary<string, object> clockData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (clockData["msg"].ToString() == "SUCCESS")
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
            }
            else
            {
                UIManager.Instance.CloningTips(clockData["msg"].ToString());
            }
        }
    }
    //接收经纬度信息
    public void Location_Android(string messg)
    {
        Debug.Log("获取经纬度信息:"+ messg);
        Dictionary<string, object> clockData = Json.Deserialize(messg) as Dictionary<string, object>;
        if(clockData["errorCode"].ToString() == "0")
        {
            DataTool.clockInAddress = clockData["clockInAddress"].ToString();
            DataTool.latitude = clockData["lat"].ToString();
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
                StartCoroutine(RequestAddress(DataTool.latitude, DataTool.longitude, "1"));
                break;
            default:
                break;
        }
    }
    //打卡图片上传
    public void CheckUrl()
    {
        gameObject.SetActive(true);
        MaskTest(true);
        StartCoroutine(CheckAddress(DataTool.pictureUrl));
    }
    private IEnumerator CheckAddress(string url)
    {
        WWWForm form = new WWWForm();

        form.AddBinaryData("file", DataTool.cheackByte, "ClockIn.png", DataTool.filePath);
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
        
        yield return webRequest.SendWebRequest();
        MaskTest(false);
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            DataTool.checkAddress = "";
            Debug.Log("打卡获取" + webRequest.downloadHandler.text);
            Dictionary<string, object> pageData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (pageData["msg"].ToString() == "SUCCESS")
            {
                DataTool.checkAddress = pageData["data"].ToString();
            }
        }
    }
    //获取当前任务信息
    public IEnumerator CurretAddress(string url)
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
            Debug.Log("当前任务信息" + webRequest.downloadHandler.text);
            Dictionary<string, object> taskData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (taskData["msg"].ToString() == "SUCCESS")
            {
                Dictionary<string, object> taskInfo = taskData["data"] as Dictionary<string, object>;
                DataTool.currentTask = taskInfo["id"].ToString();
                DataTool.taskDuration = taskInfo["currentMonthTime"].ToString();
            }
            hallPanel.UpdateTask();
        }
    }
    //接收收入信息
    //public void Acceptance_Android(string messg)
    //{
    //    Debug.Log(DataTool.salaryEntry + ":" + messg);
    //    if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    //    {
    //        switch (DataTool.salaryEntry)
    //        {
    //            case SalaryEntry.dayknot_1:
    //                //incomePanel.InitState(0, DataTool.GetDictionary(messg));
    //                break;
    //            case SalaryEntry.weeklyend_1:
    //                //incomePanel.InitState(1, DataTool.GetDictionary(messg));
    //                break;
    //            case SalaryEntry.month_1:
    //                //incomePanel.InitState(2, DataTool.GetDictionary(messg));
    //                break;
    //            case SalaryEntry.month_2:
    //                salaryPanel.OpenPanel(DataTool.GetDictionary(messg)["data"] as Dictionary<string, object>);
    //                break;
    //            case SalaryEntry.month_3:
    //                payrollPanel.OpenPanel(DataTool.GetDictionary(messg)["data"] as Dictionary<string, object>);
    //                break;
    //            case SalaryEntry.operating_1:
    //                //incomePanel.InitState(4, DataTool.GetDictionary(messg));
    //                break;
    //            case SalaryEntry.operating_2:
    //                operatingPanel.OpenPanel(DataTool.GetDictionary(messg));
    //                break;
    //            case SalaryEntry.issued_1:
    //                //incomePanel.InitState(3, null, DataTool.GetList(messg));
    //                break;
    //            case SalaryEntry.business_1:
    //                businessPanel.OpenPanel(DataTool.GetDictionary(messg));
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    else
    //    {
    //        switch (DataTool.salaryEntry)
    //        {
    //            case SalaryEntry.dayknot_1:
    //                loadTxt.GetMonthly_8();
    //                break;
    //            case SalaryEntry.weeklyend_1:
    //                loadTxt.GetMonthly_9();
    //                break;
    //            case SalaryEntry.month_1:
    //                loadTxt.GetMonthly_1();
    //                break;
    //            case SalaryEntry.month_2:
    //                loadTxt.GetMonthly_2();
    //                break;
    //            case SalaryEntry.month_3:
    //                loadTxt.GetMonthly_3();
    //                break;
    //            case SalaryEntry.operating_1:
    //                loadTxt.GetMonthly_4();
    //                break;
    //            case SalaryEntry.operating_2:
    //                loadTxt.GetMonthly_5();
    //                break;
    //            case SalaryEntry.issued_1:
    //                loadTxt.GetMonthly_6();
    //                break;
    //            case SalaryEntry.business_1:
    //                loadTxt.GetMonthly_7();
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //}
}
