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
    private void Awake()
    {
        wageView = transform.Find("Info/WageView/Viewport/Content");
        dateText = transform.Find("Info/DateText").GetComponent<Text>();
        wageText = transform.Find("Info/WageText").GetComponent<Text>();

        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(ClosePanel);
        InitData();
    }
    //2021年4月  工资单
    public void OpenPanel(int year,int month,int money)
    {
        gameObject.SetActive(true);
        dateText.text = string.Format("{0}年{1}月 工资单", year, month);
        wageText.text = string.Format("￥{0:N2}",money);
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
        public SalaryDetails(Transform parent)
        {
            durationText = parent.Find("Duration").GetComponent<Text>();
            amountText = parent.Find("Amount").GetComponent<Text>();
            wageBtn = parent.GetComponent<Button>();
            detailed = parent.Find("Duration").gameObject;
            wageBtn.onClick.AddListener(OpenDetails);
            //wageBtn.enabled = isOpen;
            //detailed.SetActive(isOpen);
        }

        public void SetInfo(int year,int month,float money)
        {
            durationText.text = string.Format("{0}年{1}月",year,month);
            amountText.text = string.Format("{0:N2}", money);
        }

        private void OpenDetails()
        {
            UIManager.Instance.payrollPanel.OpenPanel();
        }
    }
}
