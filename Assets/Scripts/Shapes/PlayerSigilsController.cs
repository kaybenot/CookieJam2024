﻿using System.Collections.Generic;
using UnityEngine;

public class PlayerSigilsController : MonoBehaviour
{
    public event System.Action<IReadOnlyList<LineShape>> OnSigilDrawn;
    public event System.Action<LineShape> OnShapeDrawn;

    [Header("Settings")]
    [SerializeField]
    private ShapesDrawingController shapesDrawingController;
    [SerializeField]
    private LineShape[] checkedShapes;
    [SerializeField]
    private Player player;
    [SerializeField]
    private float resetCooldown;
    [SerializeField]
    private float recognitionThreshold;

    [Header("States")]
    [SerializeField]
    private List<LineInstance> drawnShapes;
    [SerializeField]
    private List<LineShape> recognizedDrawnShapes;
    [SerializeField]
    private float timer;
    [SerializeField]
    private LineShape bestShape;
    public LineShape BestShape => bestShape;

    private void Awake()
    {
        drawnShapes.Clear();
        recognizedDrawnShapes.Clear();
    }

    private void OnEnable()
    {
        shapesDrawingController.OnShapeDrawingStarted += ShapesDrawingController_OnShapeDrawingStarted;
        shapesDrawingController.OnShapeDrawn += ShapesDrawingController_OnShapeDrawn;
    }

    private void ShapesDrawingController_OnShapeDrawingStarted(LineInstance lineShape)
    {
        drawnShapes.Add(lineShape);
    }

    private void ShapesDrawingController_OnShapeDrawn(LineInstance lineShape)
    {
        timer = 0;
        if (TryRecognizeShape(lineShape, out var shape))
        {
            recognizedDrawnShapes.Add(shape);
            OnShapeDrawn?.Invoke(shape);
        }
    }

    private void Update()
    {
        bestShape = GetCurrentlyDrawnShape();

        if (shapesDrawingController.IsDrawing)
            return;
        if (timer > resetCooldown)
            return;

        timer += Time.deltaTime;
        if (timer > resetCooldown)
        {
            CancelChain();
            return;
        }
    }

    private LineShape GetCurrentlyDrawnShape()
    {
        if (shapesDrawingController.CurrentlyDrawnShape == null)
            return null;

        return TryRecognizeShape(shapesDrawingController.CurrentlyDrawnShape, out var recognized)
            ? null
            : recognized;
    }

    public void CancelChain()
    {
        OnSigilDrawn?.Invoke(recognizedDrawnShapes.ToArray());

        foreach (var line in drawnShapes)
            if (line)
                Destroy(line.gameObject);

        drawnShapes.Clear();
        recognizedDrawnShapes.Clear();
    }

    private bool TryRecognizeShape(LineInstance lineInstance, out LineShape recognized)
    {
        recognized = null;
        int bestShapeIndex = 0;
        float bestShapeValue = float.MaxValue;
        for (int i = 0; i < checkedShapes.Length; i++)
        {
            var shape = checkedShapes[i];
            var shapeValue = ShapesHelper.Distance(shape, lineInstance.NormalizedPoints);
            Debug.Log($"{checkedShapes[i].name} has distance {shapeValue}");
            if (shapeValue < bestShapeValue)
            {
                bestShapeIndex = i;
                bestShapeValue = shapeValue;
            }
        }

        if (bestShapeValue > 100)
        {
            recognized = null;
            Debug.Log($"Best shape: NONE");
        }
        else
        {
            recognized = checkedShapes[bestShapeIndex];
            Debug.Log($"Best shape: {recognized.name}");
        }

        return recognized != null;
    }

    private void OnDisable()
    {
        shapesDrawingController.OnShapeDrawn -= ShapesDrawingController_OnShapeDrawn;
        shapesDrawingController.OnShapeDrawingStarted -= ShapesDrawingController_OnShapeDrawingStarted;
    }

    private void OnDrawGizmos()
    {
        foreach (var shape in checkedShapes)
        {
            if (shape == null)
                continue;

            Gizmos.color = shape.Color;
            for (int i = 1; i < shape.PointsNormalized.Count; i++)
            {
                var start = shape.PointsNormalized[i - 1];
                var end = shape.PointsNormalized[i];
                Gizmos.DrawLine(start, end);
            }
        }
    }
}

