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

    protected override void OnEnable()
    {
        base.OnEnable();
        lineRenderer.enabled = true;
    }

    protected override void OnPointAdded(int pointIndex)
    {
        var newPoint = shape.LinePoints[pointIndex];
        var worldPosition = ScreenToWorldPosition(newPoint);
        lineRenderer.positionCount = shape.LinePoints.Count;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, worldPosition);

        UpdateLastPointPosition();
    }

    private void UpdateLastPointPosition()
    {
        var lastPointWorldPosition = ScreenToWorldPosition(shape.LinePoints[^1]);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, lastPointWorldPosition);
    }

    private void Update()
    {
        if (shape != null) 
           UpdateLastPointPosition();
    }

    private Vector3 ScreenToWorldPosition(Vector3 screenPoint)
    {
        screenPoint.z = lineDepth;
        var worldPosition = viewCamera.ScreenToWorldPoint(screenPoint);
        return worldPosition;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        lineRenderer.enabled = false;
    }
}
