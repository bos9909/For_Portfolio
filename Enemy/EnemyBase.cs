using UnityEngine;
using System.Collections;
using MoreMountains.Feedbacks;

// 필요한 모든 컴포넌트를 명시
[RequireComponent(typeof(EnemyMovement), typeof(EnemyStatus))]
public class EnemyBase : MonoBehaviour, IDamageable
{
    public enum EnemyState { Chasing, Attacking, Dead }
    public EnemyState CurrentState { get; private set; }

    
    private EnemyMovement _movement;
    private EnemyStatus _status;

    private MMF_Player _mmfPlayer;
    private Renderer _renderer;
    
    [SerializeField] private Transform playerTransform;

    [Header("피격 효과 설정")]
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material hitMaterial;
    [SerializeField] private float invincibilityDuration = 0.2f;
    
    private bool isInvincible = false;
    
    private void OnEnable()
    {
        // 활성화시 구독
        _status.OnDeath += Die;
    }

    private void OnDisable()
    {
        // 파괴시 구독 해지
        _status.OnDeath -= Die;
    }
    
    private void Awake()
    {
        _status = GetComponent<EnemyStatus>();
        _movement = GetComponent<EnemyMovement>();
        _mmfPlayer = GetComponent<MMF_Player>();
        _renderer = GetComponent<Renderer>();

        if (originalMaterial == null && _renderer != null)
        {
            originalMaterial = _renderer.sharedMaterial;
        }
    }

    private void Start()
    {
        // 플레이어 할당.
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        _movement.SetTarget(playerTransform);

        // 초기 상태를 추적으로 설정
        ChangeState(EnemyState.Chasing);
    }

    private void Update()
    {
        // 현재 상태가 죽음이면 아무것도 안함
        if (CurrentState == EnemyState.Dead) return;

        // 상태로 행동 관리
        switch (CurrentState)
        {
            case EnemyState.Chasing:
                UpdateChasingState();
                break;
            case EnemyState.Attacking:
                UpdateAttackingState();
                break;
        }
    }

    private void UpdateChasingState()
    {
        // 플레이어 추적
        _movement.Move();

        // 상태 전환용 함수, 일단 임시
        // float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        // if (distanceToPlayer <= _attacker.GetAttackRange() && _attacker.CanAttack)
        // {
        //     ChangeState(EnemyState.Attacking);
        // }
    }

    private void UpdateAttackingState()
    {
        // 공격 상태에서는 움직이지 않도록 할 수 있음
        // _movement.Stop(); // 이 명령은 ChangeState에서 처리하는 것이 더 좋음
        // 추격 상태로 전환할지 나중에 결정
    }

    private void ChangeState(EnemyState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;

        switch (CurrentState)
        {
            case EnemyState.Chasing:
                _movement.Resume(); // 다시 움직임
                break;
            case EnemyState.Attacking:
                _movement.Stop(); // 공격중에는 멈춤
                break;
            case EnemyState.Dead:
                _movement.Stop(); // 죽었으니 당연히 멈춤
                break;
        }
    }
    
    public void TakeDamage(int damage)
    {
        //데미지 계산
        _status.TakeDamage((int)damage);

        //피격 이펙트
        _mmfPlayer?.PlayFeedbacks();
        StartCoroutine(FlashOnHit());
        EffectManager.Instance.PlayEffect("HitEffect01", transform.position, Quaternion.identity);

        //무적상태 관리용
        StartCoroutine(InvincibilityCoroutine());
    }
    
    //피격 효과 코루틴들
    private IEnumerator FlashOnHit()
    {
        if (hitMaterial != null && _renderer != null)
        {
            _renderer.material = hitMaterial;
            yield return new WaitForSeconds(0.05f);
            _renderer.material = originalMaterial;
        }
    }
    
    //무적시간용
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
    
    
    // EnemyStatus로부터 OnDeath 신호를 받았을 때 실행될 함수
    private void Die()
    {
        // 사망 체크
        if (CurrentState == EnemyState.Dead) return;

        //상태를 Dead로 변경
        ChangeState(EnemyState.Dead);

        //사망 체크용 디버그 로그
        Debug.Log(gameObject.name + " 사망 절차 시작!");
        // - 이동 중지 (ChangeState에서 이미 처리)
        // - 콜라이더 비활성화 (죽은 적에게 총알이 계속 맞는 것을 방지)
        GetComponent<Collider>().enabled = false;
        // - 폭발 이펙트 생성
        // - 아이템 드랍
        // - 점수 추가
        // - 2초 뒤 오브젝트 파괴 또는 풀에 반납
        Destroy(gameObject, 2f);
        //나중에 풀로 되돌리는 처리를 해야함
    }
    
}