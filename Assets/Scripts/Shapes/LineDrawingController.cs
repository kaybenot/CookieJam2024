using System.Collections.Generic;
using System.Linq;
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
    private List<Vector3> linePoints;
    [SerializeField]
    private float minPointsDistance = 0.1f;

    [SerializeField]
    private LineShape[] checkedShapes;

    private void OnEnable()
    {
        mouseInputEventProvider.OnPressed += MouseInputEventProvider_OnPressed;
        mouseInputEventProvider.OnReleased += MouseInputEventProvider_OnReleased;
    }

    private void MouseInputEventProvider_OnPressed()
    {
        linePoints.Clear();
        AddLinePoint(GetCurrentMouseWorldPosition());
    }

    private void MouseInputEventProvider_OnReleased()
    {
        lineRenderer.positionCount = 0;

        var points2D = new Vector2[linePoints.Count];
        for (int i = 0; i < linePoints.Count; i++)
            points2D[i] = (Vector2)linePoints[i];

        ShapesHelper.GetNormalizedPoints(points2D, points2D);

        int bestShapeIndex = 0;
        float bestShapeValue = float.MaxValue; 
        for (int i = 0; i < checkedShapes.Length; i++)
        {
            var shape = checkedShapes[i];
            var shapeValue = ShapesHelper.Distance(shape, points2D);
            Debug.Log($"{checkedShapes[i].name} has distance {shapeValue}");
            if (shapeValue < bestShapeValue)
            {
                bestShapeIndex = i;
                bestShapeValue = shapeValue;
            }
        }
        if (bestShapeValue > 1)
            Debug.Log($"Best shape: NONE");
        else 
            Debug.Log($"Best shape: {checkedShapes[bestShapeIndex].name}");
        linePoints.Clear();
    }

    private void Update()
    {
        bool isDrawing = mouseInputEventProvider.IsPressed;
        lineRenderer.enabled = isDrawing;
        if (isDrawing)
        {
            var currentMousePoint = GetCurrentMouseWorldPosition();
            float sqrDistance = (currentMousePoint - linePoints[^1]).sqrMagnitude;
            if (sqrDistance > minPointsDistance * minPointsDistance)
            {
                AddLinePoint(currentMousePoint);
            }
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentMousePoint);
        }
    }

    private void AddLinePoint(Vector3 newPoint)
    {
        linePoints.Add(newPoint);
        lineRenderer.positionCount = linePoints.Count + 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 2, linePoints[^1]);
    }

    private Vector3 GetCurrentMouseWorldPosition()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = lineDepth;
        return viewCamera.ScreenToWorldPoint(mousePosition);
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

