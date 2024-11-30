using Radishmouse;
using System;
using UnityEngine;

[RequireComponent(typeof(UILineRenderer))]
public class CanvasShapeRenderer : ShapeRenderer
{
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
        StartDrawing();
    }

    protected override void OnPointAdded(int pointIndex)
    {
        var point = shapesDrawingController.LinePoints[pointIndex];
        lineRenderer.AddPoint(default);
        if (lineRenderer.Points.Count < 2)
            lineRenderer.AddPoint(default);

        int lastIndex = lineRenderer.Points.Count - 1;
        lineRenderer.SetPosition(lastIndex, shapesDrawingController.LastPoint);
        lineRenderer.SetPosition(lastIndex - 1, point);
    }

    private void Update()
    {
        bool isDrawing = shapesDrawingController.IsDrawing;
        if (isDrawing && wasDrawing == false)
        {
            StartDrawing();
        }
        else if (wasDrawing && isDrawing == false)
        {
            StopDrawing();
        }

        lineRenderer.enabled = isDrawing;
        if (isDrawing)
            lineRenderer.SetPosition(lineRenderer.Points.Count - 1, shapesDrawingController.LastPoint);

        wasDrawing = isDrawing;
    }

    private void StartDrawing()
    {
        lineRenderer.Clear();
        lineRenderer.AddPoint(default);
    }

    private void StopDrawing()
    {
        lineRenderer.Clear();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        lineRenderer.Clear();
        lineRenderer.enabled = false;
    }
}
