
using System;
using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifeTime = 3f;

    //어택 패턴에서 받아옴
    private float currentSpeed;
    private int currentDamage;

    private float spawnTime;
    private Rigidbody bulletRigidbody;

    private void OnEnable()
    {
        spawnTime = Time.time;
        if (bulletRigidbody != null)
        {
            bulletRigidbody.linearVelocity = Vector3.zero;
            bulletRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // 총알과 적의 레이어 무시 설정
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("EnemyProjectile"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyProjectile"),
            LayerMask.NameToLayer("EnemyProjectile"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("NonEnemyTarget"), LayerMask.NameToLayer("EnemyProjectile"));
    }

    void Update()
    {
        //지속시간이 다 했을 때 반환
        if (Time.time - spawnTime > lifeTime)
        {
            PoolManager.Instance.Return("EnemyBullet", gameObject); // 풀 키는 오브젝트 이름으로
        }
    }

    /// <summary>
    /// 총알을 발사하고 속도와 데미지를 설정.
    /// </summary>
    /// <param name="shooterVelocity">발사체의 현재 속도</param>
    /// <param name="bulletSpeed">총알 자체의 속도</param>
    /// <param name="bulletDamage">총알의 데미지</param>
    public void Fire(Vector3 shooterVelocity, float bulletSpeed, int bulletDamage)
    {
        currentSpeed = bulletSpeed;
        currentDamage = bulletDamage;

        bulletRigidbody.linearVelocity = shooterVelocity;
        bulletRigidbody.linearVelocity += transform.forward * currentSpeed;

        //이펙트 매니저에서 이펙트 재생
        EffectManager.Instance.PlayEffect("Flash08", transform.position, Quaternion.identity);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
        {
            Debug.Log("플레이어 히트박스에 명중! 대상: " + other.gameObject.name);

            //부모에서 컴퍼넌트 찾기
            IDamageable damageable = other.GetComponentInParent<IDamageable>();

            if (damageable != null)
            {
                // 이제 other는 Capsule Collider이므로 ClosestPoint가 안전하게 작동합니다.
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                damageable.TakeDamage(currentDamage);


                Vector3 hitNormal = (transform.position - other.transform.position).normalized; // 대략적인 법선 벡터
                EffectManager.Instance.PlayEffect("Hit08", hitPoint, Quaternion.LookRotation(hitNormal));
                PoolManager.Instance.Return("EnemyBullet", gameObject); // 풀 키는 오브젝트 이름으로
            }
        }
        

    }
}