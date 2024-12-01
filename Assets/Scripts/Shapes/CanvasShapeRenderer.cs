using Radishmouse;
using System;
using UnityEngine;

[RequireComponent(typeof(UILineRenderer))]
public class CanvasShapeRenderer : ShapeRenderer
{
    private UILineRenderer lineRenderer;

    private bool wasDrawing;

    private Canvas canvas;

    private void Awake()
    {
        lineRenderer = GetComponent<UILineRenderer>();
        lineRenderer.Clear();
    }

    protected override void OnEnable()
    {
        canvas = GetComponentsInParent<Canvas>()[^1];
        base.OnEnable();
        lineRenderer.enabled = true;
        Refresh();
    }

    public void Refresh()
    {
        lineRenderer.Clear();
        if (shape)
        {
            var scaleFactor = 1f / canvas.scaleFactor;
            foreach (var point in shape.LinePoints)
            {
                var positionOnCanvas = scaleFactor * point - (Vector2)transform.position;
                lineRenderer.AddPoint(positionOnCanvas);
            }
        }
    }

    protected override void OnPointAdded(int pointIndex)
    {
        var point = shape.LinePoints[pointIndex];
        float scaleFactor = 1f / canvas.scaleFactor;
        
        var positionOnCanvas = scaleFactor * point - (Vector2)transform.position;
        lineRenderer.AddPoint(positionOnCanvas);
    }

    private void Update()
    {
        if (shape == null)
            return;

        lineRenderer.SetPosition(
            lineRenderer.Points.Count - 1,
            shape.LinePoints[^1] / canvas.scaleFactor - (Vector2)transform.position);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        lineRenderer.Clear();
        lineRenderer.enabled = false;
    }
}
