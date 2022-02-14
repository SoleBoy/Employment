using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour
{
    public Text inviteText;

    private Text firmText;
    private Text nameText;
    private Button backBtn;
    private Button yesBtn;
    private Button noBtn;

    private Transform infoParent;
    private GameObject destroyPanel;
  
    public void Init()
    {
        destroyPanel = transform.Find("DestroyPanel").gameObject;
        infoParent = transform.Find("PayrollView/Viewport/Content");
        firmText = transform.Find("TopBg/FirmText").GetComponent<Text>();
        nameText = transform.Find("TopBg/NameText").GetComponent<Text>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        yesBtn = transform.Find("DestroyPanel/YesBtn").GetComponent<Button>();
        noBtn = transform.Find("DestroyPanel/NoBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        yesBtn.onClick.AddListener(DestroyAccount);
        noBtn.onClick.AddListener(CloseDestroy);
    }
    public void InitData()
    {
        nameText.text = DataTool.roleName;
        if(DataTool.inviteCode.Length >= 8)
        {
            UpdateStatus("已输入");
        }
        else
        {
            UpdateStatus("");
        }
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
        destroyPanel.SetActive(false);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void UpdateStatus(string info)
    {
        inviteText.text = info;
    }
    //打开银行卡
    public void OpenBankCard()
    {
        UIManager.Instance.bankCardPanel.OpenPanel();
    }
    //更新手机号
    public void OpenPhone()
    {
        UIManager.Instance.phonePanel.OpenPanel();
    }
    public void OpenInvitation(Text infoText)
    {
        if (infoText.text == "已输入")
        {
            UIManager.Instance.successCodePanel.OpenPanel();
        }
        else
        {
            UIManager.Instance.invitationPanel.OpenPanel();
        }
    }
    public void OpenDestroy()
    {
        destroyPanel.SetActive(true);
    }
    private void CloseDestroy()
    {
        destroyPanel.SetActive(false);
    }
    //销毁账号
    private void DestroyAccount()
    {
        StartCoroutine(DestroyInfo(DataTool.cancel));
    }
    private IEnumerator DestroyInfo(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.SetRequestHeader("Authorization", DataTool.token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("销毁账号" + webRequest.downloadHandler.text);
            Dictionary<string, object> pageData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (pageData["code"].ToString() == "0")
            {
                DataTool.StartActivity(0);
                gameObject.SetActive(false);
                UIManager.Instance.unitPanel.ClosePanel();
                UIManager.Instance.homePanel.OpenHome();
            }
            else
            {
                UIManager.Instance.CloningTips(pageData["msg"].ToString());
            }
        }
    }
}
