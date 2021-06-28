using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayKnotPanel : MonoBehaviour
{
    private Text titleText;
    private Text dateText;
    private Text wageText;

    private Button backBtn;

    private Transform kontItem;
    private Transform knotParent;
    private int[] hous = {8,2,2};
    private float[] moneys = {0.7f,0.2f,0.1f};
    private List<DayKnotItem> dayKnots = new List<DayKnotItem>();
    private void Awake()
    {
        kontItem = transform.Find("Info/Item");
        knotParent = transform.Find("Info/Scroll View/Viewport/Content");
        titleText = transform.Find("TitleText").GetComponent<Text>();
        dateText = transform.Find("Info/DateText").GetComponent<Text>();
        wageText = transform.Find("Info/WageText").GetComponent<Text>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(ClosePanel);
        string[] messg = {"工作时长","加班","补贴" };
        for (int i = 0; i < messg.Length; i++)
        {
            var item = Instantiate(kontItem);
            item.gameObject.SetActive(true);
            item.SetParent(knotParent);
            item.localScale = Vector3.one;
            DayKnotItem day = new DayKnotItem(item, messg[i]);
            dayKnots.Add(day);
        }
    }

    public void OpenPanel(string titleMessg,string messgInfo,float money)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < dayKnots.Count; i++)
        {
            dayKnots[i].SetHour(hous[i],money * moneys[i]);
        }
        titleText.text = titleMessg;
        dateText.text = messgInfo;
        wageText.text = string.Format("￥{0:N2}", money);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private class DayKnotItem
    {
        private Text durationText;
        private Text hourText;
        private Text moneyText;

        public DayKnotItem(Transform parent,string messg)
        {
            durationText = parent.Find("Duration").GetComponent<Text>();
            hourText = parent.Find("Hour").GetComponent<Text>();
            moneyText = parent.Find("Money").GetComponent<Text>();

            durationText.text = messg;
        }

        public void SetHour(int hour,float money)
        {
            hourText.text = string.Format("{0}小时", hour);
            moneyText.text = string.Format("+{0:N2}", money);
        }
    }
}
