using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomePanel : MonoBehaviour
{
    public Color pickColor;
    public Color norColor;
    public Sprite pickSprite;
    public Sprite norSprite;
    public Image[] clickImage;
    public Text[] clickText;

    private Text grandText;
    private Text monthText;
    private Text incomeText;

    private Button dailyBtn;
    private Button payrollBtn;
    private Button monthlyBtn;
    private Button issuedBtn;
    private Button operatingBtn;
    private Button monthBtn;


    private RectTransform viewParent;
    private Transform toolView;
    private Transform dailyView;
    private Transform payItem;
    private Transform monthItem;
    private GameObject dailyState;
    private GameObject dailyParent;
    private List<PaySlipItem> slipItems = new List<PaySlipItem>();
    private List<MonthItem> monthSelect = new List<MonthItem>();

    private int monthCurrent;
    private int yearCurrent;
    private int indexCurret;
    private int monthIndex;
    private int whichWeek;
    private void Awake()
    {
        whichWeek = GetWeekOfYear(DateTime.Today)-1;

        payItem = transform.Find("Item");
        viewParent = transform.Find("ToolView").GetComponent<RectTransform>();
        dailyParent = transform.Find("DailyState/DailyView").gameObject;
        monthItem = transform.Find("DailyState/DailyItem");
        dailyView = transform.Find("DailyState/DailyView/Viewport/Content");
        toolView = transform.Find("ToolView/Viewport/Content");
        dailyState = transform.Find("DailyState").gameObject;
        
        grandText = transform.Find("Grand/MoneyText").GetComponent<Text>();
        monthText = transform.Find("DailyState/monthBtn").GetComponent<Text>();
        incomeText = transform.Find("DailyState/IncomeText").GetComponent<Text>();

        dailyBtn = transform.Find("Header/DailyBtn").GetComponent<Button>();
        payrollBtn = transform.Find("Header/PayrollBtn").GetComponent<Button>();
        monthlyBtn = transform.Find("Header/MonthlyBtn").GetComponent<Button>();
        issuedBtn = transform.Find("Header/IssuedBtn").GetComponent<Button>();
        operatingBtn = transform.Find("Header/OperatingBtn").GetComponent<Button>();
        monthBtn = transform.Find("DailyState/monthBtn").GetComponent<Button>();

        dailyBtn.onClick.AddListener(OpenDaily);
        payrollBtn.onClick.AddListener(OpenPayro);
        monthlyBtn.onClick.AddListener(OpenMonthly);
        issuedBtn.onClick.AddListener(OpenIssued);
        operatingBtn.onClick.AddListener(OpenOperatin);
        monthBtn.onClick.AddListener(OpenMonth);

        InitData();
    }
    // 740 900
    public void OpenPanel()
    {
        gameObject.SetActive(true);
        if(DataTool.isUnit)
        {
            OpenOperatin();
        }
        else
        {
            OpenDaily();
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void InitData()
    {
        indexCurret = -1;
        for (int i = 0; i < 31; i++)
        {
            var item = Instantiate(payItem);
            item.gameObject.SetActive(false);
            item.SetParent(toolView);
            item.localScale = Vector3.one;
            PaySlipItem pay = new PaySlipItem(item);
            slipItems.Add(pay);
        }
        grandText.text = string.Format("{0:N2}",0);
        toolView.GetComponent<GridLayoutGroup>().padding.top = 50;
        monthCurrent = DateTime.Now.Month - 1;
        yearCurrent = DateTime.Now.Year;
        if (DataTool.isUnit)
        {
            dailyState.SetActive(false);
            dailyBtn.gameObject.SetActive(false);
            payrollBtn.gameObject.SetActive(false);
            monthlyBtn.gameObject.SetActive(false);
            issuedBtn.gameObject.SetActive(true);
            operatingBtn.gameObject.SetActive(true);
        }
        else
        {
            dailyBtn.gameObject.SetActive(true);
            payrollBtn.gameObject.SetActive(true);
            monthlyBtn.gameObject.SetActive(true);
            issuedBtn.gameObject.SetActive(false);
            operatingBtn.gameObject.SetActive(false);
            for (int i = 0; i < 10; i++)
            {
                MonthItem itemMonth;
                var item = Instantiate(monthItem);
                item.gameObject.SetActive(true);
                item.SetParent(dailyView);
                item.localScale = Vector3.one;
                if (i < monthCurrent)
                {
                    itemMonth = new MonthItem(item, DateTime.Now.Year, monthCurrent - i, i);
                }
                else
                {
                    itemMonth = new MonthItem(item, DateTime.Now.Year - 1, monthCurrent - i + 12, i);
                }
                monthSelect.Add(itemMonth);
            }
            monthIndex = 0;
            monthSelect[monthIndex].HideImage(true);
            float maxY = 10 * 60 + 10 * 25;
            dailyView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
        }
    }

    public void InitState(int index,Dictionary<string,object> detailInfo,List<object> issuedData = null)
    {
        int day = 0;
        float maxY = 0;
        switch (indexCurret)
        {
            case 0:
                day = DateTime.DaysInMonth(yearCurrent, monthCurrent);
                maxY = day * 150 + day * 20 + 200;
                for (int i = 0; i < slipItems.Count; i++)
                {
                    if((day - i) > 0)
                    {
                        int money = UnityEngine.Random.Range(20, 30);
                        slipItems[i].SetInit(index, money,yearCurrent, monthCurrent, day-i);
                    }
                    else
                    {
                        slipItems[i].HideItem();
                    }
                }
                incomeText.text = string.Format("收入{0:N2}", 1800);
                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
                //toolView.GetComponent<GridLayoutGroup>().padding.top = 200;
                break;
            case 1:
                if(whichWeek >= 1)
                {
                    for (int i = 0; i < slipItems.Count; i++)
                    {
                        if (whichWeek > i)
                        {
                            int money = UnityEngine.Random.Range(1500, 3000);
                            Tuple<DateTime, DateTime> tuple = GetFirstEndDayOfWeek(DateTime.Now.Year, whichWeek - i, System.Globalization.CultureInfo.InvariantCulture);
                            slipItems[i].SetInit(index, tuple.Item1, tuple.Item2,money);
                        }
                        else
                        {
                            slipItems[i].HideItem();
                        }
                    }
                }
                maxY = whichWeek * 150 + whichWeek * 20 + 50;
                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
                break;
            case 2:
                grandText.text = string.Format("{0:N2}", detailInfo["total"]);
                List<object> monthData = detailInfo["data"] as List<object>;
                for (int i = 0; i < slipItems.Count; i++)
                {
                    if(i < monthData.Count)
                    {
                        Dictionary<string, object> data1 = monthData[i] as Dictionary<string, object>;
                        string[] dates = data1["month"].ToString().Split('-');
                        slipItems[i].SetInit(index,float.Parse(data1["fxrsf"].ToString()), int.Parse(dates[0]), int.Parse(dates[1]), int.Parse(data1["id"].ToString()));
                    }
                    else
                    {
                        slipItems[i].HideItem();
                    }
                }
                maxY = monthData.Count * 150 + monthData.Count * 20 + 50;
                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
                break;
            case 3:
                for (int i = 0; i < slipItems.Count; i++)
                {
                    if (i < issuedData.Count)
                    {
                        Dictionary<string, object> data1 = issuedData[i] as Dictionary<string, object>;
                        string[] dates = data1["salary_date"].ToString().Split('-');
                        slipItems[i].SetInit(index, float.Parse(data1["sfze"].ToString()), int.Parse(dates[0]), int.Parse(dates[1]), int.Parse(dates[2]));
                    }
                    else
                    {
                        slipItems[i].HideItem();
                    }
                }
                maxY = issuedData.Count * 150 + issuedData.Count * 20 + 50;
                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
                break;
            case 4:
                grandText.text = string.Format("{0:N2}", detailInfo["total"]);
                List<object> operating = detailInfo["datas"] as List<object>;
                for (int i = 0; i < slipItems.Count; i++)
                {
                    if (i < operating.Count)
                    {
                        Dictionary<string, object> data1 = operating[i] as Dictionary<string, object>;
                        string[] dates = data1["salary_month"].ToString().Split('-');
                        slipItems[i].SetInit(index, float.Parse(data1["sfze"].ToString()), int.Parse(dates[0]), int.Parse(dates[1]),0);
                    }
                    else
                    {
                        slipItems[i].HideItem();
                    }
                }
                maxY = operating.Count * 150 + operating.Count * 20 + 50;
                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
                break;
            default:
                break;
        }
        toolView.localPosition = Vector3.zero;
    }

    private void OpenOperatin()
    {
        ClcikButton(4);
        DataTool.salaryEntry = SalaryEntry.Operating_1;
        if (Application.platform == RuntimePlatform.Android)
        {
            DataTool.CallNative(187, 0);
        }
        else
        {
            UIManager.Instance.Acceptance_Android("Monthly1");
        }
        //InitState(4,null);
    }

    private void OpenIssued()
    {
        ClcikButton(3);
        DataTool.salaryEntry = SalaryEntry.Issued_1;
        if (Application.platform == RuntimePlatform.Android)
        {
            DataTool.CallNative(185, 0);
        }
        else
        {
            UIManager.Instance.Acceptance_Android("Monthly1");
        }
        //InitState(3, null);
    }

    private void OpenMonthly()
    {
        ClcikButton(2);
        DataTool.salaryEntry = SalaryEntry.month_1;
        if(Application.platform == RuntimePlatform.Android)
        {
            DataTool.CallNative(172, 0);
        }
        else
        {
            UIManager.Instance.Acceptance_Android("Monthly1");
        }
        //InitState(2, null);
    }

    private void OpenPayro()
    {
        ClcikButton(1);
        InitState(1, null);
    }

    private void OpenDaily()
    {
        ClcikButton(0);
        InitState(0, null);
    }

    private void OpenMonth()
    {
        if(dailyParent.activeInHierarchy)
        {
            dailyParent.SetActive(false);
        }
        else
        {
            dailyParent.SetActive(true);
        }
    }

    private void ClcikButton(int index)
    {
        if(indexCurret >= 0)
        {
            clickImage[indexCurret].sprite = norSprite;
            clickText[indexCurret].color = norColor;
        }
        indexCurret = index;
        clickImage[indexCurret].sprite = pickSprite;
        clickText[indexCurret].color = pickColor;
    }

    public void ClickMonth(int year, int month,int index)
    {
        monthSelect[monthIndex].HideImage(false);
        monthIndex = index;
        monthSelect[monthIndex].HideImage(true);
        monthCurrent = month;
        yearCurrent = year;
        monthText.text = string.Format("{0}年{1}月", year, month);
        dailyParent.gameObject.SetActive(false);
        if (DataTool.isUnit)
        {
            OpenOperatin();
        }
        else
        {
            OpenDaily();
        }
        //InitState(0,null);
    }

    /// <summary>
    /// 获取一年中的周
    /// </summary>
    /// <param name="dt">日期</param>
    /// <returns></returns>
    public static int GetWeekOfYear(DateTime dt)
    {
        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
        int weekOfYear = gc.GetWeekOfYear(dt, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

        return weekOfYear;
    }
    /// <summary>
    /// 根据一年中的第几周获取该周的开始日期与结束日期
    /// </summary>
    /// <param name="year"></param>
    /// <param name="weekNumber"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static Tuple<DateTime, DateTime> GetFirstEndDayOfWeek(int year, int weekNumber, System.Globalization.CultureInfo culture)
    {
        System.Globalization.Calendar calendar = culture.Calendar;
        DateTime firstOfYear = new DateTime(year, 1, 1, calendar);
        DateTime targetDay = calendar.AddWeeks(firstOfYear, weekNumber - 1);
        DayOfWeek firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;

        while (targetDay.DayOfWeek != firstDayOfWeek)
        {
            targetDay = targetDay.AddDays(-1);
        }

        return Tuple.Create<DateTime, DateTime>(targetDay, targetDay.AddDays(6));
    }
}
public class MonthItem
{
    private Text dateText;
   
    private Button selectBtn;

    private GameObject dateImage;

    private int month;
    private int year;
    private int monthIndex;
    public MonthItem(Transform parent, int year, int month,int monthIndex)
    {
        this.year = year;
        this.month = month;
        this.monthIndex = monthIndex;
        dateImage = parent.Find("Image").gameObject;
        dateText = parent.Find("Text").GetComponent<Text>();
        selectBtn = parent.GetComponent<Button>();
        selectBtn.onClick.AddListener(SelectMonth);
        dateText.text = string.Format("{0}年{1}月", year, month);
        dateImage.SetActive(false);
    }

    private void SelectMonth()
    {
        UIManager.Instance.incomePanel.ClickMonth(year,month,monthIndex);
    }

    public void HideImage(bool isHide)
    {
        dateImage.SetActive(isHide);
    }
}
public class PaySlipItem
{
    private Text dateText;
    private Text moneyText;
    private Button payBtn;

    private int type;
    private int year;
    private int month;
    private int day;
    private float money;

    private Transform slipItem;
    public PaySlipItem(Transform parent)
    {
        slipItem = parent;
        payBtn = parent.GetComponent<Button>();
        dateText = parent.Find("DateText").GetComponent<Text>();
        moneyText = parent.Find("MoneyText").GetComponent<Text>();
        payBtn.onClick.AddListener(OpenBill);
    }

    public void SetInit(int type,DateTime star, DateTime end, float money)
    {
        this.type = type;
        this.money = money;
        this.year = star.Year;
        dateText.text = string.Format("{0}月{1}日-{2}月{3}日",star.Month,star.Day,end.Month,end.Day);
        moneyText.text = string.Format("￥{0:N2}", money);
    }


    public void SetInit(int type, float money, int year, int month, int day)
    {
        this.type = type;
        this.money = money;
        this.year = year;
        this.month = month;
        this.day = day;
        slipItem.gameObject.SetActive(true);
        switch (type)
        {
            case 0:
                dateText.text = string.Format("{0}月{1}日", month, day);
                moneyText.text = string.Format("+{0:N2}", money);
                break;
            //case 1:
            //    dateText.text = string.Format("{0}年{1}月工资单", year, month);
            //    moneyText.text = string.Format("￥{0:N2}", money);
             //   break;
            case 2:
                dateText.text = string.Format("{0}年{1}月工资单", year, month);
                moneyText.text = string.Format("￥{0:N2}", money);
                break;
            case 3:
                dateText.text = string.Format("{0}年{1}月{2}日", year, month, day);
                moneyText.text = string.Format("￥{0:N2}", money);
                break;
            case 4:
                dateText.text = string.Format("{0}年{1}月", year, month);
                moneyText.text = string.Format("￥{0:N2}", money);
                break;
            default:
                break;
        }
    }

    public void HideItem()
    {
        slipItem.gameObject.SetActive(false);
    }

    private void OpenBill()
    {
        switch (type)
        {
            case 0:
                UIManager.Instance.dayKnotPanel.OpenPanel("日结明细",string.Format("【{0}年】{1}",year ,dateText.text),money);
                break;
            case 1:
                UIManager.Instance.dayKnotPanel.OpenPanel("周结明细", string.Format("【{0}年】{1}", year, dateText.text), money);
                break;
            case 2:
                UIManager.Instance.salaryPanel.SetHeadFile(year,month);
                DataTool.salaryEntry = SalaryEntry.month_2;
                if (Application.platform == RuntimePlatform.Android)
                {
                    DataTool.CallNative(173, day);
                }
                else
                {
                    UIManager.Instance.Acceptance_Android("Monthly2");
                }
                break;
            case 3:
                //UIManager.Instance.operatingPanel.OpenPanel();
                break;
            case 4:
                UIManager.Instance.operatingPanel.SetHeadFile(year, month, moneyText.text);
                DataTool.salaryEntry = SalaryEntry.Operating_2;
                if (Application.platform == RuntimePlatform.Android)
                {
                    DataTool.CallNative(190,0,string.Format("{0:D2}-{1:D2}", year, month));
                }
                else
                {
                    UIManager.Instance.Acceptance_Android("Monthly2");
                }
                break;
            default:
                break;
        }
    }
}
