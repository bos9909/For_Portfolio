
//공격을 받을 수 있는 모든 물체에게 사용되는 피격 인터페이스, 이걸 상속해서 피격판정을 구현한다.

using System.Collections;

public interface IDamageable
{
    //피해받는 함수 상속받는 쪽에서 상세구현
    void TakeDamage(int amount);

    //IEnumerator FlashOnHit();
}