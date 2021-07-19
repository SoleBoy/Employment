using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    private Button licenseBtn;
    private Button certiBtn;
    private Button serverBtn;
    private Button policyBtn;
    private Button dropBtn;

    private void Awake()
    {
        licenseBtn = transform.Find("Btn/LicenseBtn").GetComponent<Button>();
        certiBtn = transform.Find("Btn/CertiBtn").GetComponent<Button>();
        serverBtn = transform.Find("Btn/ServerBtn").GetComponent<Button>();
        policyBtn = transform.Find("Btn/PolicyBtn").GetComponent<Button>();
        dropBtn = transform.Find("Btn/DropBtn").GetComponent<Button>();

        licenseBtn.onClick.AddListener(OpenLicense);
        certiBtn.onClick.AddListener(OpenCerti);
        serverBtn.onClick.AddListener(OpenServer);
        policyBtn.onClick.AddListener(OpenPolicy);
        dropBtn.onClick.AddListener(OpenDrop);

        licenseBtn.gameObject.SetActive(DataTool.isUnit);
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    //退出
    private void OpenDrop()
    {
        UIManager.Instance.dropPanel.OpenPanel("是否退出登录？");
    }
    //隐私
    private void OpenPolicy()
    {
        UIManager.Instance.privacyPanel.OpenPanel();
    }
    //服务
    private void OpenServer()
    {
        UIManager.Instance.termsPanel.OpenPanel();
    }
    //认证信息
    private void OpenCerti()
    {
        UIManager.Instance.unitPanel.OpenPanel();
    }
    //营业执照
    private void OpenLicense()
    {
        DataTool.salaryEntry = SalaryEntry.business_1;
        if (Application.platform == RuntimePlatform.Android)
        {
            DataTool.CallNative(195, 0);
        }
        else
        {
            UIManager.Instance.Acceptance_Android("Monthly7");
        }
        //UIManager.Instance.businessPanel.OpenPanel();
    }
}
