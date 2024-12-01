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
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AvailableAttacksPanel availableAttacksPanel;
    [SerializeField] private TMP_Text killsCountLabel;
    [SerializeField] private TMP_Text killsCountSummary;

    private int killsCount = 0;

    private void Awake()
    {
        availableAttacksPanel.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
        player.OnHealthChanged += UpdateHealth;
        fightManager.OnFailure += FightManager_OnFailure;
        fightManager.OnFightStarted += FightManager_OnFightStarted;
        fightManager.OnFightEnded += FightManager_OnFightEnded;
    }

    private void FightManager_OnFightStarted()
    {
        availableAttacksPanel.gameObject.SetActive(true);
        availableAttacksPanel.SetSigils(fightManager.CurrentAvailableSigils);
    }

    private void FightManager_OnFightEnded()
    {
        availableAttacksPanel.gameObject.SetActive(false);
        availableAttacksPanel.Clear();

        killsCount++;
        killsCountLabel.text = $"Kills count: {killsCount}";
    }

    private void FightManager_OnFailure()
    {
        gameOverScreen.gameObject.SetActive(true);
        killsCountSummary.text = $"You killed {killsCount} enemies";
    }

    private void Start()
    {
        UpdateHealth(player.Health, player.MaxHealth);
        killsCountLabel.text = "Kills count: 0";
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

    private void OnDestroy()
    {
        player.OnHealthChanged -= UpdateHealth;
        fightManager.OnFailure -= FightManager_OnFailure;
        fightManager.OnFightStarted -= FightManager_OnFightStarted;
        fightManager.OnFightEnded -= FightManager_OnFightEnded;
    }
}
