using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TipPanel : MonoBehaviour
{
    private Text messgText;
    private float max_hight; 
    private void Awake()
    {
        messgText = transform.Find("Text").GetComponent<Text>();
        max_hight = Screen.height * 0.7f;
    }

    public void StartAnimal(string messg)
    {
        messgText.text = messg;
        transform.DOLocalMoveY(815,1.5f);
        StartCoroutine(HideAnimal());
    }
    private IEnumerator HideAnimal()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        transform.DOPause();
    }
}
