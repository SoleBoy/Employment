using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SigninPanel : MonoBehaviour
{
    public Transform[] itemSigins;
    public Transform[] itemActivities;

    private Text siginText;

    private Image checkProgress;
    private Button siginBtn;
    private Button backBtn;

    private List<DailyActivities> activities = new List<DailyActivities>();
    private List<SiginItem> siginItems = new List<SiginItem>();

    private void Awake()
    {
        siginText = transform.Find("bg/SiginBtn/Text").GetComponent<Text>();
        checkProgress = transform.Find("bg/Progress").GetComponent<Image>();
        siginBtn = transform.Find("bg/SiginBtn").GetComponent<Button>();
        backBtn = transform.Find("bg/BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        siginBtn.onClick.AddListener(DailyReward);
        for (int i = 0; i < itemActivities.Length; i++)
        {
            DailyActivities item = new DailyActivities(itemActivities[i], i);
            activities.Add(item);
        }
        for (int i = 0; i < itemSigins.Length; i++)
        {
            SiginItem item = new SiginItem(itemSigins[i], i);
            siginItems.Add(item);
        }
        if (DataTool.farmData.dailyCheck == "false")
        {
            siginText.text = "领取";
            checkProgress.fillAmount = (DataTool.farmData.checkTimes - 1) * 0.2f; 
        }
        else
        {
            siginText.text = "已领取";
            checkProgress.fillAmount = DataTool.farmData.checkTimes * 0.2f;
        }
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);

    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void DailyReward()
    {
        if(DataTool.farmData.dailyCheck == "false")
        {
            DataTool.farmData.dailyCheck = "true";
            siginText.text = "已领取";
            checkProgress.fillAmount = DataTool.farmData.checkTimes * 0.2f;
        }
    }

    //日常活动
    private class DailyActivities
    {
        private Text typeText;
        private Text nameText;
        private Text btnText;
        private Button completeBtn;

        private int siginIndex;
        public DailyActivities(Transform parent, int index)
        {
            siginIndex = index;
            typeText = parent.Find("TypeText").GetComponent<Text>();
            nameText = parent.Find("NameText").GetComponent<Text>();
            btnText = parent.Find("Btn/Text").GetComponent<Text>();
            completeBtn = parent.Find("Btn").GetComponent<Button>();
            completeBtn.onClick.AddListener(OpenTask);
        }

        private void OpenTask()
        {
            if (siginIndex == 0) //上班打卡（0/1）
            {

            }
            else if (siginIndex == 1)  //分享给好友（0/3）
            {

            }
            else if (siginIndex == 2)  //"蜜蜂采蜜"PK游戏（0/1）
            {

            }
            else if (siginIndex == 3)  //上传劳动成果（0/1）
            {

            }
        }

        public void SetInfo(int index)
        {
            if (index == 0) //上班打卡（0/1）
            {
                typeText.text = string.Format("上班打卡({0}/1)", 0);
            }
            else if (index == 1)  //分享给好友（0/3）
            {
                typeText.text = string.Format("分享给好友({0}/3)", 0);
            }
            else if (index == 2)  //"蜜蜂采蜜"PK游戏（0/1）
            {
                typeText.text = string.Format("蜜蜂采蜜PK游戏({0}/1)", 0);
            }
            else if (index == 3)  //上传劳动成果（0/1）
            {
                typeText.text = string.Format("上传劳动成果({0}/1)", 0);
            }
        }
    }
    //每日签到
    private class SiginItem
    {
        private Text typeText;
        private Text nameText;

        private int siginIndex;
        public SiginItem(Transform parent, int index)
        {
            siginIndex = index;
            typeText = parent.Find("TypeText").GetComponent<Text>();
            nameText = parent.Find("NameText").GetComponent<Text>();
        }

        private void OpenTask()
        {
            if (siginIndex == 0) //上班打卡（0/1）
            {
               
            }
            else if (siginIndex == 1)  //分享给好友（0/3）
            {

            }
            else if (siginIndex == 2)  //"蜜蜂采蜜"PK游戏（0/1）
            {

            }
            else if (siginIndex == 3)  //上传劳动成果（0/1）
            {

            }
        }

        public void SetInfo(int index)
        {
            if (index == 0) //上班打卡（0/1）
            {
                typeText.text = "第一天";
            }
            else if (index == 1)  //分享给好友（0/3）
            {
                typeText.text = "第二天";
            }
            else if (index == 2)  //"蜜蜂采蜜"PK游戏（0/1）
            {
                typeText.text = "第三天";
            }
            else if (index == 3)  //上传劳动成果（0/1）
            {
                typeText.text = "第四天";
            }
            else if (index == 4)  //上传劳动成果（0/1）
            {
                typeText.text = "第五天";
            }
        }
    }
}
