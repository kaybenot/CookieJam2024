using System.Collections.Generic;
using UnityEngine;

public class LineDrawingController : MonoBehaviour
{
    [SerializeField]
    private MouseInputEventProvider mouseInputEventProvider;
    [SerializeField]
    private Camera viewCamera;
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private float lineDepth = 1;
    [SerializeField]
    private List<Vector2> linePoints;
    [SerializeField]
    private float minPointsDistance = 0.1f;

    [SerializeField]
    private LineShape[] checkedShapes;

    [SerializeField]
    private List<Vector2> normalizedPoints;

    [Header("Debug")]
    public Vector2 mousePosition;
    public LineShape bestShape;

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
        bool isDrawing = mouseInputEventProvider.IsPressed;
        lineRenderer.enabled = isDrawing;
        if (isDrawing)
        {
            UpdateShape();
        }
    }

    private void UpdateShape()
    {
        var currentMousePoint = GetCurrentMouseScreenPosition();
        float sqrDistance = (currentMousePoint - linePoints[^1]).sqrMagnitude;
        if (sqrDistance > minPointsDistance * minPointsDistance)
        {
            AddLinePoint(currentMousePoint);
            RecognizeShape();
        }

        var mouseWorldPosition = ScreenToWorldPosition(currentMousePoint);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, mouseWorldPosition);
    }

    private void AddLinePoint(Vector2 newPoint)
    {
        linePoints.Add(newPoint);
        var worldPosition = ScreenToWorldPosition(newPoint);

        lineRenderer.positionCount = linePoints.Count + 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 2, worldPosition);

        normalizedPoints.Add(new Vector2());
        ShapesHelper.GetNormalizedPoints(linePoints, normalizedPoints);
    }

    private Vector2 GetCurrentMouseScreenPosition()
    {
        var mousePosition = Input.mousePosition;
        return mousePosition;
    }

    private Vector3 ScreenToWorldPosition(Vector3 screenPoint)
    {
        screenPoint.z = lineDepth;
        var worldPosition = viewCamera.ScreenToWorldPoint(screenPoint);
        return worldPosition;
    }

    private void MouseInputEventProvider_OnReleased()
    {
        lineRenderer.positionCount = 0;
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

