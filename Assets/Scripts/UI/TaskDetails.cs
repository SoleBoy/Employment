using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskDetails : MonoBehaviour
{
    private Text taskText;
    private Text priceText;
    private Text timeText;
    private Text addressText;
    private Button confirmBtn;

    private void Awake()
    {
        taskText = transform.Find("name").GetComponent<Text>();
        priceText = transform.Find("price").GetComponent<Text>();
        timeText = transform.Find("time").GetComponent<Text>();
        addressText = transform.Find("address").GetComponent<Text>();
        confirmBtn = transform.Find("ConfirmBtn").GetComponent<Button>();

        confirmBtn.onClick.AddListener(ConfirmPanel);
    }

    public void SetInfo(string name)
    {
        taskText.text = name;
    }

    private void ConfirmPanel()
    {
        UIManager.Instance.taskConfirmPanel.OpenPanel();
    }
}
