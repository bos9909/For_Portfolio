using System;
using UnityEngine;

public class EnemyHPUI : MonoBehaviour
{
    public EnemyStatus _enemyStatus;

    //적 스테이터스라는 스크립트가 없으면 의미가없음
    private void Awake()
    {
        if (!_enemyStatus)
        {
            Debug.Log("스테이터스를 넣어줘!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    //플레이어 총알에 피격시 호출됨 
    private void OnCollisionEnter(Collision other)
    {
        _enemyStatus.currentHp--;
        //9: 플레이어 총알
        if (other.gameObject.layer == 9)
        {
            HPController.Instance.ControllHP(_enemyStatus.currentHp,this.gameObject.GetInstanceID());
        }
    }
    
}
