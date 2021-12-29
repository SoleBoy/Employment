using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackPanel : MonoBehaviour
{
    public Sprite[] spritesHead;

    private Text infoText;
    private Text propsText;

    private Button fixBtn;
    private Button cloceBtn;
    private Button backBtn;

    private GameObject itemInfo;
    private Transform contParent;
    private Transform itemBeast;

    private int itemIndex;

    private List<BackItem> beastItems = new List<BackItem>();
    private void Awake()
    {
        contParent = transform.Find("Scroll View/Viewport/Content");
        itemBeast = transform.Find("Item");
        itemInfo = transform.Find("ItemInfo").gameObject;
        infoText = transform.Find("ItemInfo/Bg/InfoText").GetComponent<Text>();
        propsText = transform.Find("ItemInfo/Bg/NameText").GetComponent<Text>();

        fixBtn = transform.Find("ItemInfo/Bg/FixBtn").GetComponent<Button>();
        cloceBtn = transform.Find("ItemInfo/Bg/Close").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        cloceBtn.onClick.AddListener(CloseInfo);
        fixBtn.onClick.AddListener(UseInfo);
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
    private void UseInfo()
    {
        if (itemIndex >= 0)
        {
            beastItems[itemIndex].CancelSelection();
        }
    }
    private void CloseInfo()
    {
        itemInfo.SetActive(false);
    }
    //选中背包
    public void SelectedItem(int index)
    {
        itemIndex = index;
        propsText.text = "妙蛙种子";
        infoText.text = string.Format("{0}，满300使用可以合成{1}", "妙蛙种子","魔兽蛙");
        itemInfo.SetActive(true);
    }
    //生成
    private void CreateItem(int index)
    {
        itemIndex = -1;
        beastItems.Clear();
        float maxY = index * 0.2f * 200 + index * 0.2f * 50 + 20;
        for (int i = 0; i < index; i++)
        {
            var item = Instantiate(itemBeast);
            item.gameObject.SetActive(true);
            item.SetParent(contParent);
            item.localScale = Vector3.one;
            item.localEulerAngles = Vector3.zero;
            BackItem beast = new BackItem(item, i, spritesHead[i%5]);
            beastItems.Add(beast);
        }
        contParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
    }
}

public class BackItem
{
    private Text idText;
    private Text nameText;
    private Image headImage;
    private Button fixBtn;
    private int orderIndex;

    public BackItem(Transform parent, int index,Sprite head)
    {
        orderIndex = index;
        fixBtn = parent.GetComponent<Button>();
        headImage = parent.Find("Image").GetComponent<Image>();
        idText = parent.Find("IdText").GetComponent<Text>();
        nameText = parent.Find("NameText").GetComponent<Text>();
        fixBtn.onClick.AddListener(Selected);
        idText.text = index.ToString();
        nameText.text = "X兽碎片";
        headImage.sprite = head;
    }

    private void Selected()
    {
        Debug.Log(orderIndex);
        //UIManager.Instance.backpackPanel.SelectedItem(orderIndex);
    }

    public void CancelSelection()
    {
        //UIManager.Instance.CloningTips("碎片数量不足");
    }
}
