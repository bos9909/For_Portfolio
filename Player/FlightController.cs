using System;
using UnityEngine;

public class PlayerControllerWithMouseAim : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;       // 이동 속도
    public float altitudeSpeed = 3f;  // 고도 상승/하강 속도

    [Header("Altitude Settings")]
    public float maxAltitude = 2000f;   // 최대 상승 높이
    public float minAltitude = 0f;    // 최소 하강 높이

    [Header("Mouse Aim Settings")]
    public Camera mainCamera;         // 기준 카메라

    private CharacterController characterController;
    private Vector3 aimPoint;
    public float rayDistance = 500f;
    
    private Vector3 currentVelocity = Vector3.zero;  // 현재 이동 속도
    public float acceleration = 30f;                // 가속도
    public float deceleration = 5f;                 // 감속도
    
    private float altitudeVelocity = 0f;            // 현재 고도 변화 속도
    public float altitudeAcceleration = 15f;        // 고도 가속도
    public float altitudeDeceleration = 5f;        // 고도 감속도
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("CharacterController가 캐릭터에 추가되지 않았습니다!");
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main; // 메인 카메라 참고
        }
    }

    void Update()
    {
        HandleMovement();
        HandleAltitude();
        PlayerLookAt();
    }
    
    private void HandleMovement()
    {
        // 입력 받기: 좌우(A/D), 전후(W/S)
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // 좌/우
        float verticalInput = Input.GetAxisRaw("Vertical");     // 전/후

        // 📌 카메라 기준 방향 계산
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        // Y축(상하) 영향 제거 → 평면 이동만 하도록
        forward.y = 0f;
        right.y = 0f;

        // 표준화
        forward.Normalize();
        right.Normalize();

        // 카메라 기준 입력 방향
        Vector3 targetDirection = (forward * verticalInput + right * horizontalInput).normalized;

        // 목표 속도
        Vector3 targetVelocity = targetDirection * moveSpeed;

        // 관성 적용: 가속과 감속 처리
        if (targetDirection.magnitude > 0)
        {
            // 가속
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            // 감속
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        // 캐릭터 이동
        characterController.Move(currentVelocity * Time.deltaTime);
    }

    
    [SerializeField] private float altitudeLerpSpeed = 5f;

    private float altitudeInputSmoothed = 0f;

    private void HandleAltitude()
    {
        float altitudeInput = 0f;

        if (Input.GetKey(KeyCode.Space))
            altitudeInput = 1f;
        else if (Input.GetKey(KeyCode.LeftControl))
            altitudeInput = -1f;

        // 입력을 Lerp로 부드럽게
        altitudeInputSmoothed = Mathf.Lerp(altitudeInputSmoothed, altitudeInput, Time.deltaTime * altitudeLerpSpeed);

        // 실제 속도 = 부드러워진 입력 * 최대 속도
        float altitudeVelocity = altitudeInputSmoothed * altitudeSpeed;

        // 이동 적용
        float newAltitude = transform.position.y + altitudeVelocity * Time.deltaTime;
        newAltitude = Mathf.Clamp(newAltitude, minAltitude, maxAltitude);

        transform.position = new Vector3(transform.position.x, newAltitude, transform.position.z);
    }

    // private void HandleAltitude()
    // {
    //     // 목표 고도 변화 계산
    //     float altitudeInput = 0f;
    //
    //     if (Input.GetKey(KeyCode.Space))
    //     {
    //         altitudeInput = 1f; // 상승
    //     }
    //     else if (Input.GetKey(KeyCode.LeftControl))
    //     {
    //         altitudeInput = -1f; // 하강
    //     }
    //
    //     // 목표 속도 설정
    //     float targetAltitudeChange = altitudeInput * altitudeSpeed;
    //
    //     // 관성 적용
    //     if (altitudeInput != 0)
    //     {
    //         // 고도 가속
    //         altitudeVelocity = Mathf.MoveTowards(altitudeVelocity, targetAltitudeChange, altitudeAcceleration * Time.deltaTime);
    //     }
    //     else
    //     {
    //         // 고도 감속
    //         altitudeVelocity = Mathf.MoveTowards(altitudeVelocity, 0f, altitudeDeceleration * Time.deltaTime);
    //     }
    //
    //     // 이동 적용
    //     float newAltitude = transform.position.y + altitudeVelocity * Time.deltaTime;
    //     newAltitude = Mathf.Clamp(newAltitude, minAltitude, maxAltitude);
    //
    //     transform.position = new Vector3(transform.position.x, newAltitude, transform.position.z);
    // }

    
    
    public float rotationSpeed = 5f; // 회전 속도

    public void PlayerLookAt()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        aimPoint = ray.origin + ray.direction * rayDistance; // 기본 aimPoint는 커서 방향으로

        Vector3 targetDirection = (aimPoint - transform.position).normalized;

        // 현재 회전에서 목표 회전으로 부드럽게 전환 (Smooth Rotation)
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

}
