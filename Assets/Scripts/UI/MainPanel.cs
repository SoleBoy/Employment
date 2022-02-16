using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    private Text firmText;
    private Text nameText;

    private Button licenseBtn;
    private Button certiBtn;
    private Button serverBtn;
    private Button policyBtn;
    private Button dropBtn;
    private Button serviceBtn;
    private Button agreementBtn;

    private Transform btnParent;
    private void Awake()
    {
        btnParent = transform.Find("Btn1/Viewport/Content");
        licenseBtn = btnParent.Find("LicenseBtn").GetComponent<Button>();
        certiBtn = btnParent.Find("CertiBtn").GetComponent<Button>();
        serverBtn = btnParent.Find("ServerBtn").GetComponent<Button>();
        policyBtn = btnParent.Find("PolicyBtn").GetComponent<Button>();
        dropBtn = btnParent.Find("DropBtn").GetComponent<Button>();
        serviceBtn = btnParent.Find("ServiceBtn").GetComponent<Button>();
        agreementBtn = btnParent.Find("Agreement").GetComponent<Button>();

        firmText = transform.Find("TopBg/FirmText").GetComponent<Text>();
        nameText = transform.Find("TopBg/NameText").GetComponent<Text>();

        licenseBtn.onClick.AddListener(OpenLicense);
        certiBtn.onClick.AddListener(OpenCerti);
        serverBtn.onClick.AddListener(OpenServer);
        policyBtn.onClick.AddListener(OpenPolicy);
        dropBtn.onClick.AddListener(OpenDrop);
        serviceBtn.onClick.AddListener(OpenService);
        agreementBtn.onClick.AddListener(OpenAgreement);

        licenseBtn.gameObject.SetActive(false);
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
        SetCompany();
        //nameText.text = DataTool.roleName;
        //firmText.text = DataTool.theCompany;
    }

    public void SetCompany()
    {
        if (DataTool.isDegree)
        {
            if (DataTool.theCompany == "")
            {
                nameText.text = string.Format("{0}(自由职业者)", DataTool.roleName);
            }
            else
            {
                nameText.text = string.Format("{0}({1})", DataTool.roleName, DataTool.theCompany);
            }
        }
        else
        {
            if (DataTool.information["willingVideoVerificationStatus"].ToString() == "2")
            {
                if (DataTool.loginPhone.Length >= 11)
                    nameText.text = string.Format("注册中:{0}****{1}", DataTool.loginPhone.Substring(0, 3), DataTool.loginPhone.Substring(7, 4));
            }
            else
            {
                if (DataTool.loginPhone.Length >= 11)
                    nameText.text = string.Format("未注册:{0}****{1}", DataTool.loginPhone.Substring(0, 3), DataTool.loginPhone.Substring(7, 4));
            }
        }

    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    //客服
    private void OpenService()
    {
        if (DataTool.isDegree)
        {
            UIManager.Instance.servicePanel.OpenPanel();
        }
        else
        {
            UIManager.Instance.guidePanel.OpenPanel();
        }
    }
    //退出
    private void OpenDrop()
    {
        UIManager.Instance.dropPanel.OpenPanel("是否退出登录？");
    }
    //隐私
    private void OpenPolicy()
    {
        if (DataTool.isDegree)
        {
            UIManager.Instance.privacyPanel.OpenPanel();
        }
        else
        {
            UIManager.Instance.guidePanel.OpenPanel();
        }
    }
    //服务
    private void OpenServer()
    {
        if (DataTool.isDegree)
        {
            UIManager.Instance.termsPanel.OpenPanel();
        }
        else
        {
            UIManager.Instance.guidePanel.OpenPanel();
        }
    }
    //认证信息
    private void OpenCerti()
    {
        if (DataTool.isDegree)
        {
            UIManager.Instance.unitPanel.OpenPanel();
        }
        else
        {
            UIManager.Instance.guidePanel.OpenPanel();
        }
    }
    //我的协议
    private void OpenAgreement()
    {
        if (DataTool.isDegree)
        {
            UIManager.Instance.protocolPanel.OpenPanel();
        }
        else
        {
            UIManager.Instance.guidePanel.OpenPanel();
        }
    }
    //营业执照
    private void OpenLicense()
    {
        //DataTool.salaryEntry = SalaryEntry.business_1;
        //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    DataTool.CallNative(195, 0);
        //}
        //else
        //{
        //    UIManager.Instance.Acceptance_Android("Monthly7");
        //}
        //UIManager.Instance.businessPanel.OpenPanel();
    }
}
