using UnityEngine;

public class LookTarget : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask layerMask; // 필요한 경우 특정 레이어만 감지
    private float maxDistance = 100f;

    private void Awake()
    {
        // mainCamera 자동 설정 (null일 경우)
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // 마우스 위치 기준 레이를 생성합니다.
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        Vector3 targetPoint;

        // 레이캐스트를 수행하고 충돌 여부를 확인
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * maxDistance;
        }

        // 방향 계산
        Vector3 direction = targetPoint - transform.position;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction.normalized);
        }
        
        // 디버깅용 레이 보기
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
    }
}