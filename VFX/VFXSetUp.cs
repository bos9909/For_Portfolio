using System;
using System.Collections;
using UnityEngine;

public class VFXSetUp : MonoBehaviour
{
    
    public ParticleSystem particleSystem;
    public float particleLifeTime;
    //파티클 이름으로 풀 반환을 제어
    [SerializeField]
    public string particleName;
    
    private void Awake()
    {
        //파티클 시스템의 지속시간을 가져와서 코루틴으로 제어
        particleLifeTime = particleSystem.main.duration;
        particleSystem.Stop();
        Console.WriteLine(particleLifeTime);
    }

    private void OnEnable()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
            StartCoroutine(RetrunParticle());
        }
    }
    
    
    private IEnumerator RetrunParticle()
    {
        yield return new WaitForSeconds(particleLifeTime);
        //풀로 반환
        PoolManager.Instance.Return(particleName, gameObject);
    }
}
