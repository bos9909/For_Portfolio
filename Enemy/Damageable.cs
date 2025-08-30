using System;
using System.Collections;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Damageable : MonoBehaviour, IDamageable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private MMF_Player mmfPlayer;
    //피격 이펙트가 있을 때 사용.
    
    //따로 밖에서 넣어야함
    private Renderer originalRenderer;
    private Material tempMaterial;
    public Material hitMaterial ;
    
    
    private void Awake()
    {
        mmfPlayer = GetComponent<MMF_Player>();
        originalRenderer = GetComponent<Renderer>();
        tempMaterial = originalRenderer.material;
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
        if (hitMaterial)
        {
            originalRenderer.material = hitMaterial;
            yield return new WaitForSeconds(0.05f);
            originalRenderer.material = tempMaterial;
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
        // GameObject hitEffect = PoolManager.Instance.Get("HitEffect").gameObject;
        // hitEffect.transform.position = other[0].point;
        // hitEffect.transform.rotation = Quaternion.LookRotation(other[0].normal);
        // hitEffect.SetActive(true);
    }

    private void DestroyEffectPlay(Collision other)
    {
        
    }
    

    //총알 처럼 작은 물체는 온 트리거 엔터와 레이캐스트로 충돌지점을 잡는 게 났다고 함 나중에 수정.
    private void OnCollisionEnter(Collision other)
    {
        StartCoroutine(FlashOnHit());
        mmfPlayer.PlayFeedbacks();
        ContactPoint[] contacts = other.contacts;
        HitParticlePlay(contacts);
        Debug.Log("맞았어");
    }
}
