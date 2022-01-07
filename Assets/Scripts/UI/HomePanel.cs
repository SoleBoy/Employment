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
        {
            ClosePanel(2);
            UIManager.Instance.mainPanel.OpenPanel();
        }
    }

    private void OpenInconme()
    {
        if (indexCurret != 1)
        {
            ClosePanel(1);
            UIManager.Instance.incomePanel.OpenPanel();
        }
    }

    public void OpenHome()
    {
        ClosePanel(0);
        UIManager.Instance.hallPanel.OpenPanel();
    }

    private void OpenTask()
    {
        if (indexCurret != 3)
        {
            ClosePanel(3);
            UIManager.Instance.taskPanel.OpenPanel();
        }
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
    }
}
