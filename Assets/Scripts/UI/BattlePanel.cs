using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : MonoBehaviour
{
    public Sprite pickImage;
    public Sprite norImage;

    private Text ranText;
    private Text lastText;
    private Text timeText;

    private Button ranBtn;
    private Button battleBtn;
    private Button refreshBtn;
    private Button backBtn;

    private int battleCount;
    private int currentCount;
    private float refreshTime;
    private bool isRefresh;
    private List<UserItem> users = new List<UserItem>();
    private void Awake()
    {
        ranText = transform.Find("Ranking/RanText").GetComponent<Text>();
        lastText = transform.Find("PkNumber/NumberText").GetComponent<Text>();
        timeText = transform.Find("Bottom/SwapBtn/TimeText").GetComponent<Text>();

        ranBtn = transform.Find("Bottom/RanBtn").GetComponent<Button>();
        battleBtn = transform.Find("Bottom/StarBtn").GetComponent<Button>();
        refreshBtn = transform.Find("Bottom/SwapBtn").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        ranBtn.onClick.AddListener(OpenRanking);
        battleBtn.onClick.AddListener(OpenBattle);
        refreshBtn.onClick.AddListener(OpenRefres);
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

    public void SetUpdata(float dataTime)
    {
        if(isRefresh)
        {
            refreshTime -= dataTime;
            timeText.text = string.Format("{0:F0}秒", refreshTime);
            if(refreshTime <= 0)
            {
                isRefresh = false;
                refreshBtn.GetComponent<Image>().color=Color.white;
            }
        }
    }

    private void InitData()
    {
        battleCount = 5;
        currentCount = -1;
        refreshTime = 150;
        lastText.text = "5";
        ranText.text = UnityEngine.Random.Range(1000, 2000).ToString();
        timeText.text = string.Format("{0}秒", refreshTime);
        for (int i = 1; i < 6; i++)
        {
            UserItem user = new UserItem(transform.Find("Parent/Equal" + i),i);
            users.Add(user);
            user.SetInfo(DataTool.GetName(), UnityEngine.Random.Range(10,20), UnityEngine.Random.Range(1000, 2000));
        }
    }

    private void OpenRanking()
    {
        UIManager.Instance.rankingPanel.OpenPanel();
    }

    private void OpenBattle()
    {
        if(battleCount>=1)
        {
            if(currentCount >= 0)
            {
                battleCount -= 1; currentCount -= 1;
                lastText.text = battleCount.ToString();
                UIManager.Instance.fightPanel.OpenPanel(users[currentCount].messgName, users[currentCount].gradeIndex, 5);
                for (int i = 0; i < users.Count; i++)
                {
                    users[i].SetInfo(DataTool.GetName(), UnityEngine.Random.Range(10, 20), UnityEngine.Random.Range(1000, 2000));
                }
                //users[currentCount].IsSelected();
            }
            else
            {
                UIManager.Instance.CloningTips("请选择挑战对手");
            }
           
        }
        else
        {
            UIManager.Instance.CloningTips("战斗次数已经用完");
        }
    }

    private void OpenRefres()
    {
        if (!isRefresh)
        {
            isRefresh = true;
            currentCount = -1;
            refreshBtn.GetComponent<Image>().color = Color.gray;
            for (int i = 0; i < users.Count; i++)
            {
                users[i].SetInfo(DataTool.GetName(), UnityEngine.Random.Range(10, 20), UnityEngine.Random.Range(1000, 2000));
            }
        }
    }

    public void HideUser(int index)
    {
        if(currentCount >= 0)
        {
            users[currentCount-1].IsSelected();
        }
        currentCount = index;
    }

    //用户类
    private class UserItem
    {
        private Text nameText;
        private Text rankText;
        private Image headImage;
        private Image frameImage;
        private Button selectBtn;
        private GameObject bottom;
        private int userIndex;
        public int gradeIndex;
        public string messgName;
        private bool isSelect;
        
        public UserItem(Transform parent,int index)
        {
            userIndex = index;
            bottom = parent.Find("Bottom").gameObject;
            nameText = parent.Find("Name").GetComponent<Text>();
            rankText = parent.Find("Ranking").GetComponent<Text>();

            headImage = parent.Find("Head").GetComponent<Image>();
            frameImage = parent.Find("Frame").GetComponent<Image>();

            selectBtn = parent.Find("Head").GetComponent<Button>();
            selectBtn.onClick.AddListener(SelectHead);
        }

        private void SelectHead()
        {
            if(isSelect)
            {
                isSelect = false;
                bottom.gameObject.SetActive(true);
                frameImage.sprite = UIManager.Instance.battlePanel.pickImage;
                UIManager.Instance.battlePanel.HideUser(userIndex);
            }
        }

        public void SetInfo(string name, int grade,int ranking)
        {
            IsSelected();
            messgName = name;
            gradeIndex = grade;
            nameText.text = name;
            nameText.text = string.Format("{0} LV.{1}",name,grade);
            rankText.text = string.Format("排名:{0}", ranking);
        }

        public void IsSelected()
        {
            isSelect = true;
            bottom.gameObject.SetActive(false);
            frameImage.sprite = UIManager.Instance.battlePanel.norImage;
        }
    }
}
