// PlayerStatus.cs (하트 시스템 버전)
using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("플레이어 체력")]
    [SerializeField] private int maxLives = 5; // 최대 목숨(하트) 개수

    public int CurrentLives { get; private set; }
    public int MaxLives { get; private set; }

    public event Action<int> OnLivesChanged; // 현재 목숨 개수
    public event Action OnDeath;

    private void Awake()
    {
        MaxLives = maxLives;
        CurrentLives = MaxLives;
    }

    private void Start()
    {
        OnLivesChanged?.Invoke(CurrentLives);
    }

    /// <summary>
    /// 데미지 양과 상관없이 호출될 때마다 목숨을 1씩 깎임.
    /// </summary>
    public void TakeHit()
    {
        if (CurrentLives <= 0) return;

        CurrentLives--;
        OnLivesChanged?.Invoke(CurrentLives);

        if (CurrentLives <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}