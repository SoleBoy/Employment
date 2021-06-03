using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayrollPanel : MonoBehaviour
{
    private Text dateText;
    private Text wageText;

    private Button backBtn;

    private void Awake()
    {
        dateText = transform.Find("Info/DateText").GetComponent<Text>();
        wageText = transform.Find("Info/WageText").GetComponent<Text>();

        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(ClosePanel);
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
