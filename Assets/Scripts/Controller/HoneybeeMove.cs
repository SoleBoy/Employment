using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class HoneybeeMove : MonoBehaviour, IPointerClickHandler
{
    private float height;
    private float speed;
    private void Awake()
    {
        height = (DataTool.canvasSize.y - 500);
    }

    public void SetMove()
    {
        speed = Random.Range(6,15);
        transform.DOScale(Vector3.one,1f);
        transform.DOLocalMoveY(-height, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            ObjectPool.Instance.CollectObject(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.battlePanel.AddScore(1);
        ObjectPool.Instance.CollectObject(gameObject);
    }

    private void OnDisable()
    {
        transform.DOPause();
    }
}
