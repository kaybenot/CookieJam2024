using UnityEngine;

public class EnemySigilDisplay : MonoBehaviour
{

}

public class FightManager : MonoBehaviour
{
    [SerializeField] private SigilsSettings sigilsSettings;
    [SerializeField] private Player player;
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

        enemy.OnAttack += OnEnemyAttack;

        fighting = true;
        shapesController.gameObject.SetActive(true);
    }
    
    public void EndFight()
    {
        GameLog.Instance.Log("You have defeated an opponent");
        currentEnemy = null;

        fighting = false;
        shapesController.gameObject.SetActive(false);
    }

    private void OnEnemyAttack(Sigil sigil)
    {
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
        currentEnemy.Attack();
    }
}
