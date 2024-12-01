using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("To Link")]
    [SerializeField] private Player player;
    [SerializeField] private FightManager fightManager;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text health;
    [SerializeField] private Button skillTreeButton;
    
    
    private void Awake()
    {
        player.OnHealthChanged += UpdateHealth;
    }

    private void Start()
    {
        UpdateHealth(player.Health, player.MaxHealth);
    }

    public void UpdateHealth(int current, int max)
    {
        health.text = $"HP: {current}/{max}";
    }

    private void Update()
    {
        UpdateSkillTreeButtonVisible();
    }

    private void UpdateSkillTreeButtonVisible()
    {
        if (fightManager.CurrentEnemy && skillTreeButton.gameObject.activeSelf)
            skillTreeButton.gameObject.SetActive(false);
        else if (skillTreeButton.gameObject.activeSelf == false && fightManager.CurrentEnemy == null)
            skillTreeButton.gameObject.SetActive(true);
    }
}
