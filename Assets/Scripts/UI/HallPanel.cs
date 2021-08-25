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
    private Text firmText;
    private Text addText;
    private Text statusText;
    //private Text certeiText;
    //private Text gradeText;
    //private Text expText;
    //private Text titleText;
    private Text clockText;

    private Image roleImage;
    //private Image expImage;
    private GameObject checkRecord;

    private Button gachaBtn;
    private Button battleBtn;
    private Button beastBtn;
    private Button packBtn;
    private Button certiBtn;
    private Button infoBtn;

    private Transform roleParent;
    private Transform petParent;
    //private GameObject clockIn;
    public void Init()
    {
        checkRecord = transform.Find("Head/Boom").gameObject;
        //clockIn = transform.Find("Head/Clock").gameObject;
        roleParent = transform.Find("Bracket/RoleImage");
        petParent = transform.Find("PetParent");

        firmText = transform.Find("Head/Text").GetComponent<Text>();
        addText = transform.Find("Head/Boom/Address/Text").GetComponent<Text>();
        statusText = transform.Find("Status/Text").GetComponent<Text>();
        //certeiText = transform.Find("CertiBtn/Text").GetComponent<Text>();
        //gradeText = transform.Find("GradeBar/GradeText").GetComponent<Text>();
        //expText = transform.Find("GradeBar/ExpText").GetComponent<Text>();
        //titleText = transform.Find("GradeBar/NameText").GetComponent<Text>();
        clockText = transform.Find("Head/Boom/Clock/Text").GetComponent<Text>();

        roleImage = transform.Find("Bracket/RoleImage").GetComponent<Image>();
        //expImage = transform.Find("GradeBar/Bar").GetComponent<Image>();

        gachaBtn = transform.Find("GachaBtn").GetComponent<Button>();
        battleBtn = transform.Find("BattleBtn").GetComponent<Button>();
        beastBtn = transform.Find("BeastBtn").GetComponent<Button>();
        packBtn = transform.Find("PackBtn").GetComponent<Button>();
        certiBtn = transform.Find("CertiBtn").GetComponent<Button>();
        infoBtn = transform.Find("GradeBar/InfoBtn").GetComponent<Button>();

        gachaBtn.onClick.AddListener(OpneGacha);
        battleBtn.onClick.AddListener(OpneBattle);
        beastBtn.onClick.AddListener(OpneBeast);
        packBtn.onClick.AddListener(OpnePack);
        certiBtn.onClick.AddListener(OpenCerti);
        infoBtn.onClick.AddListener(OpenDetails);
       
        //InitData();
    }

    public void InitData()
    {
        statusText.text = "休息中......";
        firmText.text = DataTool.theCompany;
        //certeiText.text = "点击认证";
        //titleText.text = DataTool.roleTitle;
        //gradeText.text = string.Format("LV.{0}", DataTool.roleLevel);
        //expText.text = string.Format("{0}/{1}", DataTool.roleExp, DataTool.roleExp_Max);
        //expImage.fillAmount = DataTool.roleExp / DataTool.roleExp_Max;
        //StartCoroutine(RequestAddress());
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
    //增加经验
    public void AddExperience(float exp)
    {
        //if(DataTool.AddExperience(exp))
        //{
        //    gradeText.text = string.Format("LV.{0}", DataTool.roleLevel);
        //}
        //expText.text = string.Format("{0}/{1}", DataTool.roleExp, DataTool.roleExp_Max);
        //expImage.fillAmount = DataTool.roleExp / DataTool.roleExp_Max;
    }
    private void OpnePack()
    {
        UIManager.Instance.backpackPanel.OpenPanel();
    }

    private void OpneBeast()
    {
        UIManager.Instance.beastPanel.OpenPanel();
    }

    private void OpneBattle()
    {
        UIManager.Instance.CloningTips("功能暂未开启");
        //UIManager.Instance.battlePanel.OpenPanel();
    }

    private void OpneGacha()
    {
        UIManager.Instance.gachaPanel.OpenPanel();
    }

    private void OpenCerti()
    {
        UIManager.Instance.unitPanel.OpenPanel();
    }

    private void OpenDetails()
    {
        UIManager.Instance.detailsPanel.OpenPanel();
    }
}
