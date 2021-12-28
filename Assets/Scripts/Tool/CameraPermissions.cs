using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPermissions : MonoBehaviour
{
    private WebCamDevice[] webCamDevices;
    ///打开相机
    private IEnumerator Start()
    {
        // 申请相机权限
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        // 判断是否有相机权限
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            // 获取相机设备
            webCamDevices = WebCamTexture.devices;
            
        }
        else
        {
           
        }
    }
}
