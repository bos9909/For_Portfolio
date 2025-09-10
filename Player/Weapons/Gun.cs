using System;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : WeaponBase
{
    public Transform firepoint;
    public Rigidbody planeRigidbody; //본체의 리지드바디
    public GameObject muzzleFlash;
    
    public override void Attack()
    {
        if (!IsReady()) return;

        muzzleFlash.SetActive(true);
        
        // 발사 방향 보정: 우주선의 현재 회전을 무시하고 총구의 "forward"를 기준으로 설정
        Vector3 fireDirection = firepoint.forward.normalized; // 항상 총구의 forward 방향으로 설정
        
        // 총구 위치에서 총알 스폰
        Vector3 spawnPosition = firepoint.position;
        
        // Object pool을 사용해 총알 생성
        Bullet bulletObj = PoolManager.Instance.Get("Bullet").GetComponent<Bullet>();
        bulletObj.transform.position = spawnPosition;
        bulletObj.transform.rotation = Quaternion.LookRotation(fireDirection); // 총알의 회전도 forward 기준으로 설정
        
        // 플레이어의 속도와 총알 속도 계산
        // 프로젝트 함수로 조준 방향의 속력만 계산
        Vector3 playerVelocityAligned = Vector3.Project(planeRigidbody.linearVelocity, fireDirection);
        Vector3 finalVelocity = playerVelocityAligned + fireDirection * bulletObj.speed;
        bulletObj.Fire(finalVelocity);
        
        // 공격 타임 마킹
        MarkAttackTime();
    }
}