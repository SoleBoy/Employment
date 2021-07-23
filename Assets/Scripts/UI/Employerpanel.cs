using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Employerpanel : MonoBehaviour
{
    public QRcode qRcode;
    private Text firmText;
    private Text codeText;
    private Text askText;

    private Button logBtn;
    private Button certiBtn;
    private Button copyBtn;
    private Button backBtn;
    //invite_type：1代表对个人的邀请码，2代表对工商户的邀请码
    private void Awake()
    {
        firmText = transform.Find("InfoBg/FirmText").GetComponent<Text>();
        codeText = transform.Find("InfoBg/CodeText").GetComponent<Text>();
        askText = transform.Find("InfoBg/AskText").GetComponent<Text>();

        logBtn = transform.Find("LogBtn").GetComponent<Button>();
        certiBtn = transform.Find("CertiBtn").GetComponent<Button>();
        copyBtn = transform.Find("InfoBg").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        certiBtn.onClick.AddListener(OpenPersonal);
        copyBtn.onClick.AddListener(CopyToClipboard);
        backBtn.onClick.AddListener(ClosePanel);
        logBtn.onClick.AddListener(OpenLog);
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
        firmText.text = DataTool.theCompany;
        codeText.text = DataTool.inviteCode;
        if (DataTool.inviteType == "1")
        {
            askText.text = "雇主邀请码(个人)";
        }
        else
        {
            askText.text = "雇主邀请码(个体工商户)";
        }
    }

    public void ClosePanel()
    {
        //gameObject.SetActive(false);
        UIManager.Instance.dropPanel.OpenPanel("是否退出登录？");
    }

    private void OpenPersonal()
    {
        UIManager.Instance.personalPanel.OpenPanel();
    }
    //UnityReflection.onClickCopy("xxxx")
    private void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = "";
        GUIUtility.systemCopyBuffer = DataTool.inviteCode;
        Debug.Log("邀请码复制:" + GUIUtility.systemCopyBuffer);
        if(GUIUtility.systemCopyBuffer == "")
        {
            UIManager.Instance.CloningTips("邀请码复制失败请重新复制！");
        }
        else
        {
            UIManager.Instance.CloningTips("邀请码复制成功！");
        }
        
//        string input = codeText.text;
//#if UNITY_EDITOR
//        TextEditor t = new TextEditor();
//        t.text = input;
//        t.OnFocus();
//        t.Copy();
//#elif UNITY_IOS
//    CopyTextToClipboard_iOS(input);  
//#elif UNITY_ANDROID
//    AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//    AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
//    AndroidJavaClass tool = new AndroidJavaClass("com.my.ugcf.Tool");
//    tool.CallStatic("CopyTextToClipboard", currentActivity, input);
//#endif
    }
    private void OpenLog()
    {
        StartCoroutine(qRcode.OpenRcode());
    }
}
