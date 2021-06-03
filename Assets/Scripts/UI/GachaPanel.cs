using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaPanel : MonoBehaviour
{
    private Text numberText;

    private Button backBtn;
    private Button openBtn;

    private int number;
    private void Awake()
    {
        numberText = transform.Find("InfoText").GetComponent<Text>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        openBtn = transform.Find("OpenBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(ClosePanel);
        openBtn.onClick.AddListener(OpenCard);
        number = 1;
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        numberText.text = string.Format("剩余次数：{0}",number);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void OpenCard()
    {
        if(number >= 1)
        {
            number -= 1;
            numberText.text = string.Format("剩余次数：{0}", number);
            UIManager.Instance.cardPanel.OpenPanel("爱丽儿");
        }
        else
        {
            UIManager.Instance.CloningTips("次数已经用完!");
        }
    }
}
