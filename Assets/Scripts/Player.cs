using UnityEngine;

public class Player : LivingEntity
{


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnDamage(10);
        }
    }
    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
        DamagePopupSpawner.Instance.ShowPopup(transform.position, damage);

    }
    
}
