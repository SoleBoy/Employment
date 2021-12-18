using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskConfirmPanel : MonoBehaviour
{
    private Button fixBtn;
    private Button backBtn;
    private Button cloceBtn;

    private GameObject successPanel;
    private void Awake()
    {
        successPanel = transform.Find("success").gameObject;

        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        fixBtn = transform.Find("Button").GetComponent<Button>();
        cloceBtn = transform.Find("success/Button").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        fixBtn.onClick.AddListener(OpenSuccess);
        cloceBtn.onClick.AddListener(ClosePanel);
    }

    private void OpenSuccess()
    {
        successPanel.SetActive(true);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        successPanel.SetActive(false);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
