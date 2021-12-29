using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Networking;
using MiniJSON;

public class HallPanel : MonoBehaviour
{
    public GameObject[] beastObject;
    private Text firmText;
    private Text addText;
    private Text statusText;
    private Text clockText;

    private Image statusImage;
    private Image roleImage;
    private GameObject checkRecord;

    private Button gachaBtn;
    private Button battleBtn;
    private Button beastBtn;
    private Button packBtn;
    private Button certiBtn;
    private Button infoBtn;
    private Button clockinBtn;

    private Transform roleParent;
    private Transform petParent;
    public void Init()
    {
        checkRecord = transform.Find("Head/Boom").gameObject;
        roleParent = transform.Find("Bracket/RoleImage");
        petParent = transform.Find("PetParent");

        firmText = transform.Find("Head/Text").GetComponent<Text>();
        addText = transform.Find("Head/Boom/Address/Text").GetComponent<Text>();
        statusText = transform.Find("Head/Status/Text").GetComponent<Text>();
        clockText = transform.Find("Head/Boom/Clock/Text").GetComponent<Text>();

        statusImage = transform.Find("Head/Status/Image").GetComponent<Image>();
        roleImage = transform.Find("Bracket/RoleImage").GetComponent<Image>();

        gachaBtn = transform.Find("GachaBtn").GetComponent<Button>();
        battleBtn = transform.Find("BattleBtn").GetComponent<Button>();
        beastBtn = transform.Find("BeastBtn").GetComponent<Button>();
        packBtn = transform.Find("PackBtn").GetComponent<Button>();
        certiBtn = transform.Find("CertiBtn").GetComponent<Button>();
        infoBtn = transform.Find("GradeBar/InfoBtn").GetComponent<Button>();
        clockinBtn= transform.Find("ClockinBtn").GetComponent<Button>();

        gachaBtn.onClick.AddListener(OpneGacha);
        battleBtn.onClick.AddListener(OpneBattle);
        beastBtn.onClick.AddListener(OpneBeast);
        packBtn.onClick.AddListener(OpnePack);
        certiBtn.onClick.AddListener(OpenCerti);
        infoBtn.onClick.AddListener(OpenDetails);
        clockinBtn.onClick.AddListener(OpenClockin);
    }

    private void OpenClockin()
    {
        UIManager.Instance.photographPanel.OpenPanel(true,"");
    }

    public void InitData()
    {
        statusText.text = string.Format("当月累计任务时长{0}小时",DataTool.taskDuration);
        statusImage.fillAmount = float.Parse(DataTool.taskDuration) / 300;
        if (DataTool.theCompany == "")
        {
            firmText.text = string.Format("{0}(自由职业者)", DataTool.roleName);
        }
        else
        {
            firmText.text = string.Format("{0}({1})", DataTool.roleName, DataTool.theCompany);
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
    //打卡记录
    public void CheckRecord(bool isCheck)
    {
        if(isCheck)
        {
            clockText.text = PlayerPrefs.GetString("ClockInTime");
            addText.text = PlayerPrefs.GetString("ClockInAddress");
            checkRecord.transform.DOScaleY(1,1);
            checkRecord.SetActive(true);
        }
        else
        {
            checkRecord.SetActive(false);
        }
    }

    //宠物携带
    public void BringPets(bool isHide,int index,Sprite pet=null)
    {
        petParent.GetChild(index).gameObject.SetActive(isHide);
    }

    private void OpnePack()
    {
        //UIManager.Instance.backpackPanel.OpenPanel();
    }
    private bool isBeast;
    private void OpneBeast()
    {
        if(isBeast)
        {
            isBeast = false;
            for (int i = 0; i < beastObject.Length; i++)
            {
                beastObject[i].SetActive(false);
            }
        }
        else
        {
            isBeast = true;
            beastObject[UnityEngine.Random.Range(0,beastObject.Length)].SetActive(true);
        }
        //UIManager.Instance.beastPanel.OpenPanel();
    }

    private void OpneBattle()
    {
        UIManager.Instance.CloningTips("功能暂未开启");
        //UIManager.Instance.battlePanel.OpenPanel();
    }

    private void OpneGacha()
    {
        ///UIManager.Instance.gachaPanel.OpenPanel();
    }

    private void OpenCerti()
    {
        //UIManager.Instance.unitPanel.OpenPanel();
    }

    private void OpenDetails()
    {
        //UIManager.Instance.detailsPanel.OpenPanel();
    }
    public void UpdateTask()
    {
        statusText.text = string.Format("当月累计任务时长{0}小时", DataTool.taskDuration);
        statusImage.fillAmount = float.Parse(DataTool.taskDuration) / 300;
    }
   
}
