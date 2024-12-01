using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy;
    [SerializeField]
    private TMPro.TMP_Text hpLabel;

    private void OnEnable()
    {
        enemy.OnBehaviourCommand += Enemy_OnBehaviourCommand;
    }

    private void Enemy_OnBehaviourCommand(string obj)
    {
        hpLabel.SetText($"HP: {enemy.Stats.Health}");
    }

    private void OnDisable()
    {
        enemy.OnBehaviourCommand -= Enemy_OnBehaviourCommand;
    }
}