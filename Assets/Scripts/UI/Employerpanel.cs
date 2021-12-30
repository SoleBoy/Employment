using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Employerpanel : MonoBehaviour
{
    public Sprite[] pickSprite;
    public Sprite[] norSprite;
    public Image[] clickImage;
    //public GameObject[] clickObject;

    public QRcode qRcode;
    private Text firmText;
    private Text codeText;
    private Text askText;

    private Button logBtn;
    //private Button certiBtn;
    private Button copyBtn;
    //private Button backBtn;

    private Button homeBtn;
    private Button mainBtn;

    private int indexCurret;
    private GameObject copyPanel;

    public PersonalPanel personalPanel;
    //invite_type：1代表对个人的邀请码，2代表对工商户的邀请码
    private void Awake()
    {
        copyPanel = transform.Find("CopyPanel").gameObject;
        personalPanel = transform.Find("PersonalPanel").GetComponent<PersonalPanel>();
        firmText = transform.Find("InfoBg/FirmText").GetComponent<Text>();
        codeText = transform.Find("InfoBg/CodeText").GetComponent<Text>();
        askText = transform.Find("InfoBg/AskText").GetComponent<Text>();

        homeBtn = transform.Find("HomeBtn").GetComponent<Button>();
        mainBtn = transform.Find("MainBtn").GetComponent<Button>();
        logBtn = transform.Find("LogBtn").GetComponent<Button>();
        //certiBtn = transform.Find("CertiBtn").GetComponent<Button>();
        copyBtn = transform.Find("InfoBg").GetComponent<Button>();
        //backBtn = transform.Find("BackBtn").GetComponent<Button>();

        //certiBtn.onClick.AddListener(OpenPersonal);
        copyBtn.onClick.AddListener(CopyToClipboard);
        //backBtn.onClick.AddListener(ClosePanel);
        logBtn.onClick.AddListener(OpenLog);
        homeBtn.onClick.AddListener(OpenHome);
        mainBtn.onClick.AddListener(OpenMain);
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
        personalPanel.ClosePanel();
        ClcikButton(0);
        firmText.text = DataTool.theCompany;
        codeText.text = DataTool.inviteCode;
        askText.text = "雇主邀请码";
    }

    public void ClosePanel()
    {
        //gameObject.SetActive(false);
        UIManager.Instance.dropPanel.OpenPanel("是否退出登录？");
    }

    public void OpenHome()
    {
        ClcikButton(0);
        personalPanel.ClosePanel();
    }

    //营业执照
    public void OpenLicense()
    {
        //DataTool.employerInfo["buzLicensePic"].ToString()
        UIManager.Instance.businessPanel.OpenPanel();
        //UIManager.Instance.loadTxt.GetMonthly_7();
    }

    private void OpenMain()
    {
        ClcikButton(1);
        personalPanel.OpenPanel();
    }

    private void OpenPersonal()
    {
        personalPanel.OpenPanel();
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
            copyPanel.SetActive(true);
            //UIManager.Instance.CloningTips("邀请码复制成功！");
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

    private void ClcikButton(int index)
    {
        clickImage[indexCurret].sprite = norSprite[indexCurret];
        //clickObject[indexCurret].SetActive(false);
        indexCurret = index;
        clickImage[indexCurret].sprite = pickSprite[indexCurret];
        //clickObject[indexCurret].SetActive(true);
    }
}
