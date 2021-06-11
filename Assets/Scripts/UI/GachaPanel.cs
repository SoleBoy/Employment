using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaPanel : MonoBehaviour
{
    private Text numberText;
    private Text openText;

    private Button backBtn;
    private Button openBtn;

    private bool isOpen;
    private void Awake()
    {
        numberText = transform.Find("InfoText").GetComponent<Text>();
        openText = transform.Find("OpenBtn/Text").GetComponent<Text>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        openBtn = transform.Find("OpenBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(ClosePanel);
        openBtn.onClick.AddListener(OpenCard);
        isOpen = PlayerPrefs.GetString("LastDay") != DateTime.Now.Date.ToString();
        if(isOpen)
        {
            openText.text = "首次免费";
        }
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        numberText.text = string.Format("剩余次数：{0}",DataTool.blindBox);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void OpenCard()
    {
        if(DataTool.blindBox >= 1 || isOpen)
        {
            if (isOpen)
            {
                isOpen = false;
                openText.text = "打开";
                PlayerPrefs.SetString("LastDay", DateTime.Now.Date.ToString());
            }
            else
            {
                DataTool.blindBox -= 1;
                PlayerPrefs.SetInt("CurretBlindBox", DataTool.blindBox);
                numberText.text = string.Format("剩余次数：{0}", DataTool.blindBox);
            }
            UIManager.Instance.cardPanel.OpenPanel("爱丽儿");
        }
        else
        {
            UIManager.Instance.CloningTips("次数已经用完!");
        }
    }
}
