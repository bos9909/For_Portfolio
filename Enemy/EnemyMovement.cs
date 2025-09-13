using System;
using UnityEngine;
using Random = UnityEngine.Random;

//적 데이터가 필요함
    [RequireComponent(typeof(EnemyStatus))]
public class EnemyMovement : MonoBehaviour
{
    [Header("이동 스테이터스")]
    public EnemyData _enemyData;
    public float moveSpeed;
    public float rotaitonSpeed;
    public float stopDistance;
    
    [Header("Arcing Settings")]
    [Tooltip("호의 휘어지는 정도. 0이면 직선, 1에 가까울수록 크게 휩니다.")]
    [Range(0f, 1f)]
    [SerializeField] private float curveFactor = 0.5f;

    [Tooltip("선회 방향. 1이면 시계방향, -1이면 반시계방향 (위에서 봤을 때)")]
    [SerializeField] private int orbitDirection = 1;
    
    [Header("target")]
    [SerializeField] private Transform target;

    private bool isStopped;
    
    private void Awake()
    {
        //스테이터스를 할당
        moveSpeed = _enemyData.moveSpeed;
        rotaitonSpeed = _enemyData.rotaitonSpeed;
        stopDistance = _enemyData.stopDistance;
        SetTarget(target);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopped || target == null)
        {
            return;
        }
        
        // 대상과의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // 대상이 멈춰야 할 거리보다 멀리 있을 때만 이동 및 회전
        if (distanceToTarget > stopDistance)
        {
            MoveAndRotate();
        }
        
    }
    
    private void MoveAndRotate()
    {
        // 1. 플레이어를 향하는 직선 방향 (기존과 동일)
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        // 2. 옆으로 비껴가려는 방향 (새로운 핵심 로직)
        // '플레이어 방향'과 '기체 위쪽 방향'의 외적을 구해 '옆 방향'을 계산
        Vector3 sidewayDirection = Vector3.Cross(directionToTarget, transform.up).normalized * orbitDirection;

        // 3. 두 방향을 'curveFactor'에 따라 혼합하여 최종 목표 방향 설정
        // curveFactor가 0이면 sidewayDirection이 사라져 직선 운동이 됨
        // curveFactor가 1이면 directionToTarget과 sidewayDirection이 1:1로 섞여 45도 각도로 비껴감
        Vector3 finalDirection = (directionToTarget + sidewayDirection * curveFactor).normalized;

        // 4. 최종 목표 방향으로 부드럽게 회전 (기존과 동일)
        Quaternion targetRotation = Quaternion.LookRotation(finalDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotaitonSpeed * Time.deltaTime);

        // 5. 기체의 앞 방향으로 이동 (기존과 동일)
        transform.position += transform.forward * (moveSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        
        orbitDirection = (Random.value > 0.5f) ? 1 : -1;
    }
}
