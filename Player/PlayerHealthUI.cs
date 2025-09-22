// PlayerHealthUI.cs (하트 시스템 버전)
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("참조")]
    [Tooltip("하트 UI 이미지들이 담겨있는 부모 오브젝트")]
    [SerializeField] private GameObject heartsContainer;
    [Tooltip("하트 하나를 나타내는 UI")]
    [SerializeField] private GameObject heartPrefab;

    private PlayerStatus playerStatus;
    private List<Image> heartImages = new List<Image>();

    void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
        if (playerStatus != null)
        {
            // 초기 하트 UI 생성
            InitializeHearts(playerStatus.MaxLives);
            
            // 이벤트 구독
            playerStatus.OnLivesChanged += UpdateUI;
            
            // 초기값으로 UI 업데이트
            UpdateUI(playerStatus.CurrentLives);
        }
        else
        {
            Debug.LogError("씬에서 PlayerStatus를 찾을 수 없습니다!");
        }
    }

    private void OnDestroy()
    {
        if (playerStatus != null)
        {
            playerStatus.OnLivesChanged -= UpdateUI;
        }
    }

    /// <summary>
    /// 게임 시작 시 최대 하트 개수만큼 UI를 생성합니다.
    /// </summary>
    private void InitializeHearts(int maxLives)
    {
        
        foreach (Transform child in heartsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        heartImages.Clear();

        // 최대 목숨 개수만큼 하트 프리팹을 생성하여 리스트에 추가
        for (int i = 0; i < maxLives; i++)
        {
            GameObject heartObj = Instantiate(heartPrefab, heartsContainer.transform);
            heartImages.Add(heartObj.GetComponent<Image>());
        }
    }

    /// <summary>
    /// OnLivesChanged 이벤트가 발생할 때마다 호출될 함수.
    /// </summary>
    /// <param name="currentLives">현재 목숨(하트) 개수</param>
    private void UpdateUI(int currentLives)
    {
        // 모든 하트를 순회하며
        for (int i = 0; i < heartImages.Count; i++)
        {
            // 현재 목숨 개수보다 인덱스가 작으면 (예: 목숨 3개일 때 0, 1, 2번 하트)
            if (i < currentLives)
            {
                // 하트를 켠다 (보이게 한다)
                heartImages[i].enabled = true;
            }
            else
            {
                // 그렇지 않으면 하트를 끈다 (안 보이게 한다)
                heartImages[i].enabled = false;
            }
        }
    }
}