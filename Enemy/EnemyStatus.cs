using System;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public EnemyData _enemyData;
    public int currentHp;

    //초기화
    private void Awake()
    {
        //데이터에서 체력을 가져옴
        currentHp = _enemyData.maxHp;
    }

    void Update()
    {
        
    }

    //사망시 호출될 함수 나중에 구현
    public void isDead()
    { 
        this.gameObject.SetActive(false);
    }
}
