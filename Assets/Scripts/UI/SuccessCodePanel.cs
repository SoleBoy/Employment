using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccessCodePanel : MonoBehaviour
{
    private Button backBtn;
    private Button confirmBtn;

    private Text identityText;
    private Text nameText;
    private void Awake()
    {
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        confirmBtn = transform.Find("Button").GetComponent<Button>();
        identityText = transform.Find("Info/line/Name").GetComponent<Text>();
        nameText = transform.Find("Info/line/Code").GetComponent<Text>();

        backBtn.onClick.AddListener(ClosePanel);
        confirmBtn.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        identityText.text = DataTool.inviteCode;
        nameText.text = DataTool.theCompany;
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
