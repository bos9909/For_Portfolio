using UnityEngine;

// EnemyData 스크립터블 오브젝트를 사용하고 있다는 가정
[RequireComponent(typeof(EnemyStatus))]
public class EnemyMovement : MonoBehaviour
{
    [Header("데이터")]
    [SerializeField] private EnemyData _enemyData; // 스탯을 가져올 데이터 파일

    [Header("타겟")]
    [SerializeField] private Transform target;

    // 내부 변수
    private float moveSpeed;
    private float rotationSpeed;
    private float stopDistance;
    private float curveFactor;
    private int orbitDirection = 1;

    private bool isStopped = false;

    private void Awake()
    {
        // EnemyData에서 스탯을 가져와 내부 변수에 저장 (이 부분은 그대로 유지)
        if (_enemyData != null)
        {
            moveSpeed = _enemyData.moveSpeed;
            rotationSpeed = _enemyData.rotaitonSpeed;
            stopDistance = _enemyData.stopDistance;
            curveFactor = _enemyData.curveFactor;
        }
    }
    /// <summary>
    /// EnemyBase가 호출할 메인 이동 함수.
    /// </summary>
    public void Move()
    {
        // 멈춤 상태이거나 타겟이 없으면 움직이지 않음
        if (isStopped || target == null)
        {
            return;
        }

        // 타겟과의 거리가 정지 거리보다 멀 때만 움직임
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > stopDistance)
        {
            MoveAndRotate();
        }
    }

    // 실제 이동 및 회전 로직
    private void MoveAndRotate()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 sidewayDirection = Vector3.Cross(directionToTarget, transform.up).normalized * orbitDirection;
        Vector3 finalDirection = (directionToTarget + sidewayDirection * curveFactor).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(finalDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.position += transform.forward * (moveSpeed * Time.deltaTime);
    }
    
    /// <summary>
    /// 외부에서 타겟을 설정해주는 함수.
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        orbitDirection = (Random.value > 0.5f) ? 1 : -1;
    }
    
    /// <summary>
    /// EnemyBase가 이동을 멈추라고 명령할 때 호출.
    /// </summary>
    public void Stop()
    {
        isStopped = true;
    }

    /// <summary>
    /// EnemyBase가 이동을 다시 시작하라고 명령할 때 호출.
    /// </summary>
    public void Resume()
    {
        isStopped = false;
    }
}