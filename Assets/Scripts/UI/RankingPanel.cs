using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingPanel : MonoBehaviour
{
    private Text rankingText;
    private Text rewardText;

    private Image dailyImage;
    private Image weekImage;

    private Button dailyBtn;
    private Button weekBtn;
    private Button rewardBtn;
    private Button backBtn;

    private Transform rankParent;
    private Transform rankItem;
    private List<RankingItem> rankings = new List<RankingItem>();

    private bool isDaily;

    public Sprite selectSprite;
    public Sprite normalSprite;
    private void Awake()
    {
        rankParent = transform.Find("RankView/Viewport/Content");
        rankItem = transform.Find("Item");

        rankingText = transform.Find("LastBg/RankingText").GetComponent<Text>();
        rewardText = transform.Find("LastBg/RewardText").GetComponent<Text>();

        dailyImage = transform.Find("DailyBtn").GetComponent<Image>();
        weekImage = transform.Find("WeekBtn").GetComponent<Image>();

        dailyBtn = transform.Find("DailyBtn").GetComponent<Button>();
        weekBtn = transform.Find("WeekBtn").GetComponent<Button>();
        rewardBtn = transform.Find("LastBg/RewardBtn").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        dailyBtn.onClick.AddListener(OpenDaily);
        weekBtn.onClick.AddListener(OpenWeek);
        rewardBtn.onClick.AddListener(OpenRewar);
        backBtn.onClick.AddListener(ClosePanel);
        InitData();
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void InitData()
    {
        rankingText.text = string.Format("昨日,我的名次{0}",1000);
        rewardText.text = string.Format("奖励:经验值{0}",100);
        for (int i = 1; i <= 50; i++)
        {
            Transform item = null;
            if (i <= 3)
            {
                item = rankParent.GetChild(i-1);
            }
            else
            {
                item = Instantiate(rankItem);
                item.gameObject.SetActive(true);
                item.SetParent(rankParent);
                item.localScale = Vector3.one;
            }
            RankingItem ranking = new RankingItem(item,i);
            rankings.Add(ranking);
            ranking.SetInfo(DataTool.GetName(), "奖励经验值1.5k");
        }
        isDaily = false;
        dailyImage.sprite = selectSprite;
        weekImage.sprite = normalSprite;
        float maxY = 50 * 150 + 50 * 35 + 50;
        rankParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
    }

    private void OpenDaily()
    {
        if(isDaily)
        {
            isDaily = false;
            dailyImage.sprite = selectSprite;
            weekImage.sprite = normalSprite;
            rankParent.localPosition = Vector3.zero;
        }
    }

    private void OpenWeek()
    {
        if (!isDaily)
        {
            isDaily = true;
            dailyImage.sprite = normalSprite;
            weekImage.sprite = selectSprite;
            rankParent.localPosition = Vector3.zero;
        }
    }

    private void OpenRewar()
    {
        UIManager.Instance.CloningTips("经验值+100");
        rewardBtn.enabled = false;
        rewardBtn.GetComponent<Image>().color=Color.gray;
    }
    //排行类
    private class RankingItem
    {
        private Text kingText;
        private Text moreText;
        private Text nameText;
        private Text infoText;

        public RankingItem(Transform parent,int index)
        {
            kingText = parent.Find("KingText").GetComponent<Text>();
            moreText = parent.Find("MoreText").GetComponent<Text>();
            nameText = parent.Find("NameText").GetComponent<Text>();
            infoText = parent.Find("InfoText").GetComponent<Text>();

            kingText.text = index.ToString();
        }

        public void SetInfo(string name,string messg)
        {
            moreText.text = "0";
            nameText.text = name;
            infoText.text = messg;
        }
    }
}
