using UnityEngine;
using System.Collections;
using MiniJSON;
using System.Collections.Generic;
using UnityEngine.Networking;

public class LoadTxt : MonoBehaviour
{

    public TextAsset[] TxtFile;    //建立TextAsset
    private string Mytxt;          //用来存放文本内容

    private void Start()
    {
        //GetMonthly_1();
        //GetMonthly_2();
        //GetMonthly_3();
    }

    //total字段累计收入
    public void GetMonthly_1()
    {
        //Debug.Log(TxtFile.ToString());
        Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[0].ToString()) as Dictionary<string, object>;
        //foreach (var item in tokenData)
        //{
        //    Debug.Log(item.Key);
        //    Debug.Log(item.Value);
        //}
        //List<object> data1 = tokenData["data"] as List<object>;
        //for (int i = 0; i < data1.Count; i++)
        //{
        //    Dictionary<string, object> data2 = data1[i] as Dictionary<string, object>;
        //    foreach (var item in data2)
        //    {
        //        Debug.Log(item.Key);
        //        Debug.Log(item.Value);
        //    }
        //}
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
        //foreach (var item in tokenData["data"] as Dictionary<string, object>)
        //{
        //    Debug.Log(item.Key);
        //    Debug.Log(item.Value);
        //}
        UIManager.Instance.payrollPanel.OpenPanel(tokenData["data"] as Dictionary<string, object>);
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