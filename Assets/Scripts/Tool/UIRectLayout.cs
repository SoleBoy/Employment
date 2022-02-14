using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

/// <summary>

/// 自适应iPhoneX背景

/// </summary>

public class UIRectLayout : MonoBehaviour

{
#if UNITY_IPHONE
    void Awake()
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.Log("修改UI高度");
            RectTransform rect = transform.GetComponent<RectTransform>();
            Vector2 anPos = rect.anchoredPosition;
            anPos.y += 80;
            rect.offsetMin = anPos;
            //if (SystemInfo.deviceModel.Contains("iPhone10,3") || SystemInfo.deviceModel.Contains("iPhone10,6"))
            //{
            //    // 是iOS刘海屏
            //    isBangs = true;
            //}
        }
    }
#endif

}