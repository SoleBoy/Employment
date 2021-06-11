using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeastPanel : MonoBehaviour
{
    public Sprite[] spritesHead;
    public GameObject[] qualitys;

    private Text infoText;
    private Text nameText;
    private Text carryText;
    private Text expText;
    private Image barImage;
    private Button backBtn;
    private Button carryBtn;

    private Transform contParent;
    private Transform itemBeast;
    private GameObject headParent;

    private int itemIndex;
    private int carryIndex;

    private TalentData talentData;
    private List<BeastItem> beastItems = new List<BeastItem>();
    private List<int> carryItem = new List<int>();
    private void Awake()
    {
        headParent = transform.Find("HeadSprite").gameObject;

        infoText = transform.Find("Talent/InfoText").GetComponent<Text>();
        carryText = transform.Find("Bottom/CarryBtn/Text").GetComponent<Text>();
        nameText = transform.Find("HeadSprite/NameText").GetComponent<Text>();
        expText = transform.Find("HeadSprite/ExpText").GetComponent<Text>();
        barImage = transform.Find("HeadSprite/Bar").GetComponent<Image>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        carryBtn = transform.Find("Bottom/CarryBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        carryBtn.onClick.AddListener(CarryItem);

        contParent = transform.Find("Scroll View/Viewport/Content");
        itemBeast = transform.Find("Item");
        CreateItem(40);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    //选择
    public void SelectedItem(int index,string beastName,string quality, string itemId)
    {
        if(itemIndex >= 0)
        {
            beastItems[itemIndex].HideFrame();
            if (carryItem.Contains(index))
            {
                carryText.text = "卸掉";
            }
            else
            {
                carryText.text = "携带";
            }
        }
        itemIndex = index;
        headParent.SetActive(true);
        talentData = DataTool.talentDatas[itemId];
        nameText.text = string.Format("{0} LV.{1}", beastName, itemIndex);
        infoText.text = string.Format("{0}\n{1}%{2}", talentData.name, talentData.probability, talentData.info);
        int fabric = int.Parse(quality);
        for (int i = 0; i < qualitys.Length; i++)
        {
            if (i < fabric)
            {
                qualitys[i].SetActive(true);
            }
            else
            {
                qualitys[i].SetActive(false);
            }
        }
    }
    //取消选择
    public void CancelSelected()
    {
        if (itemIndex >= 0)
        {
            itemIndex = -1;
            headParent.SetActive(false);
            infoText.text = "请选择携带宠物";
            for (int i = 0; i < qualitys.Length; i++)
            {
                qualitys[i].SetActive(false);
            }
        }
    }
    //生成
    private void CreateItem(int index)
    {
        itemIndex = -1;
        beastItems.Clear();
        headParent.SetActive(false);
        infoText.text = "请选择携带宠物";
        for (int i = 0; i < index; i++)
        {
            var item = Instantiate(itemBeast);
            item.gameObject.SetActive(true);
            item.SetParent(contParent);
            item.localScale = Vector3.one;
            BeastItem beast = new BeastItem(item,i,spritesHead[i%5]);
            beastItems.Add(beast);
        }
        float maxY = index * 0.2f * 200 + index * 0.2f * 50 + 10;
        contParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0,maxY);
        for (int i = 0; i < qualitys.Length; i++)
        {
            qualitys[i].SetActive(false);
        }
    }
    //携带物品
    private void CarryItem()
    {
        if(itemIndex >= 0)
        {
            if (carryItem.Contains(itemIndex))
            {
                carryText.text = "携带";
                carryItem.Remove(itemIndex);
                carryIndex -= 1;
                beastItems[itemIndex].CarryBeast(false,carryIndex);
            }
            else
            {
                if (carryItem.Count >= 3)
                {
                    UIManager.Instance.CloningTips("携带宠物已达到上限");
                }
                else
                {
                    carryText.text = "卸掉";
                    carryItem.Add(itemIndex);
                    beastItems[itemIndex].CarryBeast(true,carryIndex);
                    carryIndex += 1;
                }
            }
        }
        else
        {
            UIManager.Instance.CloningTips("请选择宠物");
        }
    }
    //宠物信息
    private void PetInfo(string name,string talent,int grade,int fabric)
    {
        nameText.text = string.Format("{0} LV.{1}", name,grade);
        infoText.text = string.Format("{0}\n{1}%{2}",talentData.name,talentData.probability,talentData.info);
        for (int i = 0; i < qualitys.Length; i++)
        {
            if(i < fabric)
            {
                qualitys[i].SetActive(true);
            }
            else
            {
                qualitys[i].SetActive(false);
            }
        }
    }
}

public class BeastItem
{
    private Text gradeText;
    private Image headImage;
    private Button fixBtn;
    private Sprite headSprite;
    private RoleData beastData;
    private GameObject carryObject;
    private GameObject frameObject;
    private int orderIndex;
    private int carryIndex;
    private bool isPick;
    public BeastItem(Transform parent, int index,Sprite head)
    {
        orderIndex = index;
        headSprite = head;
        string itemId = string.Format("200{0}", UnityEngine.Random.Range(1, 7));
        beastData = DataTool.roleDatas[itemId];
        carryObject = parent.Find("carry").gameObject;
        frameObject = parent.Find("Frame").gameObject;
        fixBtn = parent.GetComponent<Button>();
        headImage = parent.Find("Head").GetComponent<Image>();
        gradeText = parent.Find("GradeText").GetComponent<Text>();
        fixBtn.onClick.AddListener(Selected);
        gradeText.text = string.Format("LV.{0}",index);
        headImage.sprite = head;
        frameObject.SetActive(false);
        carryObject.SetActive(false);
    }

    private void Selected()
    {
        if(isPick)
        {
            isPick = false;
            frameObject.SetActive(false);
            UIManager.Instance.beastPanel.CancelSelected();
        }
        else
        {
            isPick = true;
            frameObject.SetActive(true);
            UIManager.Instance.beastPanel.SelectedItem(orderIndex,beastData.name,beastData.quality, beastData.talentId);
        }
    }
    public void HideFrame()
    {
        isPick = false;
        frameObject.SetActive(false);
    }
    public void CarryBeast(bool isCarry,int index)
    {
        carryIndex = index;
        carryObject.SetActive(isCarry);
        UIManager.Instance.hallPanel.BringPets(isCarry, carryIndex);
    }
}
