using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Vector3 nowPos;
    private Vector3 oldPos;
    private bool isClick = false;
    public float length = 1;
    private void OnMouseUp()
    {
        isClick = false;
    }
    private void OnMouseDown()
    {
        isClick = true;
    }
    private void Update()
    {
        if (isClick == true)
        {
            nowPos = Input.mousePosition;
            Vector3 offset = nowPos - oldPos;
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y) && Mathf.Abs(offset.x) > length)
            {
                transform.Rotate(Vector3.up, -offset.x);
            }
            oldPos = Input.mousePosition;
        }
    }
}
