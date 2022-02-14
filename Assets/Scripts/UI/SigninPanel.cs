using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SigninPanel : MonoBehaviour
{
    private Text weekText;
    private Text totalText;

    private Image weekImage;
    private Image totalImage;

    private Button weekBtn;
    private Button totalBtn;
    private Button backBtn;

    private Transform rankParent;
    private Transform rankItem;
    private List<RankingItem> rankings = new List<RankingItem>();

    private bool isWeek;
    private void Awake()
    {
        //rankParent = transform.Find("bg/RankView/Viewport/Content");
        //rankItem = transform.Find("Item");

        //weekText = transform.Find("bg/WeekBtn/Text").GetComponent<Text>();
        //totalText = transform.Find("bg/TotalBtn/Text").GetComponent<Text>();

        //weekImage = transform.Find("bg/WeekBtn").GetComponent<Image>();
        //totalImage = transform.Find("bg/TotalBtn").GetComponent<Image>();

        //weekBtn = transform.Find("bg/WeekBtn").GetComponent<Button>();
        //totalBtn = transform.Find("bg/TotalBtn").GetComponent<Button>();
        backBtn = transform.Find("bg/BackBtn").GetComponent<Button>();

        //weekBtn.onClick.AddListener(OpenWeek);
        //totalBtn.onClick.AddListener(OpenTotal);
        backBtn.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        //isWeek = false;
        //OpenWeek();
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void InitData()
    {
        for (int i = 0; i < rankings.Count; i++)
        {
            rankings[i].ShowItem(false);
        }

        for (int i = 0; i < 50; i++)
        {
            if (i >= rankings.Count)
            {
                Transform item = Instantiate(rankItem);
                item.gameObject.SetActive(true);
                item.SetParent(rankParent);
                item.localScale = Vector3.one;
                RankingItem ranking = new RankingItem(item, i);
                ranking.SetInfo();
                rankings.Add(ranking);
            }
            else
            {
                rankings[i].ShowItem(true);
                rankings[i].SetInfo();
            }
            if (i == 49)
            {
                rankings[49].ShowEnd();
            }
        }
        float maxY = 50 * 150;
        rankParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
    }

    private void OpenTotal()
    {
        if (isWeek)
        {
            isWeek = false;
            totalImage.color = Color.blue;
            weekImage.color = Color.white;
            rankParent.localPosition = Vector3.zero;
            InitData();
        }
    }

    private void OpenWeek()
    {
        if (!isWeek)
        {
            isWeek = true;
            totalImage.color = Color.white;
            weekImage.color = Color.blue;
            rankParent.localPosition = Vector3.zero;
            InitData();
        }
    }

    //排行类
    private class RankingItem
    {
        private Text levelText;
        private Text typeText;
        private Text nameText;

        private int rankIndex;
        private Transform itemRank;
        private Transform lineEnd;
        public RankingItem(Transform parent, int index)
        {
            itemRank = parent;
            rankIndex = index;
            levelText = parent.Find("LevelText").GetComponent<Text>();
            typeText = parent.Find("TypeText").GetComponent<Text>();
            nameText = parent.Find("NameText").GetComponent<Text>();
            lineEnd = parent.Find("LineEnd");
            lineEnd.gameObject.SetActive(false);
        }

        public void ShowItem(bool isShow)
        {
            if (isShow)
            {
                itemRank.gameObject.SetActive(true);
            }
            else
            {
                itemRank.gameObject.SetActive(false);
                lineEnd.gameObject.SetActive(false);
            }
        }

        public void ShowEnd()
        {
            lineEnd.gameObject.SetActive(true);
        }

        public void SetInfo()
        {
            levelText.text = "11级";
            typeText.text = "红富士苹果";
            nameText.text = string.Format("{0} {1}", rankIndex, "李三");
        }
    }
}
