using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChangeEventHandler(int health, int maxHealth);

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public event HealthChangeEventHandler OnHealthChanged;

    [SerializeField]
    private SigilsSettings initialSigils;

    [SerializeField]
    private List<Sigil> unlockedSigils;

    [field:SerializeField]
    public SigilTree SigilTree { get; private set; }

    public Action OnHitReceived { get; set; }
    public event Action OnDeath;

    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }

    private void Start()
    {
        unlockedSigils.Clear();
        foreach (var sigil in initialSigils.Sigils)
        {
            unlockedSigils.Add(sigil);
        }
    }

    public void Damage(int dmg)
    {
        Health -= dmg;
        OnHealthChanged?.Invoke(Health, MaxHealth);
        OnHitReceived?.Invoke();
        animator.SetTrigger("Hit");
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

    public bool TryGetSigil(IReadOnlyList<LineShape> shapes, out Sigil recognizedSigil)
    {
        recognizedSigil = default;
        foreach (var sigil in unlockedSigils)
        {
            if (SigilShapeComparer.Instance.Equals(sigil.Shape, shapes))
            {
                recognizedSigil = sigil;
                return true;
            }
        }

        return false;
    }
}
