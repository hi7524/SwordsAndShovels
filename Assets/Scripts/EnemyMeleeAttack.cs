using UnityEngine;

// KHI
// 근거리 공격 Enemy
public class EnemyMeleeAttack : Enemy
{
    protected override void Hit()
    {
        if (Target != null)
            Target.OnDamage(damage);
    }
}
