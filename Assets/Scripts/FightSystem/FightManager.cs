using System.Collections;
using Unity.Burst;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private SigilsSettings sigilsSettings;
    [SerializeField] private Player player;
    [SerializeField] private PlayerSigilsController playerSigilsController;
    [SerializeField] private ShapesDrawingController shapesController;
    [SerializeField] private EnemySigilDisplay enemySigilDisplay;

    private bool fighting = false;
    private float lastTimeEnemyAttacked = 0f;
    private Enemy currentEnemy = null;

    private EnemyAttackData currentAttack;

    private void Awake()
    {
        shapesController.gameObject.SetActive(false);
    }

    public void BeginFight(Enemy enemy)
    {
        GameLog.Instance.Log("You have been attacked");
        lastTimeEnemyAttacked = Time.time;
        currentEnemy = enemy;

        playerSigilsController.OnShapeDrawn += PlayerSigilsController_OnShapeDrawn;
        playerSigilsController.OnSigilDrawn += PlayerSigilsController_OnSigilDrawn;
        enemy.OnAttackStarted += Enemy_OnAttackStarted;

        fighting = true;
        shapesController.gameObject.SetActive(true);
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

    private void PlayerSigilsController_OnSigilDrawn(Sigil sigil)
    {
        currentEnemy.Damage(sigil.Damage);
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
