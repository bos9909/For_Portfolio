using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class HPController : MonoBehaviour
{
    public MMProgressBar _progressBar;
    public MMF_Player OnDecreaseStart;
    public MMF_Player OnIncreaseStart;

    private void Awake()
    {
        if (!_progressBar)
        {
            Debug.Log("프로그래스 바를 넣어!");
        }
        else
        {
            _progressBar.Initialization();
        }
    }
    
    void Update()
    {
        
    }

    void DecreaseHP()
    {
        OnDecreaseStart.PlayFeedbacks();
    }
    
    void IncreaseHP()
    {
        OnIncreaseStart.PlayFeedbacks();
    }
}
