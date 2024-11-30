using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class LevelButton : MonoBehaviour
{
    public event System.Action<LevelButton> OnClicked;

    [SerializeField]
    private int levelIndex;
    public int LevelIndex => levelIndex;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Init(int level)
    {
        this.levelIndex = level;
    }

    private void OnEnable()
    {
        button.onClick.AddListener(InvokeClicked);
    }

    private void InvokeClicked()
    {
        OnClicked?.Invoke(this);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(InvokeClicked);
    }
}
