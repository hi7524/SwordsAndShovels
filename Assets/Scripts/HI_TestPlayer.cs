using UnityEngine;

public class HI_TestPlayer : LivingEntity
{
    // 피격
    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
        Debug.Log($"{gameObject.name} HP: {Health}");
    }
    
    // 죽음
    protected override void Die()
    {
        base.Die();
        Debug.Log($"{gameObject.name} 사망");
    }
}