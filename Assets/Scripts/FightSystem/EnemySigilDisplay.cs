using UnityEngine;

public class EnemySigilDisplay : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private AnimationCurve scalingCurve;

    private float scalingSpeed;
    private float currentProgress;

    public bool IsVisible => spriteRenderer.enabled;

    public void Show(EnemyAttackData enemyAttackData)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = enemyAttackData.attack.sprite;
        spriteRenderer.color = enemyAttackData.attack.shape.Color;
        currentProgress = 0;
        transform.localScale = Vector3.one * scalingCurve.Evaluate(currentProgress);
        scalingSpeed = 1f / enemyAttackData.loadingTime;
    }

    private void Update()
    {
        if (IsVisible)
        {
            currentProgress += Time.deltaTime * scalingSpeed;
            transform.localScale = Vector3.one * scalingCurve.Evaluate(currentProgress);
        }
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
        spriteRenderer.sprite = null;
        currentProgress = 0;
    }
}
