using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropPanel : MonoBehaviour
{
    private Button fixBtn;
    private Button cloceBtn;

    private Text infoText;

    private void Awake()
    {
        infoText = transform.Find("InfoText").GetComponent<Text>();
        fixBtn = transform.Find("determine").GetComponent<Button>();
        cloceBtn = transform.Find("CancelBtn").GetComponent<Button>();

        cloceBtn.onClick.AddListener(ClosePanel);
        fixBtn.onClick.AddListener(ReLog);
    }

    public void OpenPanel(string messgTip)
    {
        gameObject.SetActive(true);
        infoText.text = messgTip;
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    //登录注册
    private void ReLog()
    {
        Debug.Log("退出登录");
        gameObject.SetActive(false);
        if(DataTool.roleType == "雇主")
        {
            UIManager.Instance.employerpanel.OpenPanel();
        }
        else
        {
            UIManager.Instance.homePanel.OpenHome();
        }
        DataTool.StartActivity(0);
    }
}
