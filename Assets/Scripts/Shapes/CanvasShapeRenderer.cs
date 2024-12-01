using Radishmouse;
using UnityEngine;

[RequireComponent(typeof(UILineRenderer))]
public class CanvasShapeRenderer : ShapeRenderer
{
    [SerializeField]
    private Canvas canvas;

    private UILineRenderer lineRenderer;

    private bool wasDrawing;

    private void Awake()
    {
        lineRenderer = GetComponent<UILineRenderer>();
        lineRenderer.Clear();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        lineRenderer.enabled = true;
    }

    protected override void OnPointAdded(int pointIndex)
    {
        var point = shape.LinePoints[pointIndex];
        lineRenderer.AddPoint(default);
        if (lineRenderer.Points.Count < 2)
            lineRenderer.AddPoint(default);

        int lastIndex = lineRenderer.Points.Count - 1;

        float scaleFactor = 1f / canvas.scaleFactor;
        lineRenderer.SetPosition(lastIndex, scaleFactor * shape.NormalizedPoints[^1]);
        lineRenderer.SetPosition(lastIndex - 1, scaleFactor * point);
    }

    private void Update()
    {
        lineRenderer.SetPosition(lineRenderer.Points.Count - 1, shape.NormalizedPoints[^1] / canvas.scaleFactor);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        lineRenderer.Clear();
        lineRenderer.enabled = false;
    }
}
