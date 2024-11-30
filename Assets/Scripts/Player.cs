using System;
using UnityEngine;

public delegate void HealthChangeEventHandler(int health, int maxHealth);

public class Player : MonoBehaviour
{
    public event HealthChangeEventHandler OnHealthChanged;

    [field: SerializeField] public SigilTree SigilTree { get; private set; } = new();
    
    public Action OnHitReceived { get; set; }
    public Action OnDeath { get; set; }

    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }

    public void Damage(int dmg)
    {
        Health -= dmg;
        OnHealthChanged?.Invoke(Health, MaxHealth);
        OnHitReceived?.Invoke();
        if (Health <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void ChangeMaxHealth(int maxhp)
    {
        MaxHealth = maxhp;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        OnHealthChanged?.Invoke(Health, MaxHealth);
    }

    public void FullHeal()
    {
        Health = MaxHealth;
        OnHealthChanged?.Invoke(Health, MaxHealth);
    }
}
