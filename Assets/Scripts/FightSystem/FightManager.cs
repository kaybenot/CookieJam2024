using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public event System.Action OnFightStarted;
    public event System.Action OnFightEnded;
    public event System.Action OnFailure;

    [SerializeField] private SigilsSettings sigilsSettings;
    [SerializeField] private Player player;
    [SerializeField] private PlayerSigilsController playerSigilsController;
    [SerializeField] private ShapesDrawingController shapesController;
    [SerializeField] private EnemySigilDisplay enemySigilDisplay;

    private bool fighting = false;
    private float lastTimeEnemyAttacked = 0f;
    private Enemy currentEnemy = null;
    public Enemy CurrentEnemy => currentEnemy;

    private EnemyAttackData currentAttack;

    [SerializeField]
    private LineShape[] availableShapesForAttacks;

    [SerializeField]
    private List<Sigil> currentAvailableSigils;
    public IReadOnlyList<Sigil> CurrentAvailableSigils => currentAvailableSigils;

    private void Awake()
    {
        shapesController.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        player.OnDeath += Player_OnDeath;
    }

    private void Player_OnDeath()
    {
        player.OnDeath -= Player_OnDeath;
        fighting = false;
        OnFailure?.Invoke();
    }

    public void BeginFight(Enemy enemy)
    {
        GameLog.Instance.Log("You have been attacked");
        lastTimeEnemyAttacked = Time.time;
        currentEnemy = enemy;

        currentAvailableSigils.Clear();
        for (int i = 0; i < 3; i++)
        {
            var sigil = GenerateRandomSigil();
            currentAvailableSigils.Add(sigil);
        }

        playerSigilsController.OnShapeDrawn += PlayerSigilsController_OnShapeDrawn;
        playerSigilsController.OnSigilDrawn += PlayerSigilsController_OnSigilDrawn;
        enemy.OnAttackStarted += Enemy_OnAttackStarted;

        fighting = true;
        shapesController.gameObject.SetActive(true);

        OnFightStarted?.Invoke();
    }

    private Sigil GenerateRandomSigil()
    {
        int shapesCount = Random.Range(2, 4);

        var listClone = new List<LineShape>(availableShapesForAttacks);
        var shapesList = new List<LineShape>();
        for (int i = 0; i < shapesCount; i++)
        {
            int randomIndex = Random.Range(0, listClone.Count);
            var shape = listClone[randomIndex];
            listClone.RemoveAt(randomIndex);
            shapesList.Add(shape);
        }

        var sigil = new Sigil
        {
            Shape = shapesList,
            Damage = shapesCount + Random.Range(-1, 2),
        };

        return sigil;
    }

    private void PlayerSigilsController_OnShapeDrawn(LineShape shape)
    {
        if (currentEnemy && currentEnemy.IsAttacking)
        {
            if (shape == currentAttack.attack.shape)
            {
                currentEnemy.Defend();
                playerSigilsController.CancelChain();
                StopAllCoroutines();
                currentAttack = default;
                enemySigilDisplay.Hide();
                GameLog.Instance.Log("Defended from attack");
            }
        }
    }

    private void PlayerSigilsController_OnSigilDrawn(IReadOnlyList<LineShape> shapesList)
    {
        for (int i = 0; i < currentAvailableSigils.Count; i++)
        {
            var sigil = currentAvailableSigils[i];
            if (SigilShapeComparer.Instance.Equals(shapesList, sigil.Shape))
            {
                GameLog.Instance.Log($"Inflicted {sigil.Damage} damage to enemy");
                currentEnemy.Damage(sigil.Damage);
                break;
            }
        }
    }

    private void Enemy_OnAttackStarted(EnemyAttackData attackData)
    {
        currentAttack = attackData;
        enemySigilDisplay.Show(attackData);
        StartCoroutine(AttackLoading(attackData));
    }

    private IEnumerator AttackLoading(EnemyAttackData attackData)
    {
        yield return new WaitForSeconds(attackData.loadingTime);

        currentEnemy.FinishAttack();

        player.Damage(attackData.damage);
        enemySigilDisplay.Hide();
        currentAttack = default;
    }

    public void EndFight()
    {
        GameLog.Instance.Log("You have defeated an opponent");

        playerSigilsController.OnSigilDrawn -= PlayerSigilsController_OnSigilDrawn;
        currentEnemy.OnAttackStarted -= Enemy_OnAttackStarted;
        currentEnemy = null;
        currentAttack = default;
        StopAllCoroutines();

        enemySigilDisplay.Hide();

        fighting = false;
        shapesController.gameObject.SetActive(false);
        OnFightEnded?.Invoke();
    }

    private void Update()
    {
        if (!fighting || currentEnemy == null)
        {
            return;
        }

        if (lastTimeEnemyAttacked + currentEnemy.TimeToAttack > Time.time)
        {
            return;
        }

        lastTimeEnemyAttacked = Time.time;
        currentEnemy.StartAttack();
    }
}
