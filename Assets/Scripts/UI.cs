using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text health;
    [SerializeField] private Player player;
    
    [CanBeNull] public static UI Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        player.OnHealthChanged += UpdateHealth;
    }

    private void Start()
    {
        UpdateHealth(player.Health, player.MaxHealth);
    }

    public void UpdateHealth(int current, int max)
    {
        health.text = $"{current}/{max}";
    }
}
