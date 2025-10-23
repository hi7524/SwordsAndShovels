using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public float MaxHealth = 100f;

    public float Health { get; protected set; }
    public bool IsDead { get; private set; }

    public event Action OnDeath;


    protected virtual void OnEnable()
    {
        IsDead = false;
        Health = MaxHealth;
    }

    protected virtual void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }

    public virtual void OnDamage(int damage)
    {
        Health -= damage;
        
        if (Health <= 0 && !IsDead)
        {
            Die();
        }

    }
}
