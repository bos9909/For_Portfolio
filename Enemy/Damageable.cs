using System;
using System.Collections;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Damageable : MonoBehaviour, IDamageable
{
    //이 스크립트는 에너미 베이스에 통합되어서 필요없음
    //하지만 다른 오브젝트에 붙여서 피격 효과만 낼 수 있음

    private MMF_Player mmfPlayer;
    //피격 이펙트가 있을 때 사용.
    
    //따로 밖에서 넣어야함
    private Renderer originalRenderer;
    private Material tempMaterial;
    //public Material hitMaterial ;
    
    [Header("피격 머티리얼 설정")]
    [Tooltip("평상시 사용할 오리지널 머티리얼")]
    [SerializeField] private Material originalMaterial;
    [Tooltip("피격 시 잠시 보여줄 하얀색 또는 붉은색 머티리얼")]
    [SerializeField] private Material hitMaterial;
    
    private Renderer objectRenderer;
    
    bool isInvincible = false;
    
    private void Awake()
    {
        mmfPlayer = GetComponent<MMF_Player>();
        objectRenderer = GetComponent<Renderer>();
        if (originalMaterial == null)
        {
            originalMaterial = objectRenderer.sharedMaterial;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int incomeDmg)
    {
        
    }

    //데미지 계산 없이 피격만 가능한 경우
    public void TakeDamage()
    {
        
    }

    public IEnumerator FlashOnHit()
    {
        if (hitMaterial && !isInvincible)
        {
            isInvincible = true;
            objectRenderer.material = hitMaterial;
            yield return new WaitForSeconds(0.05f);
            objectRenderer.material = originalMaterial;
            isInvincible = false;
        }
        else
        {
            Debug.Log("피격용 마테리얼을 넣어!");
        }
    }


    private void HitParticlePlay(ContactPoint[] other)
    {
        //충돌지점의 위치와 법선 벡터를 가져와서 위치 조정
        EffectManager.Instance.PlayEffect("HitEffect01", other[0].point, Quaternion.Euler(other[0].normal));
        //이 더미 코드는 나중에 정리해야함
        // GameObject hitEffect = PoolManager.Instance.Get("HitEffect").gameObject;
        // hitEffect.transform.position = other[0].point;
        // hitEffect.transform.rotation = Quaternion.LookRotation(other[0].normal);
        // hitEffect.SetActive(true);
    }

    // private void HitParticlePlay(Vector3 hitPoint)
    // {
    //     EffectManager.Instance.PlayEffect("HitEffect01", hitPoint, Quaternion.identity);
    // }

    private void DestroyEffectPlay(Collision other)
    {
        
    }
    

    //충돌시 호출
    private void OnCollisionEnter(Collision other)
    {
        StartCoroutine(FlashOnHit());
        mmfPlayer.PlayFeedbacks();
        ContactPoint[] contacts = other.contacts;
        HitParticlePlay(contacts);
        Debug.Log("맞았어");
    }
    
    // private void OnTriggerEnter(Collider other)
    // {
    //     StartCoroutine(FlashOnHit());
    //     mmfPlayer.PlayFeedbacks();
    //     Vector3 contacts = other.ClosestPoint(transform.position);
    //     HitParticlePlay(contacts);
    //     Debug.Log("맞았어");
    // }
}
