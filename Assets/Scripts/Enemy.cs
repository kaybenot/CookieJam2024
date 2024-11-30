using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float backflipInterval = 5f;
    [SerializeField] private EnemyStats stats;

    public Action<int> OnAttack { get; set; }
    public Action<string> OnBehaviourCommand { get; set; }
    public Action<Enemy> OnDeath { get; set; }

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
