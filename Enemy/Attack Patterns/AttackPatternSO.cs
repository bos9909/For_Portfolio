// AttackPatternSO.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyAttackPattern", menuName = "Enemy Attack Patterns/New Attack Pattern")]
public class AttackPatternSO : ScriptableObject
{
    [Header("공격 타이밍")]
    [Tooltip("공격과 공격 사이의 최소 시간 간격 (초)")]
    public float attackCooldown = 2f;
    [Tooltip("한 번의 공격에 몇 발을 쏠 것인가?")]
    public int burstCount = 1;
    [Tooltip("연사 시 각 총알 사이의 시간 간격")]
    public float timeBetweenBursts = 0.2f;

    [Header("사거리 및 정확도")]
    public float attackRange = 30f; // 이 거리 안에 들어오면 공격 시작
    [Tooltip("총알이 퍼지는 각도. 0이면 정확하게, 값이 클수록 샷건처럼 퍼짐")]
    [Range(0f, 15f)]
    public float spreadAngle = 5f;

    [Header("발사체 설정")]
    public string projectilePoolKey = "EnemyBullet"; // PoolManager에서 사용할 총알의 키 (이름)
    public float projectileSpeed = 40f; // 총알의 기본 속도 (Bullet.cs의 speed와 다를 수 있음)
    public int projectileDamage = 1;    // 총알의 데미지 (하트 시스템에서는 1 고정)
}