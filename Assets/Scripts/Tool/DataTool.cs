using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class DataTool
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void startActivity(string messg);//登录注册流程传数据

    [DllImport("__Internal")]
    private static extern void getLocationInfo();//获取经纬度位置

    [DllImport("__Internal")]
    private static extern void updateBankcard();//更新银行卡
    
#endif

    public static Dictionary<string, object> information = new Dictionary<string, object>();
    //注销账号
    public static string cancel;
    //更新验证码
    public static string sendsms;
    public static string updateMobile;//更新手机号
    public static string workerInfo;//个人用户信息
    public static string verifyCode;//验证验证码
    //打卡记录
    public static string clockUrl;
    public static string pictureUrl;

    //任务显示信息
    public static string currentTaskUrl;//当前任务
    public static string typeTaskUrl;//任务类型
    public static string placeTaskUrl;//行政区
    public static string receiverTaskUrl;//接单
    public static string infoTaskUrl;//任务详情
    public static string seachTaskUrl;//任务搜索
    public static string submitTaskUrl;//任务提交
    //收入
    public static string totalSalaryUrl;//总收入
    public static string salarySeachUrl;//收入列表
    public static string salaryDetailsUrl;//薪水详情

    //邀请码认证信息
    public static string invateCodeUrl;
    public static string urlCode;
    //雇主
    public static string businessUrl;//雇主-获取登录用户信息
    public static string scanCodeUrl;//扫码提交
    public static string invitationCode ;//获取邀请码

    public static string token = "";
    public static string roleType = "";//注册类型
    public static string roleName = "";//姓名
    public static string theCompany = "";//公司名字
    public static string bankName = "";//银行卡名
    public static string bankNo = "";//银行卡号
    public static string inviteCode = "";//邀请码
    public static string taskDuration = "0";//任务时长
    public static string clockInAddress = "";//打卡地址
    public static string latitude = "";//打卡经纬度
    public static string longitude = "";//打卡经纬度
    public static string businessPic = "";//营业执照
    public static string bankCardPic = "";//银行卡照片
    public static string loginPhone = "";//用户是手机号
    public static string signaturePic = "";//签名
    public static string checkAddress = "";//打卡地址
    public static string currentTask = "";//当前任务
    public static string filePath = Application.persistentDataPath + "/" + "ClockIn.png";
    public static bool isClock;//打卡记录
    public static bool isDegree;//注册完成度
    public static byte[] cheackByte;
    public static Texture checkTexture;
    public static Vector2 canvasSize;
    public static Vector3 frontAngle;
    public static Vector3 rearAngle;
    public static SalaryEntry salaryEntry;

    public static Color color_review;
    public static Color color_start; 
    public static Color color_progress;
    public static Color color_submitted;
    public static Color color_accepted;
    public static Color color_issued;

    public static void InitData()
    {
        SwitchUrl(true);
        isClock = PlayerPrefs.GetString(System.DateTime.Now.Date.ToString() + "Clock") == "Clock";

        color_review = GetColor("2D56E9");//待审核 #D9680F
        color_start = GetColor("3CC83F");//待开始 #4CB006
        color_progress = GetColor("3CC83F");//进行中 #08AA5F
        color_submitted = GetColor("2ACCB1");//已提交 #A6A3A3
        color_accepted = GetColor("2ACCB1");//已验收 #1886F2
        color_issued = GetColor("2ACCB1");//已发放 #053EA4

        if (Application.platform == RuntimePlatform.Android)
        {
            frontAngle = new Vector3(0, 180, 90);
            rearAngle = new Vector3(0, 0, -90);

            AndroidStatusBar.statusBarState = AndroidStatusBar.States.Visible;//显示状态栏，占用屏幕最上方的一部分像素
            //AndroidStatusBar.statusBarState = AndroidStatusBar.States.VisibleOverContent;//悬浮显示状态栏，不占用屏幕像素
            //AndroidStatusBar.statusBarState = AndroidStatusBar.States.TranslucentOverContent;//透明悬浮显示状态栏，不占用屏幕像素
            //AndroidStatusBar.statusBarState = AndroidStatusBar.States.Hidden;//隐藏状态栏
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            frontAngle = new Vector3(180, 180, 90);
            rearAngle = new Vector3(0, 0, -90);
        }
        else
        {
            frontAngle = new Vector3(180, 180, 90);
            rearAngle = new Vector3(0, 0, -90);
        }
    }

    //测试: appapi.brilliantnetwork 正式: salaryapi.beeai.work
    private static void SwitchUrl(bool isUrl)
    {
        if(isUrl)
        {
            //注销账号
            cancel = "http://salaryapi.beeai.work/workerapi/workers/cancel";
            //更新验证码
            sendsms = "http://salaryapi.beeai.work/api/sms/sendsms?mobile=";
            updateMobile = "http://salaryapi.beeai.work/workerapi/workers/updateMobile";//更新手机号
            workerInfo = "http://salaryapi.beeai.work/workerapi/workers/getWorkerInfo";//个人用户信息
            verifyCode = "http://salaryapi.beeai.work/api/sms/verifyCode?smstoken={0}&code={1}&mobile={2}";
           
            //打卡记录
            clockUrl = "http://salaryapi.beeai.work/workerapi/task/lock";
            pictureUrl = "http://salaryapi.beeai.work/api/upload/uploadfile?path=daka&onlyLocal=0";

            //任务显示信息
            currentTaskUrl = "http://salaryapi.beeai.work/workerapi/task/getCurrentTask";//当前任务
            typeTaskUrl = "http://salaryapi.beeai.work/workerapi/task/getTaskTypeNames";//任务类型
            placeTaskUrl = "http://salaryapi.beeai.work/api/basicdata/getRegionByParentId?parentId=1";//行政区
            receiverTaskUrl = "http://salaryapi.beeai.work/workerapi/task/receiverTask";//接单
            infoTaskUrl = "http://salaryapi.beeai.work/workerapi/task/getTaskInfo?taskId=";//任务详情
            seachTaskUrl = "http://salaryapi.beeai.work/workerapi/task/getTaskSeach";//任务搜索
            submitTaskUrl = "http://salaryapi.beeai.work/workerapi/task/submitTask";//任务提交
            //收入
            totalSalaryUrl = "http://salaryapi.beeai.work/workerapi/salary/getTotalSalary";//总收入
            salarySeachUrl = "http://salaryapi.beeai.work/workerapi/salary/getSalarySeach";//收入列表
            salaryDetailsUrl = "http://salaryapi.beeai.work/workerapi/salary/getDetalSalary?id=";//薪水详情

            //邀请码认证信息
            invateCodeUrl = "http://salaryapi.beeai.work/workerapi/workers/updateInvateCode";
            urlCode = "http://salaryapi.beeai.work/companyapi/company/getCompanyInfoByInvateCode?invateCode=";
            //雇主
            businessUrl = "http://salaryapi.beeai.work/companyapi/getLoginInfo";//雇主-获取登录用户信息
            scanCodeUrl = "http://salaryapi.beeai.work/companyapi/company/postScanCode";//扫码提交
            invitationCode = "http://salaryapi.beeai.work/companyapi/company/getInvitationCode";//获取邀请码
        }
        else
        {
            //注销账号
            cancel = "http://appapi.brilliantnetwork.cn:5002/workerapi/workers/cancel";
            //更新验证码
            sendsms = "http://appapi.brilliantnetwork.cn:5002/api/sms/sendsms?mobile=";
            updateMobile = "http://appapi.brilliantnetwork.cn:5002/workerapi/workers/updateMobile";//更新手机号
            workerInfo = "http://appapi.brilliantnetwork.cn:5002/workerapi/workers/getWorkerInfo";//个人用户信息
            verifyCode = "http://appapi.brilliantnetwork.cn:5002/api/sms/verifyCode?smstoken={0}&code={1}&mobile={2}";
            urlCode = "http://appapi.brilliantnetwork.cn:5002/companyapi/company/getCompanyInfoByInvateCode?invateCode=";
            //打卡记录
            clockUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/lock";
            pictureUrl = "http://appapi.brilliantnetwork.cn:5002/api/upload/uploadfile?path=daka&onlyLocal=0";

            //任务显示信息
            currentTaskUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/getCurrentTask";//当前任务
            typeTaskUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/getTaskTypeNames";//任务类型
            placeTaskUrl = "http://appapi.brilliantnetwork.cn:5002/api/basicdata/getRegionByParentId?parentId=1";//行政区
            receiverTaskUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/receiverTask";//接单
            infoTaskUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/getTaskInfo?taskId=";//任务详情
            seachTaskUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/getTaskSeach";//任务搜索
            submitTaskUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/task/submitTask";//任务提交
            //收入
            totalSalaryUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/salary/getTotalSalary";//总收入
            salarySeachUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/salary/getSalarySeach";//收入列表
            salaryDetailsUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/salary/getDetalSalary?id=";//薪水详情

            //邀请码认证信息
            invateCodeUrl = "http://appapi.brilliantnetwork.cn:5002/workerapi/workers/updateInvateCode";
            //雇主
            businessUrl = "http://appapi.brilliantnetwork.cn:5002/companyapi/getLoginInfo";//雇主-获取登录用户信息
            scanCodeUrl = "http://appapi.brilliantnetwork.cn:5002/companyapi/company/postScanCode";//扫码提交
            invitationCode = "http://appapi.brilliantnetwork.cn:5002/companyapi/company/getInvitationCode";//获取邀请码
        }
    }



    private static Color GetColor(string color)
    {
        Color skyColor;
        ColorUtility.TryParseHtmlString("#" + color, out skyColor);
        return skyColor;
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
            startActivity(pageId.ToString());
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
    //更新银行卡
    public static void CallBankcard()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("updateBankcard");

            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.example.jinchang.utils.UnityReflection"))
            {
                if (pluginClass != null)
                {
                    pluginClass.CallStatic("updateBankcard");
                }
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            updateBankcard();
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
    clock,
    bankcard
}



