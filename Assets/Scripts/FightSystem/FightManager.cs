using UnityEngine;

public class EnemySigilDisplay : MonoBehaviour
{

}

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

    private void Awake()
    {
        shapesController.gameObject.SetActive(false);
    }

    public void BeginFight(Enemy enemy)
    {
        GameLog.Instance.Log("You have been attacked");
        lastTimeEnemyAttacked = Time.time;
        currentEnemy = enemy;

        playerSigilsController.OnSigilDrawn += PlayerSigilsController_OnSigilDrawn;
        enemy.OnAttackStarted += Enemy_OnAttackStarted;

        fighting = true;
        shapesController.gameObject.SetActive(true);
    }

    private void PlayerSigilsController_OnSigilDrawn(Sigil sigil)
    {
        currentEnemy.Damage(sigil.Damage);
    }

    public void EndFight()
    {
        GameLog.Instance.Log("You have defeated an opponent");

        playerSigilsController.OnSigilDrawn -= PlayerSigilsController_OnSigilDrawn;
        currentEnemy.OnAttackStarted -= Enemy_OnAttackStarted;
        currentEnemy = null;

        fighting = false;
        shapesController.gameObject.SetActive(false);
    }

    private void Enemy_OnAttackStarted(EnemyAttackData attack)
    {

        var sigil = attack.sigil;

        // TODO: Attack logic, now only deals damage
        player.Damage(sigil.Damage);
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
