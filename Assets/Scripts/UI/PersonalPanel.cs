using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonalPanel : MonoBehaviour
{
    private Text nameText;
    private Button backBtn;
    private Transform infoParent;
    private List<InfoItem> infos = new List<InfoItem>();
    //企业法人实名认证  营业执照登记  银行开户许可证 签名验证
    private string[] fieldInfo = { "com_fr_realname_auth_status", "com_identity_register_status", "com_bank_acct_open_permit_status",  "com_fr_signature_status" };
    public void Init()
    {
        infoParent = transform.Find("Info/PayrollView/Viewport/Content");
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        nameText = transform.Find("Head/NameText").GetComponent<Text>();

        backBtn.onClick.AddListener(ClosePanel);
        for (int i = 0; i < infoParent.childCount; i++)
        {
            InfoItem item = new InfoItem(infoParent.Find("Item" + (i + 1)), i);
            infos.Add(item);
        }
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        nameText.text = DataTool.theCompany;
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void CertificationInfo(Dictionary<string, object> pairs)
    {
        for (int i = 0; i < infos.Count; i++)
        {
            if (pairs.ContainsKey(fieldInfo[i]))
            {
                infos[i].SetInfo(int.Parse(pairs[fieldInfo[i]].ToString()));
            }
            else
            {
                infos[i].SetInfo(2);
            }
        }
    }
    //认证信息
    private class InfoItem
    {
        private Text infoText;
        private Button infoBtn;
        private GameObject finish;
        private GameObject infoObject;
        private int attestIndex;
        public InfoItem(Transform parent, int index)
        {
            attestIndex = index;
            infoObject = parent.gameObject;
            finish = parent.Find("Image").gameObject;
            infoBtn = parent.GetComponent<Button>();
            infoText = parent.Find("Amount").GetComponent<Text>();
            finish.SetActive(false);
            //infoBtn.onClick.AddListener(OpenAttest);
        }
        private void OpenAttest()
        {
            Debug.Log(attestIndex);
            if (attestIndex == 0)
            {
                Debug.Log("企业法人实名认证");
                DataTool.StartActivity(158);
            }
            else if (attestIndex == 1)
            {
                Debug.Log("营业执照登记");
                DataTool.StartActivity(38);
            }
            else if (attestIndex == 2)
            {
                Debug.Log("银行开户许可证");
                DataTool.StartActivity(48);
            }
            else if (attestIndex == 3)
            {
                Debug.Log("签名验证");
                DataTool.StartActivity(91);
            }
        }
        public void HideInfo(bool isHide)
        {
            infoObject.SetActive(isHide);
        }
        //1已完成 2未完成 3审核待处理 0必填项
        public void SetInfo(int index)
        {
            if (index == 0)
            {
                infoText.text = "必填项";
            }
            else if (index == 1)
            {
                infoText.text = "已完成";
                finish.SetActive(true);
            }
            else if (index == 2)
            {
                infoText.text = "未完成";
            }
            else
            {
                infoText.text = "审核待处理";
            }
        }
    }
}
