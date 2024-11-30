using System;
using UnityEngine;

public class GameLog
{
    public Action<string, float> OnMessage { get; set; }

    public static GameLog Instance { get; }

    static GameLog()
    {
        Instance = new GameLog();
    }

    public void Log(string message, float time = 4f)
    {
        if (OnMessage == null)
        {
            Debug.LogWarning("There is no listener to OnMessage action in this context. A message would not be displayed.");
        }
        OnMessage?.Invoke(message, time);
    }
}
