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

    private Button registerBtn;
    private Button clockinBtn;
    private Button fruitBtn;

    private Transform roleParent;
    private Transform petParent;
    private bool isShow;
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

        registerBtn = transform.Find("RegisterBtn").GetComponent<Button>();
        clockinBtn = transform.Find("ClockinBtn").GetComponent<Button>();
        fruitBtn = transform.Find("FruitBtn").GetComponent<Button>();

        registerBtn.onClick.AddListener(OpenRegister);
        clockinBtn.onClick.AddListener(OpenClockin);
        fruitBtn.onClick.AddListener(OpenFruit);
    }

    private void OpenRegister()
    {
        UIManager.Instance.guidePanel.OpenPanel();
    }

    private void OpenFruit()
    {
        UIManager.Instance.gameLoad.OpenPanel();
    }

    private void OpenClockin()
    {
        if (DataTool.isDegree)
        {
            UIManager.Instance.photographPanel.OpenPanel(true, "");
        }
        else
        {
            UIManager.Instance.guidePanel.OpenPanel();
        }
    }

    public void InitData()
    {
        if(DataTool.isDegree)
        {
            registerBtn.gameObject.SetActive(false);
            clockinBtn.gameObject.SetActive(true);
            if (DataTool.theCompany == "")
            {
                firmText.text = string.Format("{0}(自由职业者)", DataTool.roleName);
            }
            else
            {
                firmText.text = string.Format("{0}({1})", DataTool.roleName, DataTool.theCompany);
            }
        }
        else
        {
            registerBtn.gameObject.SetActive(true);
            clockinBtn.gameObject.SetActive(false);
            if (DataTool.information["willingVideoVerificationStatus"].ToString() == "2")
            {
                if (DataTool.loginPhone.Length >= 11)
                    firmText.text = string.Format("注册中:{0}****{1}", DataTool.loginPhone.Substring(0, 3), DataTool.loginPhone.Substring(7, 4));
            }
            else
            {
                if (DataTool.loginPhone.Length >= 11)
                    firmText.text = string.Format("未注册:{0}****{1}", DataTool.loginPhone.Substring(0, 3), DataTool.loginPhone.Substring(7, 4));
            }
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

    public void UpdateTask()
    {
        if (DataTool.taskDuration == "")
        {
            DataTool.taskDuration = "0";
        }
        statusText.text = string.Format("当月累计任务时长{0}小时", DataTool.taskDuration);
        statusImage.fillAmount = float.Parse(DataTool.taskDuration) / 300;
    }
   
    public void BeastObject()
    {
        int ran = 0;
        if(isShow)
        {
            isShow = false;
            for (int i = 0; i < beastObject.Length; i++)
            {
                beastObject[i].SetActive(false);
            }
        }
        else
        {
            isShow = true;
            ran = UnityEngine.Random.Range(0,beastObject.Length);
            beastObject[ran].SetActive(true);
        }
    }
}
