using LitJson;
using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class PhotographPanel : MonoBehaviour
{
    public int width = 950;
    public int high = 720;

    private RawImage rawImage;
    //private RawImage textImage;
    //private Button submitBtn;
    private Button takeBtn;
    private Button flipBtn;
    //private Button backBtn;
    private Button closeBtn;
    // 摄像机图片参数
    private WebCamTexture webCamTexture;

    //private Transform clockinPanel;
    private WebCamDevice[] webCamDevices;

    private Vector3 frontAngle = new Vector3(0, 180, 90);
    private Vector3 rearAngle = new Vector3(0, 0, -90);

    private bool isFlip;
    private bool isClock;
    private void Awake()
    {
        //clockinPanel = transform.Find("ClockinPanel");

        rawImage = transform.Find("RawImage").GetComponent<RawImage>();
        //textImage = transform.Find("ClockinPanel/Image").GetComponent<RawImage>();
        //submitBtn = transform.Find("ClockinPanel/SubmitBtn").GetComponent<Button>();
        takeBtn = transform.Find("TakeBtn").GetComponent<Button>();
        flipBtn = transform.Find("FlipBtn").GetComponent<Button>();
        closeBtn = transform.Find("BackBtn").GetComponent<Button>();
        //backBtn = transform.Find("ClockinPanel/BackBtn").GetComponent<Button>();
        takeBtn.onClick.AddListener(TakePhotoAndSave);
        //submitBtn.onClick.AddListener(SubmitTexture);
        flipBtn.onClick.AddListener(FlipCamera);
        //backBtn.onClick.AddListener(ClosePanel);
        closeBtn.onClick.AddListener(BackPanel);
    }

    private void BackPanel()
    {
        webCamTexture.Stop();
        gameObject.SetActive(false);
        UIManager.Instance.gameObject.SetActive(true);
    }

    public void OpenPanel(bool isClock)
    {
        this.isClock = isClock;
        gameObject.SetActive(true);
        rawImage.texture = null;
        UIManager.Instance.gameObject.SetActive(false);
        // 打开相机
        StartCoroutine("OpenCamera");
    }

    private void TakePhotoAndSave()
    {
        // 调用拍照保存函数
        TakePhotoAndSaveImage(webCamTexture);
        gameObject.SetActive(false);
        UIManager.Instance.CheckUrl();
        if (isClock)
        {
            UIManager.Instance.clockinPanel.OpenPanel(isFlip, rawImage.texture);
        }
        else
        {
            UIManager.Instance.taskSubmitPanel.OpenReplenish(rawImage.texture);
        }
        rawImage.texture = null;
        webCamTexture.Stop();
        Resources.UnloadUnusedAssets();
    }

    private void FlipCamera()
    {
        if (webCamDevices != null && webCamDevices.Length > 0)
        {
            if (isFlip)
            {
                isFlip = false;
                string webCamName = webCamDevices[1].name;
                // 设置相机渲染宽高，并运行相机
                webCamTexture = new WebCamTexture(webCamName, width, high);
                webCamTexture.Play();
                // 把获取的图像渲染到画布上
                rawImage.transform.localEulerAngles = frontAngle;
                rawImage.texture = webCamTexture;
            }
            else
            {
                isFlip = true;
                string webCamName = webCamDevices[0].name;
                // 设置相机渲染宽高，并运行相机
                webCamTexture = new WebCamTexture(webCamName, width, high);
                webCamTexture.Play();
                // 把获取的图像渲染到画布上
                rawImage.transform.localEulerAngles = rearAngle;
                rawImage.texture = webCamTexture;
            }
        }
    }
    ///打开相机
    private IEnumerator OpenCamera()
    {
        // 申请相机权限
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        // 判断是否有相机权限
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            // 获取相机设备
            webCamDevices = WebCamTexture.devices;
            flipBtn.gameObject.SetActive(webCamDevices.Length > 1);
            // 判断是否有相机设别
            if (webCamDevices != null && webCamDevices.Length > 0)
            {
                // 把 0 号设备（移动端后置摄像头）名称赋值
                string webCamName = "";
                if (webCamDevices.Length > 1)
                {
                    isFlip = false;
                    rawImage.transform.localEulerAngles = frontAngle;
                    webCamName = webCamDevices[1].name;
                }
                else
                {
                    isFlip = true;
                    rawImage.transform.localEulerAngles = rearAngle;
                    webCamName = webCamDevices[0].name;
                }
                // 设置相机渲染宽高，并运行相机
                webCamTexture = new WebCamTexture(webCamName, width, high, 25);
                webCamTexture.Play();
                // 把获取的图像渲染到画布上
                rawImage.texture = webCamTexture;
                DataTool.checkTexture = webCamTexture;
            }
            else
            {
                gameObject.SetActive(false);
                UIManager.Instance.SubmitTip(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
            UIManager.Instance.SubmitTip(false);
        }
    }
    /// <summary>
    /// 保存图片的接口函数
    /// </summary>
    /// <param name="tex"></param>
    private void TakePhotoAndSaveImage(WebCamTexture tex)
    {
        // 新建一个 Texture2D 来获取相机图片
        // 然后 把图片转成 JPG 格式的 bytes
        Texture2D texture2D = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, true);
        texture2D.SetPixels32(tex.GetPixels32());
        texture2D.Apply();

        DataTool.cheackByte = null;
        DataTool.cheackByte = texture2D.EncodeToJPG();

        System.IO.File.WriteAllBytes(DataTool.filePath,DataTool.cheackByte);
        //StartCoroutine(CheckAddress(DataTool.pictureUrl, filePath));
    }
    //path=C:\Users\Acer\AppData\LocalLow\DefaultCompany\平台游戏测试\ClockIn.png
   
}
