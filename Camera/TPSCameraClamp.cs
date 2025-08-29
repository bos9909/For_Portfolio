using UnityEngine;

public class TPSCameraClamp : MonoBehaviour
{
    [SerializeField] private Transform characterBody;
    [SerializeField] private Transform cameraArm;
    [SerializeField] private float sensitivity = 3f;

    private float xRotation; // 위/아래 회전 (Pitch)
    private float yRotation; // 좌/우 회전 (Yaw)

    void Start()
    {
        // 초기 카메라 각도를 변수에 저장
        Vector3 angles = cameraArm.rotation.eulerAngles;
        xRotation = angles.x;
        yRotation = angles.y;

        // 마우스 커서 잠금
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        LookAround();
    }
 

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
    
        // 누적 회전값에 마우스 입력 더하기
        yRotation += mouseX;
        xRotation -= mouseY; // 마우스 위로 움직이면 카메라는 아래로 내려가야 하므로 -mouseY
    
        // 상하 회전 제한
        xRotation = Mathf.Clamp(xRotation, -45f, 70f);
    
        // 카메라 회전 적용
        Quaternion lerpRotation= Quaternion.Euler(xRotation, yRotation, 0f);
        // 부드럽게 움직이기 위한 보간
        cameraArm.rotation = Quaternion.Lerp(lerpRotation, cameraArm.rotation, Time.deltaTime * 10f);
    }
}