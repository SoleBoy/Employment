using UnityEngine;
using System.Collections;
using ZXing;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;

public class QRcode : MonoBehaviour
{
    public Color32[] data;
    private bool isScan;
    public RawImage cameraTexture;
    //public Text txtQRcode;
    private WebCamTexture webCameraTexture;
    private BarcodeReader barcodeReader;
    private Button closeBtn;
    private float timer = 0;
    private Vector3 frontAngle = new Vector3(0, 180, 90);
    private Vector3 rearAngle = new Vector3(0, 0, -90);
    private void Awake()
    {
        closeBtn = transform.Find("BackBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(ClosePanel);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
        isScan = false;
        webCameraTexture.Stop();
        gameObject.SetActive(false);
    }

    public IEnumerator OpenRcode()
    {
        gameObject.SetActive(true);
        barcodeReader = new BarcodeReader();
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            try
            {
                WebCamDevice[] webCamDevices = WebCamTexture.devices;

                cameraTexture.transform.localEulerAngles = rearAngle;
                // 设置相机渲染宽高，并运行相机
                webCameraTexture = new WebCamTexture(webCamDevices[0].name, 784, 784, 25);
                //webCameraTexture = new WebCamTexture(webCamDevices[0].name, Screen.width, Screen.height, 25);
                webCameraTexture.Play();
                // 把获取的图像渲染到画布上
                cameraTexture.texture = webCameraTexture;
                isScan = true;
            }
            catch (System.Exception)
            {
                ClosePanel();
                UIManager.Instance.CloningTips("摄像头打开失败");
            }
           
        }

    }

    void Update()
    {
        if (isScan)
        {
            timer += Time.deltaTime;

            if (timer > 0.5f) //0.5秒扫描一次
            {
                StartCoroutine(ScanQRcode());
                timer = 0;
            }
        }

    }

    IEnumerator ScanQRcode()
    {
        data = webCameraTexture.GetPixels32();
        DecodeQR(webCameraTexture.width, webCameraTexture.height);
        yield return new WaitForEndOfFrame();
    }

    private void DecodeQR(int width, int height)
    {
        var br = barcodeReader.Decode(data, width, height);
        if (br != null)
        {
            //txtQRcode.text = br.Text;
            //CallNative(br.Text);
            StartCoroutine(ScanCode(DataTool.scanCodeUrl, br.Text));
            isScan = false;
            webCameraTexture.Stop();
            gameObject.SetActive(false);
        }
    }

    private IEnumerator ScanCode(string url,string codeData)
    {
        JsonData data = new JsonData();
        data["data"] = codeData;

        UnityWebRequest webRequest = new UnityWebRequest(url, "Post");
        webRequest.SetRequestHeader("Authorization", DataTool.token);

        byte[] postBytes = System.Text.Encoding.Default.GetBytes(data.ToJson());
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("扫码成功" + webRequest.downloadHandler.text);
        }

    }

    public static void CallNative(string mesgg)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("扫描信息：" + mesgg);

            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.example.jinchang.utils.UnityReflection"))
            {
                if (pluginClass != null)
                {
                    pluginClass.CallStatic("pcLogin",mesgg);
                }
            }

        }else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {

        }
    }
    //" "appToken" : "3e5e876-4c81-48ff-b6f9-86b908eQ2df" , "mobile": "19145724473" ,"qncode ":"Cc5695a7b32a4a3a91c6989c4aede073""
    private void OnDestroy()
    {
        isScan = false;
        webCameraTexture.Stop();
        gameObject.SetActive(false);
    }

}