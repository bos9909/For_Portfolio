// PlayerMovement.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 50f;
    public float acceleration = 55f;
    public float deceleration = 10f;

    [Header("Altitude Settings")]
    public float altitudeSpeed = 3f;
    public float maxAltitude = 2000f;
    public float minAltitude = 0f;
    public float altitudeLerpSpeed = 5f;

    [Header("Mouse Aim Settings")]
    public float rotationSpeed = 5f;
    public float rayDistance = 500f;
    public Camera mainCamera;

    // 내부 상태 변수
    private CharacterController characterController;
    private Vector3 currentVelocity = Vector3.zero;
    private float altitudeInputSmoothed = 0f;
    private Vector3 aimPoint;

    //구독한 곳에서 받아온 값을 저장할 변수
    private Vector2 _moveInput;
    private float _altitudeInput;
    private Vector2 _aimInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    //틱 함수로 관리
    public void TickMovement()
    {
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 targetDirection = (forward * _moveInput.y + right * _moveInput.x).normalized;
        Vector3 targetVelocity = targetDirection * moveSpeed;

        if (targetDirection.magnitude > 0)
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
        else
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);

        characterController.Move(currentVelocity * Time.deltaTime);
    }

    //고도 관리용
    public void TickAltitude()
    {
        altitudeInputSmoothed = Mathf.Lerp(altitudeInputSmoothed, _altitudeInput, Time.deltaTime * altitudeLerpSpeed);
        float altitudeVelocity = altitudeInputSmoothed * altitudeSpeed;
        float newAltitude = transform.position.y + altitudeVelocity * Time.deltaTime;
        newAltitude = Mathf.Clamp(newAltitude, minAltitude, maxAltitude);
        transform.position = new Vector3(transform.position.x, newAltitude, transform.position.z);
    }

    //회전 관리용
    public void TickRotation()
    {
        Ray ray = mainCamera.ScreenPointToRay(_aimInput);
        aimPoint = ray.origin + ray.direction * rayDistance;
        Vector3 targetDirection = (aimPoint - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    
    public void SetMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }

    public void SetAltitudeInput(float altitudeInput)
    {
        _altitudeInput = altitudeInput;
    }

    public void SetAimInput(Vector2 aimInput)
    {
        _aimInput = aimInput;
    }
}