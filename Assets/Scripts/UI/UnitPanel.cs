using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour
{
    public Transform[] buttonArray;

    public Text inviteText;

    private Text firmText;
    private Text nameText;
    private Button backBtn;
    private Button yesBtn;
    private Button noBtn;

    private Transform infoParent;
    private GameObject destroyPanel;

    private List<ButtonItem> buttonItems = new List<ButtonItem>();
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
        for (int i = 0; i < buttonArray.Length; i++)
        {
            ButtonItem item = new ButtonItem(buttonArray[i],i);
            buttonItems.Add(item);
        }
    }
    public void InitData()
    {
        nameText.text = DataTool.roleName;
        buttonItems[0].SetInfo(0, DataTool.information["realNameStatus"].ToString());
        buttonItems[1].SetInfo(1, DataTool.information["bindBankStatus"].ToString());
        buttonItems[2].SetInfo(2, DataTool.information["bodyRecognitionStatus"].ToString());
        buttonItems[3].SetInfo(3, DataTool.information["willingVideoVerificationStatus"].ToString());
        buttonItems[4].SetInfo(4, DataTool.inviteCode);
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

    private class ButtonItem
    {
        private Text infoText;
        private Button itemBtn;
        private int indexCurr;
        public ButtonItem(Transform item,int index)
        {
            this.indexCurr = index;
            infoText = item.Find("Amount").GetComponent<Text>();
            itemBtn = item.GetComponent<Button>();
            itemBtn.onClick.AddListener(ClickItem);
        }
        public void SetInfo(int index,string status)
        {
            if (index == 0) //实名
            {
                itemBtn.enabled = false;
            }
            else if (index == 1) //银行卡
            {
                
            }
            else if (index == 2) //活体认证
            {

            }
            else if (index == 3) //意愿
            {
                if(status == "1")
                {
                    infoText.text = "已完成";
                }
                else if (status == "2")
                {
                    infoText.text = "待审核";
                }
                else
                {
                    infoText.text = "未注册";
                }
            }
            else if (index == 4) //邀请码
            {
                if (DataTool.inviteCode.Length >= 8)
                {
                    infoText.text = "已输入";
                }
                else
                {
                    infoText.text = "";
                }
            }
        }
        private void ClickItem()
        {
            if(indexCurr == 0) //实名
            {

            }
            else if(indexCurr == 1) //银行卡
            {
                UIManager.Instance.bankCardPanel.OpenPanel();
            }
            else if (indexCurr == 2) //活体认证
            {

            }
            else if (indexCurr == 3) //意愿
            {

            }
            else if (indexCurr == 4) //邀请码
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
            else if (indexCurr == 5) //登录手机号
            {
                UIManager.Instance.phonePanel.OpenPanel();
            }
        }
    }

}
