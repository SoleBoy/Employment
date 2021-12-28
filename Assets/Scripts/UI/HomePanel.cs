using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePanel : MonoBehaviour
{
    public Color pickColor;
    public Color norColor;

    public Sprite[] pickSprite;
    public Sprite[] norSprite;
    public Image[] clickImage;
    public GameObject[] clickObject;

    //private Text nameText;
    //private Text dutyText;

    private Image headImage;

    private Button homeBtn;
    private Button incomeBtn;
    private Button mainBtn;
    private Button taskBtn;

    private int indexCurret;
    public void Init()
    {
        //nameText = transform.Find("Head/NameText").GetComponent<Text>();
        //dutyText = transform.Find("Bottom/InfoText").GetComponent<Text>();

        headImage = transform.Find("Head/Sprite").GetComponent<Image>();

        homeBtn = transform.Find("Bottom/HomeBtn").GetComponent<Button>();
        incomeBtn = transform.Find("Bottom/IncomeBtn").GetComponent<Button>();
        mainBtn = transform.Find("Bottom/MainBtn").GetComponent<Button>();
        taskBtn = transform.Find("Bottom/TaskBtn").GetComponent<Button>();

        homeBtn.onClick.AddListener(OpenHome);
        incomeBtn.onClick.AddListener(OpenInconme);
        mainBtn.onClick.AddListener(OpenMain);
        taskBtn.onClick.AddListener(OpenTask);
    }

    public void InitData()
    {
        //nameText.text = DataTool.roleName;
        OpenPanel();
    }

    public void OpenPanel()
    {
        this.indexCurret = 0;
        for (int i = 0; i < norSprite.Length; i++)
        {
            if(indexCurret == i)
            {
                clickImage[indexCurret].sprite = pickSprite[indexCurret];
                //clickObject[indexCurret].SetActive(true);
            }
            else
            {
                clickImage[i].sprite = norSprite[i];
                //clickObject[i].SetActive(false);
            }
        }
        UIManager.Instance.hallPanel.OpenPanel();
    }

    private void OpenMain()
    {
        ClosePanel(2);
        UIManager.Instance.mainPanel.OpenPanel();
    }

    private void OpenInconme()
    {
        ClosePanel(1);
        UIManager.Instance.incomePanel.OpenPanel();
    }

    public void OpenHome()
    {
        ClosePanel(0);
        UIManager.Instance.hallPanel.OpenPanel();
    }

    public void OpenTask()
    {
        ClosePanel(3);
        UIManager.Instance.taskPanel.OpenPanel();
    }

    private void ClcikButton(int index)
    {
        clickImage[indexCurret].sprite = norSprite[indexCurret];
        //clickObject[indexCurret].SetActive(false);
        indexCurret = index;
        clickImage[indexCurret].sprite = pickSprite[indexCurret];
        //clickObject[indexCurret].SetActive(true);
    }

    private void ClosePanel(int index)
    {
        if (indexCurret == 0)
        {
            UIManager.Instance.hallPanel.ClosePanel();
        }
        else if (indexCurret == 1)
        {
            UIManager.Instance.incomePanel.ClosePanel();
        }
        else if (indexCurret == 2)
        {
            UIManager.Instance.mainPanel.ClosePanel();
        }
        else if (indexCurret == 3)
        {
            UIManager.Instance.taskPanel.ClosePanel();
        }
        ClcikButton(index);
    }
}
