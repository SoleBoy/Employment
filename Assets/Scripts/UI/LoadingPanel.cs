using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    private Transform loading;
    private void Awake()
    {
        loading = transform.Find("Loading");
    }

    private void Update()
    {
        loading.Rotate(Vector3.back * 25 * Time.deltaTime);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
