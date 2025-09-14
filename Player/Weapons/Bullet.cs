using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //표준 속도 300 정도가 적당하다. 날라댕기는 게임이라 100이면 너무 느림
    public float speed = 300f;
    public float lifeTime = 3f;
    public int damage = 1;
    
    //플레이어 속도보다 더 빠르게 발사되기 위한 벡터 3 변수
    private float spawnTime;
    private Vector3 velocity;
    Rigidbody bulletRigidbody;
    
    private void OnEnable()
    {
        spawnTime = Time.time;
        //초기화
        if (bulletRigidbody != null)
        {
            bulletRigidbody.linearVelocity = Vector3.zero;
            bulletRigidbody.angularVelocity = Vector3.zero;
        }
        transform.position = Vector3.zero; // 위치 초기화
        transform.rotation = Quaternion.identity; // 방향 초기화

    }

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {   //총알 끼리 충돌 방지
        //내가 쏜 총알에 내가 맞는 것도 방지
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PlayerProjectile"));   
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerProjectile"), LayerMask.NameToLayer("PlayerProjectile"));   
    }

    void Update()
    {
        if (Time.time - spawnTime > lifeTime)
        {
            //시간 지나면 풀로 반환
            PoolManager.Instance.Return("Bullet", gameObject);
        }
    }

    public void Fire(Vector3 shooterVelocity)
    {
        // 발사자 속도 + 총알 속도를 합쳐 이동 방향을 결정
        bulletRigidbody.linearVelocity = shooterVelocity;
        bulletRigidbody.linearVelocity += transform.forward * speed;
        EffectManager.Instance.PlayEffect("Flash04", transform.position, Quaternion.identity);
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     IDamageable damageable = other.GetComponent<IDamageable>();
    //     if (damageable != null)
    //     {
    //         damageable.TakeDamage((int)damage);
    //     }
    //
    //     EffectManager.Instance.PlayEffect("Hit04", other.ClosestPoint(transform.position) , Quaternion.identity);
    //     PoolManager.Instance.Return("Bullet", gameObject);
    // }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("부딪힌 대상: " + other.gameObject.name);
        
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        
        if (damageable != null)
        {
            damageable.TakeDamage((int)damage);
        }
        
        //충돌 지점
        Vector3 hitPoint = other.contacts[0].point;
        
        EffectManager.Instance.PlayEffect("Hit04", hitPoint , Quaternion.identity);
        PoolManager.Instance.Return("Bullet", gameObject);
    }

}
