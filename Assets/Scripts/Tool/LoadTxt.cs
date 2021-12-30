using UnityEngine;
using System.Collections;
using MiniJSON;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;

public class LoadTxt : MonoBehaviour
{
    public RawImage Image_sprite;
    public TextAsset[] TxtFile;    //建立TextAsset
    //07-05  16:53  河南省郑州市管城回族区城东路街道城东路98号正商向阳广场
    private void Start()
    {
        //StartCoroutine(RequestAddress("http://appapi.brilliantnetwork.cn:5002/api/basicdata/getAddressFromGeo?lgn=121.459&lat=31.187117"));
        //UIManager.Instance.AcceptData_Android(TxtFile[9].ToString());
        //TaskTest(TxtFile[10].ToString());
#if UNITY_EDITOR || UNITY_IOS
        UIManager.Instance.AcceptData_Android(TxtFile[9].ToString());
#endif
    }

    public void TaskTest(string messg)
    {
        Debug.Log("当前任务信息" + messg);
        Dictionary<string, object> taskData = Json.Deserialize(messg) as Dictionary<string, object>;
        if (taskData["data"].ToString() != "")
        {
            Dictionary<string, object>  taskInfo = taskData["data"] as Dictionary<string, object>;
            DataTool.taskDuration = taskInfo["currentMonthTime"].ToString();
            foreach (var item in taskInfo)
            {
                Debug.Log(item.Key + ":" + item.Value);
            }
        }
        else
        {
            Debug.Log("无当前任务");
        }
    }

    ////total字段累计收入
    //public void GetMonthly_1()
    //{
    //    Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[0].ToString()) as Dictionary<string, object>;
    //    //UIManager.Instance.incomePanel.InitState(2,tokenData);
    //}
    //public void GetMonthly_2()
    //{
    //    Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[1].ToString()) as Dictionary<string, object>;
    //    UIManager.Instance.salaryPanel.OpenPanel(tokenData["data"] as Dictionary<string, object>);
    //}
    //public void GetMonthly_3()
    //{
    //    Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[2].ToString()) as Dictionary<string, object>;
    //    UIManager.Instance.payrollPanel.OpenPanel(tokenData["data"] as Dictionary<string, object>);
    //}
    //public void GetMonthly_4()
    //{
    //    Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[3].ToString()) as Dictionary<string, object>;
    //    //UIManager.Instance.incomePanel.InitState(4, tokenData);
    //}
    //public void GetMonthly_5()
    //{
    //    Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[4].ToString()) as Dictionary<string, object>;
    //    UIManager.Instance.operatingPanel.OpenPanel(tokenData);
    //}
    //public void GetMonthly_6()
    //{
    //    List<object> tokenData = Json.Deserialize(TxtFile[5].ToString()) as List<object>;
    //    //UIManager.Instance.incomePanel.InitState(3,null,tokenData);
    //}
    //public void GetMonthly_7()
    //{
    //    Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[6].ToString()) as Dictionary<string, object>;
    //    //UIManager.Instance.businessPanel.OpenPanel(tokenData);
    //    //StartCoroutine(RequestAddress(tokenData["url"].ToString()));
    //    //Application.OpenURL(tokenData["url"].ToString());
    //}
    //public void GetMonthly_8()
    //{
    //    Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[7].ToString()) as Dictionary<string, object>;
    //    //UIManager.Instance.incomePanel.InitState(0,tokenData);
    //    //StartCoroutine(RequestAddress(tokenData["url"].ToString()));
    //    //Application.OpenURL(tokenData["url"].ToString());
    //}
    //public void GetMonthly_9()
    //{
    //    Dictionary<string, object> tokenData = Json.Deserialize(TxtFile[8].ToString()) as Dictionary<string, object>;
    //    //UIManager.Instance.incomePanel.InitState(1, tokenData);
    //    //StartCoroutine(RequestAddress(tokenData["url"].ToString()));
    //    //Application.OpenURL(tokenData["url"].ToString());
    //}

    private IEnumerator RequestAddress()
    {
        WWWForm form = new WWWForm();
        form.AddField("appToken", "3e5e876-4c81-48ff-b6f9-86b908eQ2df");
        form.AddField("mobile", "19145724473");
        form.AddField("qncode", "Cc5695a7b32a4a3a91c6989c4aede073");
        UnityWebRequest webRequest = UnityWebRequest.Post("http://ad.lianginfo.com/api/submitqrcode", form);
        DownloadHandlerTexture tx = new DownloadHandlerTexture();
        webRequest.downloadHandler = tx;
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
            }
        }
    }

    //"http://api.map.baidu.com/location/ip?ak=bretF4dm6W5gqjQAXuvP0NXW6FeesRXb&coor=bd09ll"
    private IEnumerator RequestAddress(string url)
    {
        //http://appapi.brilliantnetwork.cn:5002/workerapi/task/getTaskTypeNames
        //WWWForm form = new WWWForm();
        //form.AddField("token", "eyJ1c2VyTmFtZSI6IjEyMyIsImFsZyI6IkhTMjU2In0.eyJqdGkiOiJmY29pbmp3dCIsImlhdCI6MTYzOTkwODQyMSwic3ViIjoie1widXNlcklkXCI6OSxcInVzZXJUeXBlXCI6MH0iLCJleHAiOjE2NDI1MDA0MjF9.OcSge3V-mekLuWtnl_imZQV5-H9C7fH4vgBQjkynohM");
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        //webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.Default.GetBytes("eyJ1c2VyTmFtZSI6IjEyMyIsImFsZyI6IkhTMjU2In0.eyJqdGkiOiJmY29pbmp3dCIsImlhdCI6MTYzOTkxNzc0NCwic3ViIjoie1widXNlcklkXCI6MTQsXCJ1c2VyVHlwZVwiOjB9IiwiZXhwIjoxNjQyNTA5NzQ0fQ.FMyYlG_WUbOV459-dx15VnuR2JVW0g4duGR9m6Qv5Ao"));
        webRequest.SetRequestHeader("Authorization", "eyJ1c2VyTmFtZSI6IjEyMyIsImFsZyI6IkhTMjU2In0.eyJqdGkiOiJmY29pbmp3dCIsImlhdCI6MTYzOTkxNzc0NCwic3ViIjoie1widXNlcklkXCI6MTQsXCJ1c2VyVHlwZVwiOjB9IiwiZXhwIjoxNjQyNTA5NzQ0fQ.FMyYlG_WUbOV459-dx15VnuR2JVW0g4duGR9m6Qv5Ao");
        //webRequest.downloadHandler = new DownloadHandlerBuffer();
        //DownloadHandlerTexture tx = new DownloadHandlerTexture();
        //webRequest.downloadHandler = tx;
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
                //byte[] data = webRequest.downloadHandler.data;
                //Texture2D tex = new Texture2D(100, 100);
                //tex.LoadImage(data);

                //Image_sprite.texture = tex;

                //Dictionary<string, object> tokenData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
                //Dictionary<string, object> pairs = tokenData["content"] as Dictionary<string, object>;
                //foreach (var item in pairs)
                //{
                //    Debug.Log(item.Key);
                //    Debug.Log(item.Value);
                //}
                //Dictionary<string, object> info = pairs["address_detail"] as Dictionary<string, object>;
                //foreach (var item in info)
                //{
                //    Debug.Log(item.Key);
                //    Debug.Log(item.Value);
                //}
            }
            //catch (System.Exception)
            //{
            //    Debug.Log("数据解析错误:");
            //}
        }
    }


    //private void ReadPDFImage()
    //{
    //    string path = Application.streamingAssetsPath + "/aa.pdf";
    //    ExtractImageEvent(path);
    //}

    //private void ExtractImageEvent(string padPath)
    //{
    //    try
    //    {
    //        int index = 0;
    //        PdfReader pdfReader = new PdfReader(padPath);
    //        Debug.Log(pdfReader.NumberOfPages);
    //        for (int pageNumber = 1; pageNumber <= pdfReader.NumberOfPages; pageNumber++)
    //        {
    //            PdfReader pdf = new PdfReader(padPath);
    //            PdfDictionary pg = pdf.GetPageN(pageNumber);
    //            PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
    //            PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
    //            try
    //            {
    //                foreach (PdfName name in xobj.Keys)
    //                {
    //                    PdfObject obj = xobj.Get(name);
    //                    if (obj.IsIndirect())
    //                    {
    //                        PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
    //                        string width = tg.Get(PdfName.WIDTH).ToString();
    //                        string height = tg.Get(PdfName.HEIGHT).ToString();
    //                        //ImageRenderInfo imgRI = ImageRenderInfo.CreateForXObject((GraphicsState)new Matrix(float.Parse(width), float.Parse(height)), (PRIndirectReference)obj, tg);
    //                        ImageRenderInfo imgRI = ImageRenderInfo.CreateForXObject(new GraphicsState(), (PRIndirectReference)obj, tg);
    //                        RenderImageByte(imgRI, index);

    //                    }
    //                }
    //            }
    //            catch
    //            { continue; }
    //        }
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    //private void RenderImageByte(ImageRenderInfo renderInfo, int index)
    //{
    //    PdfImageObject imageObj = renderInfo.GetImage();
    //    pimage = imageObj.GetDrawingImage();
    //    MemoryStream ms = new MemoryStream();
    //    pimage.Save(ms, ImageFormat.Png);
    //    byte[] byteData = new Byte[ms.Length];
    //    ms.Position = 0;
    //    ms.Read(byteData, 0, byteData.Length);
    //    ms.Close();
    //    Texture2D tex2d = new Texture2D(500, 1000);
    //    if (tex2d.LoadImage(byteData))
    //    {
    //        UIimage.texture = tex2d;
    //    }

    //    ///保存到本地
    //    //Bitmap dd = new Bitmap(pimage);
    //    //dd.Save(Application.dataPath + "/Resources/wode.Jpeg");

    //}
}