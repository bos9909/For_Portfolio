using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class HPController : MonoBehaviour
{
    public MMProgressBar progressBar;
    public static HPController Instance;
    [Range(0f,100f)] public float value;
    [MMInspectorButton("ChangeHP")] public bool ChangeHPButton;
    private int targetId = 0;
    private int currentId = 0;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        if (!progressBar)
        {
            Debug.Log("프로그래스 바를 넣어!");
        }
        else
        {
            progressBar.Initialization();
        }
        
        
    }
    
    void Update()
    {
        
    }

    public void ControllHP(int value, int targetId)
    {
        if (currentId != targetId)
        {
            //처음에 체력바 가져왔을 때 초기화
            Debug.Log(currentId+ "원본" + targetId + "타겟");
            progressBar.SetBar(value, 0f, 100f);
        }
        else
        {
            //연속으로 맞으면 이펙트 실행
            Debug.Log(currentId+ "원본" + targetId + "타겟");
            progressBar.UpdateBar(value, 0f, 100f);
        }
        //비교후 갱신
        currentId = targetId;
    }
}
