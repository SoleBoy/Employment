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
    private void Awake()
    {
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
        issuedBtn = transform.Find("Header/IssuedBtn").GetComponent<Button>();
        operatingBtn = transform.Find("Header/OperatingBtn").GetComponent<Button>();
        monthBtn = transform.Find("DailyState/monthBtn").GetComponent<Button>();

        dailyBtn.onClick.AddListener(OpenDaily);
        payrollBtn.onClick.AddListener(OpenPayro);
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
        grandText.text = string.Format("{0:N2}",3600);
        if (DataTool.isUnit)
        {
            dailyState.SetActive(false);
            dailyBtn.gameObject.SetActive(false);
            payrollBtn.gameObject.SetActive(false);
            issuedBtn.gameObject.SetActive(true);
            operatingBtn.gameObject.SetActive(true);
            toolView.GetComponent<GridLayoutGroup>().padding.top = 50;
        }
        else
        {
            monthCurrent = DateTime.Now.Month - 1;
            yearCurrent = DateTime.Now.Year;
            dailyBtn.gameObject.SetActive(true);
            payrollBtn.gameObject.SetActive(true);
            issuedBtn.gameObject.SetActive(true);
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

    private void InitState(int index)
    {
        int month = 0;
        int day = 0;
        float maxY = 0;
        switch (index)
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
                toolView.GetComponent<GridLayoutGroup>().padding.top = 200;
                break;
            case 1:
                month = DateTime.Now.Month - 1;
                day = Mathf.Clamp(12 + month, 0, 12);
                maxY = day * 150 + day * 20 + 50;
                for (int i = 0; i < slipItems.Count; i++)
                {
                    if (i < day)
                    {
                        int money = UnityEngine.Random.Range(1500, 3000);
                        if (month-i > 0)
                        {
                            slipItems[i].SetInit(index, money, DateTime.Now.Year,(month - i), day - i);
                        }
                        else
                        {
                            slipItems[i].SetInit(index, money, DateTime.Now.Year-1,(month-i+12), day - i);
                        }
                    }
                    else
                    {
                        slipItems[i].HideItem();
                    }
                }
                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
                toolView.GetComponent<GridLayoutGroup>().padding.top = 50;
                break;
            case 2:
                day = DateTime.DaysInMonth(yearCurrent, monthCurrent);
                maxY = day * 150 + day * 20 + 50;
                for (int i = 0; i < slipItems.Count; i++)
                {
                    if (i < day)
                    {
                        int money = UnityEngine.Random.Range(100, 1000);
                        slipItems[i].SetInit(index, money,yearCurrent, monthCurrent, day - i);
                    }
                    else
                    {
                        slipItems[i].HideItem();
                    }
                }
                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
                toolView.GetComponent<GridLayoutGroup>().padding.top = 50;
                break;
            case 3:
                month = DateTime.Now.Month - 1;
                day = Mathf.Clamp(12 + month,0,12);
                maxY = day * 150 + day * 20 + 50;
                for (int i = 0; i < slipItems.Count; i++)
                {
                    if (i < day)
                    {
                        int money = UnityEngine.Random.Range(2000, 4000);
                        if (month - i > 0)
                        {
                            slipItems[i].SetInit(index, money, DateTime.Now.Year, (month - i), day - i);
                        }
                        else
                        {
                            slipItems[i].SetInit(index, money, DateTime.Now.Year - 1, (month - i + 12), day - i);
                        }
                    }
                    else
                    {
                        slipItems[i].HideItem();
                    }
                }
                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
                break;
            default:
                break;
        }
        toolView.localPosition = Vector3.zero;
    }

    private void OpenOperatin()
    {
        ClcikButton(3);
        InitState(3);
    }

    private void OpenIssued()
    {
        dailyState.SetActive(false);
        ClcikButton(2);
        InitState(2);
    }

    private void OpenPayro()
    {
        dailyState.SetActive(false);
        ClcikButton(1);
        InitState(1);
    }

    private void OpenDaily()
    {
        dailyState.SetActive(true);
        ClcikButton(0);
        InitState(0);
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
        InitState(0);
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

    private int year;
    private int month;
    private int day;
    private int money;
    private int type;

    private Transform slipItem;
    public PaySlipItem(Transform parent)
    {
        slipItem = parent;
        payBtn = parent.GetComponent<Button>();
        dateText = parent.Find("DateText").GetComponent<Text>();
        moneyText = parent.Find("MoneyText").GetComponent<Text>();
        payBtn.onClick.AddListener(OpenBill);
    }

    public void SetInit(int type, int money, int year, int month, int day)
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
            case 1:
                dateText.text = string.Format("{0}年{1}月工资单", year, month);
                moneyText.text = string.Format("￥{0:N2}", money);
                break;
            case 2:
                dateText.text = string.Format("{0}年{1}月{2}日", year, month,day);
                moneyText.text = string.Format("￥{0:N2}", money);
                break;
            case 3:
                dateText.text = string.Format("{0}年{1}月收入", year, month);
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
                UIManager.Instance.dayKnotPanel.OpenPanel(year,month,day,money);
                break;
            case 1:
                UIManager.Instance.salaryPanel.OpenPanel(year,month,money);
                break;
            case 2:
                break;
            case 3:
                UIManager.Instance.operatingPanel.OpenPanel();
                break;
            default:
                break;
        }
    }
}
