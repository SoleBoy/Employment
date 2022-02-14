using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoad : MonoBehaviour
{
    private Text loadText;
    private Image loadImage;

    private float lastTime;
    private bool isLoad;
    private void Awake()
    {
        loadText = transform.Find("load/Text").GetComponent<Text>();
        loadImage = transform.Find("load/Image").GetComponent<Image>();
    }

    private void Update()
    {
        if(isLoad)
        {
            if (lastTime <= 100)
            {
                lastTime += Time.deltaTime * 35;
                loadImage.fillAmount = lastTime * 0.01f;
            }
            else
            {
                isLoad = false;
                gameObject.SetActive(false);
                UIManager.Instance.gamePanel.OpenPanel();
            }
        }
    }

    public void OpenPanel()
    {
        isLoad = true;
        lastTime = 0;
        gameObject.SetActive(true);

        loadImage.fillAmount = 0;
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

}
