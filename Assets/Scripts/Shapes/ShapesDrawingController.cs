using System.Collections.Generic;
using UnityEngine;

public delegate void ShapePointAddedEventHandler(int pointIndex);

public class ShapesDrawingController : MonoBehaviour
{
    public event ShapePointAddedEventHandler OnPointAdded;

    [SerializeField]
    private MouseInputEventProvider mouseInputEventProvider;

    [SerializeField]
    private List<Vector2> linePoints;
    public IReadOnlyList<Vector2> LinePoints => linePoints;

    [SerializeField]
    private Vector2 lastPoint;
    public Vector2 LastPoint => lastPoint;

    [SerializeField]
    private float minPointsDistance = 0.1f;

    [SerializeField]
    private LineShape[] checkedShapes;

    [SerializeField]
    private List<Vector2> normalizedPoints;

    public LineShape bestShape;

    public bool IsDrawing { get; private set; }

    private void OnEnable()
    {
        mouseInputEventProvider.OnPressed += MouseInputEventProvider_OnPressed;
        mouseInputEventProvider.OnReleased += MouseInputEventProvider_OnReleased;
    }

    private void MouseInputEventProvider_OnPressed()
    {
        bestShape = null;
        Clear();
        AddLinePoint(GetCurrentMouseScreenPosition());
    }

    public void Clear()
    {
        linePoints.Clear();
        normalizedPoints.Clear();
    }

    private void Update()
    {
        IsDrawing = mouseInputEventProvider.IsPressed;
        lastPoint = GetCurrentMouseScreenPosition();
        if (IsDrawing)
        {
            UpdateShape();
        }
    }

    private void UpdateShape()
    {
        float sqrDistance = (lastPoint - linePoints[^1]).sqrMagnitude;
        if (sqrDistance > minPointsDistance * minPointsDistance)
        {
            AddLinePoint(lastPoint);
            RecognizeShape();
        }
    }

    private void AddLinePoint(Vector2 newPoint)
    {
        linePoints.Add(newPoint);
        normalizedPoints.Add(new Vector2());
        ShapesHelper.GetNormalizedPoints(linePoints, normalizedPoints);

        OnPointAdded?.Invoke(linePoints.Count - 1);
    }

    private Vector2 GetCurrentMouseScreenPosition() => Input.mousePosition;

    private void MouseInputEventProvider_OnReleased()
    {
        RecognizeShape();
        Clear();
    }

    private void RecognizeShape()
    {
        int bestShapeIndex = 0;
        float bestShapeValue = float.MaxValue;
        for (int i = 0; i < checkedShapes.Length; i++)
        {
            var shape = checkedShapes[i];
            var shapeValue = ShapesHelper.Distance(shape, normalizedPoints);
            Debug.Log($"{checkedShapes[i].name} has distance {shapeValue}");
            if (shapeValue < bestShapeValue)
            {
                bestShapeIndex = i;
                bestShapeValue = shapeValue;
            }
        }

        if (bestShapeValue > 100)
        {
            bestShape = null;
            Debug.Log($"Best shape: NONE");
        }
        else
        {
            bestShape = checkedShapes[bestShapeIndex];
            Debug.Log($"Best shape: {bestShape.name}");
        }
    }

    private void OnDisable()
    {
        mouseInputEventProvider.OnPressed -= MouseInputEventProvider_OnPressed;
        mouseInputEventProvider.OnReleased -= MouseInputEventProvider_OnReleased;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 1; i < normalizedPoints.Count; i++)
        {
            var start = normalizedPoints[i - 1];
            var end = normalizedPoints[i];
            Gizmos.DrawLine(start, end);
        }

        foreach (var shape in checkedShapes)
        {
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

