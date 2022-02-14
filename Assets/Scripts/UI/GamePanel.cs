using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private Button backBtn;

    private Transform fruitFarm;

    private Button dailyBtn;
    private Button fertilizeBtn;
    private Button rankBtn;
    private Button receiveBtn;
    private Button helpBtn;
    private Button meteringBtn;

    private Text manureText;
    private Text numberText;
    private Text dailyText;
    private Text dailyFertilizer;
    private Text levelText;
    private Text infoText;
    private Text meteringText;
    private Image levelImage;

    private bool isDaily;

    private float manureSum;
    private float numberManure;
    private float levelCurr;
    private float empiricalCurr;
    private float empiricalSum;
    private float meteringNum;
    private float dailyNum;
    private void Awake()
    {
        fruitFarm = transform.Find("FruitFarm");
        levelImage = fruitFarm.Find("Schedule/Image").GetComponent<Image>();
        levelText = fruitFarm.Find("Schedule/Level").GetComponent<Text>();
        infoText = fruitFarm.Find("Schedule/Info").GetComponent<Text>();
        dailyText = fruitFarm.Find("Daily/receive/Text").GetComponent<Text>();
        dailyFertilizer = fruitFarm.Find("Daily/Text").GetComponent<Text>();
        manureText = fruitFarm.Find("Fertilize/Manure").GetComponent<Text>();
        numberText = fruitFarm.Find("Fertilize/Number/Text").GetComponent<Text>();
        meteringText = fruitFarm.Find("Schedule/Metering/Text").GetComponent<Text>();

        dailyBtn = fruitFarm.Find("Daily").GetComponent<Button>();
        fertilizeBtn = fruitFarm.Find("Fertilize").GetComponent<Button>();
        rankBtn = fruitFarm.Find("Ranking").GetComponent<Button>();
        receiveBtn = fruitFarm.Find("Receive").GetComponent<Button>();
        helpBtn = fruitFarm.Find("Help").GetComponent<Button>();
        meteringBtn = fruitFarm.Find("Schedule/Metering").GetComponent<Button>();

        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        dailyBtn.onClick.AddListener(DailyReward);
        fertilizeBtn.onClick.AddListener(Fertilize);
        rankBtn.onClick.AddListener(OpenRanking);
        receiveBtn.onClick.AddListener(SignInReward);
        helpBtn.onClick.AddListener(OpenHelp);
        meteringBtn.onClick.AddListener(OpenMetering);
        levelCurr = 1;
        dailyNum = 500;
        isDaily = PlayerPrefs.GetString("DailySignIn") == "Daily" + System.DateTime.Now.Date;
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        fruitFarm.gameObject.SetActive(true);
        ValueAdjustment();
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void ValueAdjustment()
    {
        Quantity(manureSum);
        empiricalSum = levelCurr * 2;
        levelText.text = levelCurr+"级";
        infoText.text = "施肥升等级";
        if (meteringNum >= 2)
        {
            meteringText.text = "可领取肥料";
        }
        else
        {
            meteringText.text = string.Format("施肥{0}次可领", meteringNum);
        }
        dailyFertilizer.text = string.Format("肥料\n{0}", dailyNum);
        levelImage.fillAmount = empiricalCurr / empiricalSum;
        if (isDaily)
        {
            dailyText.text = "明日7:00可领";
        }
        else
        {
            dailyText.text = "领取";
        }
    }

    private void DailyReward()
    {
        if(!isDaily)
        {
            Quantity(5000);
            isDaily = true;
            dailyText.text = "明日7:00可领";
            PlayerPrefs.SetString("DailySignIn", "Daily" + System.DateTime.Now.Date);
        }
    }

    private void Fertilize()
    {
        if(numberManure >= 1)
        {
            empiricalCurr += 1;
            levelImage.fillAmount = empiricalCurr / empiricalSum;
            if (empiricalCurr >= empiricalSum)
            {
                levelCurr += 1;
                empiricalCurr = 0;
                empiricalSum = levelCurr * 2;
                levelText.text = string.Format("{0}级", levelCurr);
                infoText.text = "施肥升等级";
                levelImage.fillAmount = 0;
            }
            Quantity(-600);
            meteringNum += 1;
            float number = 2 - meteringNum;
            if(number > 0)
            {
                meteringText.text = string.Format("施肥{0}次可领", number);
            }
            else
            {
                meteringText.text = "领取肥料";
            }
            if(dailyNum < 1800)
            {
                dailyNum += 100;
                dailyFertilizer.text = string.Format("肥料\n{0}", dailyNum);
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
        if(meteringNum >= 2)
        {
            meteringNum = 0;
            Quantity(500);
            UIManager.Instance.CloningTips("领取肥料500");
            meteringText.text = "施肥2次可领";
        }
    }

    private void Quantity(float number)
    {
        manureSum = manureSum + number;
        numberManure = (float)Math.Floor((double)(manureSum / 600));
        manureText.text = string.Format("我的肥料 {0}",manureSum.ToString("F0")); 
        numberText.text = numberManure.ToString("F0");
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
