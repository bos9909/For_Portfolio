using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    
//     총알 발사
//     GameObject bullet = PoolManager.Instance.Get("Bullet");
//     bullet.transform.position = firePoint.position;
//     bullet.transform.rotation = firePoint.rotation;
//
//     피격 이펙트
//     GameObject effect = PoolManager.Instance.Get("HitFX");
//     effect.transform.position = hitPoint;
//
//     되돌릴 때
//     PoolManager.Instance.Return("Bullet", bullet);
//     PoolManager.Instance.Return("HitFX", effect);
    
    
    
    
    //싱글톤으로 관리
    public static PoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        public string key;
        public GameObject prefab;
        public int size = 100;
    }
    
    public List<Pool> pools;
    
    //딕셔너리를 통해 여러 게임 오브젝트를 사용 위에서
    private Dictionary<string, Queue<GameObject>> poolDict = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();


    void Awake()
    {
        Instance = this;

        foreach (var pool in pools)
        {
            Queue<GameObject> objectsQueue = new Queue<GameObject>();
            
            //프리팹 키 이름으로 하위 폴더 생성
            Transform subFolder = new GameObject(pool.key + "_Pool").transform;
            subFolder.SetParent(transform);

            //풀 사이즈만큼 생성해서 준비
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, subFolder);
                obj.SetActive(false);
                objectsQueue.Enqueue(obj);
            }
            
            poolDict.Add(pool.key, objectsQueue);
            prefabDict.Add(pool.key, pool.prefab);
        }
    }

    //문자열 키로 프리펩을 가져옴
    public GameObject Get(string key)
    {
        //존재하지 않을시 경고
        if (!poolDict.TryGetValue(key, out Queue<GameObject> objectsQueue))
        {
            Debug.LogWarning($"이 '{key}' 는 풀 안에 존재하지 않아용");
            return null;
        }

        GameObject obj;

        //남아있으면 가져오고 없으면 새로 만듬
        //새로 만든다는 경고가 자주 나오면 풀 크기를 조정하자
        if (objectsQueue.Count > 0)
        {
            obj = objectsQueue.Dequeue();
        }
        else
        {
            obj = Instantiate(prefabDict[key]);
            Debug.LogWarning($"[PoolManager] 풀에 '{key}' 오브젝트 부족! 추가 생성 중.");
        }
        
        //활성화하고 반환
        obj.SetActive(true);
        return obj;
    }
    
    //반환 함수
    public void Return(string key, GameObject obj)
    {
        if (!obj.activeSelf)
        {
            Debug.LogWarning($"Object {obj.name} is already inactive!");
            return; // 중복 반환 방지
        }
        
        obj.SetActive(false);
        poolDict[key].Enqueue(obj);
    }
    
}
