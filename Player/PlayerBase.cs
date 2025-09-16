// PlayerBase.cs
using UnityEngine;

[RequireComponent(typeof(PlayerMovement),typeof(PlayerAttackController))]
public class PlayerBase : MonoBehaviour 
{
    private PlayerMovement _movement;
    private PlayerAttackController _attacker;
    
    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _attacker = GetComponent<PlayerAttackController>();
    }
    
    void Start()
    {
        // 게임이 시작되면 커서를 잠그기
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //이벤트를 구독
    private void OnEnable()
    {
        if (PlayerInputHandler.Instance != null)
        {
            //이동 조준 관련
            PlayerInputHandler.Instance.OnMoveInput += _movement.SetMoveInput;
            PlayerInputHandler.Instance.OnAltitudeInput += _movement.SetAltitudeInput;
            PlayerInputHandler.Instance.OnAimInput += _movement.SetAimInput;
            
            //공격관련 구독
            PlayerInputHandler.Instance.OnFirePrimaryPressed += HandleFirePrimary;
            PlayerInputHandler.Instance.OnSwitchWeaponPressed += HandleSwitchWeapon;
        }
    }

    // 이벤트 구독 해지
    private void OnDisable()
    {
        if (PlayerInputHandler.Instance != null)
        {
            //이동 조준 관련
            PlayerInputHandler.Instance.OnMoveInput -= _movement.SetMoveInput;
            PlayerInputHandler.Instance.OnAltitudeInput -= _movement.SetAltitudeInput;
            PlayerInputHandler.Instance.OnAimInput -= _movement.SetAimInput;
            //공격관련 구독 해제
            PlayerInputHandler.Instance.OnFirePrimaryPressed -= HandleFirePrimary;
            PlayerInputHandler.Instance.OnSwitchWeaponPressed -= HandleSwitchWeapon;
        }
    }

    
    private void Update()
    {
        _movement.TickMovement();
        _movement.TickAltitude();
        _movement.TickRotation();
    }
    
    //사격용 함수
    private void HandleFirePrimary()
    {
        _attacker.FirePrimary();
    }

    /// <summary>
    /// 무기 교체 버튼이 눌렸다는 '보고'를 받았을 때 실행.
    /// </summary>
    private void HandleSwitchWeapon()
    {
        _attacker.SwitchWeapon();
    }
  
    
}