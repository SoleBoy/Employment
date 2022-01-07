using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    private Transform loading;

    private bool isClose;
    private float lastTime;
    private void Awake()
    {
        loading = transform.Find("Loading");
    }

    private void Update()
    {
        
        if(lastTime >= 0)
        {
            lastTime -= Time.deltaTime;
            if(isClose && lastTime < 0)
            {
                gameObject.SetActive(false);
            }
        }
       
        loading.Rotate(Vector3.back * 25 * Time.deltaTime);
    }

    public void OpenPanel()
    {
        isClose = false;
        lastTime = Random.Range(0.2f,0.5f);
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        if(lastTime > 0)
        {
            isClose = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
