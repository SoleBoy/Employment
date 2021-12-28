using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankCardPanel : MonoBehaviour
{
    private Button backBtn;

    private Text bankText;
    private Text cardText;

    public void Init()
    {
        bankText = transform.Find("Info/line/Name").GetComponent<Text>();
        cardText = transform.Find("Info/line/Code").GetComponent<Text>();

        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
    }
    public void InitData()
    {
        bankText.text = DataTool.bankName;
        string bankNo = DataTool.bankNo;
        cardText.text = System.Text.RegularExpressions.Regex.Replace(bankNo, @"(\w{4})", "$1 ").Trim(' ');
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
