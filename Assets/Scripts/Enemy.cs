using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct EnemyAttackData
{
    public float loadingTime;
    public Sigil sigil;
}

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float backflipInterval = 5f;
    [SerializeField] private EnemyStats stats;

    public event Action<EnemyAttackData> OnAttackStarted;
    public event Action<Sigil> OnAttack;
    public event Action<string> OnBehaviourCommand;
    public event Action<Enemy> OnDeath;

    public float TimeToAttack => stats.TimeToAttack;

    private float lastBackflipTime;

    private void Awake()
    {
        lastBackflipTime = Time.time;
    }

    private void Update()
    {
        TryBackflip();
    }

    public void Damage(int dmg)
    {
        stats.Health -= dmg;
        if (stats.Health <= 0)
        {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void StartAttack()
    {
        var sigil = stats.attacks[Random.Range(0, stats.attacks.Count)];
        OnAttackStarted?.Invoke(sigil);
        OnBehaviourCommand?.Invoke("attack");
    }

    private void TryBackflip()
    {
        if (Time.time - lastBackflipTime < backflipInterval)
        {
            return;
        }
        
        OnBehaviourCommand?.Invoke("backflip");
        lastBackflipTime = Time.time;
    }


}
