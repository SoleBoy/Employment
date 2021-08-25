using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayrollPanel : MonoBehaviour
{
    private Text dateText;
    private Text wageText;

    private Button backBtn;

    private Transform shouldAdd;
    private Transform shouldSub;
    private Transform lateAdd;
    private Transform lateSub;

    private List<PayrollItem> shouldAdd_item = new List<PayrollItem>();
    private List<PayrollItem> shouldSub_item = new List<PayrollItem>();
    private List<PayrollItem> lateAdd_item = new List<PayrollItem>();
    private List<PayrollItem> lateSub_item = new List<PayrollItem>();

    private string[] shouldAdd_Info = { "nor_working_hours", "g1", "g2", "g3" };
    private string[] shouldSub_Info = { "per_absence_hours", "per_absence_hours2", "sick_leave", "medical", "marriage", "funeral", "other_leave", "out_work", "lack_work", "late_work", "early_work", "no_card" };
    private string[] lateAdd_Info = { "syfs" };
    private string[] lateSub_Info = { "income_tax", "bus_insure", "withhold_insure", "advance_cost", "cp9lfy", "hskk", "zssd"};

    private string typeItem;
    private void Awake()
    {
        shouldAdd = transform.Find("Info/ShouldAdd");
        shouldSub = transform.Find("Info/ShouldSub");
        lateAdd = transform.Find("Info/LateAdd");
        lateSub = transform.Find("Info/LateSub");

        Transform shouldAdd_parent = shouldAdd.Find("Viewport/Content");
        for (int i = 0; i < shouldAdd_parent.childCount; i++)
        {
            PayrollItem item = new PayrollItem(shouldAdd_parent.GetChild(i));
            shouldAdd_item.Add(item);
        }
        Transform shouldSub_parent = shouldSub.Find("Viewport/Content");
        for (int i = 0; i < shouldSub_parent.childCount; i++)
        {
            PayrollItem item = new PayrollItem(shouldSub_parent.GetChild(i));
            shouldSub_item.Add(item);
        }
        Transform lateAdd_parent = lateAdd.Find("Viewport/Content");
        for (int i = 0; i < lateAdd_parent.childCount; i++)
        {
            PayrollItem item = new PayrollItem(lateAdd_parent.GetChild(i));
            lateAdd_item.Add(item);
        }
        Transform lateSub_parent = lateSub.Find("Viewport/Content");
        for (int i = 0; i < lateSub_parent.childCount; i++)
        {
            PayrollItem item = new PayrollItem(lateSub_parent.GetChild(i));
            lateSub_item.Add(item);
        }

        dateText = transform.Find("TopBg/DateText").GetComponent<Text>();
        wageText = transform.Find("TopBg/WageText").GetComponent<Text>();

        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(ClosePanel);
    }

    public void SetInfo(string dateInfo,string type,string monery)
    {
        gameObject.SetActive(true);
        typeItem = type;
        dateText.text = dateInfo+" "+type;
        wageText.text = monery;
        HidePayrollItem();
        if (type == "应发前加项")
        {
            shouldAdd.gameObject.SetActive(true);
        }
        else if (type == "应发前减项")
        {
            shouldSub.gameObject.SetActive(true);
        }
        else if (type == "应发后加项")
        {
            lateAdd.gameObject.SetActive(true);
        }
        else if (type == "应发后减项")
        {
            lateSub.gameObject.SetActive(true);
        }
    }

    public void OpenPanel(Dictionary<string, object> detailInfo)
    {
        if (typeItem == "应发前加项")
        {
            for (int i = 0; i < shouldAdd_item.Count; i++)
            {
                if (detailInfo.ContainsKey(shouldAdd_Info[i]))
                {
                    shouldAdd_item[i].SetInfo(string.Format("+{0}", detailInfo[shouldAdd_Info[i]]));
                }
                else
                {
                    shouldAdd_item[i].SetInfo("+0");
                }
            }
        }
        else if (typeItem == "应发前减项")
        {
            for (int i = 0; i < shouldSub_item.Count; i++)
            {
                if(detailInfo.ContainsKey(shouldSub_Info[i]))
                {
                    shouldSub_item[i].SetInfo(string.Format("-{0}", detailInfo[shouldSub_Info[i]]));
                }
                else
                {
                    shouldSub_item[i].SetInfo("-0");
                }
            }
        }
        else if (typeItem == "应发后加项")
        {
            for (int i = 0; i < lateAdd_item.Count; i++)
            {
                if (detailInfo.ContainsKey(lateAdd_Info[i]))
                {
                    lateAdd_item[i].SetInfo(string.Format("+{0}", detailInfo[lateAdd_Info[i]]));
                }
                else
                {
                    lateAdd_item[i].SetInfo("+0");
                }
            }
        }
        else if (typeItem == "应发后减项")
        {
            for (int i = 0; i < lateSub_item.Count; i++)
            {
                if (detailInfo.ContainsKey(lateSub_Info[i]))
                {
                    lateSub_item[i].SetInfo(string.Format("-{0}", detailInfo[lateSub_Info[i]]));
                }
                else
                {
                    lateSub_item[i].SetInfo("-0");
                }
            }
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }


    private void HidePayrollItem()
    {
        shouldAdd.gameObject.SetActive(false);
        shouldSub.gameObject.SetActive(false);
        lateAdd.gameObject.SetActive(false);
        lateSub.gameObject.SetActive(false);
    }

    private class PayrollItem
    {
        private Text infoName;
        private Text infoDetails;

        public PayrollItem(Transform parent)
        {
            infoName = parent.Find("Duration").GetComponent<Text>();
            infoDetails = parent.Find("Amount").GetComponent<Text>();
        }

        public void SetInfo(string info)
        {
            infoDetails.text = info;
        }
    }
}
