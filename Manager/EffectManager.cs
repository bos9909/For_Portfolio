using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance; // 싱글톤

    [System.Serializable]
    public class EffectPrefab
    {
        public string key;
        public GameObject prefab;
        public int defaultPoolSize = 30;
    }

    //이펙트를 저장할 리스트, 큐
    [SerializeField] private List<EffectPrefab> effectPrefabs = new List<EffectPrefab>();
    private Dictionary<string, Queue<GameObject>> effectPools = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 각 이펙트 풀 초기화
        foreach (var ep in effectPrefabs)
        {
            effectPools[ep.key] = new Queue<GameObject>();
            // 기본 풀 크기만큼 미리 생성
            for (int i = 0; i < ep.defaultPoolSize; i++)
            {
                var obj = Instantiate(ep.prefab, transform); // EffectManager 밑으로
                obj.SetActive(false);
                effectPools[ep.key].Enqueue(obj);
            }
        }
    }

    /// <summary>
    /// 지정된 위치에서 이펙트 실행
    /// </summary>
    public void PlayEffect(string key, Vector3 position, Quaternion rotation)
    {
        if (!effectPools.ContainsKey(key))
        {
            Debug.LogWarning($"Effect {key} not found in EffectManager!");
            return;
        }

        GameObject effectObj;
        if (effectPools[key].Count > 0)
        {
            effectObj = effectPools[key].Dequeue();
            effectObj.SetActive(true);
        }
        else
        {
            var prefab = effectPrefabs.Find(x => x.key == key).prefab;
            effectObj = Instantiate(prefab, transform);
        }

        effectObj.transform.SetPositionAndRotation(position, rotation);

        // 파티클 재생
        var ps = effectObj.GetComponent<ParticleSystem>();
        ps.Play();

        // 수명만큼 기다렸다가 반환
        StartCoroutine(ReturnEffectAfter(ps.main.duration, key, effectObj));
    }

    private IEnumerator ReturnEffectAfter(float delay, string key, GameObject effectObj)
    {
        yield return new WaitForSeconds(delay);
        effectObj.SetActive(false);
        effectPools[key].Enqueue(effectObj);
    }
}