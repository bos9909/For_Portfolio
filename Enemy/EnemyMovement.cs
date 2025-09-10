using System;
using UnityEngine;

    //적 데이터가 필요함
    [RequireComponent(typeof(EnemyStatus))]
public class EnemyMovement : MonoBehaviour
{
    [Header("이동 스테이터스")]
    public EnemyData _enemyData;
    public float moveSpeed;
    public float rotaitonSpeed;
    public float stopDistance;
    
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
        // 1. 목표 방향 계산
        // 대상의 위치에서 나의 위치를 빼서 방향 벡터를 구하고, 정규화(normalized)하여 길이를 1로 만듦
        Vector3 direction = (target.position - transform.position).normalized;

        // 2. 목표 방향으로 부드럽게 회전
        // 목표 방향을 바라보는 회전값(Quaternion)을 계산
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        // 현재 회전값에서 목표 회전값으로 부드럽게 회전 (Slerp 사용)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotaitonSpeed * Time.deltaTime);

        // 3. 기체의 앞 방향으로 이동
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
