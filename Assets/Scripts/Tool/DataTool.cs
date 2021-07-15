using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTool
{
    public static RoleData roleData;
    public static float roleLevel;//等级
    public static float roleExp;//经验
    public static float roleExp_Max;//经验
    public static int roleRanking;//排名
    public static string roleTitle;//头衔
    public static string roleName;//姓名

    public static int blindBox;//盲盒次数
    public static bool isUnit;//个体 个人
    public static bool isClock;//打卡记录
    public static byte[] cheackByte;
    public static SalaryEntry salaryEntry;
    //获取头衔等级
    public static string GetTitle(int rankIndex)
    {
        if (rankIndex == 1)
        {
            return "王者";
        }
        else if (rankIndex == 2)
        {
            return "蓝钻";
        }
        else if (rankIndex == 3)
        {
            return "红钻";
        }
        else if (rankIndex >= 4 && rankIndex <= 10)
        {
            return "黄钻";
        }
        else if (rankIndex >= 11 && rankIndex <= 100)
        {
            return "绿钻";
        }
        else if (rankIndex >= 101 && rankIndex <= 1000)
        {
            return "白金";
        }
        else if (rankIndex >= 1001 && rankIndex <= 10000)
        {
            return "黄金";
        }
        else if (rankIndex >= 10001 && rankIndex <= 100000)
        {
            return "白银";
        }
        else if (rankIndex >= 100001 && rankIndex <= 1000000)
        {
            return "钢铁";
        }
        else if (rankIndex >= 1000001 && rankIndex <= 10000000)
        {
            return "青铜";
        }
        else
        {
            return "黄铜";
        }
    }
    //随机姓名
    private static string[] ranName = { "澄", "阳", "邈", "海", "朗", "鸿", "高", "旻", "曦", "哲", "景", "彰" };
    public static string GetName()
    {
        return ranName[Random.Range(0, ranName.Length)] + ranName[Random.Range(0, ranName.Length)];
    }
    //各类表数据
    public static Dictionary<string, RoleData> roleDatas = new Dictionary<string, RoleData>();
    public static Dictionary<string, TypeData> typeDatas = new Dictionary<string, TypeData>();
    public static Dictionary<string, TalentData> talentDatas = new Dictionary<string, TalentData>();
    public static void InitData(string messgName)
    {
        PackageRole role = Resources.Load<PackageRole>("DataAssets/roledata");
        roleDatas = role.GetItems();
        PackageType type = Resources.Load<PackageType>("DataAssets/typedata");
        typeDatas = type.GetItems();
        PackageTalent talent = Resources.Load<PackageTalent>("DataAssets/talentdata");
        talentDatas = talent.GetItems();

        roleName = messgName;
        roleData = roleDatas["1001"];
        //数据
        isClock = PlayerPrefs.GetString(System.DateTime.Now.Date.ToString()+ "Clock") == "Clock";
        blindBox = PlayerPrefs.GetInt("CurretBlindBox", 5);
        roleRanking = Random.Range(800,2000);
        roleTitle = GetTitle(roleRanking);
        roleLevel = PlayerPrefs.GetFloat("CurretLevel",1);
        roleExp = PlayerPrefs.GetFloat("CurretExp");
        roleExp_Max = float.Parse(roleData.exp) + float.Parse(roleData.exp_upgrade) * (roleLevel-1);
    }
    //加经验值
    public static bool AddExperience(float exp)
    {
        roleExp += exp;
        if(roleExp >= roleExp_Max)
        {
            roleLevel += 1;
            roleExp = roleExp - roleExp_Max;
            roleExp_Max = float.Parse(roleData.exp) + float.Parse(roleData.exp_upgrade) * (roleLevel - 1);
            PlayerPrefs.SetFloat("CurretLevel", roleLevel);
            return true;
        }
        PlayerPrefs.SetFloat("CurretExp",roleExp);
        return false;
    }
    //Open安卓
    public static void StartActivity(int pageId)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            var unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            var intentObj = new AndroidJavaObject("android.content.Intent");
            var activityCls = new AndroidJavaClass("com.example.jinchang.RegActivity");
            intentObj.Call<AndroidJavaObject>("setClass", currentActivity, activityCls);

            intentObj.Call<AndroidJavaObject>("putExtra", "pageId", pageId);

            currentActivity.Call("startActivity", intentObj);
        }
    }
    // 172 173 174  185 已发放  ,187 经营所得    ,190 经营所得二级目录-加月份
    public static void CallNative(int mesgg,int id,string month = "")
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("RequestUserInfo");

            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.example.jinchang.utils.UnityReflection"))
            {
                if (pluginClass != null)
                {
                    Debug.Log("mesgg:" + mesgg + "id:" + id + "month:" + month);
                    pluginClass.CallStatic("RequestUserInfo", mesgg,id, month);
                }
            }

        }
    }
    //打卡记录上传
    public static void CallClockInfo(double lat, double lng)
    {
        //Debug.Log(System.Convert.ToBase64String(cheackByte));
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("uploadingSignInInfo");

            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.example.jinchang.utils.UnityReflection"))
            {
                if (pluginClass != null)
                {
                    Debug.Log("lat:"+lat+"lng:"+lng);
                    pluginClass.CallStatic("uploadingSignInInfo", lat, lng,System.Convert.ToBase64String(cheackByte));
                }
            }

        }
    }

    //安卓JSON
    public static void JsonStrForUnity(string activityInfo)
    {
        UIManager.Instance.CloningTips(activityInfo);
    }
}

public enum SalaryEntry
{
    month_1,
    month_2,
    month_3,
    Operating_1,
    Operating_2,
    Issued_1,
    business_1
}
