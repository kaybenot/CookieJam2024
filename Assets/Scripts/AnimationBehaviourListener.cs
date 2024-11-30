using System;
using UnityEngine;

public class AnimationBehaviourListener : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Enemy enemy;
    
    [Header("Setttings")]
    [SerializeField] private string backflipTriggerName = "Backflip";
    [SerializeField] private string attackTriggerName = "Attack";

    private void Awake()
    {
        enemy.OnBehaviourCommand += AnimationBehaviour;
    }

    private void OnDestroy()
    {
        enemy.OnBehaviourCommand -= AnimationBehaviour;
    }

    private void AnimationBehaviour(string message)
    {
        switch (message)
        {
            case "backflip":
            {
                animator.SetTrigger(backflipTriggerName);
                break;
            }
            case "attack":
            {
                animator.SetTrigger(attackTriggerName);
                break;
            }
        }
    }

    [ContextMenu("Play Attack Animation")]
    private void AttackTest()
    {
        AnimationBehaviour("attack");
    }
}
