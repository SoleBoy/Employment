﻿using UnityEngine;
using System.Collections;
using MiniJSON;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class LoadTxt : MonoBehaviour
{

    public TextAsset[] TxtFile;    //建立TextAsset
    //07-05  16:53  河南省郑州市管城回族区城东路街道城东路98号正商向阳广场
    private void Start()
    {
        
    }
    //total字段累计收入
    public void GetMonthly_1()
    {
        Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[0].ToString()) as Dictionary<string, object>;
        UIManager.Instance.incomePanel.InitState(2,tokenData);
    }
    public void GetMonthly_2()
    {
        Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[1].ToString()) as Dictionary<string, object>;
        UIManager.Instance.salaryPanel.OpenPanel(tokenData["data"] as Dictionary<string, object>);
    }
    public void GetMonthly_3()
    {
        Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[2].ToString()) as Dictionary<string, object>;
        UIManager.Instance.payrollPanel.OpenPanel(tokenData["data"] as Dictionary<string, object>);
    }
    public void GetMonthly_4()
    {
        Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[3].ToString()) as Dictionary<string, object>;
        UIManager.Instance.incomePanel.InitState(4, tokenData);
    }
    public void GetMonthly_5()
    {
        Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[4].ToString()) as Dictionary<string, object>;
        UIManager.Instance.operatingPanel.OpenPanel(tokenData);
    }
    public void GetMonthly_6()
    {
        List<object> tokenData = Json.Deserialize(TxtFile[5].ToString()) as List<object>;
        UIManager.Instance.incomePanel.InitState(3,null,tokenData);
    }
    private IEnumerator RequestAddress()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get("http://api.map.baidu.com/location/ip?ak=bretF4dm6W5gqjQAXuvP0NXW6FeesRXb&coor=bd09ll");
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            //try
            {
                Debug.Log(webRequest.downloadHandler.text);
                Dictionary<string, object> tokenData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
                Dictionary<string, object> pairs = tokenData["content"] as Dictionary<string, object>;
                foreach (var item in pairs)
                {
                    Debug.Log(item.Key);
                    Debug.Log(item.Value);
                }
                Dictionary<string, object> info = pairs["address_detail"] as Dictionary<string, object>;
                foreach (var item in info)
                {
                    Debug.Log(item.Key);
                    Debug.Log(item.Value);
                }
            }
            //catch (System.Exception)
            //{
            //    Debug.Log("数据解析错误:");
            //}
        }
    }
}