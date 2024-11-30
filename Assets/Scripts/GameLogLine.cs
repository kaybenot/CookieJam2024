using System;
using TMPro;
using UnityEngine;

public class GameLogLine : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private AnimationCurve alphaCurve;

    private bool shown = false;
    private float timeStarted = 0f;
    private float timeToShow = 0f;

    public void ShowLine(string msg, float time)
    {
        text.text = msg;
        timeStarted = Time.time;
        timeToShow = time;
        shown = true;
    }

    private void Update()
    {
        if (!shown)
        {
            return;
        }

        var curvePoint = (Time.time - timeStarted) / timeToShow;
        var alpha = alphaCurve.Evaluate(curvePoint);
        
        text.alpha = alpha;

        if (timeStarted + timeToShow < Time.time)
        {
            shown = false;
            Destroy(gameObject);
        }
    }
}
