using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Action<int, int> OnHealthChanged { get; set; }
    public Action OnHitReceived { get; set; }
    public Action OnDeath { get; set; }

    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

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
