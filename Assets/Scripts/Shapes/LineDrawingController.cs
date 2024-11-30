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
    private List<Vector3> linePoints;
    [SerializeField]
    private float minPointsDistance = 0.1f;

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
        linePoints.Clear();
        lineRenderer.positionCount = 0;
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
}
