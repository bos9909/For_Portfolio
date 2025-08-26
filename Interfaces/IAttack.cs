

//공격 인터페이스, 이걸 상속해서 공격을 구현한다.
public interface IAttack
{
    void Attack();
    bool IsReady();
}