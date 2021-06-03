using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTool
{
    public static float roleLevel;//等级
    public static float roleExp;//经验

    public static bool isUnit;//个体 个人

    public static string[] certdata= {"输入邀请码","实名认证","银行卡绑定","签名认证","","" };//个体 个人

    private static string[] ranName = { "澄", "阳", "邈", "海", "朗", "鸿", "高", "旻", "曦", "哲", "景", "彰" };

    public static string GetName()
    {
        return ranName[Random.Range(0, ranName.Length)] + ranName[Random.Range(0, ranName.Length)];
    }

    //Open安卓
    public static void StartActivity(string activityName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            var unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            var intentObj = new AndroidJavaObject("android.content.Intent");
            var activityCls = new AndroidJavaClass(activityName);
            intentObj.Call<AndroidJavaObject>("setClass", currentActivity, activityCls);

            currentActivity.Call("startActivity", intentObj);
        }
    }

    //安卓JSON
    public static void JsonStrForUnity(string activityInfo)
    {
        UIManager.Instance.CloningTips(activityInfo);
    }
}
