using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarController : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;

    private void Awake()
    {
        if(loadingBar == null)
        {
            loadingBar = GameObject.Find("LoadingBar").GetComponent<Slider>();
        }
    }

    void Update()
    {
        if(loadingBar == null)
        {
            return;
        }

        loadingBar.value = LoadingManager.Instance.LoadingOperation.progress;
    }
}
