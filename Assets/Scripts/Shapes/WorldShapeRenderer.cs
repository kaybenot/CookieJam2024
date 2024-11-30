using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WorldShapeRenderer : ShapeRenderer
{
    [SerializeField]
    private Camera viewCamera;
    [SerializeField]
    private float lineDepth = 1;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        shapesDrawingController.OnPointAdded += ShapesDrawingController_OnPointAdded;
    }

    private void ShapesDrawingController_OnPointAdded(int pointIndex)
    {
        var newPoint = shapesDrawingController.LinePoints[pointIndex];
        var worldPosition = ScreenToWorldPosition(newPoint);
        lineRenderer.positionCount = shapesDrawingController.LinePoints.Count;
        lineRenderer.SetPosition(lineRenderer.positionCount - 2, worldPosition);

        UpdateLastPointPosition();
    }

    private void UpdateLastPointPosition()
    {
        var lastPointWorldPosition = ScreenToWorldPosition(shapesDrawingController.LastPoint);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, lastPointWorldPosition);
    }

    private void Update()
    {
        bool isDrawing = shapesDrawingController.IsDrawing;
        lineRenderer.enabled = isDrawing;
        if (isDrawing)
        {
            UpdateLastPointPosition();
        }
    }

    private Vector3 ScreenToWorldPosition(Vector3 screenPoint)
    {
        screenPoint.z = lineDepth;
        var worldPosition = viewCamera.ScreenToWorldPoint(screenPoint);
        return worldPosition;
    }

    private void OnDisable()
    {
        shapesDrawingController.OnPointAdded -= ShapesDrawingController_OnPointAdded;
    }
}

