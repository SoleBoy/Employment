using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropPanel : MonoBehaviour
{
    private Button fixBtn;
    private Button cloceBtn;

    private void Awake()
    {
        fixBtn = transform.Find("determine").GetComponent<Button>();
        cloceBtn = transform.Find("CancelBtn").GetComponent<Button>();

        cloceBtn.onClick.AddListener(ClosePanel);
        fixBtn.onClick.AddListener(ReLog);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
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
        UIManager.Instance.homePanel.OpenHome();
        DataTool.StartActivity(0);
    }
}
