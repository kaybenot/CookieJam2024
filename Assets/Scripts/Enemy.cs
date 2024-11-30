using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float backflipInterval = 5f;

    public Action<int> OnAttack { get; set; }
    public Action<string> OnBehaviourCommand { get; set; }

    private float lastBackflipTime;

    private void Awake()
    {
        lastBackflipTime = Time.time;
    }

    private void Update()
    {
        TryBackflip();
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
