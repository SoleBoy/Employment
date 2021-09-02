using Common;
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
    public Employerpanel employerpanel;
    public ServicePanel servicePanel;
    public CheckPanel checkPanel;

    private GameObject maskPanel;
    private Transform tipPanel;
    private Transform bloodParent;
    private Transform bloodPrefab;
    public override void Init()
    {
        Debug.Log("初始信息");
        if(Application.platform == RuntimePlatform.Android)
        {
            AndroidStatusBar.statusBarState = AndroidStatusBar.States.TranslucentOverContent;
        }
        FindPanel();
    }
    private void Start()
    {
        DataTool.StartActivity(0);
    }

    private void Update()
    {
        //battlePanel.SetUpdata(Time.deltaTime);
        //if (Input.GetMouseButtonDown(0))
        //{
        //    GameObject onUI;
        //    if (EventSystem.current.IsPointerOverGameObject())
        //    {
        //        onUI = ClickOnUI();
        //        if(onUI && onUI.CompareTag("Player"))
        //        {
        //            hallPanel.AddExperience(3 * DataTool.roleLevel);
        //            CloningTips("经验值+"+ 3 * DataTool.roleLevel);
        //        }
        //    }
        //}
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
        personalPanel = transform.Find("Employerpanel/PersonalPanel").GetComponent<PersonalPanel>();
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
        employerpanel = transform.Find("Employerpanel").GetComponent<Employerpanel>();
        servicePanel = transform.Find("ServicePanel").GetComponent<ServicePanel>();
        checkPanel = transform.Find("CheckPanel").GetComponent<CheckPanel>();

        hallPanel.Init();
        homePanel.Init();
        unitPanel.Init();
        personalPanel.Init();
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
            DataTool.InitData();
            DataTool.roleType = jsonData["goto"].ToString();
            DataTool.theCompany = jsonData["cust_name"].ToString(); //"cust_name";
            if (DataTool.roleType.Contains("雇主"))
            {
                DataTool.inviteCode = jsonData["invite_code"].ToString();
                DataTool.inviteType = jsonData["invite_type"].ToString();
                employerpanel.OpenPanel();
                personalPanel.CertificationInfo(jsonData);
                hallPanel.gameObject.SetActive(false);
                homePanel.gameObject.SetActive(false);
            }
            else
            {
                if (DataTool.roleType == "个人")
                {
                    DataTool.isUnit = false;
                }
                else
                {
                    DataTool.isUnit = true;
                }
                DataTool.roleName = jsonData["name"].ToString();
                hallPanel.gameObject.SetActive(true);
                homePanel.gameObject.SetActive(true);
                employerpanel.gameObject.SetActive(false);
                hallPanel.InitData();
                homePanel.InitData();
                unitPanel.InitData();
                unitPanel.CertificationInfo(jsonData);
                hallPanel.CheckRecord(DataTool.isClock);
            }
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
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            switch (DataTool.salaryEntry)
            {
                case SalaryEntry.dayknot_1:
                    incomePanel.InitState(0, DataTool.GetDictionary(messg));
                    break;
                case SalaryEntry.weeklyend_1:
                    incomePanel.InitState(1, DataTool.GetDictionary(messg));
                    break;
                case SalaryEntry.month_1:
                    incomePanel.InitState(2, DataTool.GetDictionary(messg));
                    break;
                case SalaryEntry.month_2:
                    salaryPanel.OpenPanel(DataTool.GetDictionary(messg)["data"] as Dictionary<string, object>);
                    break;
                case SalaryEntry.month_3:
                    payrollPanel.OpenPanel(DataTool.GetDictionary(messg)["data"] as Dictionary<string, object>);
                    break;
                case SalaryEntry.operating_1:
                    incomePanel.InitState(4, DataTool.GetDictionary(messg));
                    break;
                case SalaryEntry.operating_2:
                    operatingPanel.OpenPanel(DataTool.GetDictionary(messg));
                    break;
                case SalaryEntry.issued_1:
                    incomePanel.InitState(3, null, DataTool.GetList(messg));
                    break;
                case SalaryEntry.business_1:
                    businessPanel.OpenPanel(DataTool.GetDictionary(messg));
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (DataTool.salaryEntry)
            {
                case SalaryEntry.dayknot_1:
                    loadTxt.GetMonthly_8();
                    break;
                case SalaryEntry.weeklyend_1:
                    loadTxt.GetMonthly_9();
                    break;
                case SalaryEntry.month_1:
                    loadTxt.GetMonthly_1();
                    break;
                case SalaryEntry.month_2:
                    loadTxt.GetMonthly_2();
                    break;
                case SalaryEntry.month_3:
                    loadTxt.GetMonthly_3();
                    break;
                case SalaryEntry.operating_1:
                    loadTxt.GetMonthly_4();
                    break;
                case SalaryEntry.operating_2:
                    loadTxt.GetMonthly_5();
                    break;
                case SalaryEntry.issued_1:
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
