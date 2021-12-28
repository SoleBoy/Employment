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

    private Vector3 frontAngle = new Vector3(0, 180, 90);
    private Vector3 rearAngle = new Vector3(0, 0, -90);
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

    public void OpenPanel(bool isFlip, Texture texture)
    {
        gameObject.SetActive(true);
        rawImage.texture = ByteToTex2d(DataTool.cheackByte);
        if (isFlip)
        {
            rawImage.transform.localEulerAngles = rearAngle;
        }
        else
        {
            rawImage.transform.localEulerAngles = frontAngle;
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
    //string.Format("{0:D2}:{1:D2}:{2:D2} " + "{3:D4}/{4:D2}/{5:D2}", hour, minute, second, year, month, day);
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
