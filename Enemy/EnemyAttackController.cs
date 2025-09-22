// EnemyAttackController.cs
using System.Collections;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private Transform[] firePoints; // 총알이 발사될 위치들 (총구)
    [SerializeField] private AttackPatternSO attackPattern; // 사용할 공격 패턴 데이터

    private bool canAttack = true; // 현재 공격 가능한 상태인지 확인하는 플래그

    /// <summary>
    /// 외부(주로 EnemyBase)에서 호출할 공격 명령 함수.
    /// </summary>
    public void Attack(Transform target)
    {
        //공격 가능 상태인지 판별
        if (!canAttack || attackPattern == null || firePoints == null || firePoints.Length == 0)
        {
            return;
        }
        StartCoroutine(AttackCoroutine(target));
    }

    private IEnumerator AttackCoroutine(Transform target)
    {
        canAttack = false; // 공격 시작, 쿨타임 적용

        Debug.Log("발사!");
        
        for (int i = 0; i < attackPattern.burstCount; i++)
        {
            foreach (Transform firePoint in firePoints)
            {
                SpawnProjectile(firePoint, target);
            }

            if (attackPattern.burstCount > 1)
            {
                //단발이 아니면 텀을 두고 연사
                yield return new WaitForSeconds(attackPattern.timeBetweenBursts);
            }
        }

        yield return new WaitForSeconds(attackPattern.attackCooldown); // 공격 쿨타임 대기
        canAttack = true; // 다시 공격 가능한 상태로 전환
    }

    private void SpawnProjectile(Transform firePoint, Transform target)
    {
        // PoolManager를 사용하여 총알을 가져옴
        GameObject projectileObj = PoolManager.Instance.Get(attackPattern.projectilePoolKey).gameObject;
        // 없으면 오류 출력
        if (projectileObj == null)
        {
            Debug.LogError($"PoolManager에서 키 '{attackPattern.projectilePoolKey}'에 해당하는 총알을 찾을 수 없습니다!");
            return;
        }

        projectileObj.transform.position = firePoint.position;

        // 발사 방향 계산
        Vector3 direction = (target.position - firePoint.position).normalized;

        // 정확도(Spread) 적용
        if (attackPattern.spreadAngle > 0)
        {
            float angle = Random.Range(-attackPattern.spreadAngle / 2, attackPattern.spreadAngle / 2);
            // 총구의 퍼짐효과 적용. 직선으로 나가는 게 더 나을 거 같으면 나중에 없애자
            direction = Quaternion.AngleAxis(angle, firePoint.up) * direction; 
        }
        
        projectileObj.transform.rotation = Quaternion.LookRotation(direction);
        projectileObj.SetActive(true); // 활성화 

        // 총알 스크립트에 속도와 데미지 설정
        EnemyBullet bulletScript = projectileObj.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            // ★★★ 핵심: Bullet 스크립트의 Fire 함수에 발사자 속도와 총알 속도를 전달 ★★★
            // 적은 Rigidbody가 IsKinematic이므로 linearVelocity는 0.
            // 따라서 발사자 속도는 그냥 Vector3.zero를 전달하고, 총알 속도는 AttackPatternSO에서 가져옴.
            bulletScript.Fire(Vector3.zero, attackPattern.projectileSpeed, attackPattern.projectileDamage);
        }
    }

    public bool CanAttack => canAttack;
    public float GetAttackRange() => attackPattern != null ? attackPattern.attackRange : 0f;
}