using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankCardPanel : MonoBehaviour
{
    private Button backBtn;

    private Text bankText;
    private Text cardText;

    private void Awake()
    {
        bankText = transform.Find("Info/info3/Text").GetComponent<Text>();
        cardText = transform.Find("Info/info3/Text").GetComponent<Text>();

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
