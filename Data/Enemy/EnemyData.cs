using UnityEngine;


[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    
    public string enemyName;
    public int maxHp;
    public int moveSpeed;
    public float rotaitonSpeed;
    public float stopDistance;
    //플레이어한테 그냥 접근하는 게 아니라 호를 그리며 접근하는데 그 정도 1이면 아주 크게, 0이면 직선으로 향한다.
    public float curveFactor;
}
