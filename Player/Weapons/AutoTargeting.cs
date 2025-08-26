using UnityEngine;

public class RayToSphereScanning : MonoBehaviour
{
    public Camera mainCamera;         // 기준 카메라
    public LayerMask targetMask;      // 탐색할 레이어
    public float rayDistance = 1000f; // Ray의 최대 거리
    public float sphereRadius = 15f;   // SphereCast의 반경 15정도 부터 적당한 오토록온으로 작동하는데 부족하다 싶으면 크기를 키우자

    public LineRenderer lineRenderer;
    private Transform currentTarget;  // 현재 고정된 타겟
    private Vector3 aimPoint;         // 조준 위치
    
    void Awake()
    {
        if (lineRenderer ==null)
        {
            // LineRenderer가 설정되지 않았으면 동적 생성
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f; // 시작 폭
            lineRenderer.endWidth = 0.1f;   // 종료 폭
            lineRenderer.positionCount = 2; // 두 점으로 구성
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 기본 머티리얼
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.cyan;
        }
    }
    
    void Update()
    {
        // 1. 마우스 포인터 기준으로 생성된 Ray
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 2. 충돌 감지 여부와 관계없이 aimPoint를 갱신
        aimPoint = ray.origin + ray.direction * rayDistance; // 기본 aimPoint는 커서 방향으로

        // 3. SphereCast를 사용하여 충돌 탐지
        if (Physics.SphereCast(ray, sphereRadius, out hit, rayDistance, targetMask))
        {
            // 충돌 지점을 기준으로 타겟 탐색
            FindTargetsNearPoint(hit.point);
        }
        else
        {
            // 타겟 없을 경우 초기화
            currentTarget = null;
        }

        // 4. 타겟 방향 갱신 (항상 aimPoint를 따라감)
        if (currentTarget)
        {
            aimPoint = currentTarget.position; // 타겟이 있으면 타겟의 위치를 aimPoint로
        }

        Vector3 targetDirection = (aimPoint - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(targetDirection);

        // 5. Line Renderer 업데이트
        UpdateLineRenderer();
    }

    /// <summary>
    /// SphereCast의 충돌 위치를 기준으로 주변 물체 탐지
    /// </summary>
    private void FindTargetsNearPoint(Vector3 centerPoint)
    {
        // Physics.OverlapSphere를 사용하여 주변의 오브젝트 탐지
        Collider[] hitColliders = Physics.OverlapSphere(centerPoint, sphereRadius, targetMask);

        currentTarget = null; // 초기화
        float closestDistance = Mathf.Infinity;

        foreach (Collider hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = hitCollider.transform;
            }
        }

        // 탐색 결과를 기준으로 aimPoint 업데이트
        if (currentTarget)
        {
            aimPoint = currentTarget.position;
        }
        else
        {
            aimPoint = centerPoint; // 탐색 실패시 기준 지점
        }
    }

    /// LineRenderer를 사용해 레이 시각화
    private void UpdateLineRenderer()
    {
        if (lineRenderer)
        {
            // LineRenderer의 시작점과 끝점 업데이트
            lineRenderer.SetPosition(0, transform.position); // 오브젝트의 위치
            lineRenderer.SetPosition(1, aimPoint);          // 조준 지점 또는 타겟 방향
        }
    }

    
    private void OnDrawGizmos()
    {
        // 디버깅용: SphereCast와 OverlapSphere 시각화
        if (mainCamera != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // SphereCast의 레이 경로 그리기
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin, ray.direction * rayDistance);

            RaycastHit hit;
            if (Physics.SphereCast(ray, sphereRadius, out hit, rayDistance, targetMask))
            {
                // 충돌 지점과 Sphere 반경 표시
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(hit.point, sphereRadius);
            }
        }
    }
}