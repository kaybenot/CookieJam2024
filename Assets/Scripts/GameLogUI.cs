using UnityEngine;

public class GameLogUI : MonoBehaviour
{
    [SerializeField] private GameObject logLinePrefab;
    
    private void Awake()
    {
        GameLog.Instance.OnMessage += LogMessage;
    }

    private void OnDestroy()
    {
        GameLog.Instance.OnMessage -= LogMessage;
    }

    public void LogMessage(string msg, float time)
    {
        Instantiate(logLinePrefab, transform).GetComponent<GameLogLine>().ShowLine(msg, time);
    }

    [ContextMenu("Print test log message")]
    private void LogTest()
    {
        GameLog.Instance.Log("Test message");
    }
}
