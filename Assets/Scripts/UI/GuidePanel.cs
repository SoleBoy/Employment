using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidePanel : MonoBehaviour
{
    private Button backBtn;
    private Button nextBtn;
    private void Awake()
    {
        nextBtn = transform.Find("NextStep").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        nextBtn.onClick.AddListener(OpenRegister);
    }

    private void OpenRegister()
    {
        gameObject.SetActive(false);
        DataTool.StartActivity(2);
        UIManager.Instance.homePanel.OpenHome();
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        UIManager.Instance.homePanel.OpenHome();
    }
}
