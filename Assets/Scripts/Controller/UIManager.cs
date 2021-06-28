using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIManager : MonoSingleton<UIManager>
{
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

    private Transform tipPanel;
    private Transform bloodParent;
    private Transform bloodPrefab;
    public override void Init()
    {
        Debug.Log("初始信息");
        DataTool.InitData();
        FindPanel();
    }
    private void Start()
    {
        hallPanel.Init();
        homePanel.Init();
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
        tipPanel = transform.Find("TipPanel");
        bloodParent = transform.Find("DriftingBlood");
        bloodPrefab = bloodParent.Find("BloodText");

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
    public void SubmitTip(string messgInfo)
    {
        Debug.Log(messgInfo);
        gameObject.SetActive(true);
        CloningTips(messgInfo);
    }
    //接受安卓数据
    public void AcceptData_Android(string messgInfo)
    {
        Debug.Log(messgInfo);
        CloningTips(messgInfo);
    }
    //切换后台
    private void OnApplicationPause(bool focus)
    {
        if (focus)
        {
            battlePanel.ServiceData();
            Debug.Log("保存数据");
        }
        else
        {

        }
    }
    private void OnApplicationQuit()
    {
        battlePanel.ServiceData();
        Debug.Log("保存数据");
    }
}
