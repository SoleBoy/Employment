﻿using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using MiniJSON;

public class UIManager : MonoSingleton<UIManager>
{
    public LoadTxt loadTxt;
    public LocationService location;

    public HallPanel hallPanel;
    public HomePanel homePanel;
    public IncomePanel incomePanel;
    public MainPanel mainPanel;
    public CardPanel cardPanel;
    public GachaPanel gachaPanel;
    public BeastPanel beastPanel;
    public BackpackPanel backpackPanel;
    public DropPanel dropPanel;
    public PersonalPanel personalPanel;
    public UnitPanel unitPanel;
    public BusinessPanel businessPanel;
    public PrivacyPanel privacyPanel;
    public TermsPanel termsPanel;
    public LevelPanel levelPanel;
    public DayKnotPanel dayKnotPanel;
    public SalaryPanel salaryPanel;
    public OperatingPanel operatingPanel;
    public DetailsPanel detailsPanel;
    public BattlePanel battlePanel;
    public RankingPanel rankingPanel;
    public FightPanel fightPanel;
    public PayrollPanel payrollPanel;
    public LoadingPanel loadingPanel;

    private GameObject maskPanel;
    private Transform tipPanel;
    private Transform bloodParent;
    private Transform bloodPrefab;
    public override void Init()
    {
        Debug.Log("初始信息");
        FindPanel();
    }
    private void Start()
    {
        //AcceptData_Android("{\"name\":\"张大牛\",\"goto\":\"个体工商户\",\"bank_card_bind_status\":\"0\",\"jiangsubank_ii_status\":\"0\",\"realname_auth_status\":\"1\",\"signature_status\":\"0\"}");
#if UNITY_ANDROID
        Debug.Log("UNITY_ANDROID测试");//true 个体工商户  //false 个人
        AcceptData_Android("{\"name\":\"张大牛\",\"goto\":\"个体工商户\",\"bank_card_bind_status\":\"0\",\"jiangsubank_ii_status\":\"0\",\"realname_auth_status\":\"1\",\"signature_status\":\"0\"}");
#endif
        DataTool.StartActivity(0);
    }

    private void Update()
    {
        battlePanel.SetUpdata(Time.deltaTime);
        if (Input.GetMouseButtonDown(0))
        {
            GameObject onUI;
            if (EventSystem.current.IsPointerOverGameObject())
            {
                onUI = ClickOnUI();
                if(onUI && onUI.CompareTag("Player"))
                {
                    hallPanel.AddExperience(3 * DataTool.roleLevel);
                    CloningTips("经验值+"+ 3 * DataTool.roleLevel);
                }
            }
        }
    }
    //查找面板
    private void FindPanel()
    {
        maskPanel = transform.Find("MaskPanel").gameObject;
        tipPanel = transform.Find("TipPanel");
        bloodParent = transform.Find("DriftingBlood");
        bloodPrefab = bloodParent.Find("BloodText");
        maskPanel.SetActive(true);

        hallPanel = transform.Find("HallPanel").GetComponent<HallPanel>();
        homePanel = transform.Find("HomePanel").GetComponent<HomePanel>();
        incomePanel = transform.Find("IncomePanel").GetComponent<IncomePanel>();
        mainPanel = transform.Find("MainPanel").GetComponent<MainPanel>();
        cardPanel = transform.Find("CardPanel").GetComponent<CardPanel>();
        gachaPanel = transform.Find("GachaPanel").GetComponent<GachaPanel>();
        beastPanel = transform.Find("BeastPanel").GetComponent<BeastPanel>();
        backpackPanel = transform.Find("BackpackPanel").GetComponent<BackpackPanel>();
        dropPanel = transform.Find("DropPanel").GetComponent<DropPanel>();
        personalPanel = transform.Find("PersonalPanel").GetComponent<PersonalPanel>();
        unitPanel = transform.Find("UnitPanel").GetComponent<UnitPanel>();
        businessPanel = transform.Find("BusinessPanel").GetComponent<BusinessPanel>();
        privacyPanel = transform.Find("PrivacyPanel").GetComponent<PrivacyPanel>();
        termsPanel = transform.Find("TermsPanel").GetComponent<TermsPanel>();
        levelPanel = transform.Find("LevelPanel").GetComponent<LevelPanel>();
        dayKnotPanel = transform.Find("DayKnotPanel").GetComponent<DayKnotPanel>();
        salaryPanel = transform.Find("SalaryPanel").GetComponent<SalaryPanel>();
        operatingPanel = transform.Find("OperatingPanel").GetComponent<OperatingPanel>();
        detailsPanel = transform.Find("DetailsPanel").GetComponent<DetailsPanel>();
        battlePanel = transform.Find("BattlePanel").GetComponent<BattlePanel>();
        rankingPanel = transform.Find("RankingPanel").GetComponent<RankingPanel>();
        fightPanel = transform.Find("FightPanel").GetComponent<FightPanel>();
        payrollPanel = transform.Find("PayrollPanel").GetComponent<PayrollPanel>();
        loadingPanel = transform.Find("LoadingPanel").GetComponent<LoadingPanel>();
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
    //生成血条
    public void CloningBlood(Vector3 point,float hurt)
    {
        var go = ObjectPool.Instance.CreateObject(bloodPrefab.name,bloodPrefab.gameObject);
        go.transform.SetParent(bloodParent);
        go.transform.localPosition = point;
        go.transform.localScale = Vector3.zero;
        go.transform.DOScale(Vector3.one, 0.2f);
        float maxY = go.transform.localPosition.y + 260;
        go.transform.DOLocalMoveY(maxY, 0.3f);
        go.GetComponent<Text>().text = string.Format("-{0}", hurt);
        StartCoroutine(BloodAnimal(go));
    }
    private IEnumerator BloodAnimal(GameObject go)
    {
        yield return new WaitForSeconds(0.5f);
        go.SetActive(false);
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
        gameObject.SetActive(true);
        if(isClock)
        {
            location.UpdateGps();
            PlayerPrefs.SetString(System.DateTime.Now.Date.ToString() + "Clock", "Clock");
        }
        else
        {
            CloningTips("获取相机权限失败");
        }
    }
   
    //切换后台
    private void OnApplicationPause(bool focus)
    {
        if (focus)
        {
            battlePanel.ServiceData();
            Debug.Log("OnApplicationPause保存数据");
        }
        else
        {

        }
    }
    private void OnApplicationQuit()
    {
        battlePanel.ServiceData();
        Debug.Log("OnApplicationQuit保存数据");
    }

    //接受安卓数据  {"name":"张大牛","goto":"个体户"}
    public void AcceptData_Android(string messgInfo)
    {
        maskPanel.SetActive(false);
        try
        {
            Debug.Log("安卓初始数据" + messgInfo);
            Dictionary<string, object> jsonData = Json.Deserialize(messgInfo) as Dictionary<string, object>;
            if (jsonData["goto"].ToString() == "个人")
            {
                DataTool.isUnit = false;
            }
            else
            {
                DataTool.isUnit = true;
            }
            DataTool.InitData(jsonData["name"].ToString());
            hallPanel.Init();
            homePanel.Init();
            unitPanel.Init();
            unitPanel.CertificationInfo(jsonData);
            hallPanel.CheckRecord(DataTool.isClock);
        }
        catch (System.Exception e)
        {
            Debug.Log("数据解析错误"+e.ToString());
        }
    }
    //接收收入信息
    public void Acceptance_Android(string messg)
    {
        Debug.Log(DataTool.salaryEntry + ":" + messg);
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                switch (DataTool.salaryEntry)
                {
                    case SalaryEntry.month_1:
                        Dictionary<string, object> month_1 = Json.Deserialize(messg) as Dictionary<string, object>;
                        incomePanel.InitState(2, month_1);
                        break;
                    case SalaryEntry.month_2:
                        Dictionary<string, object> month_2 = Json.Deserialize(messg) as Dictionary<string, object>;
                        salaryPanel.OpenPanel(month_2["data"] as Dictionary<string, object>);
                        break;
                    case SalaryEntry.month_3:
                        Dictionary<string, object> month_3 = Json.Deserialize(messg) as Dictionary<string, object>;
                        payrollPanel.OpenPanel(month_3["data"] as Dictionary<string, object>);
                        break;
                    case SalaryEntry.Operating_1:
                        Dictionary<string, object> Operating_1 = Json.Deserialize(messg) as Dictionary<string, object>;
                        incomePanel.InitState(4, Operating_1);
                        break;
                    case SalaryEntry.Operating_2:
                        Dictionary<string, object> Operating_2 = Json.Deserialize(messg) as Dictionary<string, object>;
                        operatingPanel.OpenPanel(Operating_2);
                        break;
                    case SalaryEntry.Issued_1:
                        List<object> Issued_1 = Json.Deserialize(messg) as List<object>;
                        incomePanel.InitState(3, null, Issued_1);
                        break;
                    case SalaryEntry.business_1:
                        Dictionary<string, object> business_1 = Json.Deserialize(messg) as Dictionary<string, object>;
                        businessPanel.OpenPanel(business_1);
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception e)
            {
                CloningTips("数据返回失败"+ e.ToString());
                switch (DataTool.salaryEntry)
                {
                    case SalaryEntry.month_1:
                        loadTxt.GetMonthly_1();
                        break;
                    case SalaryEntry.month_2:
                        loadTxt.GetMonthly_2();
                        break;
                    case SalaryEntry.month_3:
                        loadTxt.GetMonthly_3();
                        break;
                    case SalaryEntry.Operating_1:
                        loadTxt.GetMonthly_4();
                        break;
                    case SalaryEntry.Operating_2:
                        loadTxt.GetMonthly_5();
                        break;
                    case SalaryEntry.Issued_1:
                        loadTxt.GetMonthly_6();
                        break;
                    case SalaryEntry.business_1:
                        loadTxt.GetMonthly_7();
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            switch (DataTool.salaryEntry)
            {
                case SalaryEntry.month_1:
                    loadTxt.GetMonthly_1();
                    break;
                case SalaryEntry.month_2:
                    loadTxt.GetMonthly_2();
                    break;
                case SalaryEntry.month_3:
                    loadTxt.GetMonthly_3();
                    break;
                case SalaryEntry.Operating_1:
                    loadTxt.GetMonthly_4();
                    break;
                case SalaryEntry.Operating_2:
                    loadTxt.GetMonthly_5();
                    break;
                case SalaryEntry.Issued_1:
                    loadTxt.GetMonthly_6();
                    break;
                case SalaryEntry.business_1:
                    loadTxt.GetMonthly_7();
                    break;
                default:
                    break;
            }
        }
    }

}
