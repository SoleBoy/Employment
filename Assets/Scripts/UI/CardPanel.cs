using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPanel : MonoBehaviour
{
    private Text numberText;
    private Image spriteImage;
    private Button backBtn;
    private Button fixBtn;
    private void Awake()
    {
        numberText = transform.Find("Bottom/NumberText").GetComponent<Text>();
        spriteImage = transform.Find("Sprite").GetComponent<Image>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        fixBtn = transform.Find("FixedBtn").GetComponent<Button>();

        fixBtn.onClick.AddListener(ClosePanel);
        backBtn.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel(string name)
    {
        gameObject.SetActive(true);
        numberText.text = string.Format("{0}碎片X{1}",name, Random.Range(5, 50));
        UIManager.Instance.CloningTips(string.Format("获得{0}：{1}",name, numberText.text));
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
