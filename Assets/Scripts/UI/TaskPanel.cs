using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : MonoBehaviour
{
    private Button filterBtn;
    private Button recomBtn;
    private Button assignBtn;
    private Button mineBtn;
    private Button confirmBtn;
    private Button areaBtn;
    private Button typeBtn;

    private Text araeNameText;
    private Text typeNameText;

    public GameObject[] clickHide;
    public GameObject taskItem;
    public GameObject araeItem;

    private GameObject filterPanel;
    private GameObject areaPanel;
    private GameObject typePanel;
    private GameObject filterObject;

    private Transform itemParent;
    private Transform mineParent;
    private Transform areaParent;
    private Transform typeParent;

    private bool isArea;
    private bool isType;
    private int index_click;
    private string[] messgName = { "场站保洁服务","驾校代驾服务","快递服务","某某公司保洁服务","家政保洁服务"};
    private string[] araeName = { "上海","江苏","河南","河北","广东","厦门","深圳", "郑州" };
    private string[] typeName = { "保洁","保安","代驾", "腾讯搜活帮", "百度众测", "京东微工", "有道众包" };
    private void Awake()
    {
        filterPanel = transform.Find("FilterPanel").gameObject;
        areaPanel = transform.Find("FilterPanel/Area").gameObject;
        typePanel = transform.Find("FilterPanel/Type").gameObject;
        filterObject=transform.Find("FilterBtn").gameObject;

        itemParent = transform.Find("TaskType/Viewport/Content");
        mineParent = transform.Find("MineTask/Viewport/Content");
        areaParent = transform.Find("FilterPanel/Area/Viewport/Content");
        typeParent = transform.Find("FilterPanel/Type/Viewport/Content");

        araeNameText = transform.Find("FilterPanel/AreaBtn/Text").GetComponent<Text>();
        typeNameText = transform.Find("FilterPanel/TypeBtn/Text").GetComponent<Text>();

        filterBtn = transform.Find("FilterBtn").GetComponent<Button>();
        recomBtn = transform.Find("Header/RecomBtn").GetComponent<Button>();
        assignBtn = transform.Find("Header/AssignBtn").GetComponent<Button>();
        mineBtn = transform.Find("Header/MineBtn").GetComponent<Button>();

        confirmBtn = transform.Find("FilterPanel/Button").GetComponent<Button>();
        areaBtn = transform.Find("FilterPanel/AreaBtn").GetComponent<Button>();
        typeBtn = transform.Find("FilterPanel/TypeBtn").GetComponent<Button>();

        filterBtn.onClick.AddListener(OpenFilter);
        recomBtn.onClick.AddListener(OpenRecommend);
        assignBtn.onClick.AddListener(OpenAssign);
        mineBtn.onClick.AddListener(OpenMine);

        confirmBtn.onClick.AddListener(ConfirmArea);
        areaBtn.onClick.AddListener(RegionSelection);
        typeBtn.onClick.AddListener(TypeSelection);

        for (int i = 0; i < 5; i++)
        {
            var item = Instantiate(taskItem);
            item.SetActive(true);
            item.transform.SetParent(itemParent);
            item.transform.localScale = Vector3.one;
            item.GetComponent<TaskDetails>().SetInfo(messgName[i]);
        }
        float maxY = 5 * 150 + 5 * 100;
        itemParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);

        for (int i = 0; i < typeName.Length; i++)
        {
            var item = Instantiate(araeItem);
            item.SetActive(true);
            item.transform.SetParent(typeParent);
            item.transform.localScale = Vector3.one;
            AraeClass arae = new AraeClass(item.transform);
            arae.SetName(typeName[i]);
        }
        float typeY = araeName.Length * 100;
        typeParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, typeY);

        for (int i = 0; i < araeName.Length; i++)
        {
            var item = Instantiate(araeItem);
            item.SetActive(true);
            item.transform.SetParent(areaParent);
            item.transform.localScale = Vector3.one;
            AraeClass arae = new AraeClass(item.transform);
            arae.SetName(araeName[i]);
        }
        float araeY = araeName.Length * 100;
        areaParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, araeY);
    }

    private void SetTaskType()
    {
        if(index_click == 0)
        {
            filterObject.SetActive(true);
            mineParent.parent.parent.gameObject.SetActive(false);
            itemParent.parent.parent.gameObject.SetActive(true);
        }
        else if(index_click == 1)
        {
            filterObject.SetActive(false);
            mineParent.parent.parent.gameObject.SetActive(false);
            itemParent.parent.parent.gameObject.SetActive(true);
        }
        else if(index_click == 2)
        {
            filterObject.SetActive(false);
            mineParent.parent.parent.gameObject.SetActive(true);
            itemParent.parent.parent.gameObject.SetActive(false);
        }
    }

    private void OpenRecommend()
    {
        OpenSubscript(0);
    }

    private void OpenAssign()
    {
        OpenSubscript(1);
    }

    private void OpenMine()
    {
        OpenSubscript(2);
    }

    private void OpenFilter()
    {
        if(filterPanel.activeSelf)
        {
            filterPanel.SetActive(false);
        }
        else
        {
            isArea = true;
            isType = true;
            araeNameText.text = "地区";
            typeNameText.text = "类型";
            filterPanel.SetActive(true);
            areaPanel.SetActive(false);
            typePanel.SetActive(false);
        }
    }

    private void ConfirmArea()
    {
        filterPanel.SetActive(false);
    }

    private void RegionSelection()
    {
        if(isArea)
        {
            isArea = false;
            areaPanel.SetActive(true);
        }
        else
        {
            isArea = true;
            areaPanel.SetActive(false);
        }
    }

    private void TypeSelection()
    {
        if (isType)
        {
            isType = false;
            typePanel.SetActive(true);
        }
        else
        {
            isType = true;
            typePanel.SetActive(false);
        }
    }

    private void OpenSubscript(int index)
    {
        clickHide[index_click].gameObject.SetActive(false);
        index_click = index;
        clickHide[index_click].gameObject.SetActive(true);
        SetTaskType();
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        filterPanel.SetActive(false);
        filterObject.SetActive(true);
        index_click = 0;
        SetTaskType();
        for (int i = 0; i < clickHide.Length; i++)
        {
            if(i == index_click)
            {
                clickHide[i].gameObject.SetActive(true);
            }
            else
            {
                clickHide[i].gameObject.SetActive(false);
            }
        }
    }


    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void RegionSelection(string messgName)
    {
        if(!isArea)
        {
            isArea = true;
            areaPanel.SetActive(false);
            araeNameText.text = messgName;
        }
        else if (!isType)
        {
            isType = true;
            typePanel.SetActive(false);
            typeNameText.text = messgName;
        }
       
    }

    class AraeClass
    {
        private Text nameText;
        private Button areaBtn;
        
        public AraeClass(Transform parent)
        {
            areaBtn = parent.GetComponent<Button>();
            nameText = parent.GetComponent<Text>();
            areaBtn.onClick.AddListener(ClickEvent);
        }

        private void ClickEvent()
        {
            UIManager.Instance.taskPanel.RegionSelection(nameText.text);
        }

        public void SetName(string messgName)
        {
            nameText.text = messgName;
        }

    }
}
