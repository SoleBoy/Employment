using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailsPanel : MonoBehaviour
{
    private Button fixBtn;
    private Button closeBtn;

    private Transform infoView;
    private Transform infoItem;

    private List<UserInfo> userInfos = new List<UserInfo>();
    private string[] title = {"姓名","头衔","等级","兽数量","更多信息", "更多信息", "更多信息", "更多信息", "更多信息", "更多信息" };
    private void Awake()
    {
        infoItem = transform.Find("Floor/InfoItem");
        infoView = transform.Find("Floor/InfoView/Viewport/Content");
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        fixBtn = transform.Find("FixBtn").GetComponent<Button>();
        fixBtn.onClick.AddListener(ClosePanel);
        closeBtn.onClick.AddListener(ClosePanel);

        for (int i = 0; i < title.Length; i++)
        {
            var user = Instantiate(infoItem);
            user.gameObject.SetActive(true);
            user.SetParent(infoView);
            user.localScale = Vector3.one;
            UserInfo info = new UserInfo(user,title[i]);
            userInfos.Add(info);
            if(i > 3)
            {
                userInfos[i].SetInfo("XXXXXX");
            }
        }
        float maxY = title.Length * 100 + title.Length * 15;
        infoView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        userInfos[0].SetInfo(DataTool.roleName);
        userInfos[1].SetInfo(DataTool.roleTitle);
        userInfos[2].SetInfo("LV."+DataTool.roleLevel);
        userInfos[3].SetInfo("10");
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    //用户信息
    private class UserInfo
    {
        private Transform curretObject;
        private Text infoText;
        private string infoMessg;
        public UserInfo(Transform parent,string title)
        {
            curretObject = parent;
            infoMessg= title;
            infoText = curretObject.Find("InfoText").GetComponent<Text>();
        }

        public void SetInfo(string info)
        {
            infoText.text = string.Format("{0}:{1}",infoMessg, info);
        }
    }
}
