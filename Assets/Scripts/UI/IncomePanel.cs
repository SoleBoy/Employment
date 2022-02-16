using LitJson;
using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class IncomePanel : MonoBehaviour
{
    private Text totalText;

    private Transform toolParent;
    private Transform taskItem;
    private GameObject bg_grey;

    List<TaskDetails> taskDetails = new List<TaskDetails>();
    private void Awake()
    {
        bg_grey = transform.Find("ToolView/Image").gameObject;
        toolParent = transform.Find("ToolView/Viewport/Content");
        taskItem = toolParent.Find("Item");
        totalText = transform.Find("TopBg/MoneyText").GetComponent<Text>();
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        StartCoroutine(TotalSalary(DataTool.totalSalaryUrl));
        StartCoroutine(SalarySeach(DataTool.salarySeachUrl));
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    
    //获取总收入
    private IEnumerator TotalSalary(string url)
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
            Debug.Log("总收入" + webRequest.downloadHandler.text);
            Dictionary<string, object> taskData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (taskData["code"].ToString() == "0")
            {
                if(taskData["data"] != null && taskData["data"].ToString() != "")
                {
                    totalText.text = string.Format("{0:N2}", float.Parse(taskData["data"].ToString()));
                }
                else
                {
                    totalText.text = string.Format("{0:N2}",0);
                }
            }
            else
            {
                totalText.text = string.Format("{0:N2}", 0);
            }
        }
    }
    private IEnumerator SalarySeach(string url)
    {
        JsonData data = new JsonData();
        data["page"] = "1";
        data["pageSize"] = "100";

        UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        webRequest.SetRequestHeader("Authorization", DataTool.token);

        byte[] postBytes = System.Text.Encoding.Default.GetBytes(data.ToJson());
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        for (int i = 0; i < taskDetails.Count; i++)
        {
            taskDetails[i].curretTask.SetActive(false);
        }
        //UIManager.Instance.loadingPanel.OpenPanel();
        yield return webRequest.SendWebRequest();
        //UIManager.Instance.loadingPanel.ClosePanel();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("收入页" + webRequest.downloadHandler.text);
            Dictionary<string, object> pageData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if(pageData["code"].ToString() == "0")
            {
                Dictionary<string, object> taskData= pageData["data"] as Dictionary<string, object>;
                List<object> taskList= taskData["list"] as List<object>;
                for (int i = 0; i < taskList.Count; i++)
                {
                    Dictionary<string, object> itemData = taskList[i] as Dictionary<string, object>;
                    if (taskDetails.Count <= i)
                    {
                        var item = Instantiate(taskItem);
                        item.gameObject.SetActive(true);
                        item.SetParent(toolParent);
                        item.localScale = Vector3.one;
                        TaskDetails deta = new TaskDetails(item);
                        deta.SetInfo(itemData);
                        taskDetails.Add(deta);
                    }
                    else
                    {
                        taskDetails[i].curretTask.SetActive(true);
                        taskDetails[i].SetInfo(itemData);
                    }
                }
                float detaMax = taskList.Count * 220 + taskList.Count * 20;
                toolParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, detaMax);
                bg_grey.SetActive(taskList.Count <= 0);
            }
            else
            {
                bg_grey.SetActive(true);
            }
        }
    }

    private class TaskDetails
    {
        private Text timeText;
        private Text nameText;
        private Text moneyText;

        private Button detailsBtn;

        private string taskId;
        public GameObject curretTask;
        public TaskDetails(Transform taskParent)
        {
            curretTask = taskParent.gameObject;
            timeText = taskParent.Find("time").GetComponent<Text>();
            nameText = taskParent.Find("name").GetComponent<Text>();
            moneyText = taskParent.Find("money").GetComponent<Text>();
            detailsBtn = taskParent.GetComponent<Button>();
            detailsBtn.onClick.AddListener(OpenDetails);
        }

        public void SetInfo(Dictionary<string, object> pairs)
        {
            nameText.text = "";
            timeText.text = "";
            moneyText.text = "";

            if (pairs["id"] != null)
                taskId = pairs["id"].ToString();
            if (pairs["title"] != null)
                nameText.text = pairs["title"].ToString();
            if (pairs["time"] != null)
                timeText.text = pairs["time"].ToString();
            if(pairs["fee"] != null)
                moneyText.text = float.Parse(pairs["fee"].ToString()).ToString("N2");
        }

        private void OpenDetails()
        {
            if (DataTool.isDegree)
            {
                UIManager.Instance.taskDetailsPanel.OpenPanel(taskId);
            }
            else
            {
                UIManager.Instance.guidePanel.OpenPanel();
            }
        }
    }

























    //    public Color pickColor;
    //    public Color norColor;
    //    public Sprite pickSprite;
    //    public Sprite norSprite;
    //    public Image[] clickImage;
    //    public Text[] clickText;
    //    public GameObject[] clickObject;

    //    private Text grandText;
    //    private Text firmText;
    //    //private Text monthText;
    //   // private Text incomeText;

    //    private Button dailyBtn;
    //    private Button payrollBtn;
    //    private Button monthlyBtn;
    //    private Button issuedBtn;
    //    private Button operatingBtn;
    //    //private Button monthBtn;


    //    private RectTransform viewParent;
    //    private Transform toolView;
    //    //private Transform dailyView;
    //    private Transform payItem;
    //    //private Transform monthItem;
    //    //private GameObject dailyState;
    //    //private GameObject dailyParent;
    //    private List<PaySlipItem> slipItems = new List<PaySlipItem>();
    //    //private List<MonthItem> monthSelect = new List<MonthItem>();

    //    private int monthCurrent;
    //    private int yearCurrent;
    //    private int indexCurret;
    //    private int monthIndex;
    //    private int whichWeek;
    //    private void Awake()
    //    {
    //        whichWeek = GetWeekOfYear(DateTime.Today)-1;

    //        payItem = transform.Find("Item");
    //        viewParent = transform.Find("ToolView").GetComponent<RectTransform>();
    //        //dailyParent = transform.Find("DailyState/DailyView").gameObject;
    //        //monthItem = transform.Find("DailyState/DailyItem");
    //        //dailyView = transform.Find("DailyState/DailyView/Viewport/Content");
    //        toolView = transform.Find("ToolView/Viewport/Content");
    //        //dailyState = transform.Find("DailyState").gameObject;

    //        grandText = transform.Find("Grand/MoneyText").GetComponent<Text>();
    //        firmText = transform.Find("TitleText").GetComponent<Text>();
    //        //monthText = transform.Find("DailyState/monthBtn").GetComponent<Text>();
    //        //incomeText = transform.Find("DailyState/IncomeText").GetComponent<Text>();

    //        dailyBtn = transform.Find("Header/DailyBtn").GetComponent<Button>();
    //        payrollBtn = transform.Find("Header/PayrollBtn").GetComponent<Button>();
    //        monthlyBtn = transform.Find("Header/MonthlyBtn").GetComponent<Button>();
    //        issuedBtn = transform.Find("Header/IssuedBtn").GetComponent<Button>();
    //        operatingBtn = transform.Find("Header/OperatingBtn").GetComponent<Button>();
    //        //monthBtn = transform.Find("DailyState/monthBtn").GetComponent<Button>();TitleText

    //        dailyBtn.onClick.AddListener(OpenDaily);
    //        payrollBtn.onClick.AddListener(OpenPayro);
    //        monthlyBtn.onClick.AddListener(OpenMonthly);
    //        issuedBtn.onClick.AddListener(OpenIssued);
    //        operatingBtn.onClick.AddListener(OpenOperatin);
    //        //monthBtn.onClick.AddListener(OpenMonth);

    //        InitData();
    //    }
    //    // 740 900
    //    public void OpenPanel()
    //    {
    //        gameObject.SetActive(true);
    //        firmText.text = DataTool.theCompany;
    //        //if (DataTool.isUnit)
    //        //{
    //        //    OpenOperatin();
    //        //}
    //        //else
    //        {
    //            OpenDaily();
    //        }
    //    }

    //    public void ClosePanel()
    //    {
    //        gameObject.SetActive(false);
    //    }

    //    private void InitData()
    //    {
    //        indexCurret = -1;
    //        for (int i = 0; i < 31; i++)
    //        {
    //            var item = Instantiate(payItem);
    //            item.gameObject.SetActive(false);
    //            item.SetParent(toolView);
    //            item.localScale = Vector3.one;
    //            PaySlipItem pay = new PaySlipItem(item);
    //            slipItems.Add(pay);
    //        }
    //        grandText.text = string.Format("{0:N2}",0);
    //        toolView.GetComponent<GridLayoutGroup>().padding.top = 50;
    //        monthCurrent = DateTime.Now.Month - 1;
    //        yearCurrent = DateTime.Now.Year;
    //        //if (DataTool.isUnit)
    //        //{
    //        //    //dailyState.SetActive(false);
    //        //    dailyBtn.gameObject.SetActive(false);
    //        //    payrollBtn.gameObject.SetActive(false);
    //        //    monthlyBtn.gameObject.SetActive(false);
    //        //    issuedBtn.gameObject.SetActive(true);
    //        //    operatingBtn.gameObject.SetActive(true);
    //        //}
    //        //else
    //        {
    //            dailyBtn.gameObject.SetActive(true);
    //            payrollBtn.gameObject.SetActive(true);
    //            monthlyBtn.gameObject.SetActive(true);
    //            issuedBtn.gameObject.SetActive(false);
    //            operatingBtn.gameObject.SetActive(false);
    //            //for (int i = 0; i < 10; i++)
    //            //{
    //            //    MonthItem itemMonth;
    //            //    var item = Instantiate(monthItem);
    //            //    item.gameObject.SetActive(true);
    //            //    item.SetParent(dailyView);
    //            //    item.localScale = Vector3.one;
    //            //    if (i < monthCurrent)
    //            //    {
    //            //        itemMonth = new MonthItem(item, DateTime.Now.Year, monthCurrent - i, i);
    //            //    }
    //            //    else
    //            //    {
    //            //        itemMonth = new MonthItem(item, DateTime.Now.Year - 1, monthCurrent - i + 12, i);
    //            //    }
    //            //    monthSelect.Add(itemMonth);
    //            //}
    //            //monthIndex = 0;
    //            //monthSelect[monthIndex].HideImage(true);
    //            //float maxY = 10 * 60 + 10 * 25;
    //            //dailyView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
    //        }
    //    }

    //    public void InitState(int index,Dictionary<string,object> detailInfo,List<object> issuedData = null)
    //    {
    //        float maxY = 0;
    //        switch (indexCurret)
    //        {
    //            case 0:
    //                if (detailInfo != null)
    //                {
    //                    grandText.text = string.Format("{0:N2}", detailInfo["total"]);
    //                    List<object> dayData = detailInfo["data"] as List<object>;
    //                    maxY = dayData.Count * 150 + dayData.Count * 20 + 200;
    //                    for (int i = 0; i < slipItems.Count; i++)
    //                    {
    //                        if (i < dayData.Count)
    //                        {
    //                            Dictionary<string, object> data1 = dayData[i] as Dictionary<string, object>;
    //                            slipItems[i].SetInit(index, data1);
    //                        }
    //                        else
    //                        {
    //                            slipItems[i].HideItem();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    maxY = 0;
    //                    grandText.text = "0.00";
    //                    for (int i = 0; i < slipItems.Count; i++)
    //                    {
    //                        slipItems[i].HideItem();
    //                    }
    //                }
    //                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
    //                //incomeText.text = string.Format("收入{0:N2}", 1800);
    //                //toolView.GetComponent<GridLayoutGroup>().padding.top = 200;
    //                break;
    //            case 1:
    //                if (detailInfo != null)
    //                {
    //                    grandText.text = string.Format("{0:N2}", detailInfo["total"]);
    //                    List<object> dayData = detailInfo["data"] as List<object>;
    //                    maxY = dayData.Count * 150 + dayData.Count * 20 + 200;
    //                    for (int i = 0; i < slipItems.Count; i++)
    //                    {
    //                        if (i < dayData.Count)
    //                        {
    //                            Dictionary<string, object> data1 = dayData[i] as Dictionary<string, object>;
    //                            slipItems[i].SetInit(index, data1);
    //                        }
    //                        else
    //                        {
    //                            slipItems[i].HideItem();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    maxY = 0;
    //                    grandText.text = "0.00";
    //                    for (int i = 0; i < slipItems.Count; i++)
    //                    {
    //                        slipItems[i].HideItem();
    //                    }
    //                }
    //                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
    //                break;
    //            case 2:
    //                if(detailInfo != null)
    //                {
    //                    grandText.text = string.Format("{0:N2}", detailInfo["total"]);
    //                    List<object> monthData = detailInfo["data"] as List<object>;
    //                    for (int i = 0; i < slipItems.Count; i++)
    //                    {
    //                        if (i < monthData.Count)
    //                        {
    //                            Dictionary<string, object> data1 = monthData[i] as Dictionary<string, object>;
    //                            string[] dates = data1["month"].ToString().Split('-');
    //                            slipItems[i].SetInit(index, float.Parse(data1["fxrsf"].ToString()), int.Parse(dates[0]), int.Parse(dates[1]), int.Parse(data1["id"].ToString()));
    //                        }
    //                        else
    //                        {
    //                            slipItems[i].HideItem();
    //                        }
    //                    }
    //                    maxY = monthData.Count * 150 + monthData.Count * 20 + 50;
    //                }
    //                else
    //                {
    //                    maxY = 0;
    //                    grandText.text = "0.00";
    //                    for (int i = 0; i < slipItems.Count; i++)
    //                    {
    //                        slipItems[i].HideItem();
    //                    }
    //                }
    //                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
    //                break;
    //            case 3:
    //                if(issuedData != null)
    //                {
    //                    for (int i = 0; i < slipItems.Count; i++)
    //                    {
    //                        if (i < issuedData.Count)
    //                        {
    //                            Dictionary<string, object> data1 = issuedData[i] as Dictionary<string, object>;
    //                            string[] dates = data1["salary_date"].ToString().Split('-');
    //                            slipItems[i].SetInit(index, float.Parse(data1["sfze"].ToString()), int.Parse(dates[0]), int.Parse(dates[1]), int.Parse(dates[2]));
    //                        }
    //                        else
    //                        {
    //                            slipItems[i].HideItem();
    //                        }
    //                    }
    //                    maxY = issuedData.Count * 150 + issuedData.Count * 20 + 50;
    //                }
    //                else
    //                {
    //                    maxY = 0;
    //                    for (int i = 0; i < slipItems.Count; i++)
    //                    {
    //                        slipItems[i].HideItem();
    //                    }
    //                }
    //                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
    //                break;
    //            case 4:
    //                if(detailInfo != null)
    //                {
    //                    grandText.text = string.Format("{0:N2}", detailInfo["total"]);
    //                    List<object> operating = detailInfo["datas"] as List<object>;
    //                    for (int i = 0; i < slipItems.Count; i++)
    //                    {
    //                        if (i < operating.Count)
    //                        {
    //                            Dictionary<string, object> data1 = operating[i] as Dictionary<string, object>;
    //                            string[] dates = data1["salary_month"].ToString().Split('-');
    //                            slipItems[i].SetInit(index, float.Parse(data1["sfze"].ToString()), int.Parse(dates[0]), int.Parse(dates[1]), 0);
    //                        }
    //                        else
    //                        {
    //                            slipItems[i].HideItem();
    //                        }
    //                    }
    //                    maxY = operating.Count * 150 + operating.Count * 20 + 50;
    //                }
    //                else
    //                {
    //                    maxY = 0;
    //                    grandText.text = "0.00";
    //                    for (int i = 0; i < slipItems.Count; i++)
    //                    {
    //                        slipItems[i].HideItem();
    //                    }
    //                }
    //                toolView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
    //                break;
    //            default:
    //                break;
    //        }
    //        toolView.localPosition = Vector3.zero;
    //    }

    //    private void OpenOperatin()
    //    {
    //        ClcikButton(4);
    //        DataTool.salaryEntry = SalaryEntry.operating_1;
    //        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    //        {
    //            DataTool.CallNative(187, 0);
    //        }
    //        else
    //        {
    //            UIManager.Instance.Acceptance_Android("Monthly1");
    //        }
    //        //InitState(4,null);
    //    }

    //    private void OpenIssued()
    //    {
    //        ClcikButton(3);
    //        DataTool.salaryEntry = SalaryEntry.issued_1;
    //        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    //        {
    //            DataTool.CallNative(185, 0);
    //        }
    //        else
    //        {
    //            UIManager.Instance.Acceptance_Android("Monthly1");
    //        }
    //        //InitState(3, null);
    //    }

    //    private void OpenMonthly()
    //    {
    //        ClcikButton(2);
    //        DataTool.salaryEntry = SalaryEntry.month_1;
    //        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    //        {
    //            DataTool.CallNative(172, 0);
    //        }
    //        else
    //        {
    //            UIManager.Instance.Acceptance_Android("Monthly1");
    //        }
    //        //InitState(2, null);
    //    }

    //    private void OpenPayro()
    //    {
    //        ClcikButton(1);
    //        DataTool.salaryEntry = SalaryEntry.weeklyend_1;
    //        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    //        {
    //            DataTool.CallNative(166, 0);
    //        }
    //        else
    //        {
    //            UIManager.Instance.Acceptance_Android("Monthly1");
    //        }
    //        //InitState(1, null);
    //    }

    //    private void OpenDaily()
    //    {
    //        ClcikButton(0);
    //        DataTool.salaryEntry = SalaryEntry.dayknot_1;
    //        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    //        {
    //            DataTool.CallNative(170, 0);
    //        }
    //        else
    //        {
    //            UIManager.Instance.Acceptance_Android("Monthly1");
    //        }
    //        //InitState(0, null);
    //    }

    //    //private void OpenMonth()
    //    //{
    //    //    if(dailyParent.activeInHierarchy)
    //    //    {
    //    //        dailyParent.SetActive(false);
    //    //    }
    //    //    else
    //    //    {
    //    //        dailyParent.SetActive(true);
    //    //    }
    //    //}

    //    private void ClcikButton(int index)
    //    {
    //        if(indexCurret >= 0)
    //        {
    //            clickImage[indexCurret].sprite = norSprite;
    //            //clickText[indexCurret].color = norColor;
    //            clickObject[indexCurret].SetActive(false);
    //        }
    //        indexCurret = index;
    //        clickImage[indexCurret].sprite = pickSprite;
    //        //clickText[indexCurret].color = pickColor;
    //        clickObject[indexCurret].SetActive(true);
    //    }

    //    public void ClickMonth(int year, int month,int index)
    //    {
    //        //monthSelect[monthIndex].HideImage(false);
    //        monthIndex = index;
    //        //monthSelect[monthIndex].HideImage(true);
    //        monthCurrent = month;
    //        yearCurrent = year;
    //        //monthText.text = string.Format("{0}年{1}月", year, month);
    //        //dailyParent.gameObject.SetActive(false);
    //        //if (DataTool.isUnit)
    //        //{
    //        //    OpenOperatin();
    //        //}
    //        //else
    //        {
    //            OpenDaily();
    //        }
    //        //InitState(0,null);
    //    }

    //    /// <summary>
    //    /// 获取一年中的周
    //    /// </summary>
    //    /// <param name="dt">日期</param>
    //    /// <returns></returns>
    //    public static int GetWeekOfYear(DateTime dt)
    //    {
    //        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
    //        int weekOfYear = gc.GetWeekOfYear(dt, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

    //        return weekOfYear;
    //    }
    //    /// <summary>
    //    /// 根据一年中的第几周获取该周的开始日期与结束日期
    //    /// </summary>
    //    /// <param name="year"></param>
    //    /// <param name="weekNumber"></param>
    //    /// <param name="culture"></param>
    //    /// <returns></returns>
    //    public static Tuple<DateTime, DateTime> GetFirstEndDayOfWeek(int year, int weekNumber, System.Globalization.CultureInfo culture)
    //    {
    //        System.Globalization.Calendar calendar = culture.Calendar;
    //        DateTime firstOfYear = new DateTime(year, 1, 1, calendar);
    //        DateTime targetDay = calendar.AddWeeks(firstOfYear, weekNumber - 1);
    //        DayOfWeek firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;

    //        while (targetDay.DayOfWeek != firstDayOfWeek)
    //        {
    //            targetDay = targetDay.AddDays(-1);
    //        }

    //        return Tuple.Create<DateTime, DateTime>(targetDay, targetDay.AddDays(6));
    //    }
    //}
    //public class MonthItem
    //{
    //    private Text dateText;

    //    private Button selectBtn;

    //    private GameObject dateImage;

    //    private int month;
    //    private int year;
    //    private int monthIndex;
    //    public MonthItem(Transform parent, int year, int month,int monthIndex)
    //    {
    //        this.year = year;
    //        this.month = month;
    //        this.monthIndex = monthIndex;
    //        dateImage = parent.Find("Image").gameObject;
    //        dateText = parent.Find("Text").GetComponent<Text>();
    //        selectBtn = parent.GetComponent<Button>();
    //        selectBtn.onClick.AddListener(SelectMonth);
    //        dateText.text = string.Format("{0}年{1}月", year, month);
    //        dateImage.SetActive(false);
    //    }

    //    private void SelectMonth()
    //    {
    //        UIManager.Instance.incomePanel.ClickMonth(year,month,monthIndex);
    //    }

    //    public void HideImage(bool isHide)
    //    {
    //        dateImage.SetActive(isHide);
    //    }
    //}
    //public class PaySlipItem
    //{
    //    private Text dateText;
    //    private Text moneyText;
    //    private Button payBtn;

    //    private int type;
    //    private int year;
    //    private int month;
    //    private int day;
    //    private float money;

    //    private Transform slipItem;
    //    private int[] housData = new int[2];
    //    private float[] moneysData = new float[2];
    //    public PaySlipItem(Transform parent)
    //    {
    //        slipItem = parent;
    //        payBtn = parent.GetComponent<Button>();
    //        dateText = parent.Find("DateText").GetComponent<Text>();
    //        moneyText = parent.Find("MoneyText").GetComponent<Text>();
    //        payBtn.onClick.AddListener(OpenBill);
    //    }

    //    public void SetInit(int type,Dictionary<string,object> dataInfo)
    //    {
    //        this.type = type;
    //        housData[0] = int.Parse(dataInfo["work_hours"].ToString());
    //        housData[1] = int.Parse(dataInfo["ot_hours"].ToString());
    //        moneysData[0] = int.Parse(dataInfo["work_sum"].ToString());
    //        moneysData[1] = int.Parse(dataInfo["ot_sum"].ToString());

    //        if (type == 0)
    //        {
    //            this.money = float.Parse(dataInfo["daily_sum"].ToString());
    //            string[] dates = dataInfo["daily_salary_date"].ToString().Split('-');
    //            this.year = int.Parse(dates[0]);
    //            dateText.text = string.Format("{0}月{1}日", dates[1], dates[2]);
    //            moneyText.text = string.Format("+{0:N2}", money);
    //        }
    //        else if (type == 1)
    //        {
    //            this.money = float.Parse(dataInfo["week_sum"].ToString());
    //            string[] dates1 = dataInfo["salary_cal_week_startdate"].ToString().Split('-');
    //            string[] dates2 = dataInfo["salary_cal_week_enddate"].ToString().Split('-');
    //            this.year = int.Parse(dates2[0]);
    //            moneyText.text = string.Format("+{0:N2}", money);
    //            dateText.text = string.Format("{0}月{1}日-{2}月{3}日", dates1[1], dates1[2], dates2[1], dates2[2]);
    //        }
    //        slipItem.gameObject.SetActive(true);
    //    }


    //    public void SetInit(int type, float money, int year, int month, int day)
    //    {
    //        this.type = type;
    //        this.money = money;
    //        this.year = year;
    //        this.month = month;
    //        this.day = day;
    //        slipItem.gameObject.SetActive(true);
    //        switch (type)
    //        {
    //            //case 0:
    //            //    dateText.text = string.Format("{0}月{1}日", month, day);
    //            //    moneyText.text = string.Format("+{0:N2}", money);
    //            //    break;
    //            //case 1:
    //            //    dateText.text = string.Format("{0}年{1}月工资单", year, month);
    //            //    moneyText.text = string.Format("￥{0:N2}", money);
    //             //   break;
    //            case 2:
    //                dateText.text = string.Format("{0}年{1}月工资单", year, month);
    //                moneyText.text = string.Format("￥{0:N2}", money);
    //                break;
    //            case 3:
    //                dateText.text = string.Format("{0}年{1}月{2}日", year, month, day);
    //                moneyText.text = string.Format("￥{0:N2}", money);
    //                break;
    //            case 4:
    //                dateText.text = string.Format("{0}年{1}月", year, month);
    //                moneyText.text = string.Format("￥{0:N2}", money);
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //    public void HideItem()
    //    {
    //        slipItem.gameObject.SetActive(false);
    //    }

    //    private void OpenBill()
    //    {
    //        switch (type)
    //        {
    //            case 0:
    //                UIManager.Instance.dayKnotPanel.OpenPanel("日结明细",string.Format("【{0}年】{1}",year ,dateText.text),money, housData,moneysData);
    //                break;
    //            case 1:
    //                UIManager.Instance.dayKnotPanel.OpenPanel("周结明细", string.Format("【{0}年】{1}", year, dateText.text), money, housData, moneysData);
    //                break;
    //            case 2:
    //                UIManager.Instance.salaryPanel.SetHeadFile(year,month);
    //                DataTool.salaryEntry = SalaryEntry.month_2;
    //                if (Application.platform == RuntimePlatform.Android)
    //                {
    //                    DataTool.CallNative(173, day);
    //                }
    //                else
    //                {
    //                    UIManager.Instance.Acceptance_Android("Monthly2");
    //                }
    //                break;
    //            case 3:
    //                //UIManager.Instance.operatingPanel.OpenPanel();
    //                break;
    //            case 4:
    //                UIManager.Instance.operatingPanel.SetHeadFile(year, month, moneyText.text);
    //                DataTool.salaryEntry = SalaryEntry.operating_2;
    //                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    //                {
    //                    DataTool.CallNative(190,0,string.Format("{0:D2}-{1:D2}", year, month));
    //                }
    //                else
    //                {
    //                    UIManager.Instance.Acceptance_Android("Monthly2");
    //                }
    //                break;
    //            default:
    //                break;
    //        }
    //    }
}
