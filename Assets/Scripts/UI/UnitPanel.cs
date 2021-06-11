using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour
{
    private Button backBtn;

    private Transform infoParent;
    private List<InfoItem> infos = new List<InfoItem>();
    private void Awake()
    {
        infoParent = transform.Find("Info/PayrollView/Viewport/Content");
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);

        for (int i = 0; i < infoParent.childCount; i++)
        {
            InfoItem item = new InfoItem(infoParent.Find("Item"+(i+1)),i);
            infos.Add(item);
            item.SetInfo(Random.Range(0,3));
        }
        if(DataTool.isUnit)
        {
            for (int i = 0; i < infos.Count; i++)
            {
                if (i == 4)
                {
                    infos[4].HideInfo(false);
                }
                else if (i >= 5)
                {
                    infos[i].HideInfo(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < infos.Count; i++)
            {
                if (i == 4)
                {
                    infos[4].HideInfo(true);
                }
                else if (i >= 5)
                {
                    infos[i].HideInfo(false);
                }
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
    //认证信息
    private class InfoItem
    {
        private Text infoText;
        private Button infoBtn;
        private GameObject finish;
        private GameObject infoObject;
        private int attestIndex;
        public InfoItem(Transform parent,int index)
        {
            attestIndex = index;
            infoObject = parent.gameObject;
            finish = parent.Find("Image").gameObject;
            infoBtn = parent.GetComponent<Button>();
            infoText = parent.Find("Amount").GetComponent<Text>();
            finish.SetActive(false);
            infoBtn.onClick.AddListener(OpenAttest);
        }
        private void OpenAttest()
        {
            Debug.Log(attestIndex);
            if (attestIndex == 0)
            {
                Debug.Log("输入邀请码");
                DataTool.StartActivity(158);
            }
            else if (attestIndex == 1)
            {
                Debug.Log("实名认证");
                DataTool.StartActivity(38);
            }
            else if (attestIndex == 2)
            {
                Debug.Log("银行卡绑定");
                DataTool.StartActivity(48);
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
        public void SetInfo(int index)
        {
            if(index == 0)
            {
                infoText.text = "必须项";
            }
            else if(index == 1)
            {
                infoText.text = "审核中";
            }
            else
            {
                infoText.text = "完成";
                finish.SetActive(true);
            }
        }
    }
}
