using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.IO;
using System;
using MiniJSON;
using System.Collections.Generic;
using UnityEngine.Networking;
using LitJson;

public class LocationService : MonoBehaviour
{
    private string GetGps = "";
    //880ef1cb42dd34128b666542829fdff5
    private const string key = "880ef1cb42dd34128b666542829fdff5";      //去高德地图开发者申请  ac611ea2fc1e9b9309cbb0486904e101

    private float lat = 0;
    private float lon = 0;
    /// <summary>
    /// 初始化一次位置
    /// </summary>
    private IEnumerator Start()
    {
        // Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置  
        // LocationService.isEnabledByUser 用户设置里的定位服务是否启用  
        if (!Input.location.isEnabledByUser)
        {
            GetGps = "isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString() + " Please turn on the GPS";
            yield return false;
        }

        // LocationService.Start() 启动位置服务的更新,最后一个位置坐标会被使用  
        Input.location.Start(10.0f, 10.0f);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            // 暂停协同程序的执行(1秒)  
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            GetGps = "Init GPS service time out";
            yield return false;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GetGps = "Unable to determine device location";
            yield return false;
        }
        else
        {
            GetGps = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
            //StartCoroutine(RequestAddress(Input.location.lastData.latitude.ToString(), Input.location.lastData.longitude.ToString(),"0"));
            yield return new WaitForSeconds(100);
        }
        // 如果不需要连续查询位置更新则停止服务
        Input.location.Stop();
    }
    /// <summary>
    /// 刷新位置信息（按钮的点击事件）
    /// </summary>
    public void UpdateGps()
    {
        StartCoroutine(StartGPS());
    }
    /// <summary>
    /// 停止刷新位置（节省手机电量）
    /// </summary>
    private void StopGPS()
    {
        Input.location.Stop();
    }

    private IEnumerator StartGPS()
    {
        // Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置  
        // LocationService.isEnabledByUser 用户设置里的定位服务是否启用  
        if (!Input.location.isEnabledByUser)
        {
            //GetGps = "isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString() + " Please turn on the GPS";
            yield return false;
        }

        // LocationService.Start() 启动位置服务的更新,最后一个位置坐标会被使用  
        Input.location.Start(10.0f, 10.0f);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            // 暂停协同程序的执行(1秒)  
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            //GetGps = "Init GPS service time out";
            yield return false;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            //GetGps = "Unable to determine device location";
            yield return false;
        }
        else
        {
            GetGps = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
            lat = Input.location.lastData.latitude;
            lon = Input.location.lastData.longitude;
            if (lat == 0 && lon == 0)
            {
                UIManager.Instance.CloningTips("位置获取失败,请检查GPS是否开启");
                PlayerPrefs.SetString("ClockInAddress", "位置获取失败");
                string messgInfo = string.Format("{0:D2}-{1:D2} " + " {2:D2}:{3:D2}  ", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                PlayerPrefs.SetString("ClockInTime", messgInfo);
                UIManager.Instance.hallPanel.CheckRecord(true);
            }
            else
            {
                StartCoroutine(RequestAddress(lat.ToString(), lon.ToString()));
                GetLocationByLngLat(lon, lat);
            }
            //DataTool.CallClockInfo(Input.location.lastData.latitude, Input.location.lastData.longitude);
            Input.location.Stop();
            yield return new WaitForSeconds(100);
        }
    }

    private IEnumerator RequestAddress(string lat, string lgn)
    {
        JsonData data = new JsonData();
        data["lat"] = lat;
        data["lgn"] = lgn;
        data["lockType"] = "1";
        data["pic"] = DataTool.checkAddress;
        data["taskId"] = DataTool.currentTask;

        UnityWebRequest webRequest = new UnityWebRequest(DataTool.clockUrl, "POST");
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
            Debug.Log("打卡成功"+webRequest.downloadHandler.text);
        }
    }

    /// <summary>
    /// 根据经纬度获取地址
    /// </summary>
    /// <param name="lng">经度 例如:113.692100</param>
    /// <param name="lat">维度 例如:34.752853</param>
    /// <param name="timeout">超时时间默认10秒</param>
    /// <returns>失败返回"" </returns>
    public void GetLocationByLngLat(double lng, double lat, int timeout = 10000)
    {
        string url = $"http://restapi.amap.com/v3/geocode/regeo?key={key}&location={lng},{lat}";
        GetLocationByURL(url, timeout);
    }
    /// <summary>
    /// 根据URL获取地址
    /// </summary>
    /// <param name="url">Get方法的URL</param>
    /// <param name="timeout">超时时间默认10秒</param>
    /// <returns></returns>
    private void GetLocationByURL(string url, int timeout = 10000)
    {
        string strResult = "";
        try
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.ContentType = "multipart/form-data";
            req.Accept = "*/*";
            //req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
            req.UserAgent = "";
            req.Timeout = timeout;
            req.Method = "GET";
            req.KeepAlive = true;
            HttpWebResponse response = req.GetResponse() as HttpWebResponse;
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                strResult = sr.ReadToEnd();
            }
            int formatted_addressIndex = strResult.IndexOf("formatted_address");
            int addressComponentIndex = strResult.IndexOf("addressComponent");
            int cutIndex = addressComponentIndex - formatted_addressIndex - 23;
            int subIndex = formatted_addressIndex + 20;
        }
        catch (Exception)
        {
            strResult = "";
        }
        if(strResult != "")
        {
            Debug.Log("最新地址：" + strResult);
            Debug.Log("N:" + lat + " E:" + lon);
            Dictionary<string, object> tokenData = Json.Deserialize(strResult) as Dictionary<string, object>;
            
            if (tokenData["status"].ToString() == "1")
            {
                Dictionary<string, object> pairs1 = tokenData["regeocode"] as Dictionary<string, object>;

                string messgInfo = string.Format("{0:D2}-{1:D2} " + " {2:D2}:{3:D2}  ", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                PlayerPrefs.SetString("ClockInTime", messgInfo);
                PlayerPrefs.SetString("ClockInAddress", pairs1["formatted_address"].ToString());
                UIManager.Instance.hallPanel.CheckRecord(true);
                UIManager.Instance.checkPanel.OpenPanel();
                lat = 0; lon = 0;
            }
            else
            {
                UIManager.Instance.CloningTips("位置获取失败,错误信息:"+ tokenData["status"].ToString());
            }
        }
    }
}