using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ClockinPanel : MonoBehaviour
{
    private RawImage rawImage;
    private RawImage textImage;
    private Button submitBtn;
    private Button takeBtn;
    private Button flipBtn;
    // 摄像机图片参数
    private WebCamTexture webCamTexture;

    private Transform clockinPanel;
    private WebCamDevice[] webCamDevices;

    private bool isFlip;
    private void Awake()
    {
        clockinPanel = transform.Find("ClockinPanel");
       
        rawImage = transform.Find("RawImage").GetComponent<RawImage>();
        textImage = transform.Find("ClockinPanel/Image").GetComponent<RawImage>();
        submitBtn = transform.Find("ClockinPanel/SubmitBtn").GetComponent<Button>();
        takeBtn = transform.Find("TakeBtn").GetComponent<Button>();
        flipBtn = transform.Find("FlipBtn").GetComponent<Button>();
        takeBtn.onClick.AddListener(TakePhotoAndSave);
        submitBtn.onClick.AddListener(SubmitTexture);
        flipBtn.onClick.AddListener(FlipCamera);
        
    }
    public void OpenPanel()
    {
        //if (DataTool.isClock)
        //{
        //    UIManager.Instance.CloningTips("今日已完成打卡");
        //}
        //else
        {
            gameObject.SetActive(true);
            flipBtn.gameObject.SetActive(true);
            takeBtn.gameObject.SetActive(true);
            rawImage.gameObject.SetActive(true);
            clockinPanel.gameObject.SetActive(false);
            UIManager.Instance.gameObject.SetActive(false);
            // 打开相机
            StartCoroutine("OpenCamera");
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void TakePhotoAndSave()
    {
        // 调用拍照保存函数
        rawImage.texture = null;
        TakePhotoAndSaveImage(webCamTexture);
        flipBtn.gameObject.SetActive(false);
        takeBtn.gameObject.SetActive(false);
        rawImage.gameObject.SetActive(false);
        clockinPanel.gameObject.SetActive(true);
        webCamTexture.Stop();
    }

    private void SubmitTexture()
    {
        DataTool.isClock = true;
        UIManager.Instance.gameObject.SetActive(true);
        UIManager.Instance.CloningTips("打卡成功");
        PlayerPrefs.SetString(System.DateTime.Now.Date.ToString() + "Clock", "Clock");
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
                webCamTexture = new WebCamTexture(webCamName, 768, 1024);
                webCamTexture.Play();
                // 把获取的图像渲染到画布上
                rawImage.texture = webCamTexture;
            }
            else
            {
                isFlip = true;
                string webCamName = webCamDevices[0].name;
                // 设置相机渲染宽高，并运行相机
                webCamTexture = new WebCamTexture(webCamName, 768, 1024);
                webCamTexture.Play();
                // 把获取的图像渲染到画布上
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
                isFlip = false;
                // 把 0 号设备（移动端后置摄像头）名称赋值
                string webCamName = webCamDevices[1].name;
                // 设置相机渲染宽高，并运行相机
                webCamTexture = new WebCamTexture(webCamName, 768, 1024);
                webCamTexture.Play();
                // 把获取的图像渲染到画布上
                rawImage.texture = webCamTexture;
            }
            else
            {
                UIManager.Instance.CloningTips("获取相机权限失败");
            }
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
        textImage.texture = texture2D;
//        byte[] imageBytes = texture2D.EncodeToJPG();
//        // 判断图片 bytes 是否为空
//        if (imageBytes != null && imageBytes.Length > 0)
//        {
//            // 判断Android 平台，进行对应路径设置
//            string savePath;
//            string platformPath = Application.streamingAssetsPath + "/MyTempPhotos";
//#if UNITY_ANDROID && !UNITY_EDITOR
//            platformPath = "/sdcard/DCIM/MyTempPhotos";
//#endif
//            // 如果文件夹不存在，就创建文件夹
//            if (!Directory.Exists(platformPath))
//            {
//                Directory.CreateDirectory(platformPath);
//            }
//            // 保存图片
//            savePath = platformPath + "/" + Time.deltaTime + ".jpg";
//            File.WriteAllBytes(savePath, imageBytes);
//        }
    }
}
