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

    private Image headImage;

    private Button homeBtn;
    private Button incomeBtn;
    private Button mainBtn;
    private Button taskBtn;

    private int indexCurret;
    public void Init()
    {
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
            }
            else
            {
                clickImage[i].sprite = norSprite[i];
            }
        }
        UIManager.Instance.hallPanel.OpenPanel();
    }

    private void OpenMain()
    {
        if(indexCurret != 2)
            OpenGuide(2);
    }

    private void OpenInconme()
    {
        if (indexCurret != 1)
        {
            OpenGuide(1);
        }
    }

    public void OpenHome()
    {
        if (indexCurret != 0)
            ClosePanel(0);
    }

    private void OpenTask()
    {
        if (indexCurret != 3)
            ClosePanel(3);
    }

    private void ClcikButton(int index)
    {
        clickImage[indexCurret].sprite = norSprite[indexCurret];
        indexCurret = index;
        clickImage[indexCurret].sprite = pickSprite[indexCurret];
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
        if (index == 0)
        {
            UIManager.Instance.hallPanel.OpenPanel();
        }
        else if (index == 1)
        {
            UIManager.Instance.incomePanel.OpenPanel();
        }
        else if (index == 2)
        {
            UIManager.Instance.mainPanel.OpenPanel();
        }
        else if (index == 3)
        {
            UIManager.Instance.taskPanel.OpenPanel();
        }
    }

    private void OpenGuide(int index)
    {
        ClosePanel(index);
        //if (DataTool.isDegree)
        //{
        //    //UIManager.Instance.CloningTips("要有收益，赶紧注册哦");
           
        //}
        //else
        //{
        //    UIManager.Instance.guidePanel.OpenPanel();
        //}
    }
}
