using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text health;
    
    [CanBeNull] public static UI Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHealth(int current, int max)
    {
        health.text = $"{current}/{max}";
    }
}
