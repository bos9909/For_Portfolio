using System;
using UnityEngine;

public class EnemyHPUI : MonoBehaviour
{
    
    //적 스테이터스라는 스크립트가 없으면 의미가없음
    [Header("참조")]
    public EnemyStatus _enemyStatus;

    //활성화시 초기화
    private void OnEnable()
    {
        // _enemyStatus가 할당되어 있는지 반드시 확인
        if (_enemyStatus != null)
        {
            // EnemyStatus의 OnHealthChanged 이벤트에 UpdateUI 함수를 등록
            // 이걸 기반으로 UI표시를 갱신
            _enemyStatus.OnHealthChanged += UpdateUI;
            
            // UI 초기값을 현재 체력으로 바로 설정
            UpdateUI(_enemyStatus.CurrentHp, _enemyStatus.MaxHp);
        }
        else
        {
            // 스테이터스가 없을 시
            Debug.LogError("EnemyStatus가 할당되지 않았습니다!", this.gameObject);
        }
    }
    
    private void OnDisable()
    {
        //없어도 괜찮을 거 같긴 한데, 혼란을 방지하기 위해 이 UI 오브젝트가 비활성화될 때, 등록했던 UpdateUI 함수를 해지
        if (_enemyStatus != null)
        {
            _enemyStatus.OnHealthChanged -= UpdateUI;
        }
    }
    
    private void UpdateUI(int currentHealth, int maxHealth)
    {
        // 체력이 변경되었다는 신호를 받았을 때, HPController 에셋에 알립니다.
        // GetInstanceID()는 이 게임오브젝트의 고유 ID를 반환합니다.
        if (HPController.Instance != null)
        {
            HPController.Instance.ControllHP(currentHealth, this.gameObject.GetInstanceID());
        }
    }
    
}
