using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 5, -12.5f);
    public float smoothSpeed = 5f;

    [Header("카메라 회전 제한")]
    public float yawMin = -30f;   // 좌우 제한
    public float yawMax = 30f;
    public float pitchMin = -20f; // 위아래 제한
    public float pitchMax = 20f;

    private float yaw;
    private float pitch;
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        //카메라 기본 위치
        Vector3 desiredPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        //마우스 위치를 화면 좌표 기준으로 정규화
        Vector2 mouse = new Vector2(
            (Input.mousePosition.x / Screen.width - 0.5f) * 2f,
            (Input.mousePosition.y / Screen.height - 0.5f) * 2f
        );

        // 마우스를 yaw/pitch에 반영
        yaw = Mathf.Clamp(mouse.x * yawMax, yawMin, yawMax);
        pitch = Mathf.Clamp(-mouse.y * pitchMax, pitchMin, pitchMax);

        //카메라 회전 적용
        Quaternion targetRot = Quaternion.Euler(pitch, yaw + player.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * smoothSpeed);
    }
}