using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class DataTool
{
#if UNITY_IOS
	[DllImport ("__Internal")]
	private static extern void startActivity(int pageId);//登录注册流程传数据

    [DllImport("__Internal")]
    private static extern void getLocationInfo();//获取经纬度位置
#endif

    public static Dictionary<string, object> information = new Dictionary<string, object>();
    //public static Dictionary<string, object> employerInfo = new Dictionary<string, object>();

    public static RoleData roleData;
    public static float roleLevel;//等级
    public static float roleExp;//经验
    public static float roleExp_Max;//经验
    public static int roleRanking;//排名
    public static string roleType;//注册类型
    public static string roleTitle;//头衔

    public static string token = "";
    public static string roleName = "";//姓名
    public static string theCompany = "";//公司名字
    public static string bankName = "";//银行卡名
    public static string bankNo = "";//银行卡号
    public static string inviteCode = "";//邀请码
    public static string taskDuration = "0";//任务时长
    public static string clockInAddress = "";//打卡地址
    public static string latitude = "";//打卡经纬度
    public static string longitude = "";
    public static string businessPic = "";

    public static string workerInfo = "http://appapi.brilliantnetwork.cn:5002/workerapi/workers/getWorkerInfo";//个人用户信息
    //打卡记录
    public static string clockUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/lock";
    public static string pictureUrl = "http://appapi.brilliantnetwork.cn:5002/api/upload/uploadfile?path=daka&onlyLocal=0";

    //任务显示信息
    public static string currentTaskUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/getCurrentTask";//当前任务
    public static string typeTaskUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/getTaskTypeNames";//任务类型
    public static string placeTaskUrl = "http://appapi.brilliantnetwork.cn:5002/api/basicdata/getRegionByParentId?parentId=1";//行政区
    public static string receiverTaskUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/receiverTask";//接单
    public static string infoTaskUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/getTaskInfo?taskId=";//任务详情
    public static string seachTaskUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/getTaskSeach";//任务搜索
    public static string submitTaskUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/submitTask";//任务提交
    //
    //收入
    public static string totalSalaryUrl="http://appapi.brilliantnetwork.cn:5002/workerapi/salary/getTotalSalary";//总收入
    public static string salarySeachUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/salary/getSalarySeach";//收入列表
    public static string salaryDetailsUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/salary/getDetalSalary?id=1";//薪水详情

    //邀请码认证信息
    public static string invateCodeUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/workers/updateInvateCode";
    //雇主
    public static string businessUrl = "http://appapi.brilliantnetwork.cn:5002/companyapi/getLoginInfo";//雇主-获取登录用户信息
    public static string scanCodeUrl = "http://appapi.brilliantnetwork.cn:5002/companyapi/company/postScanCode";//扫码提交
    public static string invitationCode = "http://appapi.brilliantnetwork.cn:5002/companyapi/company/getInvitationCode";//获取邀请码

    public static int blindBox;//盲盒次数
    //public static bool isUnit;//个体 个人
    public static bool isClock;//打卡记录
    public static byte[] cheackByte;
    public static Texture checkTexture;
    public static string checkAddress = "";
    public static string currentTask = "";
    public static string filePath = Application.persistentDataPath + "/" + "ClockIn.png";
    public static SalaryEntry salaryEntry;
    public static Vector3 frontAngle = new Vector3(0, 180, 90);
    public static Vector3 rearAngle = new Vector3(0, 0, -90);


    public static Color color_review;
    public static Color color_start; 
    public static Color color_progress;
    public static Color color_submitted;
    public static Color color_accepted;
    public static Color color_issued;

    public static void InitData()
    {
        isClock = PlayerPrefs.GetString(System.DateTime.Now.Date.ToString() + "Clock") == "Clock";

        color_review = GetColor("2D56E9");//待审核 #D9680F
        color_start = GetColor("3CC83F");//待开始 #4CB006
        color_progress = GetColor("3CC83F");//进行中 #08AA5F
        color_submitted = GetColor("2ACCB1");//已提交 #A6A3A3
        color_accepted = GetColor("2ACCB1");//已验收 #1886F2
        color_issued = GetColor("2ACCB1");//已发放 #053EA4
    }
    private static Color GetColor(string color)
    {
        Color skyColor;
        ColorUtility.TryParseHtmlString("#" + color, out skyColor);
        return skyColor;
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
            intentObj.Call<AndroidJavaObject>("addCategory", "android.intent.category.LAUNCHER");//.addCategory(Intent.CATEGORY_LAUNCHER);

            intentObj.Call<AndroidJavaObject>("putExtra", "pageId", pageId);

            currentActivity.Call("startActivity", intentObj);
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            startActivity(pageId);
#endif
        }
    }
    //打卡地址获取
    public static void CallClockInfo()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("getLocationInfo");

            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.example.jinchang.utils.UnityReflection"))
            {
                if (pluginClass != null)
                {
                    pluginClass.CallStatic("getLocationInfo");
                }
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            getLocationInfo();
#endif
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
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            //requestUserInfo(mesgg);
#endif
        }
    }
   
    //获取字典
    public static Dictionary<string,object> GetDictionary(string activityInfo)
    {
        try
        {
            return Json.Deserialize(activityInfo) as Dictionary<string, object>;
        }
        catch (System.Exception)
        {
            return new Dictionary<string, object>();
        }
    }
    //获取list
    public static List<object> GetList(string activityInfo)
    {
        try
        {
            return Json.Deserialize(activityInfo) as List<object>;
        }
        catch (System.Exception)
        {
            return new List<object>();
        }
    }

}

public enum SalaryEntry
{
    dayknot_1,
    weeklyend_1,
    month_1,
    month_2,
    month_3,
    operating_1,
    operating_2,
    issued_1,
    business_1,
    submit,
    clock
}
