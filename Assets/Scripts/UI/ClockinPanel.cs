using LitJson;
using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ClockinPanel : MonoBehaviour
{
    private RawImage rawImage;
    private Button submitBtn;
    private Button backBtn;

    private Transform clockinPanel;
    private void Awake()
    {
        clockinPanel = transform.Find("ClockinPanel");
       
        rawImage = transform.Find("Image").GetComponent<RawImage>();
        submitBtn = transform.Find("SubmitBtn").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        submitBtn.onClick.AddListener(SubmitTexture);
        backBtn.onClick.AddListener(ClosePanel);
    }

    private void BackPanel()
    {
        rawImage.texture = null;
        gameObject.SetActive(false);
        UIManager.Instance.gameObject.SetActive(true);
    }

    public void OpenPanel(bool isFlip)
    {
        gameObject.SetActive(true);
        rawImage.texture = ByteToTex2d(DataTool.cheackByte);
        if (isFlip)
        {
            rawImage.transform.localEulerAngles = DataTool.frontAngle;
        }
        else
        {
            rawImage.transform.localEulerAngles = DataTool.rearAngle;
        }
    }

    public static Texture2D ByteToTex2d(byte[] bytes)
    {
        Texture2D tex = new Texture2D(1015, 850);
        tex.LoadImage(bytes);
        return tex;
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void SubmitTexture()
    {
        PlayerPrefs.SetString(System.DateTime.Now.Date.ToString() + "Clock", "Clock");
        UIManager.Instance.SubmitTip(true);
    }

    public void SuccessCheck()
    {
        gameObject.SetActive(false);
        DataTool.isClock = true;
    }
}
