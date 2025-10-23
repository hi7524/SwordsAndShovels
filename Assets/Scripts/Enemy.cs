using UnityEngine;

public class Enemy : LivingEntity
{
    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
        DamagePopupSpawner.Instance.ShowPopup(transform.position, damage);

    }
}
