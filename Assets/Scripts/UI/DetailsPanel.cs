using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailsPanel : MonoBehaviour
{
    private Button fixBtn;
    private Button closeBtn;
    private void Awake()
    {
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        fixBtn = transform.Find("FixBtn").GetComponent<Button>();
        fixBtn.onClick.AddListener(ClosePanel);
        closeBtn.onClick.AddListener(ClosePanel);
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
