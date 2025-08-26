using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IAttack
{
    //武器のパラメータを設定、相続
    [SerializeField]
    public string weaponName;
    public float cooldown = 1f;
    protected float lastAttackTime = -Mathf.Infinity;

    public abstract void Attack();

    //攻撃準備を確認
    public virtual bool IsReady()
    {
        return Time.time >= lastAttackTime + cooldown;
    }

    //攻撃時点を記録
    protected void MarkAttackTime()
    {
        lastAttackTime = Time.time;
    }
    
}
