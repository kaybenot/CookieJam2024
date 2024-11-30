using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    public int Health;
    public List<Sigil> Sigils;
    public float TimeToAttack = 4f;
}
