using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalaryPanel : MonoBehaviour
{
    private Text dateText;
    private Text wageText;

    private Button backBtn;

    private Transform wageView;
    private List<SalaryDetails> salaries = new List<SalaryDetails>();

    public int infoID;
    private void Awake()
    {
        wageView = transform.Find("Info/WageView/Viewport/Content");
        dateText = transform.Find("TopBg/DateText").GetComponent<Text>();
        wageText = transform.Find("TopBg/WageText").GetComponent<Text>();

        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(ClosePanel);
        InitData();
    }
    //2021年4月  工资单 发薪日实发 fxrsf  应发前加项	yf_before_add 应发前减项 yf_before_deduct
    //应发后加项 yf_after_add 应发后减项 yf_after_deduct 应发总额 yfze
    public void SetHeadFile(int year, int month)
    {
        gameObject.SetActive(true);
        dateText.text = string.Format("{0}年{1}月 工资单", year, month);
    }
    public void OpenPanel(Dictionary<string,object> detailInfo)
    {
        infoID = int.Parse(detailInfo["id"].ToString());
        if (detailInfo.ContainsKey("fxrsf"))
        {
            wageText.text = string.Format("￥{0:N2}", detailInfo["fxrsf"]);
        }
        else
        {
            wageText.text = string.Format("￥{0:N2}", 3600);
        }
        salaries[0].SetInfo(string.Format("￥{0:N2}", detailInfo["yfze"]), dateText.text);
        salaries[1].SetInfo(string.Format("+{0:N2}", detailInfo["yf_before_add"]), dateText.text);
        salaries[2].SetInfo(string.Format("-{0:N2}", detailInfo["yf_before_deduct"]), dateText.text);
        salaries[3].SetInfo(string.Format("+{0:N2}", detailInfo["yf_after_add"]), dateText.text);
        salaries[4].SetInfo(string.Format("-{0:N2}", detailInfo["yf_after_deduct"]), dateText.text);
        salaries[5].SetInfo(string.Format("￥{0:N2}", detailInfo["fxrsf"]), dateText.text);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    //数据初始
    private void InitData()
    {
        for (int i = 0; i < wageView.childCount; i++)
        {
            SalaryDetails item = new SalaryDetails(wageView.GetChild(i));
            salaries.Add(item);
        }
    }
    //工资条目
    private class SalaryDetails
    {
        private Text durationText;
        private Text amountText;
        private Button wageBtn;
        private GameObject detailed;
        private string dateInfo;
        public SalaryDetails(Transform parent)
        {
            durationText = parent.Find("Duration").GetComponent<Text>();
            amountText = parent.Find("Amount").GetComponent<Text>();
            wageBtn = parent.GetComponent<Button>();
            detailed = parent.Find("Duration").gameObject;
            wageBtn.onClick.AddListener(OpenDetails);
        }

        public void SetInfo(string wageInfo,string date)
        {
            amountText.text = wageInfo;
            dateInfo = date;
        }

        private void OpenDetails()
        {
            //UIManager.Instance.payrollPanel.SetInfo(dateInfo, durationText.text, amountText.text);
            //DataTool.salaryEntry = SalaryEntry.month_3;
            //if (Application.platform == RuntimePlatform.Android)
            //{
            //    DataTool.CallNative(174, UIManager.Instance.salaryPanel.infoID);
            //}
            //else
            //{
            //    UIManager.Instance.Acceptance_Android("Monthly3");
            //}
        }
    }
}
