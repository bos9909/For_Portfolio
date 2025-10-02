// PlayerBase.cs
using UnityEngine;

[RequireComponent(typeof(PlayerMovement),typeof(PlayerAttackController), typeof(PlayerStatus))]
public class PlayerBase : MonoBehaviour, IDamageable
{
    private PlayerMovement _movement;
    private PlayerAttackController _attacker;
    private PlayerStatus _status; //플레이어 스탯 등을 가지고 있는 스크립트
    
    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _attacker = GetComponent<PlayerAttackController>();
        _status = GetComponent<PlayerStatus>();
        //게임 오버 판정용 구독 이것만 우선 구독
        _status.OnDeath += Die;
    }
    
    void Start()
    {
        // 게임이 시작되면 커서를 잠그기
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // 스크립트 실행 순서 꼬임을 방지하기 위한 함수
        SubscribeToInputEvents();
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
            //게임 오버 관련 구독 해제
            _status.OnDeath -= Die;
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
    
    
    public void TakeDamage(int damage)
    {
        Debug.Log("맞았어");
        _status.TakeHit(); 
    }
    
    //게임 오버 처리용 함수
    private void Die()
    {
        Debug.LogError("플레이어 사망!");
        this.enabled = false; // PlayerBase 스크립트를 꺼서 Update와 입력 처리를 막음
        _movement.enabled = false;
    }
    
    private void SubscribeToInputEvents()
    {
        //순서 꼬임을 방지하기 위해 널 체크
        if (PlayerInputHandler.Instance != null)
        {
            PlayerInputHandler.Instance.OnMoveInput += _movement.SetMoveInput;
            PlayerInputHandler.Instance.OnAltitudeInput += _movement.SetAltitudeInput;
            PlayerInputHandler.Instance.OnAimInput += _movement.SetAimInput;
            PlayerInputHandler.Instance.OnFirePrimaryPressed += HandleFirePrimary;
            PlayerInputHandler.Instance.OnSwitchWeaponPressed += HandleSwitchWeapon;
        }
        else
        {
            // 만약 Instance를 못 찾으면 경고를 띄워 문제를 바로 알 수 있게 함
            Debug.LogError("PlayerInputHandler.Instance를 찾을 수 없습니다! InputHandler 오브젝트가 씬에 있는지 확인하세요.");
        }
    }
    
    private void UnsubscribeFromInputEvents()
    {
        // 널 체크하고 해지
        if (PlayerInputHandler.Instance != null)
        {
            PlayerInputHandler.Instance.OnMoveInput -= _movement.SetMoveInput;
            PlayerInputHandler.Instance.OnAltitudeInput -= _movement.SetAltitudeInput;
            PlayerInputHandler.Instance.OnAimInput -= _movement.SetAimInput;
            PlayerInputHandler.Instance.OnFirePrimaryPressed -= HandleFirePrimary;
            PlayerInputHandler.Instance.OnSwitchWeaponPressed -= HandleSwitchWeapon;
        }
    }
    
    private void OnDestroy()
    {
        // Status 관련 구독 해지
        if (_status != null)
        {
            _status.OnDeath -= Die;
        }
        
        // InputHandler 관련 구독 해지
        UnsubscribeFromInputEvents();
    }
  
    //플레이어 피격 처리용 함수
    // public void TakeDamage(int damage)
    // {
    //     //데미지 계산
    //     _status.TakeDamage((int)damage);
    //
    //     //피격 이펙트
    //     _mmfPlayer?.PlayFeedbacks();
    //     StartCoroutine(FlashOnHit());
    //     EffectManager.Instance.PlayEffect("HitEffect01", transform.position, Quaternion.identity);
    //
    //     //무적상태 관리용
    //     StartCoroutine(InvincibilityCoroutine());
    // }
    
}