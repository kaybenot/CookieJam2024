using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct EnemyAttackData
{
    public EnemyAttack attack;
    public int damage;
    public float loadingTime;
}

public class Enemy : MonoBehaviour
{
    public event Action<EnemyAttackData> OnAttackStarted;
    public event Action<Sigil> OnAttack;
    public event Action<string> OnBehaviourCommand;
    public event Action<Enemy> OnDeath;

    [Header("Settings")]
    [SerializeField] private float backflipInterval = 5f;
    [SerializeField] private EnemyStats stats;

    [Header("States")]
    [SerializeField] private bool isAttacking;
    public bool IsAttacking => isAttacking;

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
        GameLog.Instance.Log($"Enemy HP: {stats.Health}");
        stats.Health -= dmg;
        if (stats.Health <= 0)
        {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }

    private void TryBackflip()
    {
        if (isAttacking)
            return;

        if (Time.time - lastBackflipTime < backflipInterval)
        {
            return;
        }
        
        OnBehaviourCommand?.Invoke("backflip");
        lastBackflipTime = Time.time;
    }

    public void StartAttack()
    {
        isAttacking = true;
        var sigil = stats.attacks[Random.Range(0, stats.attacks.Count)];
        OnAttackStarted?.Invoke(sigil);
        OnBehaviourCommand?.Invoke("attack start");
    }

    public void FinishAttack()
    {
        isAttacking = false;
        OnBehaviourCommand?.Invoke("attack");
    }
}
