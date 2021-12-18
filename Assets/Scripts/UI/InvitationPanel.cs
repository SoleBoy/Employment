using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvitationPanel : MonoBehaviour
{
    private Button backBtn;

    private InputField codeText;
    private Button codeBtn;

    private GameObject codePanel;
    private Button confirmBtn;
    private Text identityText;
    private Text nameText;


    private void Awake()
    {
        codePanel = transform.Find("CodePanel").gameObject;

        confirmBtn = transform.Find("CodePanel/Button").GetComponent<Button>();
        identityText = transform.Find("CodePanel/Info/info3/Text").GetComponent<Text>();
        nameText = transform.Find("CodePanel/Info/info3/Text").GetComponent<Text>();

        codeText = transform.Find("Info/InputField").GetComponent<InputField>();
        codeBtn = transform.Find("Info/Button").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        codeBtn.onClick.AddListener(OpenCode);
        confirmBtn.onClick.AddListener(ClosePanel);
    }

    private void OpenCode()
    {
        if(string.IsNullOrEmpty(codeText.text))
        {
            UIManager.Instance.CloningTips("请输入正确的邀请码");
        }
        else
        {
            Debug.Log(codeText.text);
            codePanel.SetActive(true);
        }
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        codeText.text = "";
        codePanel.SetActive(false);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
