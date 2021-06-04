using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
/// <summary>
/// 手机陀螺仪
/// </summary>
public class GyroData : MonoBehaviour
{
    public Transform textInput;
    //private bool draw = false;
    private bool gyinfo;
    private Gyroscope go;
    private FileInfo fileInfo;
    private float lastTime;
    private void Start()
    {
        gyinfo = SystemInfo.supportsGyroscope;
        go = Input.gyro;
        go.enabled = true;
        
        
    }
    private void Update()
    {
        //if (gyinfo)
        //{
        //    Vector3 a = go.attitude.eulerAngles;
        //    a = new Vector3(-a.x, -a.y, a.z); //直接使用读取的欧拉角发现不对，于是自己调整一下符号
        //    this.transform.eulerAngles = a;
        //    this.transform.Rotate(Vector3.right * 90, Space.World);
        //    textInput.text = a.ToString();
        //    draw = false;
        //}
        //else
        //{
        //    draw = true;
        //}
        if(Input.GetMouseButtonDown(0))
        {
            CreateFile();
        }
        if (gyinfo)
        {
            lastTime += Time.deltaTime;
            if (lastTime >= 0.1f)
            {
                lastTime = 0;
                StreamWriter sw = fileInfo.AppendText();
                //以行的形式写入信息
                sw.WriteLine(textInput.position);
                //关闭流
                sw.Close();
                //销毁流
                sw.Dispose();
            }
        }
    }

    private void CreateFile()
    {
        if(gyinfo)
        {
            gyinfo = false;
        }
        else
        {
            gyinfo = true;
            string messg = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour
             + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second;
            fileInfo = new FileInfo(Application.persistentDataPath + "/Log" + messg + ".txt");
            Debug.Log(Application.persistentDataPath + "/Log" + messg + ".txt");
            if (!fileInfo.Exists)
            {
                //如果此文件不存在则创建
                StreamWriter sw = fileInfo.CreateText();
                //关闭流
                sw.Close();
                //销毁流
                sw.Dispose();
            }
            Debug.Log("kaishiluzhi");
        }
    }
}