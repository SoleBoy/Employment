using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private Button backBtn;
    private Button dailyBtn;
    private Button factoryBtn;
    private Button fertilizeBtn;
    private Button rankBtn;
    private Button receiveBtn;
    private Button helpBtn;
    private Button meteringBtn;

    private Text manureText;
    private Text numberText;
    private Text dailyText;
    private Text factoryText;
    private Text dailyFertilizer;
    private Text levelText;
    private Text infoText;
    private Text meteringText;
    private Image levelImage;

    private void Awake()
    {
        levelImage = transform.Find("Schedule/Image").GetComponent<Image>();
        levelText = transform.Find("Schedule/Level").GetComponent<Text>();
        infoText = transform.Find("Schedule/Info").GetComponent<Text>();
        dailyText = transform.Find("Daily/receive/Text").GetComponent<Text>();
        factoryText = transform.Find("Factory/receive/Text").GetComponent<Text>();
        dailyFertilizer = transform.Find("Daily/Text").GetComponent<Text>();
        manureText = transform.Find("Fertilize/Manure").GetComponent<Text>();
        numberText = transform.Find("Fertilize/Number/Text").GetComponent<Text>();
        meteringText = transform.Find("Schedule/Metering/Text").GetComponent<Text>();

        dailyBtn = transform.Find("Daily").GetComponent<Button>();
        factoryBtn = transform.Find("Factory").GetComponent<Button>();
        fertilizeBtn = transform.Find("Fertilize").GetComponent<Button>();
        rankBtn = transform.Find("Ranking").GetComponent<Button>();
        receiveBtn = transform.Find("Receive").GetComponent<Button>();
        helpBtn = transform.Find("Help").GetComponent<Button>();
        meteringBtn = transform.Find("Schedule/Metering").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        dailyBtn.onClick.AddListener(DailyReward);
        factoryBtn.onClick.AddListener(FactoryReward);
        fertilizeBtn.onClick.AddListener(Fertilize);
        rankBtn.onClick.AddListener(OpenRanking);
        receiveBtn.onClick.AddListener(SignInReward);
        helpBtn.onClick.AddListener(OpenHelp);
        meteringBtn.onClick.AddListener(OpenMetering);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        ValueAdjustment();
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void ValueAdjustment()
    {
        Quantity(0);
        infoText.text = "施肥升等级";

        //dailyFertilizer.text = string.Format("肥料\n{0}", dailyNum);
        levelImage.fillAmount = DataTool.farmData.expCurrent / DataTool.farmData.expMax;
        if (DataTool.farmData.dailyMuck == "true")
        {
            factoryText.text = "明日7:00可领";
        }
        else
        {
            factoryText.text = "领取";
        }
    }

    private void DailyReward()
    {
        //if(!isDaily)
        //{
        //    Quantity(50000);
        //    isDaily = true;
        //    dailyText.text = "明日7:00可领";
        //    PlayerPrefs.SetString("DailySignIn", "Daily" + System.DateTime.Now.Date);
        //}
    }

    private void FactoryReward()
    {
        if (DataTool.farmData.dailyMuck == "false")
        {
            Quantity(20000000000);
            DataTool.farmData.dailyMuck = "true";
            factoryText.text = "明日可领";
        }
    }

    private void Fertilize()
    {
        if(DataTool.farmData.dailyFer < 10 && DataTool.farmData.numberMuck >= 1 && DataTool.farmData.treeGrade < 16)
        {
            DataTool.farmData.dailyFer += 1;
            Quantity(-600);
            DataTool.farmData.expCurrent += 600;
            float billie = DataTool.farmData.expCurrent / DataTool.farmData.expMax;
            levelImage.fillAmount = billie;
            infoText.text = string.Format("在施肥{0}%花香飘逸", billie.ToString("F2"));
            if (DataTool.farmData.expCurrent >= DataTool.farmData.expMax)
            {
                DataTool.farmData.treeGrade += 1;
                DataTool.farmData.expCurrent = 0;
                if (DataTool.farmData.treeGrade == 1)
                {
                    DataTool.farmData.expMax = 3000 * DataTool.farmData.treeGrade;
                }
                else if (DataTool.farmData.treeGrade > 1 && DataTool.farmData.treeGrade <= 10)
                {
                    DataTool.farmData.expMax = (DataTool.farmData.treeGrade - 1) * 6000;
                }
                else if (DataTool.farmData.treeGrade > 10)
                {
                    DataTool.farmData.expMax = 72000 + (DataTool.farmData.treeGrade - 11) * 18000;
                }
                Debug.Log(DataTool.farmData.expMax);
                levelText.text = string.Format("{0}级", DataTool.farmData.treeGrade);

                levelImage.fillAmount = 0;
            }
        }
        else
        {
            if (DataTool.farmData.numberMuck < 1)
            {
                UIManager.Instance.CloningTips("请完成活动获取更多肥料");
            }
            else
            {
                UIManager.Instance.CloningTips("今日施肥数已达上限");
            }
        }
    }

    private void OpenRanking()
    {
        UIManager.Instance.rankingPanel.OpenPanel();
    }

    private void SignInReward()
    {
        UIManager.Instance.signinPanel.OpenPanel();
    }

    private void OpenHelp()
    {

    }

    private void OpenMetering()
    {
        //if(meteringNum >= meteringSum)
        //{
        //    meteringNum = 0;
        //    meteringSum += 2;
        //    Quantity(600);
        //    UIManager.Instance.CloningTips("领取肥料500");
        //    meteringText.text = string.Format("施肥{0}次可领", meteringSum);
        //}
    }

    private void Quantity(float number)
    {
        DataTool.farmData.totalMuck += number;
        DataTool.farmData.numberMuck = (float)Math.Floor((double)(DataTool.farmData.totalMuck / 600));
        manureText.text = string.Format("我的肥料 {0}", DataTool.UnitConversion(DataTool.farmData.totalMuck)); 
        numberText.text = DataTool.farmData.numberMuck.ToString("F0");
    }

    //private void OpenFruit()
    //{
    //    //string[] messg = {"苹果","猕猴桃","橘子","橙子","香蕉" };
    //    //for (int i = 0; i < 4; i++)
    //    //{
    //    //    var item = Instantiate(fruitItem);
    //    //    item.gameObject.SetActive(true);
    //    //    item.SetParent(fruitParent);
    //    //    item.localScale = Vector3.one;
    //    //    FruitInfo fruit = new FruitInfo(item);
    //    //    fruit.SetInfo(messg[i],Random.Range(2,5));
    //    //    fruitInfos.Add(fruit);
    //    //}
    //    //float maxY = 2 * 360 + 2 * 50;
    //    //fruitParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
    //}

    //public void CurretFruit(string messg)
    //{
    //    for (int i = 0; i < fruitInfos.Count; i++)
    //    {
    //        fruitInfos[i].Cancel();
    //    }
    //    currentFruit = messg;
    //    Debug.Log("当前选择：" + messg);
    //}

    //private void OpenFruitFarm()
    //{
    //    chooseFruit.gameObject.SetActive(false);
    //    fruitFarm.gameObject.SetActive(true);
    //}

    private class FruitInfo
    {
        private Image headImage;
        private Text nameText;
        private Text weightText;

        private Button checkBtn;
        private GameObject checkObject;
        public FruitInfo(Transform parent)
        {
            checkObject = parent.Find("Check/Image").gameObject;
            headImage = parent.Find("Head").GetComponent<Image>();
            nameText = parent.Find("Name").GetComponent<Text>();
            weightText = parent.Find("Weight").GetComponent<Text>();
            checkBtn = parent.GetComponent<Button>();

            checkObject.SetActive(false);
            checkBtn.onClick.AddListener(ChooseFruit);
        }

        public void SetInfo(string nameMessg,float weight)
        {
            nameText.text = nameMessg;
            weightText.text = string.Format("{0}斤", weight);
        }

        private void ChooseFruit()
        {
            //UIManager.Instance.gamePanel.CurretFruit(nameText.text);
            checkObject.SetActive(true);
        }

        public void Cancel()
        {
            checkObject.SetActive(false);
        }

    }
}
