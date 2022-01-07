using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TaskStatus
{
    start,
    submit,
}

public class TaskDetails : MonoBehaviour
{
    private Text taskText;
    private Text priceText;
    private Text timeText;
    private Text addressText;
    private Text stateText;
    private Text submitText;
    private Button confirmBtn;
    private Button submitBtn;

    private GameObject line_1;
    private GameObject line_2;

    private TaskStatus taskStatus;
    private string taskType;
    Dictionary<string, object> infoTask = new Dictionary<string, object>();
    private void Awake()
    {
        taskText = transform.Find("name").GetComponent<Text>();
        priceText = transform.Find("price").GetComponent<Text>();
        timeText = transform.Find("time").GetComponent<Text>();
        addressText = transform.Find("address").GetComponent<Text>();
        stateText = transform.Find("stateText").GetComponent<Text>();
        submitText = transform.Find("SubmitBtn/Text").GetComponent<Text>();
        confirmBtn = transform.Find("ConfirmBtn").GetComponent<Button>();
        submitBtn = transform.Find("SubmitBtn").GetComponent<Button>();

        confirmBtn.onClick.AddListener(ConfirmPanel);
        submitBtn.onClick.AddListener(SubmitPanel);
        line_1 = transform.Find("line1").gameObject;
        line_2 = transform.Find("line2").gameObject;
    }

    private void SubmitPanel()
    {
        UIManager.Instance.taskSubmitPanel.OpenPanel(taskStatus , infoTask["id"].ToString(), taskType);
    }

    public void SetInfo(bool isMe,Dictionary<string,object> pairs,bool isend)
    {
        infoTask = pairs;
        line_2.SetActive(isend);
        if (infoTask["taskName"] != null)
        {
            taskText.text = infoTask["taskName"].ToString();
        }
        else
        {
            taskText.text = "";
        }
        if (infoTask["taskPlanStartDate"] != null && infoTask["taskPlanEndDate"] != null && infoTask["timeHHmm"] != null)
        {
            timeText.text = string.Format("{0}一{1}\n{2}", infoTask["taskPlanStartDate"].ToString(), infoTask["taskPlanEndDate"].ToString(), infoTask["timeHHmm"].ToString());
        }
        else
        {
            timeText.text = "";
        }
        if (infoTask["taskAddress"] != null)
        {
            addressText.text = infoTask["taskAddress"].ToString();
        }
        else
        {
            addressText.text = "";
        }
       
        submitBtn.gameObject.SetActive(false);
        if (isMe)
        {
            priceText.gameObject.SetActive(false);
            confirmBtn.gameObject.SetActive(false);
            stateText.gameObject.SetActive(true);
            if(infoTask["taskStatus"] != null)
            {
                stateText.color = GetColor(infoTask["taskStatus"].ToString());
            }
            else
            {
                stateText.color = GetColor("10");
            }
        }
        else
        {
            priceText.gameObject.SetActive(true);
            confirmBtn.gameObject.SetActive(true);
            stateText.gameObject.SetActive(false);
            if (infoTask["unitAmount"] == null && infoTask["unitAmount"].ToString() == "")
            {
                priceText.text = "未知";//string.Format("{0:N2}元/天", 1);
            }
            else
            {
                priceText.text = string.Format("{0:N2}元/{1}", float.Parse(infoTask["unitAmount"].ToString()), infoTask["billingUnit"].ToString());
            }
        }
    }

    private Color GetColor(string messg)
    {
        if(messg == "0")
        {
            stateText.text = "待确认";
            return DataTool.color_review;
        }
        else if (messg == "1")
        {
            stateText.text = "待接单";
            return DataTool.color_start;
        }
        else if (messg == "2")
        {
            taskType = "2";
            stateText.text = "已接单";
            taskStatus = TaskStatus.start;
            submitText.text = "开始";
            submitBtn.gameObject.SetActive(true);
            return DataTool.color_submitted;
        }
        else if (messg == "3")
        {
            stateText.text = "待录用";
            return DataTool.color_accepted;
        }
        else if (messg == "4")
        {
            taskType = "4";
            taskStatus = TaskStatus.start;
            stateText.text = "待开始";
            submitText.text = "开始";
            submitBtn.gameObject.SetActive(true);
            return DataTool.color_issued;
        }
        else if (messg == "5")
        {
            taskType = "5";
            taskStatus = TaskStatus.submit;
            stateText.text = "进行中";
            submitText.text = "提交";
            submitBtn.gameObject.SetActive(true);
            return DataTool.color_progress;
        }
        else if (messg == "6")
        {
            stateText.text = "待确认成果";
            return DataTool.color_review;
        }
        else if (messg == "7")
        {
            stateText.text = "待结算";
            return DataTool.color_start;
        }
        else if (messg == "8")
        {
            stateText.text = "已结算";
            return DataTool.color_submitted;
        }
        else if (messg == "9")
        {
            stateText.text = "雇主取消加入";
            return DataTool.color_issued;
        }
        else
        {
            stateText.text = "状态异常";
            return Color.red;
        }
    }
//    状态:0：待确认，任务发布个人抢单后
//1：待接单（雇主后台邀请生成），
//2：已接单，雇主邀请确认后
//4：待开始，录用后任务尚未开始
//5：进行中，录用后，任务开始
//6：待确认成果，个人提交任务结束，供雇主确认
//7：待结算，雇主确认后进入
//8：已结算，雇主确认完成结算
//9：雇主取消加入 雇主拒绝申请后
    private void ConfirmPanel()
    {
        UIManager.Instance.taskConfirmPanel.OpenPanel(infoTask["id"].ToString());
    }
}
