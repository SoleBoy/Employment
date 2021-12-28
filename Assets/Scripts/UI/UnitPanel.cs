using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour
{
    private Text firmText;
    private Text nameText;
    private Button backBtn;

    private Transform infoParent;
    private List<InfoItem> infos = new List<InfoItem>();
    //                                输入邀请码               实名认证                银行卡绑定,             签名验证，           江苏银行二类卡开卡,      活体认证，                意愿视频，          税务办理，           农业二类卡开卡，          银税签订协议
    private string[] fieldInfo = { "invite_code", "realname_auth_status", "bank_card_bind_status", "signature_status", "jiangsubank_ii_status", "living_check_status", "entrust_video_status", "tax_deal_status", "abc_bank_card_bind_status", "tripartite_agreement_status" };
    public void Init()
    {
        infoParent = transform.Find("PayrollView/Viewport/Content");
        firmText = transform.Find("TopBg/FirmText").GetComponent<Text>();
        nameText = transform.Find("TopBg/NameText").GetComponent<Text>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);

        for (int i = 0; i < infoParent.childCount; i++)
        {
            InfoItem item = new InfoItem(infoParent.Find("Item"+(i+1)),i);
            infos.Add(item);
            //item.SetInfo(1);
        }
    }
    public void InitData()
    {
        //firmText.text = DataTool.theCompany;
        nameText.text = DataTool.roleName;
        for (int i = 0; i < infos.Count; i++)
        {
            if (DataTool.information.ContainsKey(fieldInfo[i]))
            {
                infos[i].SetInfo(int.Parse(DataTool.information[fieldInfo[i]].ToString()));
            }
            else
            {
                infos[i].SetInfo(2);
            }
        }
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void UpdateStatus(int index,string info)
    {
        infos[index].SetInfo(info);
    }

    //认证信息
    private class InfoItem
    {
        private Text infoText;
        private Button infoBtn;
        //private GameObject finish;
        private GameObject infoObject;
        private int attestIndex;
        public InfoItem(Transform parent,int index)
        {
            attestIndex = index;
            infoObject = parent.gameObject;
            //finish = parent.Find("Image").gameObject;
            infoBtn = parent.GetComponent<Button>();
            infoText = parent.Find("Amount").GetComponent<Text>();
            //finish.SetActive(false);
            infoBtn.onClick.AddListener(OpenAttest);
        }
        private void OpenAttest()
        {
            if (attestIndex == 0)
            {
                if (infoText.text == "已输入")
                {
                    UIManager.Instance.successCodePanel.OpenPanel();
                }
                else
                {
                    UIManager.Instance.invitationPanel.OpenPanel();
                }
                //Debug.Log("输入邀请码");
                //DataTool.StartActivity(158);
            }
            else if (attestIndex == 1)
            {
                Debug.Log("实名认证");
                DataTool.StartActivity(38);
            }
            else if (attestIndex == 2)
            {
                if (infoText.text == "已完成")
                {
                    UIManager.Instance.bankCardPanel.OpenPanel();
                }
                else
                {
                    Debug.Log("银行卡绑定:" + infoText.text);
                    DataTool.StartActivity(48);
                }
            }
            else if (attestIndex == 3)
            {
                Debug.Log("签名认证");
                DataTool.StartActivity(91);
            }
            else if (attestIndex == 4)
            {
                Debug.Log("江苏银行二类开卡");
                DataTool.StartActivity(72);
            }
            else if (attestIndex == 5)
            {
                Debug.Log("活体认证");
                DataTool.StartActivity(95);
            }
            else if (attestIndex == 6)
            {
                Debug.Log("意愿视频");
                DataTool.StartActivity(99);
            }
            else if (attestIndex == 7)
            {
                Debug.Log("税务办理");
                DataTool.StartActivity(105);
            }
            else if (attestIndex == 8)
            {
                Debug.Log("农业二类卡开卡");
                DataTool.StartActivity(108);
            }
            else if (attestIndex == 9)
            {
                Debug.Log("银税协议签订");
                DataTool.StartActivity(110);
            }
        }
        public void HideInfo(bool isHide)
        {
            infoObject.SetActive(isHide);
        }
        public void SetInfo(string info)
        {
            infoText.text = info;
            infoText.color = DataTool.color_submitted;
        }

        //1已完成 2未完成 3审核待处理 0必填项
        public void SetInfo(int index)
        {
            if (attestIndex == 0)
            {
                if(index >= 1)
                {
                    infoText.text = "已输入";
                    infoText.color = DataTool.color_submitted;
                }
                else
                {
                    infoText.text = "";
                }
            }
            else
            {
                if (index == 0)
                {
                    infoText.text = "必填项";
                    infoText.color = DataTool.color_progress;
                }
                else if (index == 1)
                {
                    infoText.text = "已完成";
                    infoText.color = DataTool.color_submitted;
                    if (attestIndex != 2)
                        infoBtn.enabled = false;
                }
                else if (index == 2)
                {
                    infoText.color = DataTool.color_start;
                    infoText.text = "未完成";
                }
                else
                {
                    infoText.color = DataTool.color_review;
                    infoText.text = "审核待处理";
                }
            }
        }
    }
}
