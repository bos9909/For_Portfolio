using System;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [Header("데이터")]
    [SerializeField] private EnemyData _enemyData;

    public int CurrentHp { get; private set; }
    public int MaxHp { get; private set; }

    // 이벤트 선언
    public event Action OnDeath;
    
    // int: 현재 체력, int: 최대 체력을 함께 전달하는 이벤트
    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        if (_enemyData != null)
        {
            MaxHp = _enemyData.maxHp;
            CurrentHp = MaxHp;
        }
    }
    
    // 게임 시작 시 UI가 초기값을 설정할 수 있도록 이벤트를 한 번 호출해 줌
    private void Start()
    {
        OnHealthChanged?.Invoke(CurrentHp, MaxHp);
    }

    public void TakeDamage(int damage)
    {

        CurrentHp -= damage;
        
        //체력이 변경될 때 마다 이 이벤트를 구독하고 있는 스크립트에 알림
        OnHealthChanged?.Invoke(CurrentHp, MaxHp);
        
        Debug.Log(gameObject.name + " 체력: " + CurrentHp + "/" + MaxHp);

        //사망 처리
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            Die();
        }
    }

    //사망 함수, 나중에 에너미 베이스 같은 곳에서 구독하여 관리
    private void Die()
    {
        OnDeath?.Invoke();
    }
}