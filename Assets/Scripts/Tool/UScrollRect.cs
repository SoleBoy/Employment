using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UScrollRect : ScrollRect
{

    //高度 往下拉是负数   往上拉是正数
    float f = -180f;
    //是否刷新
    bool isRef = false;
    //是否处于拖动
    bool isDrag = false;
    //如果满足刷新条件 执行的方法
    public Action callback2;

    protected override void Awake()
    {
        base.Awake();
        onValueChanged.AddListener(ScrollValueChanged);
    }

    /// <summary>
    /// 当ScrollRect被拖动时
    /// </summary>
    /// <param name="vector">被拖动的距离与Content的大小比例</param>
    void ScrollValueChanged(Vector2 vector)
    {
        //如果不拖动 当然不执行之下的代码
        if (!isDrag)
            return;
        //这个就是Content
        RectTransform rect = GetComponentInChildren<ContentSizeFitter>().GetComponent<RectTransform>();
        //如果拖动的距离大于给定的值
        if (f > rect.rect.height * vector.y)
        {
            isRef = true;
            Debug.Log("OnBeginDrag执行");
        }
        else
        {
            isRef = false;
            Debug.Log("OnEndDrag执行");
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        isDrag = true;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        callback2.Invoke();
        isRef = false;
        isDrag = false;
    }
}