using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.IO;
using System;

public class LocationService : MonoBehaviour
{
    private string GetGps = "";
    /// <summary>
    /// 初始化一次位置
    /// </summary>
    private void Start()
    {
        StartCoroutine(StartGPS());
        GetGps = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
    }
    /// <summary>
    /// 刷新位置信息（按钮的点击事件）
    /// </summary>
    public void updateGps()
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
            //GetGps = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
            //GetGps = GetGps + " Time:" + Input.location.lastData.timestamp;
            GetGps = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
            string messgInfo = string.Format("{0:D2}-{1:D2} " + " {2:D2}:{3:D2}  ", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
            messgInfo += GetGps;
            PlayerPrefs.SetString("ClockInTime", messgInfo);
            UIManager.Instance.hallPanel.CheckRecord(true);
            yield return new WaitForSeconds(100);
        }
    }


    const string key = "46e7e5ba34d76192d8172172742ae1c4";		//去高德地图开发者申请 这个key的流量不知道被哪位同学用完了，

    /// <summary>
    /// 根据经纬度获取地址
    /// </summary>
    /// <param name="LngLatStr">经度纬度组成的字符串 例如:"113.692100,34.752853"</param>
    /// <param name="timeout">超时时间默认10秒</param>
    /// <returns>失败返回"" </returns>
    public string GetLocationByLngLat(string LngLatStr, int timeout = 10000)
    {
        LngLatStr = "113.692100,34.752853";
        string url = $"http://restapi.amap.com/v3/geocode/regeo?key={key}&location={LngLatStr}";
        return GetLocationByURL(url, timeout);
    }

    /// <summary>
    /// 根据经纬度获取地址
    /// </summary>
    /// <param name="lng">经度 例如:113.692100</param>
    /// <param name="lat">维度 例如:34.752853</param>
    /// <param name="timeout">超时时间默认10秒</param>
    /// <returns>失败返回"" </returns>
    public string GetLocationByLngLat(double lng, double lat, int timeout = 10000)
    {
        string url = $"http://restapi.amap.com/v3/geocode/regeo?key={key}&location={lng},{lat}";
        return GetLocationByURL(url, timeout);
    }
    /// <summary>
    /// 根据URL获取地址
    /// </summary>
    /// <param name="url">Get方法的URL</param>
    /// <param name="timeout">超时时间默认10秒</param>
    /// <returns></returns>
    private string GetLocationByURL(string url, int timeout = 10000)
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
            return strResult;
        }
        catch (Exception)
        {
            strResult = "";
        }
        return strResult;
    }
}