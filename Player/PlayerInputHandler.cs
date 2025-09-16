// PlayerInputHandler.cs
using UnityEngine;
using System;

public class PlayerInputHandler : MonoBehaviour
{
    //싱글톤
    public static PlayerInputHandler Instance { get; private set; }

    // 이벤트 선언
    public event Action<Vector2> OnMoveInput;      // WASD 입력 (Vector2: x, z)
    public event Action<float> OnAltitudeInput;    // 고도 입력 (float: -1, 0, 1)
    public event Action<Vector2> OnAimInput;       // 마우스 위치 입력 (Vector2: screen position)

    public event Action OnFirePrimaryPressed; // 주 공격 버튼 (계속 누르는 것 감지)
    public event Action OnSwitchWeaponPressed;  // 무기 교체 버튼

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // 이동 입력 감지 및 방송
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        OnMoveInput?.Invoke(new Vector2(horizontal, vertical));

        // 고도 입력 감지 및 방송
        float altitude = 0f;
        if (Input.GetKey(KeyCode.Space)) altitude = 1f;
        else if (Input.GetKey(KeyCode.LeftControl)) altitude = -1f;
        OnAltitudeInput?.Invoke(altitude);

        // 조준 입력 감지 및 방송
        OnAimInput?.Invoke(Input.mousePosition);
        
        // 발사 입력 감지 
        // GetButton/GetKey는 누르고 있는 동안 계속 true를 반환
        if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Alpha1))
        {
            OnFirePrimaryPressed?.Invoke();
        }

        // GetButtonDown/GetKeyDown은 눌리는 '순간'에만 true를 반환
        if (Input.GetButtonDown("Fire2"))
        {
            OnSwitchWeaponPressed?.Invoke();
        }
    }
}