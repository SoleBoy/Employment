using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperatingPanel : MonoBehaviour
{
    private Text dateText;
    private Text wageText;

    private Button backBtn;

    private string month_Curr;

    private string[] messgInfos = { "yfze", "kzfy", "rhfy", "sfze" };
    private List<OperatingItem> operatings = new List<OperatingItem>();
    private void Awake()
    {
        dateText = transform.Find("TopBg/DateText").GetComponent<Text>();
        wageText = transform.Find("TopBg/WageText").GetComponent<Text>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(ClosePanel);

        Transform parent = transform.Find("Info/Scroll View/Viewport/Content");
        for (int i = 0; i < parent.childCount; i++)
        {
            OperatingItem item = new OperatingItem(parent.GetChild(i));
            operatings.Add(item);
        }
    }

    public void SetHeadFile(int year, int month, string monery)
    {
        gameObject.SetActive(true);
        month_Curr = month.ToString();
        dateText.text = string.Format("{0}年{1}月 工资单", year, month);
        wageText.text = monery;
    }

    public void OpenPanel(Dictionary<string, object> detailInfo)
    {
        for (int i = 0; i < operatings.Count; i++)
        {
            if (detailInfo.ContainsKey(messgInfos[i]))
            {
                operatings[i].SetInfo(string.Format("￥{0:N2}", detailInfo[messgInfos[i]]));
            }
            else
            {
                operatings[i].SetInfo("￥0");
            }
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private class OperatingItem
    {
        private Text infoName;
        private Text infoDetails;

        public OperatingItem(Transform parent)
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
